using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Billing;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IBillingItemQuery : IQuery<BillingItem, IBillingItemQuery>
    {
        /// <summary>
        /// Filters by details related to ACA Reporting billing.
        /// </summary>
        /// <returns></returns>
        IBillingItemQuery ByIsAcaReportingItem();

        /// <summary>
        /// Filters by history belonging to the specified client(s).
        /// </summary>
        /// <param name="clientIds">ID(s) of the clients to filter by.</param>
        /// <returns></returns>
        IBillingItemQuery ByClientIds(params int[] clientIds);

        /// <summary>
        /// Filters by history belonging to the specified client(s).
        /// </summary>
        /// <param name="year"></param>
        /// <param name="clientIds">ID(s) of the clients to filter by.</param>
        /// <returns></returns>
        IBillingItemQuery ByYear(short year);
        IBillingItemQuery IsOneTime(bool oneTime);
        IBillingItemQuery ByClientId(int clientId);
        IBillingItemQuery ByPayrollIdOrHasNoPayrollId(int payrollId);
        IBillingItemQuery ByBillingItemId(int billingItemId);
        IBillingItemQuery ByFeatureOptionId(int featureOptionId);
    }
}
