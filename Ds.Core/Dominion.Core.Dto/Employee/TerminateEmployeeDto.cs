using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public class TerminateEmployeeDto
    {
        public int EmployeeId { get; set; }
        public DateTime? SeparationDate { get; set; }
        public EmployeeStatusType? EmployeeStatusId { get; set; }
    }
}
