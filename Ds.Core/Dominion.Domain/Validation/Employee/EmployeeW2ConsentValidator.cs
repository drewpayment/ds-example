using System;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Validation;
using FluentValidation;
using Dominion.Domain.Entities.Onboarding;

namespace Dominion.Domain.Validation.Employee
{
    public class EmployeeW2ConsentValidator : EntityValidator<EmployeeW2Consent>
    {
        public EmployeeW2ConsentValidator(ValidationReason reason = ValidationReason.Default)
            : base(reason)
        {

        }



        protected override void Rules()
        {
            
        }
    }
}
