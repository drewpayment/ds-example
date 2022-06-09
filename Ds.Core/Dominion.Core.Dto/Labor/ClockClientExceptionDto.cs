using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Labor
{
    public class ClockClientExceptionDto : IHasClockClientExceptionValidation
    {
        public int ClockClientExceptionId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public List<ClockClientExceptionDetailDto> ExceptionDetails { get; set; }

    }
}
