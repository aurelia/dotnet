using System.ComponentModel;

namespace Aurelia.DotNet.Extensions.Models
{
    public enum HttpProtocol
    {
        [Description("HTTP 1.1")]
        HTTP1,
        [Description("HTTP 2")]
        HTTP2,
    }
}