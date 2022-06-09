using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class ScheduleListByDate
    {
        public class Day
        {
            public int EmployeeID { get; set; }
            public DateTime? StartTime { get; set; }
            public DateTime? StopTime { get; set; }
            public DateTime EventDate { get; set; }

            public int ClockEmployeeScheduleID { get; set; }

            public DateTime? StartTime_Schedule2 { get; set; }
            public DateTime? StopTime_Schedule2 { get; set; }
            public DateTime? StartTime_Schedule3 { get; set; }
            public DateTime? StopTime_Schedule3 { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public string EmployeeNumber { get; set; }

            public string MiddleInitial { get; set; }

            public int? ClockClientSchedule_ChangeHistory_Change_ID { get; set; }
        }

        public IEnumerable<Day> Days { get; set; }
    }
}
