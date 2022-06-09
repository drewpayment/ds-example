using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public partial class PayrollControlTotalDto
    {
        public int PayrollControlTotalId { get; set; }
        public int PayrollId { get; set; }
        public int ClientEarningId { get; set; }
        public double Hours { get; set; }
        public double Amount { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public ClientEarningDto ClientEarning { get; set; }
    }
}
