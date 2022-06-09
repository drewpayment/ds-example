using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicationViewedQuery : IQuery<ApplicationViewed, IApplicationViewedQuery>
    {
        IApplicationViewedQuery ByClientId(int clientId);
        IApplicationViewedQuery ByUserId(int userId);
        IApplicationViewedQuery ByApplicationHeaderId(int applicationHeaderId);
        IApplicationViewedQuery ByApplicationViewedId(int applicationViewedId);
    }
}