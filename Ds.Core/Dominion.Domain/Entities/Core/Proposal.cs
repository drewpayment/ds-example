using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;

namespace Dominion.Domain.Entities.Core
{
    public class Proposal : Entity<Proposal>, IHasModifiedData
    {
        public int ProposalId { get; set; }
        public int ProposedByUserId { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public User.User ProposedBy { get; set; }
        public virtual ICollection<ProposalRemark> Remarks { get; set; }
        public virtual ICollection<MeritIncrease> MeritIncreases { get; set; }
        public virtual ReviewProposal ReviewProposal { get; set; }
        public virtual OneTimeEarning OneTimeEarning { get; set; }

    }
}
