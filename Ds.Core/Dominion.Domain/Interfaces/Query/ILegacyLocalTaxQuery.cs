using Dominion.Core.Dto.Tax;
using Dominion.Domain.Entities.Tax.Legacy;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Represents a query on <see cref="LegacyLocalTax"/> entities.
    /// </summary>
    public interface ILegacyLocalTaxQuery : IQuery<LegacyLocalTax, ILegacyLocalTaxQuery>
    {
        /// <summary>
        /// Filters the results by taxes containing the specified description.
        /// </summary>
        /// <param name="desc">Case-insensitive text the tax's description must contain.</param>
        /// <returns></returns>
        ILegacyLocalTaxQuery ByDescription(string desc);

        /// <summary>
        /// Filters the results by taxes with the specified ID.
        /// </summary>
        /// <param name="id">ID of the local tax to filter by.</param>
        /// <returns></returns>
        ILegacyLocalTaxQuery ByLocalTaxId(int id);

        /// <summary>
        /// Filters the results by taxes for the given state.
        /// </summary>
        /// <param name="stateId">ID of the state to filter state taxes by.</param>
        /// <returns></returns>
        ILegacyLocalTaxQuery ByStateId(int stateId);
        ILegacyLocalTaxQuery ByCountyId(int? countyId);
        ILegacyLocalTaxQuery ByTaxType(LegacyTaxType taxType);
        ILegacyLocalTaxQuery BySchoolDistrictId(int schoolDistrictId);
        ILegacyLocalTaxQuery ByCode(string code);
    }
}
