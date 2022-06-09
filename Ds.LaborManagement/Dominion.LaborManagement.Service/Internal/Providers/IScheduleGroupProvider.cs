using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.LaborManagement.Dto.GroupScheduling;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public interface    IScheduleGroupProvider
    {
        /// <summary>
        /// This doesn't goto the database directly. This call sets up a call to the database.
        /// There will be different queries based on the type and if using supervisor security.
        /// </summary>
        /// <param name="useSupervisorSecurity">If we need to use supervisor security.</param>
        /// <param name="clientId">The client.</param>
        /// <param name="scheduleGroupType">The type of schedule groups.</param>
        /// <param name="scheduleGroupId">A particular schedule group.</param>
        /// <param name="withScheduleGroupShiftNames">True if you want to include the schedule group shift names (sub-groups)</param>
        /// <returns></returns>
        IOpResult<IEnumerable<GroupScheduleDtos.ScheduleGroupDto>> GetScheduleGroupsByGroupType(
            bool useSupervisorSecurity,
            int clientId,
            ScheduleGroupType scheduleGroupType,
            int? scheduleGroupId = null,
            bool withScheduleGroupShiftNames = false);

        /// <summary>
        /// Getting schedule group information based on cost centers and the user requesting the data.
        /// Schedule groups are wrappers for cost centers (possibly others upcoming).
        /// </summary>
        /// <param name="clientId">The client.</param>
        /// <param name="userId">User if concerned with supervisor security on cost centers.</param>
        /// <param name="scheduleGroupId">A particular schedule group.</param>
        /// <param name="withScheduleGroupShiftNames">True if you want to include the schedule group shift names (sub-groups)</param>
        /// <returns></returns>
        IEnumerable<GroupScheduleDtos.ScheduleGroupDto> GetClientCostCenterScheduleGroups(
            int clientId,
            int? userId = null,
            int? scheduleGroupId = null,
            bool withScheduleGroupShiftNames = false);

        /// <summary>
        /// Returns the schedule groups that are accessible by the current user (eg: if a supervisor will only return
        /// the groups they have supervisor access to). 
        /// </summary>
        /// <returns></returns>
        IOpResult<IEnumerable<int>> GetCurrentUserAccessibleScheduleGroupIds(); 
    }
}
