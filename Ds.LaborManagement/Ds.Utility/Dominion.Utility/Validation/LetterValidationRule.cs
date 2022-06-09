using System;
using Dominion.Utility.Constants;

namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Regular expression validation rule which validates text based on if it has the correct number of lower/upper 
    /// case letters.
    /// </summary>
    public class LetterValidationRule : RegularExpressionValidationRule
    {
        /// <summary>
        /// Flag used to set if the text must contain upper, lower or either case letters.
        /// </summary>
        [Flags]
        public enum CaseOptions
        {
            Upper = 1, 
            Lower = 2
        }

        #region VARIABLES & PROPERTIES

        public static readonly CaseOptions DefaultCaseOptions = CaseOptions.Upper | CaseOptions.Lower;
        public static readonly int DefaultMinimumLettersRequired = 1;

        /// <summary>
        /// Indication of whether lowercase, uppercase or either are required.
        /// </summary>
        public CaseOptions SelectedCaseOptions { get; set; }

        /// <summary>
        /// Minimum number of letters required to satisfy the rule.
        /// </summary>
        public int MinimumLettersRequired { get; set; }

        /// <summary>
        /// User-friendly message describing the letter requirements.
        /// </summary>
        public override string Description
        {
            get { return "Contain " + MinimumLettersRequired + CaseMessage; }
        }

        /// <summary>
        /// Error message that can be used when the rule fails.
        /// </summary>
        public override string ErrorMessage
        {
            get { return "At least " + MinimumLettersRequired + CaseMessage + "must be specified"; }
        }

        /// <summary>
        /// Regular expression representation of the letter rule: ^(?=.*[a-zA-Z]).*$ (will be modified based on rule 
        /// settings)
        /// </summary>
        public override string RegularExpression
        {
            get
            {
                // build a regex in the form of: ^(?=.*[a-zA-Z]).*$
                // where:
                // .*[a-zA-Z] will be repeated for the # of required letters
                // and will be modified for lower / upper case letters based on the configuration
                string regex = @"^(?=";

                for (int i = 0; i < MinimumLettersRequired; i++)
                {
                    regex += @".*[";

                    if ((int) (SelectedCaseOptions & CaseOptions.Lower) != 0)
                        regex += "a-z";
                    if ((int) (SelectedCaseOptions & CaseOptions.Upper) != 0)
                        regex += "A-Z";

                    regex += "]";
                }

                regex += @").*$";

                return regex;
            }
        }

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// Instantiates a new LetterValidationRule with default settings.
        /// </summary>
        public LetterValidationRule()
        {
            MinimumLettersRequired = DefaultMinimumLettersRequired;
            SelectedCaseOptions = DefaultCaseOptions;
        }

        #endregion

        #region PRIVATE HELPERS

        /// <summary>
        /// Message that indicates if only upper or lower case letters are allowed which can be concatenated to other
        /// messages.
        /// </summary>
        private string CaseMessage
        {
            get
            {
                string letters = MinimumLettersRequired > 1 ? "letters" : "letter";

                string msg = CommonConstants.EMPTY_STRING;

                // check if only uppercase letters are allowed
                if ((int) (SelectedCaseOptions ^ CaseOptions.Upper) == 0)
                    msg += " upper-case ";

                // check if only lowercase letters are allowed
                if ((int) (SelectedCaseOptions ^ CaseOptions.Lower) == 0)
                    msg += " lower-case ";

                msg += CommonConstants.SINGLE_SPACE + letters + CommonConstants.SINGLE_SPACE;

                return msg;
            }
        }

        #endregion
    }
}