using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class PayrollCheckSeq
    {
        public virtual byte PayrollCheckSeqId { get; set; }
        public virtual string PayrollCheckSeq_ { get; set; }
    }
}
