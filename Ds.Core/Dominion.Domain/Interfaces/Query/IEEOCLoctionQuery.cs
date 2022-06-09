using Dominion.Aca.Dto.Forms;
using Dominion.Domain.Entities.EEOC;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
      /// <summary>
    /// Query on <see cref="EEOCLocation"/>(s).
    /// </summary>
    public interface IEEOCLocationQuery : IQuery<EEOCLocation, IEEOCLocationQuery>
      {
        /// <summary>
        /// Filters by the given EEOC Location ID
        /// </summary>
        /// <param name="id">The ID of the location to filter by.</param>
        /// <returns></returns>
        IEEOCLocationQuery ByEeocLocationId(int id);

        /// <summary>
        /// Filters by the given client.
        /// </summary>
        /// <param name="CLientId">ID of the Client.</param>
        /// <returns></returns>
        IEEOCLocationQuery ByClientId(int clientID);

                /// <summary>
        /// Filters by a location's active status.
        /// </summary>
        /// <param name="isActive">Includes locations"units" that meet the specified activity status.</param>
        /// <returns></returns>
        IEEOCLocationQuery ByIsActive(bool isActive = true);

        IEEOCLocationQuery OrderByEEOCLocationId();

        IEEOCLocationQuery OrderByEEOCLocationDescription();

        IEEOCLocationQuery ByLocationId(int locationId);

        IEEOCLocationQuery ByClientIds(int[] clientIds);

        IEEOCLocationQuery ByIsHeadquarters(bool isHeadquarters = true);
      }
}
