using Dominion.Domain.Entities.Tax;
using Dominion.Utility.Validation;
using FluentValidation;

namespace Dominion.Domain.Validation.Misc
{
    /// <summary>
    /// Employee entity validation rules.
    /// </summary>
    public class FilingStatusValidator : EntityValidator<FilingStatusInfo>
    {
        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="userRepo">The employee repository to be used for validation logic that must query the db.</param>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public FilingStatusValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
        }

        /// <summary>
        /// Validation rule definitions for the entity.
        /// </summary>
        protected override void Rules()
        {
            // required properties
            RuleFor(x => x.FilingStatus).Enum().When(x => Reason == ValidationReason.Update).WithRequiredContext();
            RuleFor(x => x.Description).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
        }

        // Rules()
    } // class UserValidator
}