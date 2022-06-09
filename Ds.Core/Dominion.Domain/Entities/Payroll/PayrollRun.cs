using Dominion.Core.Dto.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class PayrollRun
    {
        public virtual PayrollRunType PayrollRunId { get; set; }
        public virtual string Description { get; set; }
        public virtual ICollection<Payroll> Payrolls { get; set; }
    }
}
