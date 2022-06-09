using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface ICompanyResourceQuery : IQuery<CompanyResource, ICompanyResourceQuery>
    {
        /// <summary>
        /// Filters by rates for a given client
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        ICompanyResourceQuery ByClientId(int clientId);
        ICompanyResourceQuery ByFolderId(int folderId);
        ICompanyResourceQuery ByResourceId(int? ResourceId);
        ICompanyResourceQuery RemoveCompanyAdminResources();
        ICompanyResourceQuery RemoveSupervisorResources();
        ICompanyResourceQuery RemoveManagerLinks();
        ICompanyResourceQuery OrderByResourceName();
    }
}
