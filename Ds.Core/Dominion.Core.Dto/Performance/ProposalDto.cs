using Dominion.Core.Dto.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class ProposalDto
    {
        public int ProposalID { get; set; }
        public int ProposedByUserID { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ReviewId { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public IEnumerable<RemarkDto> Remarks { get; set; }
        public OneTimeEarningDto OneTimeEarning { get; set; }
        public ICollection<MeritIncreaseDto> MeritIncreases { get; set; }

    }
}
