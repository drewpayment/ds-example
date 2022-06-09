using System.Collections.Generic;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientGroupQuery : IQuery<ClientGroup, IClientGroupQuery>
    {
        /// <summary>
        /// Filters groups by a single client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClientGroupQuery ByClientId(int clientId);
        IClientGroupQuery ByGroupId(int clientGroupId);
        IClientGroupQuery ByGroupIds(IEnumerable<int> clientGroupIds);
        IClientGroupQuery ByClientGroupId(int clientGroupId);
        IClientGroupQuery OrderByClientGroupId();
        IClientGroupQuery OrderByDescription();        

        /// <summary>
        /// Filters by one or more particular groups.
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        IClientGroupQuery ByGroup(params int[] groupIds);
        IClientGroupQuery ExcludeGroup(params int[] groupIds);
    }
}
