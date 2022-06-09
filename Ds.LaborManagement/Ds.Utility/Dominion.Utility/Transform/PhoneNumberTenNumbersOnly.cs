using System;
using System.Text.RegularExpressions;
using Dominion.Utility.Constants;

namespace Dominion.Utility.Transform
{
    /// <summary>
    /// A phone number with numbers only, no dashes or no other silly business.
    /// YES: 1112223333
    /// NO: 111-222-3333
    /// </summary>
    public class PhoneNumberTenNumbersOnly : Transformer<string>
    {
        public override string Transform(string value)
        {
            var origVal = value;
            if (!string.IsNullOrEmpty(value))
            {
                // don't do anything if already valid
                if (!Regex.Match(value, RegexPatternConstants.TEN_DIGITS_MATCH).Success)
                {
                    // remove non-digits
                    value = Regex.Replace(
                        value,
                        RegexPatternConstants.PHONE_NUMBER_NON_DIGIT_CHARS_MATCH_EX,
                        string.Empty);

                    if(value.Length > 10)
                        value = value.Substring(0, 10);

                    //return if successfully transformed
                    if(Regex.Match(value, RegexPatternConstants.TEN_DIGITS_MATCH).Success)
                        return value;
                }
                else
                    return value;
            }

            return string.Empty;
        }

        //public override string TransformAndValidate(string value)
        //{
        //    var origVal = value;
        //    value = Transform(value);

        //    // return valid format if we're left with only 10 digits
        //    if(string.IsNullOrWhiteSpace(value))
        //    {
        //        throw new FormatException(
        //            "Invalid Phone Number Format (PhoneNumberTenNumbersOnly). Original value: " + origVal);
        //    }
        //    else
        //        return value;
        //}
    }
}