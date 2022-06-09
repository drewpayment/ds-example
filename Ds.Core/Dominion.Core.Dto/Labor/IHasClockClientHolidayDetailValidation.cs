using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public interface IHasClockClientHolidayDetailValidation
    {
        int ClockClientHolidayDetailId { get; set; }
        int ClockClientHolidayId { get; set; }
        string ClientHolidayName { get; set; }
        bool IsPaid { get; set; }
        DateTime EventDate { get; set; }
        double? OverrideHours { get; set; }
        int? OverrideClientEarningId { get; set; }
        int? OverrideHolidayWorkedClientEarningId { get; set; }
    }
}
