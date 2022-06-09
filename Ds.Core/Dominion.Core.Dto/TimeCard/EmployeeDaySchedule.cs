using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class EmployeeDaySchedule
    {
        public int ScheduleID { get; set; }
        public EmployeeDayScheduleRange Schedule1 { get; set; }
        public EmployeeDayScheduleRange Schedule2 { get; set; }
        public EmployeeDayScheduleRange Schedule3 { get; set; }
        public string Name { get; set; }

        public int? ClockClientScheduleChangeHistoryChangeID { get; set; }
    }
}
