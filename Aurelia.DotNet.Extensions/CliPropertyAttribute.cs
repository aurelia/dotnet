﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Aurelia.DotNet.Extensions
{
    public class CliPropertyAttribute : Attribute
    {
        public CliPropertyAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
