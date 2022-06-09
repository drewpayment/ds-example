using System;
using System.Text.RegularExpressions;
using Dominion.Utility.Constants;

namespace Dominion.Utility.Transform
{
    /// <summary>
    /// Federal Employer Identification Number (FEIN) with no dashes).
    /// </summary>
    public class ZipCodeFiveDigitsOnly : Transformer<string>
    {
        public override string Transform(string value)
        {
            var origVal = value;
            if (!string.IsNullOrEmpty(value))
            {
                // don't do anything if already valid
                if (!Regex.Match(value, RegexPatternConstants.FIVE_DIGITS_MATCH).Success)
                {
                    // remove non-digits
                    value = Regex.Replace(value, RegexPatternConstants.PHONE_NUMBER_NON_DIGIT_CHARS_MATCH, 
                        string.Empty);

                    //only return 5 chars
                    if(value.Length > 5)
                        value = value.Substring(0, 5);

                    //return if successfully transformed
                    if(Regex.Match(value, RegexPatternConstants.FIVE_DIGITS_MATCH).Success)
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