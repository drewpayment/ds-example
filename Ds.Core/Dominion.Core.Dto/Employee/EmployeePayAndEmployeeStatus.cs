using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeePayAndEmployeeStatus
    {
        public PayFrequencyType PayFrequencyId { get; set; }
        public EmployeeStatusType EmployeeStatusId { get; set; }
        public string EmployeeStatus { get; set; }
        public DateTime? Modified { get; set; }
        public int EmployeeId { get; set; }
        public bool Active { get; set; }
        public string TerminationReason { get; set; }
    }
}
