using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Validation;
using FluentValidation;

namespace Dominion.Domain.Validation.Misc
{
    /// <summary>
    /// SecretQuestion entity validation rules.
    /// </summary>
    public class FeatureOptionValidator : EntityValidator<AccountFeatureInfo>
    {
        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public FeatureOptionValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
        }

        /// <summary>
        /// Validation rule definitions for the entity.
        /// </summary>
        protected override void Rules()
        {
            // required properties
            RuleFor(x => x.Description).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();

            // required value range
            RuleFor(x => x.AccountFeatureId)
                .Enum()
                .WithOutOfRangeContext();
        }

        // Rules()
    } // class FeatureOptionValidator
}