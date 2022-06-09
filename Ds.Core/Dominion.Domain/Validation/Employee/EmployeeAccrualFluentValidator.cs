using System;
using Dominion.Domain.Entities.Benefit;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Validation;
using Dominion.Utility.Validation.FluentValidate;
using FluentValidation;

namespace Dominion.Domain.Validation.Employee
{
    /// <summary>
    /// EmployeeAccrual entity validation rules.
    /// </summary>
    public class EmployeeAccrualFluentValidator : FluentValidator<EmployeeAccrual>
    {
        public const int NEW_ENTITY_ID = 0;
        public const int MINIMUM_NAVIGATIONAL_ID = 1;
        public const int MINIMUM_CLIENT_EMPLOYMENT_CLASS_ID = 0;
        public DateTime MIN_REALISTIC_DATE = new DateTime(1885, 9, 2);
        public DateTime MAX_REALISTIC_DATE = new DateTime(2999, 12, 31);

        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public EmployeeAccrualFluentValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
            DateTime minDate = MIN_REALISTIC_DATE;
            DateTime maxDate = DateTime.Now.AddDays(1.0);

            // Keys
            RuleFor(x => x.EmployeeAccrualId)
                .GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID)
                .When(x => Reason == ValidationReason.Update)
                .WithRequiredContext();

            RuleFor(x => x.EmployeeAccrualId)
                .Equal(0)
                .When(x => Reason == ValidationReason.Insert)
                .WithRequiredContext();

            // Navigational Properties
            RuleFor(x => x.EmployeeId)
                .GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID)
                .When(x => Reason == ValidationReason.Update)
                .WithRequiredContext();

            RuleFor(x => x.ClientAccrualId)
                .GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID)
                .When(x => Reason == ValidationReason.Update)
                .WithRequiredContext();

            //
            RuleFor(x => x.Modified).NotNull().WithRequiredContext();

            RuleFor(x => x.Modified)
                .InclusiveBetween(minDate, maxDate)
                .When(x => x.Modified != null)
                .WithOutOfRangeContext();

        }
    }
}