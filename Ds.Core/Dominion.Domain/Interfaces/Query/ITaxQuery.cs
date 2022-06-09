using System.Collections.Generic;
using Dominion.Domain.Entities.Tax;
using Dominion.Taxes.Dto.TaxTypes;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Builds a query on <see cref="Tax"/>es.
    /// </summary>
    public interface ITaxQuery : IQuery<Tax, ITaxQuery>
    {
        /// <summary>
        /// Filters the query by a particular <see cref="TaxType"/>.
        /// </summary>
        /// <param name="type">Tax type to filter by.</param>
        /// <returns>Query to be further built.</returns>
        ITaxQuery ByTaxType(TaxType type);

        /// <summary>
        /// Filters the query by a particular <see cref="TaxCategory"/>.
        /// </summary>
        /// <param name="category">Tax category to filter by.</param>
        /// <returns>Query to be further built.</returns>
        ITaxQuery ByTaxCategory(TaxCategory category);

        /// <summary>
        /// Filters the query by a given Legacy Tax ID. Use this in combination with a particular 
        /// tax type to get the generic Tax entity version of a particular legacy tax.
        /// </summary>
        /// <param name="id">ID of the legacy tax to filter by.</param>
        /// <returns>Query to be further built.</returns>
        ITaxQuery ByLegacyTaxId(int? id);

        /// <summary>
        /// Filters the query by taxes which contain the given name.
        /// </summary>
        /// <param name="name">Partial text the tax name must contain. (Case-insensitive)</param>
        /// <returns>Query to be further built.</returns>
        ITaxQuery ByName(string name);

        /// <summary>
        /// Filters the query by taxes for the given state. If null, will query for non-state related taxes.
        /// </summary>
        /// <param name="stateId">State ID to filter by.</param>
        /// <returns>Query to be further built.</returns>
        ITaxQuery ByStateId(int? stateId);

        /// <summary>
        /// Filters taxes by those with the specified ID.  Typically, used to find a unique tax.
        /// </summary>
        /// <param name="taxId">ID of the tax to find.</param>
        /// <returns></returns>
        ITaxQuery ByTaxId(int taxId);

        /// <summary>
        /// Filters taxes by those with the specified ids.
        /// </summary>
        /// <param name="taxIds">IDs of the taxes to find.</param>
        /// <returns></returns>
        ITaxQuery ByTaxIds(IEnumerable<int> taxIds);
    }
}
