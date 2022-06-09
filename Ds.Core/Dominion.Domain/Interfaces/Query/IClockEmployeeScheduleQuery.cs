using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockEmployeeScheduleQuery : IQuery<ClockEmployeeSchedule, IClockEmployeeScheduleQuery>
    {
        IClockEmployeeScheduleQuery ByClientId(int clientId);
        IClockEmployeeScheduleQuery ByClockClientTimePolicyId(int policyId);
        IClockEmployeeScheduleQuery ByEventDateRange(DateTime startDate, DateTime endDate);
        IClockEmployeeScheduleQuery ByEmployeeId(int employeeId);
        IClockEmployeeScheduleQuery ByEmployeeIds(IEnumerable<int> employeeIds);

        IClockEmployeeScheduleQuery ByScheduleGroupId(int scheduleGroupId);
        IClockEmployeeScheduleQuery ByScheduleGroupsOtherThanId(int scheduleGroupIdToExclude);

        /// <summary>
        /// Filters by schedules associated with the specified cost center. Checks
        /// <see cref="ClockEmployeeSchedule.ClientCostCenterId1"/>, 
        /// <see cref="ClockEmployeeSchedule.ClientCostCenterId2"/> and 
        /// <see cref="ClockEmployeeSchedule.ClientCostCenterId3"/>. 
        /// </summary>
        /// <param name="costCenterId"></param>
        /// <returns></returns>
        IClockEmployeeScheduleQuery ByIsAssociatedWithCostCenterId(int costCenterId);
        IClockEmployeeScheduleQuery ByPunchTime(DateTime punchTime, short startGrace, short endGrace);
    }
}
