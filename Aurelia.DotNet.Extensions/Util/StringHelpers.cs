using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aurelia.DotNet.Extensions
{
    public static class StringHelpers
    {
        public static string ToPascalCase(this string original)
        {
            Regex invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            Regex whiteSpace = new Regex(@"(?<=\s)");
            Regex startsWithLowerCaseChar = new Regex("^[a-z]");
            Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            // replace white spaces with undescore, then replace all invalid chars with empty string
            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(original, "_"), string.Empty)
                // split by underscores
                .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                // set first letter to uppercase
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
                // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
                // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
                // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

            return string.Concat(pascalCase);
        }

        public static string ToCamelCase(this string stringToConvert) => stringToConvert?.ToPascalCase()?.LowerCaseFirstLetter();

        public static string LowerCaseFirstLetter(this string stringToConvert) => stringToConvert?.Length > 1 ? stringToConvert[0].ToString().ToLower() + stringToConvert.Substring(1) : stringToConvert;
        public static bool IsNullOrWhiteSpace(this string stringToConvert) => string.IsNullOrEmpty(stringToConvert);
        public static string PascalToKebabCase(this string stringToConvert) => stringToConvert.IsNullOrWhiteSpace() ? string.Empty :
            Regex.Replace(stringToConvert, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", "-$1", RegexOptions.Compiled).Trim().ToLower();


    }
}
