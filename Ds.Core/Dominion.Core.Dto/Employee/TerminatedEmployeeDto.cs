using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public class TerminatedEmployeeDto : EmployeeHireDto
    {
        public EmployeeStatusType EmployeeStatusId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string CostCenter { get; set; }
        public string TerminationReason { get; set; }
        public bool Active { get; set; }
        public DateTime? Modified { get; set; }
    }
}
