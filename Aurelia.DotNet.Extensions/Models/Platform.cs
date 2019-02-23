namespace Aurelia.DotNet.Extensions.Models
{
    public class Platform
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public bool Hmr { get; set; }
        public bool Open { get; set; }
        public long Port { get; set; }
        public string Output { get; set; }
    }
}