using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public partial class RemoteCheckHistoryDto
    {
        public int GenRemoteCheckHistoryId { get; set; }
        public int? GenPaycheckHistoryId { get; set; }
        public int? GenVendorPaymentHistoryId { get; set; }
        public int PayrollId { get; set; }
        public int? CheckNumber { get; set; }
        public bool? IsWasPrinted { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
    }
}
