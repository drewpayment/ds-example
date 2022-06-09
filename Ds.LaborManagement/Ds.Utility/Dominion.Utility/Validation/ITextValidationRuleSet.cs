using System.Collections.Generic;
using Dominion.Utility.Messaging;

namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Defines a set of validation rules to apply to text.
    /// </summary>
    public interface ITextValidationRuleSet
    {
        /// <summary>
        /// Collection of rules that have been registered on this rule set.
        /// </summary>
        IEnumerable<ITextValidationRule> Rules { get; }

        /// <summary>
        /// Validates the given text against the rules that are registered in the rule set.
        /// </summary>
        /// <param name="text">Text to validate.</param>
        /// <param name="messages">Message list to add any validation errors to.</param>
        /// <returns>Indication if all of the rules in the rule set passed validation.</returns>
        bool Validate(string text, IValidationStatusMessageList messages = null);

        /// <summary>
        /// Registers the provided validation rule in the rule set to be validated later.
        /// </summary>
        /// <param name="rule">Rule to add to the rule set.</param>
        /// <returns>Rule set to be further manipulated fluently.</returns>
        TextValidationRuleSet RegisterRule(ITextValidationRule rule);
    }
}