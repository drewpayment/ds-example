using System.Collections.Generic;

using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class GroupScheduleShiftQuery : Query<GroupScheduleShift, IGroupScheduleShiftQuery>, IGroupScheduleShiftQuery
    {
        #region Constructor

        /// <summary>
        /// Intitializes a new <see cref="GroupScheduleQuery"/>.
        /// </summary>
        /// <param name="data">Tax data the query will be performed on.</param>
        /// <param name="resultFactory">Result factory used to create & execute query results. If null, a 
        /// <see cref="BasicQueryResultFactory"/> will be used.
        /// </param>
        public GroupScheduleShiftQuery(IEnumerable<GroupScheduleShift> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        #endregion

        /// <summary>
        /// Filter by matching id.
        /// </summary>
        /// <param name="groupScheduleId">The ID of a specific group schedule.</param>
        /// <returns></returns>
        IGroupScheduleShiftQuery IGroupScheduleShiftQuery.ByGroupScheduleId(int groupScheduleId)
        {
            FilterBy(x => x.GroupScheduleId == groupScheduleId);
            return this;
        }

        IGroupScheduleShiftQuery IGroupScheduleShiftQuery.ByScheduleGroupId(int scheduleGroupId)
        {
            FilterBy(x => x.ScheduleGroup.ScheduleGroupId == scheduleGroupId);
            return this;
        }

        IGroupScheduleShiftQuery IGroupScheduleShiftQuery.ByGroupScheduleIsActive(bool isActive)
        {
            FilterBy(x => x.GroupSchedule.IsActive == isActive);
            return this;
        }

        IGroupScheduleShiftQuery IGroupScheduleShiftQuery.ByIsNotDeleted()
        {
            FilterBy(x => !x.IsDeleted);
            return this;
        }
    }
}
