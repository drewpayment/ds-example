using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Queries <see cref="ClientCalendar"/> data.
    /// </summary>
    public interface IClientCalendarQuery : IQuery<ClientCalendar, IClientCalendarQuery>
    {
        /// <summary>
        /// Filters by calendars belonging to one of the specified clients.
        /// </summary>
        /// <param name="clientIds">ID(s) of the clients to query by.</param>
        /// <returns></returns>
        IClientCalendarQuery ByClients(params int[] clientIds);

        /// <summary>
        /// Filters by as single client.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IClientCalendarQuery ByClient(int clientId);
    }
}
