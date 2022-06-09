using System.Collections.Generic;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockClientTimePolicyQuery : IQuery<ClockClientTimePolicy, IClockClientTimePolicyQuery>
    {

        IClockClientTimePolicyQuery ByClientId(int clientId);

        IClockClientTimePolicyQuery ByClockClientExceptionId(int clockClientExceptionId);

        IClockClientTimePolicyQuery ByClockClientHoliday(int clockClientHolidayId);

        /// <summary>
        /// Filter entities by clock client time policy id
        /// </summary>
        /// <param name="clockClientTimePolicyId"></param>
        /// <returns></returns>
        IClockClientTimePolicyQuery ByClockClientTimePolicyId(int clockClientTimePolicyId);
        IClockClientTimePolicyQuery ByClockClientRulesId(int clockClientRulesId);

        /// <summary>
        /// Fitler by a list of time policy ids.
        /// </summary>
        /// <param name="clockClientTimePolicies"></param>
        /// <returns></returns>
        IClockClientTimePolicyQuery ByClockClientTimePolicyList(List<int> clockClientTimePolicies);

        /// <summary>
        /// Order by time policy name.
        /// </summary>
        /// <returns></returns>
        IClockClientTimePolicyQuery OrderByName();


        /// Fitler by a list of client ids.
        /// </summary>
        /// <param name="clientIds"></param>
        /// <returns></returns>
        IClockClientTimePolicyQuery ByClientIds(List<int> clientIds);

        /// <summary>
        /// Fitler by geofence enabled
        /// </summary>
        /// <returns></returns>
        IClockClientTimePolicyQuery ByGeofenceEnabled();

        /// <summary>
        /// Filter by list of department Ids
        /// </summary>
        /// <returns></returns>
        IClockClientTimePolicyQuery ByClientDepartmentIds(List<int?> clientDepartmentIds);
    }
}
