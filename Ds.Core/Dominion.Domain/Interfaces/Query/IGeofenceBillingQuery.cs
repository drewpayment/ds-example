using Dominion.Domain.Entities.Geofence;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query.Geofence
{
    public interface IGeofenceBillingQuery : IQuery<GeofenceBilling, IGeofenceBillingQuery>
    {
        /// <summary>
        /// Filter entities by Geofence Billing id.
        /// </summary>
        /// <param name="ClockClientGeofenceID"></param>
        /// <returns></returns>
        IGeofenceBillingQuery ByGeofenceBillingID(int geofenceBillingID);

        /// <summary>
        /// Filter entities by Client id.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        IGeofenceBillingQuery ByClientID(int clientID);

        /// <summary>
        /// Filter entities by Client id.
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        IGeofenceBillingQuery ByEmployeeID(int employeeID);

        /// <summary>
        /// Order by date
        /// </summary>
        /// <returns></returns>
        IGeofenceBillingQuery OrderByDate();

        /// <summary>
        /// Filter entities by Client ids.
        /// </summary>
        /// <param name="ClientIds"></param>
        /// <returns></returns>
        IGeofenceBillingQuery ByClientIds(List<int> clientIds);
    }
}