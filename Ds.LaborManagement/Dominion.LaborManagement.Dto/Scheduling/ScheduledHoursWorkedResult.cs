using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Scheduling
{
    public class ScheduledHoursWorkedResult
    {
        public IEnumerable<ScheduledHoursWorkedDto> Days { get; set; }
        public bool HasSchedule { get; set; }
        public double TotalHoursScheduled { get; set; }
        public double TotalHoursWorked { get; set; }
    }
}
