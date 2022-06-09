using System;
using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class EmployeeSchedulePreviewQuery : Query<EmployeeSchedulePreview, IEmployeeSchedulePreviewQuery>, IEmployeeSchedulePreviewQuery
    {
        #region Constructor

        /// <summary>
        /// Intitializes a new <see cref="GroupScheduleQuery"/>.
        /// </summary>
        /// <param name="data">Tax data the query will be performed on.</param>
        /// <param name="resultFactory">Result factory used to create & execute query results. If null, a 
        /// <see cref="BasicQueryResultFactory"/> will be used.
        /// </param>
        public EmployeeSchedulePreviewQuery(IEnumerable<EmployeeSchedulePreview> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        #endregion

        IEmployeeSchedulePreviewQuery IEmployeeSchedulePreviewQuery.ByEventDateRange(DateTime startDate, DateTime endDate)
        {
            this.FilterBy(x => x.EventDate >= startDate && x.EventDate <= endDate);
            return this;
        }

        IEmployeeSchedulePreviewQuery IEmployeeSchedulePreviewQuery.ByScheduleGroupId(int scheduleGroupId)
        {
            this.FilterBy(x => x.GroupScheduleShift.ScheduleGroupId == scheduleGroupId);
            return this;
        }

        IEmployeeSchedulePreviewQuery IEmployeeSchedulePreviewQuery.ByScheduleGroupsOtherThanId(int scheduleGroupIdToExclude)
        {
            this.FilterBy(x => x.GroupScheduleShift.ScheduleGroupId != scheduleGroupIdToExclude);
            return this;
        }

        IEmployeeSchedulePreviewQuery IEmployeeSchedulePreviewQuery.ByEmployeeIds(IEnumerable<int> employeeIds)
        {
            this.FilterBy(x => employeeIds.Contains(x.EmployeeId));
            return this;
        }
    }
}
