using System;
using System.Collections;
using System.Collections.Generic;
using Dominion.Domain.Entities.Tax;
using Dominion.Taxes.Dto.TaxOptions;
using Dominion.Taxes.Dto.TaxRates;
using Dominion.Taxes.Dto.TaxTypes;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Builds a query on <see cref="Tax"/>es.
    /// </summary>
    public interface ITaxRateHeaderQuery : IQuery<TaxRateHeader, ITaxRateHeaderQuery>
    {
        /// <summary>
        /// Filter by the tax id.
        /// </summary>
        /// <param name="taxId">The tax id.</param>
        /// <returns></returns>
        ITaxRateHeaderQuery ByTaxId(int taxId);

        /// <summary>
        /// Filter by the effective date.
        /// </summary>
        /// <param name="effectiveDate">The effective date.</param>
        /// <returns></returns>
        ITaxRateHeaderQuery ByEffectiveDate(DateTime effectiveDate);

        /// <summary>
        /// Filter by filing status.
        /// </summary>
        /// <param name="filingStatus">The value of the filing status.</param>
        /// <returns></returns>
        ITaxRateHeaderQuery ByFilingStatus(FilingStatus? filingStatus);

        /// <summary>
        /// Filter by the tax rate type.
        /// </summary>
        /// <param name="taxRateType">The tax rate type.</param>
        /// <returns></returns>
        ITaxRateHeaderQuery ByTaxRateType(TaxRateType taxRateType);

        /// <summary>
        /// Gets tax rate header records less than or equal to the effective date specified.
        /// The top result parameters allows you to specify you want only the most recent of those mostRecentOnly.
        /// </summary>
        /// <param name="effectiveDate">The effective date you're searching by.</param>
        /// <param name="onlyTopResult">By default this is true. Pass false if you want all that are LTE.</param>
        /// <returns></returns>
        ITaxRateHeaderQuery ByEffectiveDateLTE(DateTime effectiveDate, bool mostRecentOnly = true);

        /// <summary>
        /// Orders the query results by <see cref="TaxRateHeader.EffectiveDate"/>.
        /// </summary>
        /// <param name="direction">Defaults to ascending.</param>
        /// <returns></returns>
        ITaxRateHeaderQuery OrderByEffectiveDate(SortDirection direction);
    }
}
