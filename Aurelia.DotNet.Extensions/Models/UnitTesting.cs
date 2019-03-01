using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Aurelia.DotNet.Extensions.Models
{
    public enum UnitTesting
    {
        None,
        [Description("Karma + Jasmine")]
        KarmaJasmine,
        Jest
    }
}
