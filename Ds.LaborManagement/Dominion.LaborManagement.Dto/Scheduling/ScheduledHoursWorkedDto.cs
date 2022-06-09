using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Scheduling
{
    public class ScheduledHoursWorkedDto
    {
        public DateTime Date { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? StartTime2 { get; set; }
        public DateTime? EndTime2 { get; set; }
        public DateTime? StartTime3 { get; set; }
        public DateTime? EndTime3 { get; set; }
        public double ScheduledHours { get; set; }
        public double HoursWorked { get; set; }

        // metadata for ess mobile app
        public bool IsSystemDefault { get; set; }
    }
}
