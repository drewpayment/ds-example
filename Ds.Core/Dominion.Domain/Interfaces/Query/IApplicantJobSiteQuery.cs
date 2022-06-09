using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.Enums;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Builds a query on <see cref="Client"/> data.
    /// </summary>
    public interface IApplicantJobSiteQuery : IQuery<ApplicantJobSite, IApplicantJobSiteQuery>
    {
        IApplicantJobSiteQuery ByApplicantJobSiteId(ApplicantJobSiteEnum applicantJobSiteId);
        IApplicantJobSiteQuery OrderByDescription();
    }
}
