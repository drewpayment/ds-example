using Dominion.Domain.Entities.User;
using Dominion.Utility.Validation;
using FluentValidation;

namespace Dominion.Domain.Validation.User
{
    /// <summary>
    ///  UserGroupMembershipValidator entity validation rules.
    /// </summary>
    public class UserGroupMembershipValidator : EntityValidator<UserGroupMembership>
    {
        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="userRepo">The user repository to be used for validation logic that must query the db.</param>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public UserGroupMembershipValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
        }

        /// <summary>
        /// Validation rule definitions for the entity.
        /// </summary>
        protected override void Rules()
        {
            // required properties
            RuleFor(x => x.UserGroupMembershipId)
                .GreaterThan(0)
                .When(x => Reason == ValidationReason.Update)
                .WithRequiredContext();
            RuleFor(x => x.ClientUserGroupId).GreaterThan(0).WithRequiredContext();
        }

        // Rules()
    } // class UserGroupMembershipValidator
}