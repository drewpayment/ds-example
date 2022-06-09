using System.Collections.Generic;
using Dominion.Domain.Entities.Tax;
using Dominion.Taxes.Dto.TaxOptions;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Represents a query that can be executed on a set of client earnings.
    /// </summary>
    public interface ITaxesOptionQuery : IQuery<TaxOption, ITaxesOptionQuery>
    {
        /// <summary>
        /// Filters the tax options by id.
        /// </summary>
        /// <param name="taxOptionId">Id of the entity.</param>
        /// <returns>Query to be further built.</returns>
        ITaxesOptionQuery ByTaxOptionId(int taxOptionId);

        /// <summary>
        /// Returns tax options matching the tax option ids passed in.
        /// </summary>
        /// <param name="taxOptionIds">Array of integers representing the tax option.</param>
        /// <returns></returns>
        ITaxesOptionQuery ByTaxOptionIds(IEnumerable<int> taxOptionIds);

        /// <summary>
        /// Filters the tax options by type.
        /// </summary>
        /// <param name="taxOptionType"><see cref="TaxOptionTypes"/>.</param>
        /// <returns>Query to be further built.</returns>
        ITaxesOptionQuery ByTaxOptionType(TaxOptionTypes taxOptionType);

    }
}
