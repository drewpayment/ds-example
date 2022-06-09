using System;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Query.Geofence;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IGeofenceRepository : IRepository, IDisposable
    {
        IClockClientGeofenceQuery QueryGeofences();
        IClockEmployeePunchLocationQuery QueryPunchLocations();
        IClockEmployeePunchAttemptQuery QueryPunchAttempts();
        IGeofenceBillingQuery QueryGeofenceBilling();
    }
}
