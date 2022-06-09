using Dominion.Domain.Entities.TimeClock;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockClientScheduleQuery : IQuery<ClockClientSchedule, IClockClientScheduleQuery>
    {
        IClockClientScheduleQuery ByClient(int clientId);

        IClockClientScheduleQuery ByIsActive(bool isActive = true);

        IClockClientScheduleQuery ByClientDepartmentIds(List<int?> clientDepartmentIds);
    }
}