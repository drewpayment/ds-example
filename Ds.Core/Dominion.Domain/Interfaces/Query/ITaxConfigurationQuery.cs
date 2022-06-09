using System;

using Dominion.Domain.Entities.Tax;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Provides a way to query <see cref="TaxConfiguration"/>s.
    /// </summary>
    public interface ITaxConfigurationQuery : IQuery<TaxConfiguration, ITaxConfigurationQuery>
    {
        /// <summary>
        /// Filters tax configurations by those which were effective on the given date. 
        /// </summary>
        /// <param name="dateEffective">Date the tax config should be effective on.</param>
        /// <returns>Query to be further manipulated.</returns>
        ITaxConfigurationQuery ByEffectiveAsOf(DateTime dateEffective);

        /// <summary>
        /// Provides a hook to filter <see cref="TaxConfiguration"/>s using a <see cref="ITaxQuery"/>.
        /// </summary>
        /// <param name="taxQueryBuilder">Function used to apply the desired tax filters.</param>
        /// <returns>Query to be further manipulated.</returns>
        ITaxConfigurationQuery FilterTaxes(Action<ITaxQuery> taxQueryBuilder);
    }
}
