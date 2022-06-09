using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public partial class ClientEarningBreakdownInDateRangeDto
    {
        public string Description { get; set; }
        public int ClientEarningId { get; set; }
        public decimal TotalAmount { get; set; }
        public IEnumerable<DateTime> PayDateList { get; set; }
        public ICollection<ClientEarningBreakdownByPaydateDto> BreakdownByPaydate { get; set; }
    }
}
