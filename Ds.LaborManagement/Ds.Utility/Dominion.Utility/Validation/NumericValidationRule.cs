namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Regular expression validation rule requiring text to have a certain number of numeric characters.
    /// </summary>
    public class NumericValidationRule : RegularExpressionValidationRule
    {
        #region VARIABLES & PROPERTIES

        public static readonly int DefaultMinimumNumbersRequired = 1;

        /// <summary>
        /// Number of numeric characters the text should contain.
        /// </summary>
        public int MinimumNumbersRequired { get; set; }

        /// <summary>
        /// User-friendly message used to describe the number rule.
        /// </summary>
        public override string Description
        {
            get { return "Contain " + MinimumNumbersRequired + " number"; }
        }

        /// <summary>
        /// Error message that can be used if text does not contain the correct number of numeric characters.
        /// </summary>
        public override string ErrorMessage
        {
            get { return "At least " + MinimumNumbersRequired + " digits (0-9) must be specified"; }
        }

        /// <summary>
        /// Regular expression representation of the numeric rule: ^(?=.*\d).*$
        /// </summary>
        public override string RegularExpression
        {
            get
            {
                string regex = @"^(?=";

                for (int i = 0; i < MinimumNumbersRequired; i++)
                {
                    regex += @".*\d";
                }

                regex += @").*$";

                return regex;
            }
        }

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Instantiates a new NumericValidationRule with default settings.
        /// </summary>
        public NumericValidationRule()
        {
            MinimumNumbersRequired = DefaultMinimumNumbersRequired;
        }

        #endregion
    }
}