using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockEmployeePunchQuery : IQuery<ClockEmployeePunch, IClockEmployeePunchQuery>
    {
        IClockEmployeePunchQuery ByClientId(int clientId);

        IClockEmployeePunchQuery ByClientIds(int[] clientIds);

        IClockEmployeePunchQuery ByEmployeeId(int employeeId);
        IClockEmployeePunchQuery ByEmployeeIds(IEnumerable<int> employeeId);

        IClockEmployeePunchQuery ByDates(DateTime? startDate, DateTime? endDate, bool attemptToUseShiftDate = false);

        IClockEmployeePunchQuery ByCostCenter(int? costCenterId);
        IClockEmployeePunchQuery OrderByRawPunch(SortDirection direction);
        IClockEmployeePunchQuery ByEmployeeHireDateBefore(DateTime hireDate);
        IClockEmployeePunchQuery ByClockEmployeePunchId(int clockEmployeePunchId);
        IClockEmployeePunchQuery ByClockEmployeePunchIdList(int[] clockEmployeePunchIdList);
        IClockEmployeePunchQuery ByClockEmployeePunchIdList(IEnumerable<int> clockEmployeePunchIdList);
        IClockEmployeePunchQuery ByNonNullPunchLocations();
        IClockEmployeePunchQuery ByClientDepartmentIds(List<int?> clientDepartmentIds);
    }
}
