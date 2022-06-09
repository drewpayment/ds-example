using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dominion.Domain.Entities.Tax;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository used to query tax related entities.
    /// </summary>
    public interface ITaxesRepository : IRepository, IDisposable
    {
        /// <summary>
        /// Constructs a new <see cref="TaxOption"/> query.
        /// </summary>
        /// <returns>New query on <see cref="TaxOption"/> entities.</returns>
        ITaxesOptionQuery TaxOptionQuery();

        /// <summary>
        /// Constructs a new <see cref="TaxOptionRule"/> query.
        /// </summary>
        /// <returns>New query on <see cref="TaxOptionRule"/> entities.</returns>
        ITaxOptionRuleQuery TaxOptionRuleQuery();

        /// <summary>
        /// Constructs a new query on <see cref="Tax"/>es.
        /// </summary>
        /// <returns>New <see cref="Tax"/> query.</returns>
        ITaxQuery TaxQuery();

        /// <summary>
        /// Constructs a new query on <see cref="TaxConfiguration"/>s.
        /// </summary>
        /// <returns>New query on <see cref="TaxConfiguration"/> entities.</returns>
        ITaxConfigurationQuery TaxConfigurationQuery();

        /// <summary>
        /// Constructs a new <see cref="TaxOptionTypeInfo"/> query.
        /// </summary>
        /// <returns>New query on <see cref="TaxOptionTypeInfo"/> entities.</returns>
        IQuery<TaxOptionTypeInfo> TaxOptionTypeQuery();

        /// <summary>
        /// Constructs a new <see cref="TaxTypeInfoQuery"/> query.
        /// </summary>
        /// <returns>New query on <see cref="TaxTypeInfoQuery"/> entities.</returns>
        ITaxTypeInfoQuery TaxTypeInfoQuery();
        ITaxDeferralQuery TaxDeferralQuery();

        /// <summary>
        /// Constructs a new <see cref="EmployeeTaxChangeHistoryQuery"/> query.
        /// </summary>
        /// <returns>New query on <see cref="EmployeeTaxChangeHistoryQuery"/> entities.</returns>
        IEmployeeTaxChangeHistoryQuery EmployeeTaxChangeHistoryQuery();

        /// <summary>
        /// Constructs a new <see cref="EmployeeTaxQuery"/> query.
        /// </summary>
        /// <returns>New query on <see cref="EmployeeTaxQuery"/> entities.</returns>
        IEmployeeTaxQuery EmployeeTaxQuery();

        /// <summary>
        /// Constructs a new <see cref="FilingStatusInfoQuery"/> query.
        /// </summary>
        /// <returns>New query.</returns>
        IFilingStatusInfoQuery FilingStatusInfoQuery();

        /// <summary>
        /// Constructs a new <see cref="TaxRateHeaderQuery"/> query.
        /// </summary>
        /// <returns>New query on <see cref="TaxRateHeaderQuery"/> entities.</returns>
        ITaxRateHeaderQuery TaxRateHeaderQuery();

        ///// <summary>
        ///// Constructs a new <see cref="TaxRateQuery"/> query.
        ///// </summary>
        ///// <returns>New query on <see cref="TaxRateQuery"/> entities.</returns>
        //ITaxRateQuery TaxRateQuery();

        /// <summary>
        /// Constructs a new <see cref="TaxEntityFilingStatusQuery"/> query.
        /// </summary>
        /// <returns>New query on <see cref="TaxEntityFilingStatus"/> entities.</returns>
        ITaxEntityFilingStatusQuery TaxEntityFilingStatusQuery();

        /// <summary>
        /// Constructs a new <see cref="EmployeeTaxCostCenterQuery"/> query.
        /// </summary>
        /// <returns>New query on <see cref="EmployeeTaxCostCenter"/> entities.</returns>
        IEmployeeTaxCostCenterQuery EmployeeTaxCostCenterQuery();

        /// <summary>
        /// Constructs a new <see cref="WotcReasonQuery"/> query.
        /// </summary>
        /// <returns>New query on <see cref="WotcReason"/> entities.</returns>
        IWotcReasonQuery WotcReasonQuery();
    }
}
