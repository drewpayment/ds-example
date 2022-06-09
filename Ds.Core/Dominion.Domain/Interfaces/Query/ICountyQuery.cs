using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Methods for querying the <see cref="State"/> entity.
    /// EXAMPLE: Query (interface): 
    /// This is a wrapper for the DbSet in the <see cref="IDominionContext"/>.
    /// This allows us to write queries that are unit testable without a database connection.
    /// Rules: 
    /// - Folder: Dominion.Domain\Interfaces\Query\file.
    /// </summary>
    public interface  ICountyQuery : IQuery<County, ICountyQuery>
    {
        /// <summary>
        /// Get all the counties for a state by state id.
        /// </summary>
        /// <param name="id">ID of the state.</param>
        /// <returns>Query to be further built.</returns>
        ICountyQuery ByStateId(int id);
    }
}
