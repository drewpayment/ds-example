using Dominion.Core.Dto.Labor;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.Utility.Constants;
using Dominion.Utility.Validation.FluentValidate;
using FluentValidation;

namespace Dominion.LaborManagement.Service.Internal.Validation
{
    public class ClockClientNoteValidator : FluentValidator<IHasClockClientNoteValidation>
    {
        public ClockClientNoteValidator()
        {
            RuleFor(x => x.Note).NotEmpty();
            RuleFor(x => x.ClientId).GreaterThan(CommonConstants.NEW_ENTITY_ID);
        }
    }
}
