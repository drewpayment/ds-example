using System;
using System.Text.RegularExpressions;
using Dominion.Utility.Constants;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.Transform
{
    /// <summary>
    /// This is a common transformation pattern with some variations.
    /// </summary>
    public class AdHocRegEx : Transformer<string>
    {
        public string MatchPattern { get; set; }
        public string ReplacePattern { get; set; }
        public string ReplaceChar { get; set; }
        public int? MaxLength { get; set; }

        public AdHocRegEx(
            string matchPattern, 
            string replacePattern,
            string replacementChar = CommonConstants.EMPTY_STRING)
        {
            MatchPattern = matchPattern;
            ReplacePattern = replacePattern;
            ReplaceChar = replacementChar;
        }

        public override string Transform(string value)
        {
            var origVal = value;
            if (!string.IsNullOrEmpty(value))
            {
                // don't do anything if already valid
                if (!value.IsTotalMatch(MatchPattern))
                {
                    // remove non-digits
                    value = Regex.Replace(value, ReplacePattern, 
                        ReplaceChar);

                    //only return the max length portion of string from start of string 
                    if(MaxLength.HasValue && value.Length > MaxLength.Value)
                        value = value.Substring(0, MaxLength.Value);

                    //return if successfully transformed
                    if(value.IsTotalMatch(MatchPattern))
                        return value;
                }
                else
                {
                    return value;
                }
            }

            return string.Empty;
        }

    }
}