using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Scheduling
{
    public class ClockEmployeeOvertimeDto
    {
        public int employeeId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string supervisor { get; set; }
        public string department { get; set; }
        public bool hasSchedule { get; set; }
        public double TotalHoursScheduled { get; set; }
        public double TotalHoursWorked { get; set; }
        public double overtimeHours { get; set; }
    }
}
