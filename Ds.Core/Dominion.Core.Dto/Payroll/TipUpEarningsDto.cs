using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class TipUpEarningsDto
    {
        public int     PreviewPaycheckPayDataId { get; set; }
        public int     EmployeeId               { get; set; }
        public int     Week                     { get; set; }
        public decimal Tips                     { get; set; }
        public decimal StraightHours            { get; set; }
        public decimal StraightPay              { get; set; }
        public decimal CustomEarningsAmt        { get; set; }
    }
}
