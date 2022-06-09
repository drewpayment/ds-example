using System;
using Dominion.Utility.Validation;
using FluentValidation;

namespace Dominion.Domain.Validation.Employee
{
    /// <summary>
    /// Employee entity validation rules.
    /// </summary>
    public class EmployeeValidator : EntityValidator<Entities.Employee.Employee>
    {
        private const string GENDER_VALIDATION_REGEX = "^(M|F)$";
        private DateTime maxBirthDate = DateTime.Today;

        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="userRepo">The employee repository to be used for validation logic that must query the db.</param>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public EmployeeValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
        }

        /// <summary>
        /// Validation rule definitions for the entity.
        /// </summary>
        protected override void Rules()
        {
            // required properties
            RuleFor(x => x.EmployeeId).GreaterThan(0).When(x => Reason == ValidationReason.Update).WithRequiredContext();
            RuleFor(x => x.FirstName).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            RuleFor(x => x.LastName).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            RuleFor(x => x.AddressLine1).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            RuleFor(x => x.City).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            RuleFor(x => x.PostalCode).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            RuleFor(x => x.HomePhoneNumber).NotNull().WithRequiredContext();
            RuleFor(x => x.EmployeeNumber).NotNull().WithRequiredContext();
            RuleFor(x => x.CostCenterType).NotNull().WithRequiredContext();

            // Date Validations
            RuleFor(x => x.BirthDate)
                .InclusiveBetween(MIN_REALISTIC_DATE, maxBirthDate) // note: maxBirthDate
                .When(x => x.BirthDate != null)
                .WithOutOfRangeContext();

            RuleFor(x => x.HireDate)
                .InclusiveBetween(MIN_REALISTIC_DATE, MAX_REALISTIC_DATE)
                .When(x => x.HireDate != null)
                .WithOutOfRangeContext();

            RuleFor(x => x.SeparationDate)
                .InclusiveBetween(MIN_REALISTIC_DATE, MAX_REALISTIC_DATE)
                .When(x => x.SeparationDate != null)
                .WithOutOfRangeContext();

            RuleFor(x => x.AnniversaryDate)
                .InclusiveBetween(MIN_REALISTIC_DATE, MAX_REALISTIC_DATE)
                .When(x => x.AnniversaryDate != null)
                .WithOutOfRangeContext();

            RuleFor(x => x.RehireDate)
                .InclusiveBetween(MIN_REALISTIC_DATE, MAX_REALISTIC_DATE)
                .When(x => x.RehireDate != null)
                .WithOutOfRangeContext();

            RuleFor(x => x.EligibilityDate)
                .InclusiveBetween(MIN_REALISTIC_DATE, MAX_REALISTIC_DATE)
                .When(x => x.EligibilityDate != null)
                .WithOutOfRangeContext();

            // Social Security Regex
            RuleFor(x => x.SocialSecurityNumber)
                .SocialSecurityNumber()
                .When(x => !string.IsNullOrEmpty(x.SocialSecurityNumber))
                .WithInvalidFormatContext();

            // Gender Regex
            RuleFor(x => x.Gender)
                .Matches(GENDER_VALIDATION_REGEX)
                .WithInvalidFormatContext();

            RuleFor(x => x.ClientGroupId)
                .GreaterThanOrEqualTo(0)
                .When(x => x.ClientGroupId != null)
                .WithRequiredContext();
            RuleFor(x => x.ClientDivisionId)
                .GreaterThanOrEqualTo(0)
                .When(x => x.ClientDivisionId != null)
                .WithRequiredContext();
            RuleFor(x => x.ClientDepartmentId)
                .GreaterThanOrEqualTo(0)
                .When(x => x.ClientDepartmentId != null)
                .WithRequiredContext();
            RuleFor(x => x.ClientCostCenterId)
                .GreaterThanOrEqualTo(0)
                .When(x => x.ClientCostCenterId != null)
                .WithRequiredContext();
            RuleFor(x => x.ClientWorkersCompId)
                .GreaterThanOrEqualTo(0)
                .When(x => x.ClientWorkersCompId != null)
                .WithRequiredContext();
            RuleFor(x => x.MaritalStatusId).Must(x => x > 0).When(x => x.MaritalStatusId != null).WithRequiredContext();

            RuleFor(x => x.EeocRaceId).GreaterThanOrEqualTo(0).When(x => x.EeocRaceId != null).WithRequiredContext();
            RuleFor(x => x.EeocJobCategoryId)
                .GreaterThanOrEqualTo(0)
                .When(x => x.EeocJobCategoryId != null)
                .WithRequiredContext();
            RuleFor(x => x.EeocLocationId)
                .GreaterThanOrEqualTo(0)
                .When(x => x.EeocLocationId != null)
                .WithRequiredContext();
            RuleFor(x => x.JobProfileId)
                .GreaterThanOrEqualTo(0)
                .When(x => x.JobProfileId != null)
                .WithRequiredContext();

            // Navigational Properties
            RuleFor(x => x.StateId).GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID).WithRequiredContext();
            RuleFor(x => x.CountryId).GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID).WithRequiredContext();
            RuleFor(x => x.ClientId).GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID).WithRequiredContext();
        }

        // Rules()
    } // class UserValidator
}