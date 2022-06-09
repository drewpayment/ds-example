using System;
using System.Linq;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.Utility.Containers;
using Dominion.Utility.Query;
using Dominion.Utility.Validation;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;

namespace Dominion.Domain.Validation.User
{
    /// <summary>
    /// User entity validation rules.
    /// </summary>
    public class UserValidator : EntityValidator<Entities.User.User>
    {
        private IUserRepository _userRepo;

        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="userRepo">The user repository to be used for validation logic that must query the db.</param>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public UserValidator(IUserRepository userRepo, ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
            _userRepo = userRepo;
        }

        /// <summary>
        /// Validation rule definitions for the entity.
        /// </summary>
        protected override void Rules()
        {
            // required properties
            RuleFor(x => x.UserId)
                .GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID)
                .When(x => Reason == ValidationReason.Update)
                .WithRequiredContext();
            RuleFor(x => (int) x.UserTypeId).GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID).WithRequiredContext();
            RuleFor(x => x.FirstName).NotNull().WithRequiredContext();
            RuleFor(x => x.LastName).NotNull().WithRequiredContext();
            RuleFor(x => x.UserName).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            RuleFor(x => x.PasswordHash).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            RuleFor(x => x.SecretQuestionAnswer).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();

            // last login date
            DateTime minDate = MIN_REALISTIC_DATE;
            DateTime maxDate = DateTime.Now.AddDays(1.0);

            RuleFor(x => x.Session.LastLogin)
                .InclusiveBetween(minDate, maxDate)
                .When(x => x.Session.LastLogin != null)
                .WithOutOfRangeContext();

            // TempEnableFromDate & TempEnableToDate
            minDate = new DateTime(2000, 1, 1); // this date was selected somewhat arbitrarily
            maxDate = Entities.User.User.MAX_TEMP_ENABLE_DATE;

            // check date range
            RuleFor(x => x.TempEnableFromDate)
                .InclusiveBetween(minDate, maxDate)
                .When(x => x.TempEnableFromDate != null)
                .WithOutOfRangeContext();
            RuleFor(x => x.TempEnableToDate)
                .InclusiveBetween(minDate, maxDate)
                .When(x => x.TempEnableToDate != null)
                .WithOutOfRangeContext();

            // check that fromDate <= toDate
            var additionalProperties = new PropertyList<Entities.User.User> {u => u.TempEnableToDate};
            RuleFor(x => x.TempEnableFromDate)
                .LessThanOrEqualTo(x => x.TempEnableToDate)
                .When(x => x.TempEnableFromDate != null && x.TempEnableToDate != null)
                .WithIncompatibleContext(additionalProperties.GetPropertyNames());
        }

        // Rules()


        /// <summary>
        /// Override of FluentValidation's Validate() method in order to generate rules that must be created at the
        /// time Validate() is called, not when this object is instantiated. For example, rules that require info
        /// from the database.
        /// </summary>
        /// <remarks>
        /// It appears that this Validate() overload is always called, regarless of which overload is originally called.
        /// </remarks>
        /// <param name="context">The fluent validation context.</param>
        /// <returns>Validation results.</returns>
        public override ValidationResult Validate(ValidationContext<Entities.User.User> context)
        {
            Entities.User.User user = context.InstanceToValidate;

            // if UserName is being validated, ensure that it isn't already in use.
            if (user.ShouldValidateProperty(x => x.UserName) && !string.IsNullOrWhiteSpace(user.UserName))
            {
                // if there is an existing user record with the same user name and the record does not represent 
                // the target user, create a rule that will generate the appropriate error.
                var qb = new QueryBuilder<Entities.User.User>().FilterBy(u => u.UserName == user.UserName);
                if (user.UserId != 0)
                    qb.FilterBy(u => u.UserId != user.UserId);

                var existingUser = _userRepo.GetUsers(qb).SingleOrDefault();
                if (existingUser != null)
                {
                    this.RemoveRule(x => x.UserName, typeof (NotEqualValidator));
                    RuleFor(x => x.UserName)
                        .NotEqual(user.UserName)
                        .WithIncompatibleContext()
                        .WithMessage(string.Format("The user name '{0}' already exists.", user.UserName));
                }
            }

            return base.Validate(context);
        }

        // Validate()
    } // class UserValidator
}