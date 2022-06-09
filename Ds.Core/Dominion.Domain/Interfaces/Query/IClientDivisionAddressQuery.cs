using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientDivisionAddressQuery : IQuery<ClientDivisionAddress, IClientDivisionAddressQuery>
    {
        IClientDivisionAddressQuery ByClientDivisionAddressId(int clientDivisionAddressId);
        IClientDivisionAddressQuery ByClientDivisionId(int clientDivisionId);
        IClientDivisionAddressQuery OrderByClientDivisionId();
        IClientDivisionAddressQuery OrderByName();

        /// <summary>
        /// Filters by one or more particular divisions.
        /// </summary>
        /// <param name="divisionIds">One or more divisions to filter by.</param>
        /// <returns></returns>
        IClientDivisionAddressQuery ByDivisions(List<int> divisionIds);
    }
}