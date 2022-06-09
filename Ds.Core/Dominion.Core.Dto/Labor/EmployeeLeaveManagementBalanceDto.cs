using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class EmployeeLeaveManagementBalanceDto
    {
        public DateTime LastPeriodStart { get; set; }
        public DateTime LastPeriodEnd { get; set; }
        public DateTime LastCheckDate { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
