using System;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockClientScheduleChangeHistoryQuery :
        IQuery<ClockClientScheduleChangeHistory, IClockClientScheduleChangeHistoryQuery>
    {
        IClockClientScheduleChangeHistoryQuery ByClientId(int clientId);

        IClockClientScheduleChangeHistoryQuery ByDateRange(DateTime startDate, DateTime endDate);

        IClockClientScheduleChangeHistoryQuery ByEmployeeId(int employeeId);
    }
}