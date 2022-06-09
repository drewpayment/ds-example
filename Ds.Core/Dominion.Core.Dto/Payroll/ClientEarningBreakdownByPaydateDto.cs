using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class ClientEarningBreakdownByPaydateDto
    {
        public decimal TotalAmount { get; set; }
        public DateTime PayDate { get; set; }
    }
}
