using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class BankDepositCountInfoDto
    {
        public int VendorDirectDepositCount   { get; set; }
        public int VendorCheckCount           { get; set; }
        public int EmployeeDirectDepositCount { get; set; }
        public int EmployeeCheckCount         { get; set; }
    }
}
