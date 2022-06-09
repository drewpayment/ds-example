using System;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Interfaces.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Used to get information about location data.
    /// ie. Countries, states, addresses, etc.
    /// EXAMPLE: Repository (interface): 
    /// Rules: 
    /// - Folder: Dominion.Domain\Interfaces\Repositories\file .
    /// </summary>
    public interface ILocationRepository : IRepository, IDisposable
    {
        /// <summary>
        /// Constructs a new <see cref="State"/> query.
        /// </summary>
        /// <returns>New query on <see cref="State"/> entities.</returns>
        IStateQuery StateQuery();

         /// <summary>
        /// Constructs a new <see cref="County"/> query.
        /// </summary>
        /// <returns>New query on <see cref="County"/> entities.</returns>
        ICountyQuery CountyQuery();

              /// <summary>
        /// Constructs a new <see cref="Country"/> query.
        /// </summary>
        /// <returns>New query on <see cref="Country"/> entities.</returns>
        ICountryQuery CountryQuery();
    }
}
