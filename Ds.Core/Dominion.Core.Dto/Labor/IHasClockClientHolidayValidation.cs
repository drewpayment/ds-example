using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public interface IHasClockClientHolidayValidation
    {
        int ClockClientHolidayId { get; set; }
        int ClientId { get; set; }
        string Name { get; set; }
        int WaitingPeriod { get; set; }
        int HolidayWaitingPeriodDateId { get; set; }
        IEnumerable<ClockClientHolidayDetailDto> ClientHolidayDetails { get; set; }
    }
}