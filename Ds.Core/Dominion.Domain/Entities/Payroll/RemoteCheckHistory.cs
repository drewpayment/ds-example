using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class RemoteCheckHistory
    {
        public virtual int GenRemoteCheckHistoryId { get; set; }
        public virtual int? GenPaycheckHistoryId { get; set; }
        public virtual int? GenVendorPaymentHistoryId { get; set; }
        public virtual int PayrollId { get; set; }
        public virtual int? CheckNumber { get; set; }
        public virtual bool? IsWasPrinted { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual DateTime? Modified { get; set; }

        public RemoteCheckHistory()
        {
        }
    }
}
