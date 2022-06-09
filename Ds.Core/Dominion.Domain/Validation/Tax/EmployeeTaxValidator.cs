using Dominion.Domain.Entities.Tax;
using Dominion.Utility.Validation;
using FluentValidation;

namespace Dominion.Domain.Validation.Tax
{
    /// <summary>
    /// Employee entity validation rules.
    /// </summary>
    public class EmployeeTaxValidator : EntityValidator<EmployeeTax>
    {
        private const string GENDER_VALIDATION_REGEX = "^(M|F)$";


        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="userRepo">The employee repository to be used for validation logic that must query the db.</param>
        /// <param name="reason">The reason that validation is being performed (ie: insert, update).</param>
        public EmployeeTaxValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {
        }

        /// <summary>
        /// Validation rule definitions for the entity.
        /// </summary>
        protected override void Rules()
        {
            // required properties
            RuleFor(x => x.EmployeeTaxId)
                .GreaterThan(0)
                .When(x => Reason == ValidationReason.Update)
                .WithRequiredContext();
            RuleFor(x => x.ClientTaxId).GreaterThan(0).When(x => x.ClientTaxId != null).NotEmpty().WithRequiredContext();
            RuleFor(x => x.NumberOfExemptions).NotNull().WithRequiredContext();


// TODO: Get Byte to work with GreaterThanZero
            RuleFor(x => x.NumberOfDependents).NotNull().WithRequiredContext();


// TODO: Get Byte to work with GreaterThanZero
            RuleFor(x => x.AdditionalTaxAmountTypeId).NotNull().WithRequiredContext();


// TODO: Get Byte to work with GreaterThanZero
            RuleFor(x => x.AdditionalTaxPercent)
                .NotNull()
                .WithRequiredContext()
                .GreaterThanOrEqualTo(0)
                .WithOutOfRangeContext();
            RuleFor(x => x.AdditionalTaxAmount)
                .NotNull()
                .WithRequiredContext()
                .GreaterThanOrEqualTo(0)
                .WithOutOfRangeContext();
            RuleFor(x => x.IsResident).NotNull().WithRequiredContext();
            RuleFor(x => x.IsActive).NotNull().WithRequiredContext();
            RuleFor(x => x.ResidentId).GreaterThan(0).When(x => x.ResidentId != null).WithRequiredContext();


            // Navigational Properties     - 
            RuleFor(x => x.FilingStatus).Enum().WithRequiredContext();
            RuleFor(x => x.ResidentId).GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID).WithRequiredContext();
            RuleFor(x => x.ClientId).GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID).WithRequiredContext();
            RuleFor(x => x.EmployeeId).GreaterThanOrEqualTo(MINIMUM_NAVIGATIONAL_ID).WithRequiredContext();
        }

        // Rules()
    } // class UserValidator
}