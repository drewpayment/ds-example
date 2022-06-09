using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class GetClockEmployeeHoursComparisonDto
    {
        public string EmployeeName { get; set; }
        public string CostCenterCode { get; set; }
        public decimal HoursScheduled { get; set; }
        public double ActualHours { get; set; }
        public string StartSchedule { get; set; }
        public string StopSchedule { get; set; }
        public string StartSchedule1 { get; set; }
        public string StopSchedule1 { get; set; }
        public string StartSchedule2 { get; set; }
        public string StopSchedule2 { get; set; }
        public string ShortDate { get; set; }
        public string DepartmentName { get; set; }
        public string SupervisorName { get; set;  }
        public int EmployeeId { get; set; }
    }
}
