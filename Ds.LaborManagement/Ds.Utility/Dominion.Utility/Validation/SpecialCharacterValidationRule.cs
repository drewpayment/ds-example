namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Regular expression rule which validates text has a required number of special characters.
    /// </summary>
    public class SpecialCharacterValidationRule : RegularExpressionValidationRule
    {
        #region VARIABLES & PROPERTIES

        public static readonly int DefaultMinimumCharactersRequired = 1;
        public static readonly string DefaultSpecialCharacterSet = "$%&_^";

        public string SpecialCharacters { get; set; }
        public int MinimumRequiredCharacters { get; set; }

        /// <summary>
        /// User-friendly rule description containing the configured special character requirements.
        /// </summary>
        public override string Description
        {
            get
            {
                return
                    "Contain at least" +
                    MinimumRequiredCharacters +
                    " special characters '" +
                    SpecialCharacters +
                    "'";
            }
        }

        /// <summary>
        /// Error message used when text does not meet the special character requirements.
        /// </summary>
        public override string ErrorMessage
        {
            get
            {
                return
                    "At least " +
                    MinimumRequiredCharacters +
                    " special characters '" +
                    SpecialCharacters +
                    "' must be specified";
            }
        }

        /// <summary>
        /// Regular expression representation of the configured special character rule: ^(?=.*['sp_chars']).*$
        /// </summary>
        public override string RegularExpression
        {
            get
            {
                string regex = @"^(?=";

                for (int i = 0; i < MinimumRequiredCharacters; i++)
                {
                    regex += @".*[" + SpecialCharacters + "]";
                }

                regex += @").*$";

                return regex;
            }
        }

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Instantiates a new SpecialCharacterValidationRule with default settings.
        /// </summary>
        public SpecialCharacterValidationRule()
        {
            MinimumRequiredCharacters = DefaultMinimumCharactersRequired;
            SpecialCharacters = DefaultSpecialCharacterSet;
        }

        #endregion
    }
}