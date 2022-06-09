using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientPayrollQuery : IQuery<ClientPayroll, IClientPayrollQuery>
    {
        /// <summary>
        /// Filters by rates for a given client
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IClientPayrollQuery ByClientId(int clientId);
    }
}
