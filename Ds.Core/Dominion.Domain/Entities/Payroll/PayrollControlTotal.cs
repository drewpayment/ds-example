using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class PayrollControlTotal
    {
        public virtual int PayrollControlTotalId { get; set; }
        public virtual int PayrollId { get; set; }
        public virtual int ClientEarningId { get; set; }
        public virtual double Hours { get; set; }
        public virtual double Amount { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual string ModifiedBy { get; set; }

        public virtual ClientEarning ClientEarning { get; set; }
    }
}
