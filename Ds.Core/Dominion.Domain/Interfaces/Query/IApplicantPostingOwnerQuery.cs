using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantPostingOwnerQuery : IQuery<ApplicantPostingOwner, IApplicantPostingOwnerQuery>
    {
        IApplicantPostingOwnerQuery ByPostingId(int postingId);
        IApplicantPostingOwnerQuery ByUserId(int userId);
    }
}