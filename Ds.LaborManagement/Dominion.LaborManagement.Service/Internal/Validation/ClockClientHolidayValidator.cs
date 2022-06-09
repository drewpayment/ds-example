using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;
using Dominion.Utility.Constants;
using Dominion.Utility.Validation.FluentValidate;
using FluentValidation;

namespace Dominion.LaborManagement.Service.Internal.Validation
{
    public class ClockClientHolidayValidator : FluentValidator<IHasClockClientHolidayValidation>
    {
        public ClockClientHolidayValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.WaitingPeriod).NotNull();
            RuleFor(x => x.ClientId).GreaterThan(CommonConstants.NEW_ENTITY_ID);
            RuleFor(x => x.HolidayWaitingPeriodDateId).NotNull();
        }
    }
}
