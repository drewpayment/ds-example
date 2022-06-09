using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.Utility.Constants;
using Dominion.Utility.Validation.FluentValidate;
using FluentValidation;

namespace Dominion.LaborManagement.Service.Internal.Validation
{
    public class ClockClientExceptionDetailValidator : FluentValidator<IHasClockClientExceptionDetailValidation>
    {
        public ClockClientExceptionDetailValidator()
        {
            
        }
    }
}
