using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public partial class PayrollRunDto
    {
        public PayrollRunType PayrollRunId { get; set; }
        public string Description { get; set; }

        public ICollection<PayrollDto> Payrolls { get; set; }
    }
}
