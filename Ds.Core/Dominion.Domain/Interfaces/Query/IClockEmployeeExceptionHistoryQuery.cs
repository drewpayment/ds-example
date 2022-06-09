using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockEmployeeExceptionHistoryQuery : IQuery<ClockEmployeeExceptionHistory, IClockEmployeeExceptionHistoryQuery>
    {
        /// <summary>
        /// Filters by exception history belonging to a single client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClockEmployeeExceptionHistoryQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by exception history occurring on or after a given day.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        IClockEmployeeExceptionHistoryQuery ByEventDateFrom(DateTime fromDate);

        /// <summary>
        /// Filters by exception history occurring on or before a given day.
        /// </summary>
        /// <param name="toDate"></param>
        /// <returns></returns>
        IClockEmployeeExceptionHistoryQuery ByEventDateTo(DateTime toDate);

        /// <summary>
        /// Filters by exception history belonging to a single employee.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        IClockEmployeeExceptionHistoryQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters by exception history belonging punch Ids
        /// </summary>
        /// <param name="punchIds"></param>
        /// <returns></returns>
        IClockEmployeeExceptionHistoryQuery ByPunchIds(IEnumerable<int> punchIds);

        /// <summary>
        /// Filters by exception history belonging exception Ids
        /// </summary>
        /// <param name="exceptionyIds"></param>
        /// <returns></returns>
        IClockEmployeeExceptionHistoryQuery ByExceptionTypeIds(IEnumerable<ClockExceptionType> exceptionIds);
    }
}
