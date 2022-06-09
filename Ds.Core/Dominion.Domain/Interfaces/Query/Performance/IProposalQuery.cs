using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IProposalQuery : IQuery<Proposal, IProposalQuery>
    {
        IProposalQuery ByProposalId(int proposalId);
        IProposalQuery ByProposalId(ICollection<int> proposalIds);
        IProposalQuery ByReviewId(int reviewId);		
    }
}
