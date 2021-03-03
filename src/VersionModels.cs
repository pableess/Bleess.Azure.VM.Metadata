using System;
using System.Collections.Generic;
using System.Text;

namespace Bleess.Azure.VM.Metadata
{
    public record Versions(IList<string> ApiVersions);
}
