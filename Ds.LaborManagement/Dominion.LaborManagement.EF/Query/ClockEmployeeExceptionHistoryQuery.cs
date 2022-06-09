using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ClockEmployeeExceptionHistoryQuery : Query<ClockEmployeeExceptionHistory, IClockEmployeeExceptionHistoryQuery>, IClockEmployeeExceptionHistoryQuery
    {
        #region Constructor

        public ClockEmployeeExceptionHistoryQuery(IEnumerable<ClockEmployeeExceptionHistory> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        /// <summary>
        /// Filters by exception history belonging to a single client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClockEmployeeExceptionHistoryQuery IClockEmployeeExceptionHistoryQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }

        /// <summary>
        /// Filters by exception history occurring on or after a given day.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        IClockEmployeeExceptionHistoryQuery IClockEmployeeExceptionHistoryQuery.ByEventDateFrom(DateTime fromDate)
        {
            FilterBy(x => x.EventDate >= fromDate);
            return this;
        }

        /// <summary>
        /// Filters by exception history occurring on or before a given day.
        /// </summary>
        /// <param name="toDate"></param>
        /// <returns></returns>
        IClockEmployeeExceptionHistoryQuery IClockEmployeeExceptionHistoryQuery.ByEventDateTo(DateTime toDate)
        {
            FilterBy(x => x.EventDate <= toDate);
            return this;
        }

        /// <summary>
        /// Filters by exception history belonging to a single employee.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        IClockEmployeeExceptionHistoryQuery IClockEmployeeExceptionHistoryQuery.ByEmployeeId(int employeeId)
        {
            FilterBy(x => x.EmployeeId == employeeId);
            return this;
        }

        /// <summary>
        /// Filters by exception history belonging to punchIds
        /// </summary>
        /// <param name="punchIds"></param>
        /// <returns></returns>
        IClockEmployeeExceptionHistoryQuery IClockEmployeeExceptionHistoryQuery.ByPunchIds(IEnumerable<int> punchIds)
        {
            if (punchIds != null)
            {
                this.FilterBy(x => punchIds.Contains(x.ClockEmployeePunchId.Value));
            }
            return this;
        }

        /// <summary>
        /// Filters by exception history belonging to exceptionIds
        /// </summary>
        /// <param name="exceptionIds"></param>
        /// <returns></returns>
        IClockEmployeeExceptionHistoryQuery IClockEmployeeExceptionHistoryQuery.ByExceptionTypeIds(IEnumerable<ClockExceptionType> exceptionIds)
        {
            if (exceptionIds != null)
            {
                this.FilterBy(x => exceptionIds.Contains(x.ClockExceptionTypeId.Value));
            }
            return this;
        }
    }
}