using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class ReportSchedulingControlIdOption
    {
        public virtual int ClientOptionId { get; set; }
        public virtual int ClientId { get; set; }
        public int ClientOptionControlId { get; set; }
        public char Value { get; set; }
    
    }
}
