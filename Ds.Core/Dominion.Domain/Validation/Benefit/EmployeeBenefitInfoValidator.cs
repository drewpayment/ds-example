using System;
using Dominion.Domain.Entities.Benefit;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Validation;
using FluentValidation;

namespace Dominion.Domain.Validation.Benefit
{
    /// <summary>
    /// Employee Benefit Info entity validation rules.
    /// </summary>
    public class EmployeeBenefitInfoValidator : EntityValidator<EmployeeBenefitInfo>
    {
        public const int MINIMUM_CLIENT_EMPLOYMENT_CLASS_ID = 0;

        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public EmployeeBenefitInfoValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
        }


        /// <summary>
        /// Defines the rules for an EmployeeDependent entity.
        /// </summary>
        protected override void Rules()
        {
            DateTime minDate = MIN_REALISTIC_DATE;
            DateTime maxDate = DateTime.Now.AddDays(1.0);

            // Keys
            RuleFor(x => x.EmployeeId)
                .GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID)
                .When(x => Reason == ValidationReason.Update)
                .WithRequiredContext();


            RuleFor(x => x.EligibilityDate)
                .InclusiveBetween(MIN_REALISTIC_DATE, MAX_REALISTIC_DATE)
                .When(x => x.EligibilityDate != null)
                .WithOutOfRangeContext();


            RuleFor(x => x.BenefitPackageId).GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID).When(x => x.BenefitPackageId != null);

            RuleFor(x => x.ClientEmploymentClassId).GreaterThanOrEqualTo(MINIMUM_CLIENT_EMPLOYMENT_CLASS_ID).When(x => x.ClientEmploymentClassId != null);

            RuleFor(x => x.Modified).NotNull().WithRequiredContext();

            RuleFor(x => x.Modified)
                .InclusiveBetween(minDate, maxDate)
                .When(x => x.Modified != null)
                .WithOutOfRangeContext();
        }

    } 
}