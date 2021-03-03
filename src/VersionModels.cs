using System;
using System.Collections.Generic;
using System.Text;

namespace Bleess.Azure.VM.Metadata
{
    /// <summary>
    /// versions payload from versions endpoint
    /// </summary>
    public record Versions(IList<string> ApiVersions);
}
