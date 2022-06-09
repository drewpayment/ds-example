using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class GetClockClientRulesByEmployeeIdResultDto
    {
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public int? WeeklyStartingDayOfWeekId { get; set; }
        public int? ClockAllocateHoursFrequencyId { get; set; }
        public int? PunchOption { get; set; }
        public bool  ApplyHoursOption { get; set; }
        public int? ClockClientExceptionId { get; set; }
        public int NoPunchesOnScheduledDayException { get; set; }
        public int NoPunchesBeforeHoliday { get; set; }
        //public 
    }
}
