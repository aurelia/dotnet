using System;
using System.Collections.Generic;
using System.Text;

namespace Aurelia.DotNet.Extensions.Models
{
    public partial class AureliaCli
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Item Bundler { get; set; }
        public Item HttpProtocol { get; set; }
        public Build Build { get; set; }
        public Platform Platform { get; set; }
        public Item Loader { get; set; }
        public Processor Transpiler { get; set; }
        public Processor MarkupProcessor { get; set; }
        public Processor CssProcessor { get; set; }
        public Processor JsonProcessor { get; set; }
        public Item Editor { get; set; }
        public Item UnitTestRunner { get; set; }
        public Item IntegrationTestRunner { get; set; }
        public Paths Paths { get; set; }
    }

}
