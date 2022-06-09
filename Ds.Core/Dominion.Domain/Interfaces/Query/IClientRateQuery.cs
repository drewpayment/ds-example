using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientRateQuery : IQuery<ClientRate, IClientRateQuery>
    {
        /// <summary>
        /// Filters by rates for a given client
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IClientRateQuery ByClientId(int clientId);


        /// <summary>
        /// Filters by rates for a given client
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IClientRateQuery ByClientRateId(int clientRateId);
    }
}
