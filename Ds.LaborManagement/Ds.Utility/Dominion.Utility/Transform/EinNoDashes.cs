using System;
using System.Text.RegularExpressions;
using Dominion.Utility.Constants;

namespace Dominion.Utility.Transform
{
    /// <summary>
    /// Federal Employer Identification Number (FEIN) with no dashes).
    /// This will also work for SSN that required no dashes.
    /// There should only be 9 digits in a valid transformation.
    /// </summary>
    public class EinSsnNoDashes : Transformer<string>
    {
        public override string Transform(string value)
        {
            var origVal = value;

            if (!string.IsNullOrEmpty(value))
            {
                // don't do anything if already valid
                if (!Regex.Match(value, RegexPatternConstants.NINE_DIGITS_MATCH).Success)
                {
                    // remove non-digits
                    value = Regex.Replace(
                        value, 
                        RegexPatternConstants.PHONE_NUMBER_NON_DIGIT_CHARS_MATCH, 
                        string.Empty);

                    if(value.Length > 9)
                        value = value.Substring(0, 9);

                    //return if successfully transformed
                    if(Regex.Match(value, RegexPatternConstants.NINE_DIGITS_MATCH).Success)
                        return value;
                }
                else
                {
                    return value;
                }
            }

            return string.Empty;
        }

        //public override string TransformAndValidate(string value)
        //{
        //    var origVal = value;
        //    value = Transform(value);

        //    if(string.IsNullOrWhiteSpace(value))
        //    {
        //        throw new FormatException(
        //            "Invalid EIN Format (EinNoDashes). Original value: " + origVal);
        //    }
        //    else
        //        return value;
        //}

        //public override string TransformAndValidate(string value, Action errAction)
        //{
        //    var origVal = value;
        //    value = Transform(value);

        //    if(string.IsNullOrWhiteSpace(value))
        //    {
        //        errAction();
        //    }
        //    else
        //        return value;
        //}
    }


}