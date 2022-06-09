using Dominion.Domain.Entities.Employee.ClockEmployeeInfo;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query.Geofence
{
    public interface IClockEmployeePunchLocationQuery : IQuery<ClockEmployeePunchLocation, IClockEmployeePunchLocationQuery>
    {
        /// <summary>
        /// Filter entities by Clock Client Geofence id.
        /// </summary>
        /// <param name="ClockClientGeofenceID"></param>
        /// <returns></returns>
        IClockEmployeePunchLocationQuery ByClockClientGeofenceID(int clockClientGeofenceID);

        /// <summary>
        /// Filter entities by location id.
        /// </summary>
        /// <param name="punchLocationIds"></param>
        /// <returns></returns>
        IClockEmployeePunchLocationQuery ByClockEmployeePunchLocationIDs(List<int> punchLocationIds);
    }
}