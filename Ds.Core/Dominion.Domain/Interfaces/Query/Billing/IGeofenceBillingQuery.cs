using Dominion.Domain.Entities.Geofence;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query.Billing

{
    public interface IGeofenceBillingQuery : IQuery<GeofenceBilling, IGeofenceBillingQuery>
    {
        /// <summary>
        /// Filters by start and end date of the billing counting
        /// </summary>
        /// <returns></returns>
        IGeofenceBillingQuery ByDateRange(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Filters by clientId
        /// </summary>
        /// <returns></returns>
        IGeofenceBillingQuery ByClientID (int clientId);

        /// Fitler by a list of client ids.
        /// </summary>
        /// <param name="clientIds"></param>
        /// <returns></returns>
        IGeofenceBillingQuery ByClientIds(List<int> clientIds);

        /// <summary>
        /// Filters by active date
        /// </summary>
        /// <returns></returns>
        IGeofenceBillingQuery ByActiveDate(DateTime activeDate);

    }
}
