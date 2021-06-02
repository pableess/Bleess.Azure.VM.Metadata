using System;
using System.Collections.Generic;
using System.Text;

namespace Bleess.Azure.VM.Metadata
{

    /// <summary>
    /// Validation attested azure domains
    /// </summary>
    public enum AttestedDataValidationDomain
    {
        /// <summary>
        /// Do not validate domain
        /// </summary>
        NoValidate,

        /// <summary>
        /// Public azure cloud 
        /// </summary>
        AzureGlobal,

        /// <summary>
        /// Azure gov 
        /// </summary>
        AzureGovernment,

        /// <summary>
        /// Azure china
        /// </summary>
        AzureChina,

        /// <summary>
        /// Azure Germany
        /// </summary>
        AzureGermany
    }

    /// <summary>
    /// Advanced options for VM metadata client
    /// </summary>
    public class VmMetadataOptions
    {
        /// <summary>
        /// Azure domain to verify attested data certificate from 
        /// </summary>
        public AttestedDataValidationDomain AttestedDataAzureDomain { get; set; } = AttestedDataValidationDomain.NoValidate;

        /// <summary>
        /// Api version for the scheduled events endpoint.  
        /// Default is '2019-08-01'
        /// </summary>
        public string ScheduledEventsApiVersion { get; set; }  = "2019-08-01";

        /// <summary>
        /// A specific metadata api version.  If left null, the client will discover the most recent available version and use that.
        /// Default is '2021-01-01'
        /// </summary>
        public string MetadataApiVersion { get; set; } = "2021-01-01";

        internal string AttestationAzureDomainValue
        {
            get 
            {
                switch (AttestedDataAzureDomain)
                {
                    case AttestedDataValidationDomain.AzureGlobal:
                        return "metadata.azure.com";
                    case AttestedDataValidationDomain.AzureGovernment:
                        return "metadata.azure.us";
                    case AttestedDataValidationDomain.AzureChina:
                        return "metadata.azure.cn";
                    case AttestedDataValidationDomain.AzureGermany:
                        return "metadata.microsoftazure.de";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
