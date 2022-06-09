using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class EmployeeLeaveManagementLastPayrollDto
    {
        public DateTime LastPayrollDate { get; set; }
        public DateTime YearBefore { get; set; }
    }
}
