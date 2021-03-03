using System;
using System.Collections.Generic;
using System.Text;

namespace Bleess.Azure.VM.Metadata
{
    public record AttestedDocument(string Encoding, string Signature);

    public record AttestedData(string Nonce, string VmId, string Sku, string SubscriptionId, AttestedPlan Plan, AttestedTimestamp TimeStamp);
    public record AttestedPlan(string Name, string Product, string Publisher);

    // for some reason the dates aren't in a valid system.text.json date format, we would probably have to write a converter to use DateTime types
    public record AttestedTimestamp(DateTime? CreatedOn, DateTime? ExpiresOn);
}
