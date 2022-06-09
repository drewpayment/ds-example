using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.LaborManagement.Dto.GroupScheduling;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Api
{
    public interface IGroupScheduleService
    {
        /// <summary>
        /// Get a list of group schedules for display in a list.
        /// Basic data for selection purposes.
        /// Returns based on the currently selected client.
        /// </summary>
        /// <returns></returns>
        IOpResult<IEnumerable<GroupScheduleDtos.GroupScheduleDto>> GetClientGroupScheduleList(bool? isActive = null);

        /// <summary>
        /// Get a schedule with full group scheduling data.
        /// </summary>
        /// <param name="groupScheduleId">The group schedule you want data for.</param>
        /// <returns></returns>
        IOpResult<GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto> GetGroupSchedule(int groupScheduleId);

        /// <summary>
        /// Get schedule groups based on the client and schedule group type.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="scheduleGroupType">The schedule group type.</param>
        /// <returns></returns>
        IOpResult<IEnumerable<GroupScheduleDtos.ScheduleGroupDto>> GetScheduleGroups(
            ScheduleGroupType scheduleGroupType,
            int? scheduleGroupId,
            bool withScheduleGroupShiftNames = false);

        /// <summary>
        /// Saves or updates a group schedule.
        /// </summary>
        /// <param name="data">The group schedule data.</param>
        /// <returns>A result with the group schedule obj that was constructed from the DTO.</returns>
        IOpResult<GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto> SaveOrUpdateGroupSchedule(GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto data);

        /// <summary>
        /// Get group schedule data with shifts that only have a reference wo the schedule group.
        /// highFix: jay: the scheduleGroupId isn't being checked for security because only scheduleGroupIds the caller has access to should be coming in. If it doesn't then they're hacking the system. So we need to add a security check.
        /// </summary>
        /// <param name="scheduleGroupId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<GroupScheduleDtos.GroupScheduleForSchedulingDto>> GetGroupScheduleForScheduling(int scheduleGroupId);

        /// <summary>
        /// Get the scheduled information based on the parameters passed in.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="scheduleGroupId">The schedule group id.</param>
        /// <param name="scheduleGroupSourceId">The source id of the source group (cost center id).</param>
        /// <returns></returns>
        IOpResult<IEnumerable<EmployeeSchedulesDto>> GetEmployeeScheduleShifts(
            DateTime startDate, 
            DateTime endDate, 
            int scheduleGroupId, 
            int scheduleGroupSourceId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="scheduleGroupSourceId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<EmployeeSchedulesDto>> SaveOrUpdateEmployeeSchedule(EmployeeSchedulesPersistDto data);

        /// <summary>
        /// Get group schedules associated with a given cost-center, if any exist.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientCostCenterId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<IGroupScheduleDto>> GetGroupSchedulesAssociatedWithClientCostCenter(int clientId, int clientCostCenterId);

        /// <summary>
        /// Get group schedules associated with a given cost-center, if any exist.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientCostCenterId"></param>
        /// <returns></returns>
        IOpResult<bool> GetClientCostCenterHasAssociatedGroupSchedule(int clientId, int clientCostCenterId);
    }
}
