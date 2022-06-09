using System;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Validation;
using Dominion.Utility.Validation.FluentValidate;
using FluentValidation;

namespace Dominion.Domain.Validation.Employee
{
    /// <summary>
    /// EmployeeEmergencyContact entity validation rules.
    /// </summary>
    public class EmployeeEmergencyContactFluentValidator : FluentValidator<EmployeeEmergencyContact>
    {
        public const int NEW_ENTITY_ID = 0;
        public const int MINIMUM_NAVIGATIONAL_ID = 1;
        public DateTime MIN_REALISTIC_DATE = new DateTime(1885, 9, 2);

        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public EmployeeEmergencyContactFluentValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
            // Keys
            RuleFor(x => x.EmployeeEmergencyContactId)
                .GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID)
                .When(x => Reason == ValidationReason.Update)
                .WithRequiredContext();

            // Required Properties
            RuleFor(x => x.EmployeeId)
                .NotNull()
                .WithRequiredContext()
                .GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID)
                .WithRequiredContext();
            RuleFor(x => x.FirstName).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            RuleFor(x => x.LastName).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            //RuleFor(x => x.AddressLine1).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            //RuleFor(x => x.City).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            // RuleFor(x => x.HomePhoneNumber).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            RuleFor(x => x.Relation).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
            //RuleFor(x => x.ClientId)
            //    .NotNull()
            //    .WithRequiredContext()
            //    .GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID)
            //    .WithRequiredContext();
            RuleFor(x => x.Modified).NotNull().WithRequiredContext();

            // last modified date
            DateTime minDate = MIN_REALISTIC_DATE;
            DateTime maxDate = DateTime.Now.AddDays(1.0);

            RuleFor(x => x.Modified)
                .InclusiveBetween(minDate, maxDate)
                .When(x => x.Modified != null)
                .WithOutOfRangeContext();

        }
    }
}