using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Defines an extension interface for queries that can filter by Client ID.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    public interface IByClientIdQuery<out TQuery>
    {
        /// <summary>
        /// Filters the entity by the given client ID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        TQuery ByClientId(int clientId);
    }
}
