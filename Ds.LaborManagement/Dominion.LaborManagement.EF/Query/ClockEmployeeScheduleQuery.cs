using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ClockEmployeeScheduleQuery : Query<ClockEmployeeSchedule, IClockEmployeeScheduleQuery>, IClockEmployeeScheduleQuery
    {
        #region Constructor

        /// <summary>
        /// Intitializes a new <see cref="GroupScheduleQuery"/>.
        /// </summary>
        /// <param name="data">Tax data the query will be performed on.</param>
        /// <param name="resultFactory">Result factory used to create & execute query results. If null, a 
        /// <see cref="BasicQueryResultFactory"/> will be used.
        /// </param>
        public ClockEmployeeScheduleQuery(IEnumerable<ClockEmployeeSchedule> data, IQueryResultFactory resultFactory= null)
            : base(data, resultFactory)
        {
        }

        #endregion

        IClockEmployeeScheduleQuery IClockEmployeeScheduleQuery.ByClientId(int clientId)
        {
            this.FilterBy(x => x.ClientId.Equals(clientId));
            return this;
        }
        IClockEmployeeScheduleQuery IClockEmployeeScheduleQuery.ByClockClientTimePolicyId(int policyId)
        {
            this.FilterBy(x => x.ClockClientTimePolicyId.HasValue && x.ClockClientTimePolicyId == policyId);
            return this;
        }

        IClockEmployeeScheduleQuery IClockEmployeeScheduleQuery.ByEventDateRange(DateTime startDate, DateTime endDate)
        {
            this.FilterBy(x => x.EventDate >= startDate && x.EventDate <= endDate);
            return this;
        }

        IClockEmployeeScheduleQuery IClockEmployeeScheduleQuery.ByPunchTime(DateTime punchTime, short startGrace = 0, short endGrace = 0)
        {
            this.FilterBy(x => (x.StartTime1.HasValue && x.EndTime1.HasValue && (DbFunctions.AddMinutes(x.EndTime1, endGrace) >= punchTime && DbFunctions.AddMinutes(x.StartTime1, startGrace * -1) <= punchTime)) || (x.StartTime2.HasValue && x.EndTime2.HasValue && (DbFunctions.AddMinutes(x.EndTime2, endGrace) >= punchTime && DbFunctions.AddMinutes(x.StartTime2, startGrace * -1) <= punchTime)) || (x.StartTime3.HasValue && x.EndTime3.HasValue && (DbFunctions.AddMinutes(x.EndTime3, endGrace) >= punchTime && DbFunctions.AddMinutes(x.StartTime3, startGrace * -1) <= punchTime)));
            return this;
        }

        IClockEmployeeScheduleQuery IClockEmployeeScheduleQuery.ByEmployeeId(int employeeId)
        {
            FilterBy(x => x.EmployeeId == employeeId);
            return this;
        }

        IClockEmployeeScheduleQuery IClockEmployeeScheduleQuery.ByEmployeeIds(IEnumerable<int> employeeIds)
        {
            this.FilterBy(x => employeeIds.Contains(x.EmployeeId));
            return this;
        }

        IClockEmployeeScheduleQuery IClockEmployeeScheduleQuery.ByScheduleGroupId(int scheduleGroupId)
        {
            this.FilterBy(x => 
                (x.GroupScheduleShiftId1 != null && x.GroupScheduleShift1.ScheduleGroupId == scheduleGroupId) ||
                (x.GroupScheduleShiftId2 != null && x.GroupScheduleShift2.ScheduleGroupId == scheduleGroupId) ||
                (x.GroupScheduleShiftId3 != null && x.GroupScheduleShift3.ScheduleGroupId == scheduleGroupId));
            return this;
        }

        IClockEmployeeScheduleQuery IClockEmployeeScheduleQuery.ByScheduleGroupsOtherThanId(int scheduleGroupIdToExclude)
        {
            this.FilterBy(x => 
                (x.StartTime1 == null) ||
                (x.GroupScheduleShiftId1 != null && x.GroupScheduleShift1.ScheduleGroupId != scheduleGroupIdToExclude) ||
                (x.GroupScheduleShiftId2 != null && x.GroupScheduleShift2.ScheduleGroupId != scheduleGroupIdToExclude) ||
                (x.GroupScheduleShiftId3 != null && x.GroupScheduleShift3.ScheduleGroupId != scheduleGroupIdToExclude));
            return this;
        }

        /// <summary>
        /// Filters by schedules associated with the specified cost center. Checks
        /// <see cref="ClockEmployeeSchedule.ClientCostCenterId1"/>, 
        /// <see cref="ClockEmployeeSchedule.ClientCostCenterId2"/> and 
        /// <see cref="ClockEmployeeSchedule.ClientCostCenterId3"/>. 
        /// </summary>
        /// <param name="costCenterId"></param>
        /// <returns></returns>
        IClockEmployeeScheduleQuery IClockEmployeeScheduleQuery.ByIsAssociatedWithCostCenterId(int costCenterId)
        {
            FilterBy(x => 
                x.ClientCostCenterId1 == costCenterId ||
                x.ClientCostCenterId2 == costCenterId ||
                x.ClientCostCenterId3 == costCenterId);
            return this;
        }
    }
}
