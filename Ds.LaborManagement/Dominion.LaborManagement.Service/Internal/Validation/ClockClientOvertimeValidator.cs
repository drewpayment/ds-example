using Dominion.Core.Dto.Labor;
using Dominion.Utility.Validation.FluentValidate;
using FluentValidation;

namespace Dominion.LaborManagement.Service.Internal.Validation
{
    public class ClockClientOvertimeValidator : FluentValidator<IHasClockClientOvertimeValidation>
    {
        public ClockClientOvertimeValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.ClientEarningId).NotEmpty();
            RuleFor(x => x.ClockOvertimeFrequencyId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Hours).NotEmpty();
        }
    }
}
