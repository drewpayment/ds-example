using System.Collections.Generic;
using Dominion.Utility.Messaging;
using FluentValidation;

namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Utility class used to check the validity of text against pre-defined requirements (such as password strength).
    /// </summary>
    public class TextValidationRuleSet : ITextValidationRuleSet
    {
        #region VARIABLES & PROPERTIES

        private const string DEFAULT_NAME = "text";

        /// <summary>
        /// Descriptor of the text that will be validated (ex: 'password' if validating password strength)
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// Rules currently registered in the rule set.
        /// </summary>
        private readonly List<ITextValidationRule> _rules;

        /// <summary>
        /// Underlying FluentValidation.Validator that will actually perform the validation.
        /// </summary>
        private readonly InlineValidator<string> _validator;

        /// <summary>
        /// Collection of rules that have been registered on this rule set.
        /// </summary>
        public IEnumerable<ITextValidationRule> Rules
        {
            get { return _rules; }
        }

        #endregion

        #region CONSTRUCTOR(S) & INIT

        /// <summary>
        /// Instantiates a new PasswordValidator with the predefined password validation rules.
        /// </summary>
        /// <param name="textDescriptor">Description of the object this rule set will validate (ex: 'password' if 
        /// validating password strength)</param>
        public TextValidationRuleSet(string textDescriptor = DEFAULT_NAME)
        {
            _name = textDescriptor;

            // create an underlying FluentValidation validator to perform the validation
            // and configure it to validate all registered rules no matter what
            _validator = new InlineValidator<string> {CascadeMode = CascadeMode.Continue};

            _rules = new List<ITextValidationRule>();

            ApplyBuiltInRules();
        }

        /// <summary>
        /// Applies any password rules must be applied to any rule set instance. 
        /// </summary>
        private void ApplyBuiltInRules()
        {
            // required
            _validator.RuleFor(x => x).NotNull().WithName(_name);
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Registers the provided validation rule in the rule set to be validated later.
        /// </summary>
        /// <param name="rule">Rule to add to the rule set.</param>
        /// <returns>Rule set to be further manipulated fluently.</returns>
        public TextValidationRuleSet RegisterRule(ITextValidationRule rule)
        {
            _rules.Add(rule);

            _validator
                .RuleFor(x => x)
                .Must(rule.Validate)
                .WithMessage(rule.ErrorMessage)
                .WithName(_name);

            return this;
        }


        /// <summary>
        /// Validates the given text against the rules that are registered in the rule set.
        /// </summary>
        /// <param name="text">Text to validate.</param>
        /// <param name="messages">Message list to add any validation errors to.</param>
        /// <returns>Indication if all of the rules in the rule set passed validation.</returns>
        public bool Validate(string text, IValidationStatusMessageList messages = null)
        {
            ResetRuleValidity();

            var validationResults = _validator.Validate(text);

            if (messages != null)
                messages.Add((IEnumerable<IValidationStatusMessage>) validationResults.AsValidationStatusMessages());

            return validationResults.IsValid;
        }


        /// <summary>
        /// Resets all rules' validity to null (not checked) state.
        /// </summary>
        public void ResetRuleValidity()
        {
            _rules.ForEach(x => x.IsValid = null);
        }

        #endregion
    }
}