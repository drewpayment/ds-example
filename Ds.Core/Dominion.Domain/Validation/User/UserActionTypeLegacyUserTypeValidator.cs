using Dominion.Domain.Entities.User;
using Dominion.Utility.Validation;
using FluentValidation;

namespace Dominion.Domain.Validation.User
{
    /// <summary>
    /// UserActionTypeLegacyUserType entity validation rules.
    /// </summary>
    public class UserActionTypeLegacyUserTypeValidator : EntityValidator<UserActionTypeLegacyUserType>
    {
        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="userRepo">The user repository to be used for validation logic that must query the db.</param>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public UserActionTypeLegacyUserTypeValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
        }

        /// <summary>
        /// Validation rule definitions for the entity.
        /// </summary>
        protected override void Rules()
        {
            // required properties
            RuleFor(x => x.UserActionTypeLegacyUserTypeId)
                .GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID)
                .When(x => Reason == ValidationReason.Update)
                .WithRequiredContext();
            RuleFor(x => x.UserActionTypeId).GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID).WithRequiredContext();
        }

        // Rules()
    } // class UserActionTypeLegacyUserTypeValidator
}