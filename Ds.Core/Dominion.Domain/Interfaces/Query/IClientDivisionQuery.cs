using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientDivisionQuery : IQuery<ClientDivision, IClientDivisionQuery>
    {
        /// <summary>
        /// Filters by a single client.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IClientDivisionQuery ByClientId(int clientId);
        IClientDivisionQuery ByIsActive(bool isActive);
        IClientDivisionQuery ByIsActive(bool isActive, int clientDivisionId);
        IClientDivisionQuery ByClientDivisionId(int clientDivisionId);
        IClientDivisionQuery OrderByClientDivisionId();
        IClientDivisionQuery OrderByName();

        /// <summary>
        /// Filters by one or more particular divisions.
        /// </summary>
        /// <param name="divisionIds">One or more divisions to filter by.</param>
        /// <returns></returns>
        IClientDivisionQuery ByDivisions(params int[] divisionIds);
        IClientDivisionQuery ExcludeDivisions(params int[] divisionIds);
    }
}
