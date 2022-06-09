using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class ClockClientRulesStartingDayOfWeekIdsDto : IHasClockClientRulesStartingDayOfWeekIds
    {
        public byte? WeeklyStartingDayOfWeekId { get; set; }
        public byte? BiWeeklyStartingDayOfWeekId { get; set; }
        public byte? SemiMonthlyStartingDayOfWeekId { get; set; }
        public byte? MonthlyStartingDayOfWeekId { get; set; }
    }
}
