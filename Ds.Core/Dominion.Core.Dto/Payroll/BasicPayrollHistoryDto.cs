using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class BasicPayrollHistoryDto
    {
        public int PayrollId { get; set; }
        public DateTime CheckDate { get; set; }
        public DateTime PeriodEnded { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime RunDate { get; set; }
        public PayrollRunType PayrollRunId { get; set; }
        public PayFrequencyType? PayFrequencyType { get; set; }
    }
}
