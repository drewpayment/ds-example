using System.Collections.Generic;
using System.Linq;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class GroupScheduleQuery : Query<GroupSchedule, IGroupScheduleQuery>, IGroupScheduleQuery
    {
        #region Constructor

        /// <summary>
        /// Intitializes a new <see cref="GroupScheduleQuery"/>.
        /// </summary>
        /// <param name="data">Tax data the query will be performed on.</param> 
        /// <param name="resultFactory">Result factory used to create & execute query results. If null, a 
        /// <see cref="BasicQueryResultFactory"/> will be used.
        /// </param>
        public GroupScheduleQuery(IEnumerable<GroupSchedule> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        #endregion

        /// <summary>
        /// Filter by matching id.
        /// </summary>
        /// <param name="groupScheduleId">The ID of a specific group schedule.</param>
        /// <returns></returns>
        IGroupScheduleQuery IGroupScheduleQuery.ByGroupScheduleId(int groupScheduleId)
        {
            FilterBy(x => x.GroupScheduleId == groupScheduleId);
            return this;
        }

        /// <summary>
        /// Filter results by client id.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns></returns>
        IGroupScheduleQuery IGroupScheduleQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }

        /// <summary>
        /// Filters by schedules that are for the specified groups.
        /// </summary>
        /// <param name="scheduleGroupIds">ID(s) of groups to filter schedules by.</param>
        /// <returns></returns>
        IGroupScheduleQuery IGroupScheduleQuery.ByScheduleGroupIds(IEnumerable<int> scheduleGroupIds)
        {
            if(scheduleGroupIds != null)
                FilterBy(x => x.GroupScheduleShifts.Any(y => scheduleGroupIds.Contains(y.ScheduleGroupId)));

            return this;
        }

        /// <summary>
        /// Filter results by CostCenter id.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="costCenterId">The client CostCenter id.</param>
        /// <returns></returns>
        IGroupScheduleQuery IGroupScheduleQuery.ByCostCenterId(int costCenterId)
        {
            FilterBy(x => x.GroupScheduleShifts.Select(y => y.ScheduleGroup.ClientCostCenterId).Contains(costCenterId));
            return this;
        }

        /// <summary>
        /// Filter results by isActive
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        IGroupScheduleQuery IGroupScheduleQuery.ByIsActive(bool isActive)
        {
            FilterBy(x => x.IsActive == isActive);
            return this;
        }
    }
}
