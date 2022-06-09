using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Scheduling
{
    public class ClockEmployeeOvertimeInformationDto
    {
        public int employeeId { get; set; }
        public string employeeName { get; set; }
        public string employeeSupervisor { get; set; }
        public string employeeDepartment { get; set; }
        public int overtimeFrequencyId { get; set; }
        public double? overtimeHours { get; set; }
        public double? hoursWorked { get; set; }
        public double? hoursScheduled { get; set; }
        public int clientEarningID { get; set; }
        public double? Hours { get; set; }
        public int clientEarningCategoryID { get; set; }
        public DateTime? clockedInPunch { get; set; }
        public DateTime? clockedOutPunch { get; set; }
    }
}