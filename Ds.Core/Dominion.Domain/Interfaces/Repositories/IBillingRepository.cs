using Dominion.Domain.Entities.Billing;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Query.Billing;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Provides query access to billing related datasets.
    /// </summary>
    public interface IBillingRepository
    {
        /// <summary>
        /// Queries <see cref="BillingPriceChart"/> data.
        /// </summary>
        /// <returns></returns>
        IBillingPriceChartQuery QueryBillingPriceCharts();

        /// <summary>
        /// Queries <see cref="BillingHistoryDetail"/> data (ie: actual/invoiced amounts for past payrolls).
        /// </summary>
        /// <returns></returns>
        IBillingHistoryDetailQuery QueryBillingHistoryDetail();

        /// <summary>
        /// Queries <see cref="BillingHistoryDetail"/> data (ie: actual/invoiced amounts for past payrolls).
        /// </summary>
        /// <returns></returns>
        IBillingItemQuery QueryBillingItem();
        
        IAutomaticBillingQuery QueryAutomaticBilling();
        IPendingBillingCreditQuery QueryPendingBillingCredit();
        IGeofenceBillingQuery GeofenceBillingQuery();

        /// <summary>
        /// Queries <see cref="BillingItemDescription"/> data.
        /// </summary>
        /// <returns></returns>
        IBillingItemDescriptionQuery QueryBillingItemDescription();

        /// <summary>
        /// Queries <see cref="BillingHistory"/> data.
        /// </summary>
        /// <returns></returns>
        IBillingHistoryQuery QueryBillingHistory();
    }
}
