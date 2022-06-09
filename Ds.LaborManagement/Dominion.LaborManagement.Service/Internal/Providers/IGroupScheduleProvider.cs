using System.Collections.Generic;
using Dominion.Domain.Entities.Labor;
using Dominion.LaborManagement.Dto.GroupScheduling;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public interface IGroupScheduleProvider
    {
        /// <summary>
        /// Gets the raw data and flattens it.
        /// </summary>
        /// <param name="groupScheduleId">The id of the group sched.</param>
        /// <returns></returns>
        IOpResult<GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto> GetGroupScheduleGroupedData(int groupScheduleId);

        /// <summary>
        /// Saves or updates a group schedule.
        /// </summary>
        /// <param name="data">The group schedule data.</param>
        /// <returns>A result with the group schedule obj that was constructed from the DTO.</returns>
        IOpResult<GroupSchedule> SaveOrUpdateGroupSchedule(GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduleGroupId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<GroupScheduleDtos.GroupScheduleForSchedulingDto>> GetGroupScheduleForScheduling(int scheduleGroupId);

        /// <summary>
        /// Sets each <see cref="GroupScheduleDtos.ScheduleGroupDto.IsReadOnly"/> property based on if the group is accessible 
        /// by the current user or not.
        /// </summary>
        /// <param name="schedule">Schedule to update group accessibility on.</param>
        /// <param name="accessibleScheduleGroupIds">Schedule group ID(s) the current user has access to.</param>
        /// <returns></returns>
        IOpResult<GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto> UpdateScheduleGroupAccess(GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto schedule, IEnumerable<int> accessibleScheduleGroupIds);
    }
}
