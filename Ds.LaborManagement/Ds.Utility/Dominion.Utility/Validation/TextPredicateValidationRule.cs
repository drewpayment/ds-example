using System;
using Dominion.Utility.Constants;

namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Text validation rule allowing a custom predicate to be defined that the text must satisfy.
    /// </summary>
    public class TextPredicateValidationRule : ITextValidationRule
    {
        #region VARIABLES & PROPERTIES

        public const string DEFAULT_ERROR_MESSAGE = "Text is not valid";

        /// <summary>
        /// Set to TextValidationRuleType.Custom for all predicate rules.
        /// </summary>
        public TextValidationRuleType RuleType
        {
            get { return TextValidationRuleType.Custom; }
        }

        /// <summary>
        /// Predicate definition the text will be validated against. (eg: text => isValid(text))
        /// </summary>
        public Func<string, bool> Rule { get; set; }

        /// <summary>
        /// User-friendly description of the rule.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Error message to use when text does not meet the predicate criteria.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Indication if the rule passed validation.  If null, validation has not been yet evaluated on any text.
        /// </summary>
        public bool? IsValid { get; set; }

        /// <summary>
        /// Set to null for all PredicateValidationRule instances. Use a RegularExpressionValidationRule for regex patterns.
        /// </summary>
        public string RegularExpression
        {
            get { return null; }
        }

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Instantiates a new TextPredicateValidationRule instance.
        /// </summary>
        /// <param name="rule">Predicate used to evaluate the validity of a provided text. Hint: Be sure to consider null text 
        /// conditions when defining the predicate.</param>
        /// <param name="description">User-friendly rule description.</param>
        /// <param name="errorMessage">Error message used when validation fails.</param>
        public TextPredicateValidationRule(
            Func<string, bool> rule, 
            string description = CommonConstants.EMPTY_STRING, 
            string errorMessage = DEFAULT_ERROR_MESSAGE)
        {
            Rule = rule;
            Description = description;
            ErrorMessage = errorMessage;
        }

        #endregion

        /// <summary>
        /// Validates the provided text using the defined predicate.
        /// </summary>
        /// <param name="text">Text to validate.</param>
        /// <returns></returns>
        public bool Validate(string text)
        {
            IsValid = Rule(text);

            return IsValid.Value;
        }
    }
}