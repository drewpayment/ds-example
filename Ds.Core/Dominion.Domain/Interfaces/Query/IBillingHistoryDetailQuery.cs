using System;

using Dominion.Domain.Entities.Billing;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Queries <see cref="BillingHistoryDetail"/> data.
    /// </summary>
    public interface IBillingHistoryDetailQuery : IQuery<BillingHistoryDetail, IBillingHistoryDetailQuery>
    {
        /// <summary>
        /// Filters by history belonging to the specified client(s).
        /// </summary>
        /// <param name="clientIds">ID(s) of the clients to filter by.</param>
        /// <returns></returns>
        IBillingHistoryDetailQuery ByClientIds(params int[] clientIds);

        /// <summary>
        /// Filters by billing year for a given year
        /// </summary>
        /// <param name="billingYear">Year the billing date equal to.
        /// <returns></returns>
        IBillingHistoryDetailQuery ByBillingYear(int billingYear);

        /// <summary>
        /// Filters by details related to ACA Reporting billing.
        /// </summary>
        /// <returns></returns>
        IBillingHistoryDetailQuery ByIsAcaReportingItem();
    }
}
