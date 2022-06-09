using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class CheckDto
    {
        public int PayrollID { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public int genPaycheckHistoryID { get; set; }
        public decimal CheckAmount { get; set; }
        public int? CheckNumber { get; set; }
        public decimal GrossPay { get; set; }
        public int genPayrollHistoryID { get; set; }
        public decimal NetPay { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime CheckDate { get; set; }
        public string SubCheck { get; set; }

    }
}
