using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;
using Dominion.Utility.Validation.FluentValidate;
using FluentValidation;

namespace Dominion.LaborManagement.Service.Internal.Validation
{
    public class ClockClientHolidayDetailValidator : FluentValidator<IHasClockClientHolidayDetailValidation>
    {
        public ClockClientHolidayDetailValidator()
        {
            RuleFor(x => x.ClientHolidayName).NotEmpty();
            RuleFor(x => x.EventDate).NotEmpty();
        }
    }
}
