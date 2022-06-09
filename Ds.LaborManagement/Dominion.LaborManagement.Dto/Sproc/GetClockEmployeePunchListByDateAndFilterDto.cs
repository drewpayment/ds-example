using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class GetClockEmployeePunchListByDateAndFilterDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime EmployeeHireDate { get; set; }
        public DateTime EmployeeSeparationDate { get; set; }
        public DateTime EmployeeRehireDate { get; set; }
        public int? ClockClientTimePolicyID { get; set; }
        public string ClockClientTimePolicyName { get; set; }
    }
}
