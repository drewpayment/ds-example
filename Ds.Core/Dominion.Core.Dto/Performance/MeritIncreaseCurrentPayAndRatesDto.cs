using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Performance
{
    public class MeritIncreaseCurrentPayAndRatesDto
    {
        public bool Selected { get; set; }
        public string RateDesc { get; set; }
        public PayFrequencyType PayFrequencyId { get; set; }
        public string PayFrequencyDesc { get; set; }
        public double? CurrentAmount { get; set; }
        public IncreaseType IncreaseType { get; set; }
        public double IncreaseAmount { get; set; }
        public double ProposedTotal { get; set; }
        public IEnumerable<RemarkDto> Comments { get; set; }
        public bool IsSalaryRow { get; set; }
        public bool IsRate { get; set; }
        public int? EmployeePayId { get; set; }
        public int? EmployeeClientRateId { get; set; }
        public int? ProposalId { get; set; }
        public int? MeritIncreaseId { get; set; }
        public DateTime? ApplyMeritIncreaseOn { get; set; }
    }
}
