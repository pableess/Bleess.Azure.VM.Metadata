using Microsoft.Extensions.Options;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Bleess.Azure.VM.Metadata
{
    internal class VmMetadataClient : IVmMetadataClient
    {
        private readonly HttpClient http;
        private readonly JsonSerializerOptions jsonInstanceServiceOptions;
        private readonly JsonSerializerOptions jsonEventsServiceOptions;
        private string version;
        private readonly IOptionsSnapshot<VmMetadataOptions> options;

        private ScheduledEvents cachedEventResult;
        private string cachedEventResultEtag;

        public VmMetadataClient(HttpClient http, IOptionsSnapshot<VmMetadataOptions> options)
        {
            this.options = options;
            this.http = http;
            this.jsonInstanceServiceOptions = new JsonSerializerOptions();
            this.jsonInstanceServiceOptions.Converters.Add(new JsonStringEnumConverter());
            this.jsonInstanceServiceOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            this.jsonInstanceServiceOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
            this.jsonInstanceServiceOptions.Converters.Add(new JsonStringBoolConverter());
            this.jsonInstanceServiceOptions.Converters.Add(new DateTimeConverterUsingDateTimeParse());

            this.jsonEventsServiceOptions = new JsonSerializerOptions();
            this.jsonEventsServiceOptions.Converters.Add(new JsonStringEnumConverter());
            this.jsonEventsServiceOptions.Converters.Add(new DateTimeConverterUsingDateTimeParse());

        }

        /// <summary>
        /// Gets the versions of the Metadata API
        /// </summary>
        /// <returns></returns>
        public async Task<string[]> GetVersions(CancellationToken cancel = default) {
            
            var versions = await this.http.GetFromJsonAsync<Versions>("versions", this.jsonInstanceServiceOptions, cancel);
            return versions.ApiVersions?.ToArray();
        }

        private async ValueTask<string> GetLatestVersion(CancellationToken cancel)
        {
            if (this.version == null) 
            {
                var versions = await GetVersions(cancel);
                this.version = versions.LastOrDefault();
            }
            return this.version;
        }

        public async Task<VmInstance> GetVmInstanceMetadata(CancellationToken cancel = default)
        {
            var version = await this.GetLatestVersion(cancel);
            return await this.http.GetFromJsonAsync<VmInstance>($"instance?api-version={version}", this.jsonInstanceServiceOptions, cancel);
        }

        public async Task<ScheduledEvents> GetScheduledEvents(CancellationToken cancel = default)
        {
            // scheduled events API version is not the same as instance metadata
            // because this can be called quite frequently on polling cycles we should cache the last result and compare the etags before the entire response
            // is read and deserialzed
            using var result = await this.http.GetAsync($"scheduledevents?api-version={this.options.Value.ScheduledEventsApiVersion}", HttpCompletionOption.ResponseHeadersRead, cancel);

            // the api doesn't return a proper quoted etag, so the fallback here works, but we'll keep check in case they fix it
            var resultEtag = result.Headers?.ETag?.Tag ?? result.Headers?.GetValues("ETag")?.FirstOrDefault();

            if (this.cachedEventResult == null || string.IsNullOrEmpty(this.cachedEventResultEtag) || !string.Equals(this.cachedEventResultEtag, resultEtag, StringComparison.Ordinal)) 
            {
                this.cachedEventResult = await result.Content.ReadFromJsonAsync<ScheduledEvents>(this.jsonEventsServiceOptions, cancellationToken: cancel);
                this.cachedEventResultEtag = resultEtag;
            }

            return this.cachedEventResult;
        }


        public Task StartEvents(IEnumerable<Event> evts, CancellationToken cancel = default) => this.StartEvents(cancel, evts?.Select(e => e.EventId).ToArray());

        public async Task StartEvents(CancellationToken cancel = default, params string[] eventIds)
        {
            var req = new StartRequest(eventIds?.Select(id => new EventStartRequest(id))?.ToList());
            var res = await this.http.PostAsJsonAsync($"scheduledevents?api-version={this.options.Value.ScheduledEventsApiVersion}", req, this.jsonEventsServiceOptions, cancellationToken: cancel);
            res.EnsureSuccessStatusCode();
        }

        public async Task<AttestedData> GetAttestedInstanceMetadata(CancellationToken cancel) 
        {
            // create a nonce 
            string nonce = GetNonce();

            var version = await this.GetLatestVersion(cancel);

            var documentResp = await this.http.GetAsync($"attested/document?api-version={version}&nonce={nonce}", cancel);

            var document = await documentResp.Content.ReadFromJsonAsync<AttestedDocument>(this.jsonInstanceServiceOptions, cancel);
            ValidateCertificate(document);
            return this.ValidateAttestedData(document.Signature, nonce);
        }

        private void ValidateCertificate(AttestedDocument document)
        {

            if (document.Encoding.Equals("pkcs7"))
            {
                try
                {

                    // Build certificate from response
                    X509Certificate2 cert = new X509Certificate2(System.Text.Encoding.UTF8.GetBytes(document.Signature));
                    // Build certificate chain
                    X509Chain chain = new X509Chain();
                    chain.Build(cert);
                    
                    // validate the cert
                    foreach (var element in chain.ChainElements)
                    {
                        element.Certificate.Verify();
                    }

                    // optionally check the domain against configured azure
                    if (this.options.Value.AttestedDataAzureDomain != AttestedDataValidationDomain.NoValidate)
                    {
                        var root = chain.ChainElements[0]?.Certificate;

                        if (root != null) 
                        {
                            if (!root.SubjectName.Name.Split(',').Any(n => n.EndsWith(this.options.Value.AttestationAzureDomainValue)))
                            {
                                throw new SecurityException($"Could not validate azure domain from root certificate {this.options.Value.AttestationAzureDomainValue}");
                            }
                        }

                    }
                }
                catch (CryptographicException ex)
                {
                    throw new SecurityException("Invalid certificate", ex);
                }
            }
            else
            {
                throw new SecurityException($"Invalid Encoding {document.Encoding}");
            }
        }

        private AttestedData ValidateAttestedData(string signature, string nonce)
        {
            try
            {
                byte[] blob = Convert.FromBase64String(signature);
                SignedCms signedCms = new SignedCms();
                signedCms.Decode(blob);
                string result = System.Text.Encoding.UTF8.GetString(signedCms.ContentInfo.Content);
                AttestedData data = JsonSerializer.Deserialize<AttestedData>(result, this.jsonInstanceServiceOptions);

                //// we cannot validate the nonce because the service caches responses and will not always return the correct nonce
                // as far as I can tell there is no way to determin if the response was cached and we shouldn't validate the nonce, so 
                // I guess it can't really be validated
                
                //if (!data.Nonce.Equals(nonce))
                //{
                //  throw new SecurityException("Nonce does not match");
                //}

                return data;
            }
            catch (Exception ex)
            {
                throw new SecurityException($"Error checking signature blob: '{ex.Message}'", ex);
            }
        }

        private string GetNonce()
        {
            // random 10 digit number
            var bytes = new byte[4];

            var rand = RandomNumberGenerator.Create();
            rand.GetBytes(bytes);
            return BitConverter.ToUInt32(bytes, 0).ToString("0000000000");
        }
    }
}
