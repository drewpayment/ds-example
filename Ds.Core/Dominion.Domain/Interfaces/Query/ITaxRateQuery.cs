using System;
using Dominion.Domain.Entities.Tax;
using Dominion.Taxes.Dto.TaxOptions;
using Dominion.Taxes.Dto.TaxRates;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface ITaxRateQuery : IQuery<TaxRate, ITaxRateQuery>
    {
        /// <summary>
        /// Filter by the effective date.
        /// </summary>
        /// <param name="effectiveDate">The effective date.</param>
        /// <returns></returns>
        ITaxRateQuery ByEffectiveDate(DateTime effectiveDate);

        /// <summary>
        /// Filter by filing status.
        /// </summary>
        /// <param name="filingStatus">The value of the filing status.</param>
        /// <returns></returns>
        ITaxRateQuery ByFilingStatus(FilingStatus? filingStatus);

        /// <summary>
        /// Filter by the tax rate type.
        /// </summary>
        /// <param name="taxRateType">The tax rate type.</param>
        /// <returns></returns>
        ITaxRateQuery ByTaxRateType(TaxRateType taxRateType);

        /// <summary>
        /// Gets tax rate header records less than or equal to the effective date specified.
        /// The top result parameters allows you to specify you want only the most recent of those mostRecentOnly.
        /// </summary>
        /// <param name="effectiveDate">The effective date you're searching by.</param>
        /// <param name="onlyTopResult">By default this is true. Pass false if you want all that are LTE.</param>
        /// <returns></returns>
        ITaxRateQuery ByEffectiveDateLTE(DateTime effectiveDate, bool mostRecentOnly = true);

    }
}