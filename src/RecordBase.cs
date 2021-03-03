using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Bleess.Azure.VM.Metadata
{
    /// <summary>
    /// Base class for all deserialized records
    /// </summary>
    public record RecordBase
    {
        /// <summary>
        /// Any properties that are returned from the API not present in the model objects
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalInfo { get; init; }
    }
}
