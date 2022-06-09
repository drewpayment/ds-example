using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public class ProposalRemark : Entity<ProposalRemark>, IHasModifiedData
    {
        public int RemarkId { get; set; }
        public int ProposalId { get; set; }
        public virtual Remark Remark { get; set; }
        public virtual Proposal Proposal { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    }
}
