using System;
using System.Collections.Generic;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.Labor;

namespace Dominion.Domain.Interfaces
{
    public interface IClockEarningDesHistoryQuery : IQuery<ClockEarningDesHistory, IClockEarningDesHistoryQuery>
    {
        IClockEarningDesHistoryQuery ByClient(int clientId);
        IClockEarningDesHistoryQuery ByClients(int[] clientIds);
        //IClockEarningDesHistoryQuery ByEventDate(DateTime eventDate);
        IClockEarningDesHistoryQuery ByEventDateRange(DateTime startTime, DateTime endTime);

        IClockEarningDesHistoryQuery ByEmployee(int employeeId);
        IClockEarningDesHistoryQuery ByEmployeeIds(IEnumerable<int> empIds);
        IClockEarningDesHistoryQuery ByClientShiftIds(IEnumerable<int?> clientShiftIds);
        IClockEarningDesHistoryQuery ByClientDepartmentIds(List<int?> clientDepartmentIds);
        IClockEarningDesHistoryQuery ByDivisionIds(IEnumerable<int?> divisionIds);
    }
}
