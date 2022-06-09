using Dominion.Core.Services.Dto.Employee;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.Labor;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.Utility.Containers;
using Dominion.Utility.OpResult;
using System.Collections.Generic;
using System.Linq;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public class ClockSyncProvider : IClockSyncProvider
    {
        private readonly IBusinessApiSession _session;

        internal IClockSyncProvider Self { get; set; }

        public ClockSyncProvider(IBusinessApiSession session)
        {
            Self = this;

            _session = session;
        }

        public IOpResult<IEnumerable<EmployeeDto>> UpdateClockEmployeesGeofence(IEnumerable<EmployeeDto> employees)
        {
            var result = new OpResult<IEnumerable<EmployeeDto>>();

            List<int> employeeIds = new List<int>();
            int changedCount = 0;

            foreach (var employee in employees)
            {
                employeeIds.Add(employee.EmployeeId);
            }

            var employeePay = _session.UnitOfWork.PayrollRepository
                .QueryEmployeePay()
                .ByEmployeeIds(employeeIds)
                .ExecuteQueryAs(x => new EmployeeDto()
                {
                    ClockEmployee = new ClockEmployeeDto()
                    {
                        ClockClientTimePolicyId = x.ClockEmployee.ClockClientTimePolicyId,
                        GeofenceEnabled = x.ClockEmployee.GeofenceEnabled,
                    },
                    EmployeeId = x.EmployeeId
                }).ToList();

            foreach (var employee in employeePay)
            {
                var tmpEmployee = employees.Where(x => x.EmployeeId == employee.EmployeeId).First();

                var clockEmployeeEntity = new ClockEmployee
                {
                    GeofenceEnabled = tmpEmployee.ClockEmployee.GeofenceEnabled,
                    EmployeeId = employee.EmployeeId,
                };

                var changed = new PropertyList<ClockEmployee>();
                if (employee.ClockEmployee.GeofenceEnabled != tmpEmployee.ClockEmployee.GeofenceEnabled)
                    changed.Include(e => e.GeofenceEnabled);

                // check if anything changed
                if (changed.Any())
                {
                    _session.UnitOfWork.RegisterModified(clockEmployeeEntity, changed);
                    changedCount++;
                }
            }

            if(changedCount > 0) _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }
    }
}