using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aurelia.DotNet.Extensions
{
    public static class StringHelpers
    {
        public static string ToPascalCase(this string stringToConvert) => string.Concat(stringToConvert?.Split(new[] { '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(word => word.Substring(0, 1).ToUpper() +
                                         word.Substring(1).ToLower()));

        public static string ToCamelCase(this string stringToConvert) => stringToConvert?.ToPascalCase()?.LowerCaseFirstLetter();

        public static string LowerCaseFirstLetter(this string stringToConvert) => stringToConvert?.Length > 1 ? stringToConvert[0].ToString().ToLower() + stringToConvert.Substring(1) : stringToConvert;
        public static bool IsNullOrWhiteSpace(this string stringToConvert) => string.IsNullOrEmpty(stringToConvert);
        public static string PascalToKebabCase(this string stringToConvert) => stringToConvert.IsNullOrWhiteSpace() ? string.Empty :
            Regex.Replace(stringToConvert, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", "-$1", RegexOptions.Compiled).Trim().ToLower();


    }
}
