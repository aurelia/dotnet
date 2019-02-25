using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Aurelia.DotNet.Extensions
{
    public static class EnumHelpers
    {
        public static string Description(this Enum enumeration) => enumeration.GetType().GetField(enumeration.ToString()).GetCustomAttribute<DescriptionAttribute>()?.Description ?? enumeration.ToString();
    }
}
