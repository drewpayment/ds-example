using System;
using Dominion.Domain.Entities.Benefit;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Validation;
using FluentValidation;

namespace Dominion.Domain.Validation.Employee
{
    /// <summary>
    /// Employee Accrual Info entity validation rules.
    /// </summary>
    public class EmployeeAccrualValidator : EntityValidator<EmployeeAccrual>
    {
        public const int MINIMUM_CLIENT_EMPLOYMENT_CLASS_ID = 0;

        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public EmployeeAccrualValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
        }


        /// <summary>
        /// Defines the rules for an EmployeeAccrual entity. 
        /// </summary>
        protected override void Rules()
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