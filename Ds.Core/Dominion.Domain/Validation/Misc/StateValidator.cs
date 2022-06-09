using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Validation;
using FluentValidation;

namespace Dominion.Domain.Validation.Misc
{
    /// <summary>
    /// SecretQuestion entity validation rules.
    /// </summary>
    public class StateValidator : EntityValidator<State>
    {
        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public StateValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
        }

        /// <summary>
        /// Validation rule definitions for the entity.
        /// </summary>
        protected override void Rules()
        {
            // required properties
            RuleFor(x => x.CountryId)
                .GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID)
                .WithState(_requiredContext);

            RuleFor(x => x.Name).NotEmpty().WithState(_requiredContext);
            RuleFor(x => x.Code).NotEmpty().WithState(_requiredContext);
            RuleFor(x => x.Abbreviation).NotEmpty().WithState(_requiredContext);
        }
    }
}