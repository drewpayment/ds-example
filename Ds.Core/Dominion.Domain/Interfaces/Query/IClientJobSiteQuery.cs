using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.Enums;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Builds a query on <see cref="Client"/> data.
    /// </summary>
    public interface IClientJobSiteQuery : IQuery<ClientJobSite, IClientJobSiteQuery>
    {
        IClientJobSiteQuery ByApplicantJobSiteId(ApplicantJobSiteEnum applicantJobSiteId);
        IClientJobSiteQuery ByClientId(int clientId);
        IClientJobSiteQuery ByClientJobSiteId(int clientJobSiteId);
        IClientJobSiteQuery BySharePosts(bool canShare);
    }
}
