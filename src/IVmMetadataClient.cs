using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bleess.Azure.VM.Metadata
{
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
        /// Gets the scheduled events
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<ScheduledEvents> GetScheduledEvents(CancellationToken cancel = default);

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
        /// <param name="cancel"></param>
        /// <param name="eventIds"></param>
        /// <returns></returns>
        Task StartEvents(IEnumerable<Event> evts, CancellationToken cancel = default);

        /// <summary>
        /// Gets the attested metadata
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<AttestedData> GetAttestedInstanceMetadata(CancellationToken cancel = default);
    }
}
