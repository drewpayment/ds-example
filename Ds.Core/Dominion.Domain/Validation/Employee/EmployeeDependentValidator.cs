using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Validation;
using FluentValidation;
using FluentValidation.Results;

namespace Dominion.Domain.Validation.Employee
{
    /// <summary>
    /// Standard Validation Rules for an EmployeeDependent entity.
    /// </summary>
    public class EmployeeDependentValidator : EntityValidator<EmployeeDependent>
    {
        // refactor: jay: this needs to be a public constant somewhere
        public const int NEW_ENTITY_ID = 0;

        // refactor: jay: this needs to be a public constant somewhere
        private const string GENDER_VALIDATION_REGEX = "^(M|F)$";


        /// <summary>
        /// Instantiates a new EmployeeDependentValidator.
        /// </summary>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public EmployeeDependentValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
        }


        /// <summary>
        /// Defines the rules for an EmployeeDependent entity.
        /// </summary>
        protected override void Rules()
        {
            // Keys
            RuleFor(x => x.EmployeeDependentId)
                .GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID)
                .When(x => Reason == ValidationReason.Update)
                .WithRequiredContext();

            RuleFor(x => x.EmployeeDependentId)
                .Equal(0)
                .When(x => Reason == ValidationReason.Insert)
                .WithRequiredContext();

            // Required Properties
            RuleFor(x => x.FirstName).NotEmpty().WithRequiredContext().NotNull().WithRequiredContext();
            RuleFor(x => x.LastName).NotEmpty().WithRequiredContext().NotNull().WithRequiredContext();
            RuleFor(x => x.Relationship).NotEmpty().WithRequiredContext().NotNull().WithRequiredContext();
            RuleFor(x => x.Gender).NotEmpty().WithRequiredContext().NotNull().WithRequiredContext();
            RuleFor(x => x.Modified).NotEmpty().WithRequiredContext().NotNull().WithRequiredContext();
            RuleFor(x => x.SocialSecurityNumber).NotNull().WithRequiredContext();
            RuleFor(x => x.Comments).NotNull().WithRequiredContext();
            RuleFor(x => x.BirthDate).NotEmpty().WithRequiredContext();

            // Social Security Regex
            RuleFor(x => x.SocialSecurityNumber)
                .SocialSecurityNumber()
                .WithInvalidFormatContext();

            // Gender Regex
            RuleFor(x => x.Gender)
                .Matches(GENDER_VALIDATION_REGEX)
                .WithInvalidFormatContext();

            // Navigational Properties
            RuleFor(x => x.EmployeeId).GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID).WithRequiredContext();
            RuleFor(x => x.ClientId).GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID).WithRequiredContext();
        }

        // Rules()

        public override ValidationResult Validate(ValidationContext<EmployeeDependent> context)
        {
            return base.Validate(context);
        }

        // Validate()
    }
}