using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;

namespace Dominion.Core.Dto.Labor
{
    public class ClockClientTimePolicySearchLists
    {
        public List<ClockClientOvertimeDto> OvertimeList { get; set; }
        public List<ClockClientLunchDto> LunchBreakList { get; set; }
        public List<ClockClientAddHoursDto> AddHoursList { get; set; }
        public List<ClientShiftDto> ShiftList { get; set; }
    }
}
