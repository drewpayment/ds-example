using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class ClientEarningMonthlyBreakdownDto
    {
        public decimal TotalAmount { get; set; }
        public double  TotalHours  { get; set; }
        public string  Month       { get; set; }
        
    }
}
