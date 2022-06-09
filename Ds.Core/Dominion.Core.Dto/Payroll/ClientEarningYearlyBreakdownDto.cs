using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public partial class ClientEarningYearlyBreakdownDto
    {
        public string Description { get; set; }
        public int ClientEarningId { get; set; }
        public double TotalHours { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<ClientEarningMonthlyBreakdownDto> YearlyEarningMonthlyBreakdown { get; set; }
    }
}
