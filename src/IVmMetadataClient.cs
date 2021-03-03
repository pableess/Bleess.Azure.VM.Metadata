using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bleess.Azure.VM.Metadata
{
    /// <summary>
    /// Interface for interacting with Azure Instance Metadata Service (IMDS) and Azure Scheduled events API
    /// 
    /// See <seealso href="https://docs.microsoft.com/en-us/azure/virtual-machines/windows/instance-metadata-service"/> for more information on the service and the data returned
    /// </summary>
    public interface IVmMetadataClient
    {
        /// <summary>
        /// Gets the api versions, the latest API version will be used
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<string[]> GetVersions(CancellationToken cancel = default);

        /// <summary>
        /// Gets the VM instance metadata
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<VmInstance> GetVmInstanceMetadata(CancellationToken cancel = default);

        /// <summary>
        /// Gets the scheduled events from the scheduled events api
        /// By default this will return events from all resources in the Availability group.
        /// 
        /// To only receive events pertaining to the current VM instance, use the optional 'onlyThisInstance' argument/>
        /// </summary>
        /// <param name="onlyThisInstance">Filters the events for only events that are associated with this VM instance</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<ScheduledEvents> GetScheduledEvents(bool onlyThisInstance = false, CancellationToken cancel = default);

        /// <summary>
        /// Starts the list of scheduled events
        /// </summary>
        /// <param name="cancel"></param>
        /// <param name="eventIds"></param>
        /// <returns></returns>
        Task StartEvents(CancellationToken cancel = default, params string[] eventIds);

        /// <summary>
        /// Starts the list of scheduled events
        /// </summary>
        /// <param name="evts"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task StartEvents(IEnumerable<Event> evts, CancellationToken cancel = default);

        /// <summary>
        /// Gets the attested metadata, this data is garunteed to be authentic as from Azure environment
        /// Allowed Azure domain can be restricted via <see cref="VmMetadataOptions"/>
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="System.Security.SecurityException">Error validating the attested certificate</exception>
        Task<AttestedData> GetAttestedInstanceMetadata(CancellationToken cancel = default);

        /// <summary>
        /// Determines if current executing code in running in an azure VM
        /// </summary>
        /// <param name="validateCertificate">use the attested endpoint to ping if the result is not cached</param>
        /// <param name="force">force a requery</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<bool> IsRunningInAzure(bool validateCertificate = false, bool force = false, CancellationToken cancel = default);
    }
}
