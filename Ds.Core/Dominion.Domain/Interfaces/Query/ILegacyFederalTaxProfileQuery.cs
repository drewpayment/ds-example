using System;

using Dominion.Domain.Entities.Tax.Legacy;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Provides methods to query <see cref="LegacyFederalTaxProfile"/> data.
    /// </summary>
    public interface ILegacyFederalTaxProfileQuery : IQuery<LegacyFederalTaxProfile, ILegacyFederalTaxProfileQuery>
    {
        /// <summary>
        /// Filters by profiles whose effective date is before the given date.
        /// </summary>
        /// <param name="date">Date which profiles must be effective before.</param>
        /// <returns></returns>
        ILegacyFederalTaxProfileQuery ByEffectiveBeforeDate(DateTime date);

        /// <summary>
        /// Orders the query results by <see cref="LegacyFederalTaxProfile.EffectiveDate"/>.
        /// </summary>
        /// <param name="direction">Defaults to ascending.</param>
        /// <returns></returns>
        ILegacyFederalTaxProfileQuery OrderByEffectiveDate(SortDirection direction = SortDirection.Ascending);
    }
}
