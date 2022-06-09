using Dominion.Utility.Constants;

namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Base regular expression validation rule.
    /// </summary>
    public class RegularExpressionValidationRule : ITextValidationRule
    {
        private readonly string _regularExpression;
        private readonly string _description;
        private readonly string _errorMessage;

        /// <summary>
        /// Indication if the rule passed (true) or failed (false) validation or null if validation has not yet been 
        /// performed.
        /// </summary>
        public bool? IsValid { get; set; }

        /// <summary>
        /// User-friendly description of the rule.
        /// </summary>
        public virtual string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Regular expression representation of the rule, if available.
        /// </summary>
        public virtual string RegularExpression
        {
            get { return _regularExpression; }
        }

        /// <summary>
        /// Error message to be used when the text does not satisfy the given rule.
        /// </summary>
        public virtual string ErrorMessage
        {
            get { return _errorMessage; }
        }

        /// <summary>
        /// Default value = false.
        /// </summary>
        public virtual bool AllowNullText
        {
            get { return false; }
        }

        /// <summary>
        /// Set to TextValidationRuleType.RegularExpression for all RegularExpressionValidationRules.
        /// </summary>
        public TextValidationRuleType RuleType
        {
            get { return TextValidationRuleType.RegularExpression; }
        }

        /// <summary>
        /// Instantiates a new RegularExpressionValidationRule.
        /// </summary>
        /// <param name="regularExpression">Regular expression to perform validation with.</param>
        /// <param name="description">Description of the validation rule.</param>
        /// <param name="errorMessage">Error message to return as a result of a failed validation.</param>
        public RegularExpressionValidationRule(
            string regularExpression = CommonConstants.EMPTY_STRING, 
            string description = CommonConstants.EMPTY_STRING, 
            string errorMessage = CommonConstants.EMPTY_STRING)
        {
            _regularExpression = regularExpression;
            _description = description;
            _errorMessage = errorMessage;
        }

        /// <summary>
        /// Validates the given text based on the configured regular expression. Also handles a null text value based on 
        /// if the rule allows null text or not.
        /// </summary>
        /// <param name="text">Text to validate against the regular expression.</param>
        /// <returns>Indication if there was a regular expression match.</returns>
        public virtual bool Validate(string text)
        {
            if (text == null)
                return AllowNullText;

            IsValid = System.Text.RegularExpressions.Regex.IsMatch(text, RegularExpression);

            return IsValid.Value;
        }
    }
}