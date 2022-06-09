using System.Text.RegularExpressions;
using Dominion.Utility.Constants;

namespace Dominion.Utility.Transform
{
    public class PhoneNumberTenNumbersWithDashes : Transformer<string>
    {
        public override string Transform(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                // don't do anything if already valid
                if (!Regex.Match(value, RegexPatternConstants.PHONE_10_NUMBER_DASH_MATCH).Success)
                {
                    // remove non-digits
                    value = Regex.Replace(value, RegexPatternConstants.PHONE_NUMBER_NON_DIGIT_CHARS_MATCH, 
                        string.Empty);

                    // return valid format if we're left with only 10 digits
                    if (Regex.Match(value, RegexPatternConstants.TEN_DIGITS_MATCH).Success)
                    {
                        // reformat the 10 numbers into the desired format: 111-222-3333
                        value = Regex.Replace(
                            value, 
                            RegexPatternConstants.PHONE_10_NUMBER_3_GROUP_MATCH, 
                            RegexPatternConstants.PHONE_10_NUMBER_3_GROUP_REPLACEMENT);

                        return value;
                    }
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