using System;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Validation;
using FluentValidation;

namespace Dominion.Domain.Validation.Employee
{
    /// <summary>
    /// EmployeeEmergencyContact entity validation rules.
    /// </summary>
    public class EmployeeEmergencyContactValidator : EntityValidator<EmployeeEmergencyContact>
    {
        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public EmployeeEmergencyContactValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
        }


        /// <summary>
        /// Defines the rules for an EmployeeDependent entity.
        /// </summary>
        protected override void Rules()
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
            //RuleFor(x => x.HomePhoneNumber).NotNull().WithRequiredContext().NotEmpty().WithRequiredContext();
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