using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class MeritIncreaseDto
    {
        public int MeritIncreaseId { get; set; }
        public int ProposalId { get; set; }
        public PayFrequencyType PayFrequencyId { get; set; }
        public IncreaseType Type { get; set; }
        public double CurrentAmount { get; set; }
        public double IncreaseAmount { get; set; }
        public string  RateName { get; set; }
        public int? ClientRateId { get; set; }
        public int? EmployeeClientRateId { get; set; }
        public int? EmployeePayId { get; set; }
        public double ProposedAmount { get; set; }
        public IEnumerable<RemarkDto> Comments  { get; set; }
        public int ProposedBy { get; set; }
        public DateTime? ApplyMeritIncreaseOn { get; set; }
        public ApprovalStatus ApprovalStatusID { get; set; }
        public EffectiveDateDto EffectiveDate { get; set; }


    }
}
