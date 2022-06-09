using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class ReviewMeritIncreaseDto : MeritIncreaseDto
    {
        public int ReviewId { get; set; }
        public int? ReviewTemplateId { get; set; }
        public ProposalDto Proposal { get; set; }
        /// <summary>
        /// True iff the effective date has either been accepted or rejected.
        /// </summary>
        public bool ProcessedByPayroll { get; set; }
        /// <summary>
        /// True iff the the effective date has been accepted. 
        /// </summary>
        public bool AcceptedByPayroll { get; set; }
        public DateTime? ApplyMeritIncreaseOn { get; set; }
    }
}
