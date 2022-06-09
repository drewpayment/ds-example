using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class GetReportClockEmployeeOnSiteDto
    {
        public int? EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Punch1 { get; set; }
        public string Punch2 { get; set; }
        public string Punch3 { get; set; }
        public string Punch4 { get; set; }
        public string Filter { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public string LastPunch { get; set; }
        public int? ClientDepartmentID { get; set; }
        public string Department { get; set; }
    }
}
