using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class EmployeeDefaultShiftQuery : Query<EmployeeDefaultShift, IEmployeeDefaultShiftQuery>, IEmployeeDefaultShiftQuery
    {
        #region Constructor

        /// <summary>
        /// Intitializes a new <see cref="GroupScheduleQuery"/>.
        /// </summary>
        /// <param name="data">Tax data the query will be performed on.</param>
        /// <param name="resultFactory">Result factory used to create & execute query results. If null, a 
        /// <see cref="BasicQueryResultFactory"/> will be used.
        /// </param>
        public EmployeeDefaultShiftQuery(IEnumerable<EmployeeDefaultShift> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        #endregion

        IEmployeeDefaultShiftQuery IEmployeeDefaultShiftQuery.ByEmployeesIds(IEnumerable<int> employeeIds)
        {
            FilterBy(x => employeeIds.Contains(x.EmployeeId));
            return this;
        }

        IEmployeeDefaultShiftQuery IEmployeeDefaultShiftQuery.ByGroupScheduleId(int groupScheduleId)
        {
            FilterBy(x => x.GroupScheduleShift.GroupScheduleId == groupScheduleId);
            return this;
        }

        IEmployeeDefaultShiftQuery IEmployeeDefaultShiftQuery.ByScheduleGroupId(int scheduleGroupId)
        {
            FilterBy(x => x.GroupScheduleShift.ScheduleGroupId == scheduleGroupId);
            return this;
        }
    }
}
