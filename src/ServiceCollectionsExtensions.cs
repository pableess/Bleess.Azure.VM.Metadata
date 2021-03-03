using Bleess.Azure.VM.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// DI extension for VM metadata service
    /// </summary>
    public static class ServiceCollectionsExtensions
    {
        /// <summary>
        /// Adds a VM metadata client to the service collection
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddAzureVmMetadataClient(this IServiceCollection serviceCollection, Action<VmMetadataOptions> configure = null) 
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddOptions();

            if (configure != null) 
            {
                serviceCollection.Configure(configure);
            }

            serviceCollection.AddHttpClient<IVmMetadataClient, VmMetadataClient>().ConfigureHttpClient(http => 
            {
                http.BaseAddress = new Uri("http://169.254.169.254/metadata/");
                http.DefaultRequestHeaders.Add("Metadata", true.ToString());                
            }).ConfigurePrimaryHttpMessageHandler(h => new HttpClientHandler() { UseProxy = false });

            return serviceCollection;
        }
    }
}
