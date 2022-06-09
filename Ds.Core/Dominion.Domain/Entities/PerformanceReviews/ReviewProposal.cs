using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Entities;
using System;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public class ReviewProposal : Entity<ReviewProposal>, IHasModifiedData
    {
        public int ProposalId { get; set; }
        public int ReviewId { get; set; }
        public virtual Proposal Proposal { get; set; }
        public virtual Review Review { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    }
}
