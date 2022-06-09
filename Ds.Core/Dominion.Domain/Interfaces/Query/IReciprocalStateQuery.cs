using System.Collections.Generic;
using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Methods for querying the <see cref="ReciprocalState"/> entity.
    /// EXAMPLE: Query (interface): 
    /// This is a wrapper for the DbSet in the <see cref="IDominionContext"/>.
    /// This allows us to write queries that are unit testable without a database connection.
    /// Rules: 
    /// - Folder: Dominion.Domain\Interfaces\Query\file.
    /// </summary>
    public interface IReciprocalStateQuery : IQuery<ReciprocalState, IReciprocalStateQuery>
    {
        /// <summary>
        /// Get all the reciprocal states for a given state id
        /// </summary>
        /// <param name="id">ID of the country.</param>
        /// <returns>Query to be further built.</returns>
        IReciprocalStateQuery ByStateId(int id);
    }
}
