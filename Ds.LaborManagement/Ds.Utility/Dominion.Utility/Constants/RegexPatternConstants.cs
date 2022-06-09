namespace Dominion.Utility.Constants
{
    public class RegexPatternConstants
    {
        public const string IRS_STANDARD_ILLEGAL_CHARACTERS = "[^0-9a-zA-Z\\s\\-\\(\\)&amp&apos]+";

        /// <summary>
        /// Example: 1112223333 (ie. 10 digit phone number without non-digit chars)
        /// Return one group of ten numbers if match.
        /// Will return success if it matches.
        /// </summary>
        public const string TEN_DIGITS_MATCH = @"(^[\d]{10}$)";

        /// <summary>
        /// Example: 111-222-3333
        /// Doesn't return any regex groups.
        /// Will return success if it matches.
        /// </summary>
        public const string PHONE_10_NUMBER_DASH_MATCH = @"^\d{3}-\d{3}-\d{4}$";

        /// <summary>
        /// Example: 111-222-3333
        /// Will return 3 regex groups.
        /// Will return success if it matches.
        /// </summary>
        public const string PHONE_10_NUMBER_3_GROUP_MATCH = @"(\d{3})(\d{3})(\d{4})";

        /// <summary>
        /// Example: 1112223333 --> 111-222-3333
        /// </summary>
        public const string PHONE_10_NUMBER_3_GROUP_REPLACEMENT = "$1-$2-$3";

        /// <summary>
        /// Matches: dash OR whitespace OR parens OR underscore
        /// </summary>
        public const string PHONE_NUMBER_NON_DIGIT_CHARS_MATCH = @"[-\s)(_]*";

        /// <summary>
        /// Like the original but this one includes periods.
        /// </summary>
        public const string PHONE_NUMBER_NON_DIGIT_CHARS_MATCH_EX = @"[-\s)(_.]*";


        public const string NINE_DIGITS_MATCH = @"(^[\d]{9}$)";

        public const string FIVE_DIGITS_MATCH = @"(^[\d]{5}$)";

        /// <summary>
        /// Provided by the IRS for ACA XSD validation.
        /// This is for all name fields: first, mid, last, suffix
        /// </summary>
        public const string IRS_ACA_NAME = @"([A-Za-z\-] ?)*[A-Za-z\-]";

        /// <summary>
        /// Provided by the IRS for ACA XSD validation.
        /// This is for all name fields: first, mid, last, suffix
        /// </summary>
        public const string IRS_CITY_NAME = @"(A-Za-z] ?)*[A-Za-z])";

        /// <summary>
        /// Provided by the IRS for ACA XSD validation.
        /// This is for all name fields: first, mid, last, suffix
        /// </summary>
        public const string IRS_CITY_NAME_REMOVE_CHARS = @"[^A-Za-z\-]";

        /// <summary>
        /// Chars that shouldn't be in value.
        /// This is for all name fields: first, mid, last, suffix
        /// </summary>
        public const string IRS_ACA_NAME_REMOVE_CHARS = @"[^A-Za-z\-]";

        /// <summary>
        /// Chars that shouldn't be in value.
        /// This is for all name fields: first, mid, last, suffix
        /// </summary>
        public const string IRS_ACA_CITY_NAME_REMOVE_CHARS = @"[^A-Za-z\s]";

        /// <summary>
        /// Chars that shouldn't be in value.
        /// </summary>
        public const string IRS_ACA_ADDRESS_LINE_REMOVE_CHARS = @"[^ A-Za-z0-9\-/]";

        /// <summary>
        /// 
        /// </summary>
        public const string NUMBER_ONLY = @"[^ A-Za-z0-9\-/]";

        public const string SINGLE_INTEGER_ONLY = @"^[0-9]$";
        public const string INTEGER_ONLY = @"^[0-9]*$";

        ///// <summary>
        ///// Must be between 1-50 chars.
        ///// ^[\w]{1,50}$
        ///// ^ asserts position at start of a line
        ///// Match a single character present in the list below [\w]{1,50}
        ///// {1,50} Quantifier — Matches between 1 and 50 times, as many times as possible, giving back as needed (greedy)
        ///// \w matches any word character (equal to [a-zA-Z0-9_])
        ///// $ asserts position at the end of a line
        ///// </summary>
        //public const string UN_SAFE_PASSWORD = @"^[\w]{1,50}$";


    }
}