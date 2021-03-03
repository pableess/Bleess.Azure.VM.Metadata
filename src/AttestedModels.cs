using System;
using System.Collections.Generic;
using System.Text;

namespace Bleess.Azure.VM.Metadata
{
    /// <summary>
    /// Attested document
    /// </summary>
    public record AttestedDocument(string Encoding, string Signature) : RecordBase;

    /// <summary>
    /// Attested instance data
    /// </summary>
    public record AttestedData(string Nonce, string VmId, string Sku, string SubscriptionId, AttestedPlan Plan, AttestedTimestamp TimeStamp) : RecordBase;
    
    /// <summary>
    /// Plan information
    /// </summary>
    public record AttestedPlan(string Name, string Product, string Publisher) : RecordBase;

    /// <summary>
    /// Timestamps for attested data
    /// </summary>
    public record AttestedTimestamp(DateTime? CreatedOn, DateTime? ExpiresOn) : RecordBase;
}
