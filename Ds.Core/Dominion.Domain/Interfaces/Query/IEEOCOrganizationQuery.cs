using Dominion.Domain.Entities.EEOC;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query on <see cref="EEOCOrganization"/>(s).
    /// </summary>
    public interface IEEOCOrganizationQuery : IQuery<EEOCOrganization, IEEOCOrganizationQuery>
    {
        /// <summary>
        /// Filters by the given EEOC Organization ID
        /// </summary>
        /// <param name="id">The ID of the EEOC Organization to filter by.</param>
        /// <returns></returns>
        IEEOCOrganizationQuery ByEeocOrganizationId(int id);

        /// <summary>
        /// Filters by the given client.
        /// </summary>
        /// <param name="FEIN">ID of the Client.</param>
        /// <returns></returns>
        IEEOCOrganizationQuery ByFEIN(int FEIN);
    }
}
