using Dominion.Domain.Entities.Tax;
using Dominion.Utility.Validation;
using Dominion.Utility.Validation.FluentValidate;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Validation.Tax
{
    public class EmployeeTaxEntityValidator : FluentValidator<EmployeeTax>
    {
        public const int MINIMUM_NAVIGATIONAL_ID = 1;

        public EmployeeTaxEntityValidator(ValidationReason reason = ValidationReason.Default): base(reason)
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
    }
}
