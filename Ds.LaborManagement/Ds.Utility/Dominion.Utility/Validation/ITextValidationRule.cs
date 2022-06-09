namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Types describing how the text rule is constructed.
    /// </summary>
    public enum TextValidationRuleType
    {
        /// <summary>
        /// Validation rule is defined by a custom rule; typically, via a custom predicate.
        /// </summary>
        Custom = 0, 

        /// <summary>
        /// Validation rule is defined by a Regular Expression.
        /// </summary>
        RegularExpression = 1
    }


    /// <summary>
    /// Defines a validation rule for a clear-text password.
    /// </summary>
    public interface ITextValidationRule
    {
        /// <summary>
        /// Rule type indicating how the rule is constructed and can be handled (eg: whether or not a regex is used)
        /// </summary>
        TextValidationRuleType RuleType { get; }

        /// <summary>
        /// User-friendly description of the rule.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Regular expression representation of the rule, if available.
        /// </summary>
        string RegularExpression { get; }

        /// <summary>
        /// Message to display when the rule fails.
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// Indication if the rule passed (true) or failed (false) validation or null if validation has not yet been 
        /// performed.
        /// </summary>
        bool? IsValid { get; set; }

        /// <summary>
        /// Validates the given text using the rule's definition.
        /// </summary>
        /// <param name="text">Text to validate.</param>
        /// <returns>Indication if the text met the rule criteria.</returns>
        bool Validate(string text);
    }
}