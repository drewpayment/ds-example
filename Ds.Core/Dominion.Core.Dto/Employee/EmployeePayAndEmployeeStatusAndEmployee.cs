using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Authentication.Interface.Db;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeePayAndEmployeeStatusAndEmployee : EmployeeHireDto
    {
        public string LastName { get; set; }
        public string Department { get; set; }
        public string CostCenter { get; set; }
        public string EmployeeStatus { get; set; }
        public DateTime? Modified { get; set; }
        public string FullName { get; set; }
        public bool Active { get; set; }
        public DateTime? TerminationDate { get; set; }
    }
}
