using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IGroupScheduleQuery : IQuery<GroupSchedule, IGroupScheduleQuery>
    {
        /// <summary>
        /// pass:1
        /// Filter by matching id.
        /// </summary>
        /// <param name="groupScheduleId">The ID of a specific group schedule.</param>
        /// <returns></returns>
        IGroupScheduleQuery ByGroupScheduleId(int groupScheduleId);

        /// <summary>
        /// pass:1
        /// Filter results by client id.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns></returns>
        IGroupScheduleQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by schedules that are for the specified groups.
        /// </summary>
        /// <param name="scheduleGroupIds">ID(s) of groups to filter schedules by.</param>
        /// <returns></returns>
        IGroupScheduleQuery ByScheduleGroupIds(IEnumerable<int> scheduleGroupIds);

        /// <summary>
        /// Filter results by CostCenter id.
        /// </summary>
        /// <param name="costCenterId">The client CostCenter id.</param>
        /// <returns></returns>
        IGroupScheduleQuery ByCostCenterId(int costCenterId);

        /// <summary>
        /// Filter results by isActive
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        IGroupScheduleQuery ByIsActive(bool isActive);
    }
}
