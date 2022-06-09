using Dominion.Domain.Entities.User;
using Dominion.Utility.Validation;
using FluentValidation;

namespace Dominion.Domain.Validation.User
{
    /// <summary>
    /// UserType entity validation rules.
    /// </summary>
    public class UserTypeValidator : EntityValidator<UserTypeInfo>
    {
        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public UserTypeValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
        }

        /// <summary>
        /// Validation rule definitions for the entity.
        /// </summary>
        protected override void Rules()
        {
            // required properties
            RuleFor(x => x.Label).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();

            // required value range
            RuleFor(x => x.UserTypeId)
                .Enum()
                .WithOutOfRangeContext();
        }

        // Rules()
    } // class UserTypeValidator
}