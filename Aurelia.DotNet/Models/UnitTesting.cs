using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Aurelia.DotNet.Models
{
    public enum UnitTesting
    {
        None,
        [Description("Karma + Jasmine")]
        KarmaJasmine,
        Jest
    }
}
