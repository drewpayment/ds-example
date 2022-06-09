using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.User;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientRelationQuery : IQuery<ClientOrganization, IClientRelationQuery>
    {
        IClientRelationQuery ByClientRelationId(int clientRelationId);
        IClientRelationQuery ByClientId(int clientId);

        /// <summary>
        /// Filters down to relations/organizations a user has access to at least one client of.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IClientRelationQuery ByUserHasAccessToAtLeaseOneClientInRelation(int userId);

        IClientRelationQuery OrderByName();
    }
}
