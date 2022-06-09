using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ClockEmployeePunchQuery : Query<ClockEmployeePunch, IClockEmployeePunchQuery>, IClockEmployeePunchQuery
    {
        public ClockEmployeePunchQuery(IEnumerable<ClockEmployeePunch> data) : base(data)
        {
        }

        IClockEmployeePunchQuery IClockEmployeePunchQuery.ByClockEmployeePunchId(int clockEmployeePunchId)
        {
            FilterBy(x => x.ClockEmployeePunchId == clockEmployeePunchId);
            return this;
        }

        IClockEmployeePunchQuery IClockEmployeePunchQuery.ByClockEmployeePunchIdList(int[] clockEmployeePunchIdList)
        {
            FilterBy(x => clockEmployeePunchIdList.Contains(x.ClockEmployeePunchId));
            return this;
        }

        IClockEmployeePunchQuery IClockEmployeePunchQuery.ByClockEmployeePunchIdList(IEnumerable<int> clockEmployeePunchIdList)
        {
            if (clockEmployeePunchIdList != null)
            {
                this.FilterBy(x => clockEmployeePunchIdList.Contains(x.ClockEmployeePunchId));
            }
            //FilterBy(x => clockEmployeePunchIdList.Any(n => n == x.ClockEmployeePunchId));
            return this;
        }

        public IClockEmployeePunchQuery ByClientId(int clientId)
        {
            FilterBy(p => p.ClientId.Equals(clientId));

            return this;
        }

        public IClockEmployeePunchQuery ByClientIds(int[] clientIds)
        {
            if (clientIds != null)
            {
                if (clientIds.Length == 1)
                {
                    var clientId = clientIds[0];
                    this.FilterBy(x => x.ClientId == clientId);
                }
                else
                {
                    this.FilterBy(x => clientIds.Any(n => n == x.ClientId));
                }
            }
            return this;
        }

        public IClockEmployeePunchQuery ByEmployeeId(int employeeId)
        {
            FilterBy(x => x.EmployeeId == employeeId);
            return this;
        }

        public IClockEmployeePunchQuery ByEmployeeIds(IEnumerable<int> employeeIds)
        {
            if (employeeIds != null)
            {
                this.FilterBy(x => employeeIds.Contains(x.EmployeeId));
            }

            return this;
        }

        public IClockEmployeePunchQuery ByDates(DateTime? startDate, DateTime? endDate, bool attemptToUseShiftDate)
        {
            if(startDate != null && endDate != null)
            {
                if (attemptToUseShiftDate)
                {
                    // Ensure that startDate/endDate begin/end at the earliest/latest possible times.
                    var actualStartDate = new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day);
                    var actualEndDate = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day).ToEndOfDay();
                    FilterBy(x => (x.ShiftDate ?? x.ModifiedPunch) >= actualStartDate && (x.ShiftDate ?? x.ModifiedPunch) <= actualEndDate);
                    //FilterBy(x => (x.ShiftDate ?? x.ModifiedPunch) >= startDate && (x.ShiftDate ?? x.ModifiedPunch) <= endDate);
                }
                else
                {
                    FilterBy(x => x.ModifiedPunch != null && (x.ModifiedPunch >= startDate && x.ModifiedPunch <= endDate));
                }
            }

            return this;
        }

        public IClockEmployeePunchQuery ByEmployeeHireDateBefore(DateTime hireDate)
        {
            FilterBy(x => (x.Employee.RehireDate ?? x.Employee.HireDate) <= hireDate);
            return this;
        }

        IClockEmployeePunchQuery IClockEmployeePunchQuery.ByCostCenter(int? costCenterId)
        {
            FilterBy(x => x.ClientCostCenterId == costCenterId);
            return this;
        }

        IClockEmployeePunchQuery IClockEmployeePunchQuery.OrderByRawPunch(SortDirection direction)
        {
            OrderBy(p => p.RawPunch, direction);
            return this;
        }

        IClockEmployeePunchQuery IClockEmployeePunchQuery.ByNonNullPunchLocations()
        {
            FilterBy(p => p.ClockEmployeePunchLocation != null );
            return this;
        }

        IClockEmployeePunchQuery IClockEmployeePunchQuery.ByClientDepartmentIds(List<int?> clientDepartmentIds)
        {
            FilterBy(x => clientDepartmentIds.Contains(x.ClientDepartmentId));
            return this;
        }
    }
}