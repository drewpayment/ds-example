using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class ScheduleListByEmployeeIDAndDate
    {
        public class Schedule
        {
            public int EmployeeID { get; set; }

            public string Name { get; set; }

            public DateTime? StartTime { get; set; }

            public DateTime? StopTime { get; set; }

            public DateTime StartDate { get; set; }

            public DateTime EndDate { get; set; }

            public bool IsSunday { get; set; }

            public bool IsMonday { get; set; }

            public bool IsTuesday { get; set; }

            public bool IsWednesday { get; set; }

            public bool IsThursday { get; set; }

            public bool IsFriday { get; set; }

            public bool IsSaturday { get; set; }

            public int ClockClientScheduleID { get; set; }
            public string EmployeeFirstName { get; set; }

            public string EmployeeLastName { get; set; }

            public string EmployeeMiddleInitial { get; set; }

            public string EmployeeNumber { get; set; }

            public int RepeatInterval { get; set; } = 1;

            public RecurrenceOptionTypes RecurrenceOption { get; set; } = RecurrenceOptionTypes.Weekly;

            public int? ChangeID { get; set; }
        }
        public List<Schedule> List { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? EmployeeID { get; set; }
    }
}
