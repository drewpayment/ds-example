using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Validation;
using FluentValidation;

namespace Dominion.Domain.Validation.Misc
{
    /// <summary>
    /// SecretQuestion entity validation rules.
    /// </summary>
    public class SecretQuestionValidator : EntityValidator<SecretQuestion>
    {
        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public SecretQuestionValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
        }

        /// <summary>
        /// Validation rule definitions for the entity.
        /// </summary>
        protected override void Rules()
        {
            // required properties
            RuleFor(x => x.SecretQuestionId)
                .GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID)
                .When(x => Reason == ValidationReason.Update)
                .WithRequiredContext();
            RuleFor(x => x.Text).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
        }

        // Rules()
    } // class SecretQuestionValidator
}