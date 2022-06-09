using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockEmployeeQuery : IQuery<ClockEmployee, IClockEmployeeQuery>
    {
        IClockEmployeeQuery ByBadgeNumber(string badgeNumber);
        IClockEmployeeQuery ByClientId(int clientId, bool includeAllClientsInOrganization = false);
        IClockEmployeeQuery ByEmployeePin(string employeePin);
        IClockEmployeeQuery ByEmployeeId(int employeeId);
        IClockEmployeeQuery ByActiveEmployee(bool isActive);
        IClockEmployeeQuery ByClientIds(List<int> clientId);
        IClockEmployeeQuery ByClockClientTimePolicyIds(List<int> timePolicyIds);
        IClockEmployeeQuery ByGeofenceEnabled();
        IClockEmployeeQuery ByTimePolicyId(int timePolicyId);
    }
}