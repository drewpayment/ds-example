using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IGroupScheduleShiftQuery : IQuery<GroupScheduleShift, IGroupScheduleShiftQuery>
    {
        /// <summary>
        /// highfix: jay: need to test.
        /// Filter by matching group schedule id.
        /// </summary>
        /// <param name="groupScheduleId">The ID of a specific group schedule.</param>
        /// <returns></returns>
        IGroupScheduleShiftQuery ByGroupScheduleId(int groupScheduleId);

        IGroupScheduleShiftQuery ByScheduleGroupId(int scheduleGroupId);

        IGroupScheduleShiftQuery ByGroupScheduleIsActive(bool isActive);

        IGroupScheduleShiftQuery ByIsNotDeleted();

    }
}
