using Dominion.Domain.Entities.ClientFeatures;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Geofence
{
    public interface IClockClientGeofenceQuery : IQuery<ClockClientGeofence, IClockClientGeofenceQuery>
    {
        /// <summary>
        /// Filter entities by Clock Client Geofence id.
        /// </summary>
        /// <param name="ClockClientGeofenceID"></param>
        /// <returns></returns>
        IClockClientGeofenceQuery ByClockClientGeofenceID(int clockClientGeofenceID);

        /// <summary>
        /// Filter entities by Client id.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        IClockClientGeofenceQuery ByClientID(int clientID, bool showArchived);
    }
}