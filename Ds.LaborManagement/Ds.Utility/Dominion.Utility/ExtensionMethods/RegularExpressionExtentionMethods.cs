using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dominion.Utility.ExtensionMethods
{
    /// <summary>
    /// Takes a string and expression and determines if there is a FULL match.
    /// </summary>
    public static class RegularExpressionExtensionMethods
    {
        public static bool IsTotalMatch(this string val, string expression)
        {
            var matchLength = Regex.Match(val, expression).Length;
            return val.Length == matchLength;
        }
    }
}
