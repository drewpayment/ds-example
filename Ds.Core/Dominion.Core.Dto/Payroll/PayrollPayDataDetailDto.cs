using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class PayrollPayDataDetailDto
    {
        public int PayrollPayDataDetailId { get; set; }
        public int PayrollPayDataId { get; set; }
        public int ClientEarningId { get; set; }
        public double? Hours { get; set; }
        public double? Amount { get; set; }
        public int? ClientPayDataInterfaceId { get; set; }
        public string SourceId { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public int? ClientId { get; set; }
    }
}
