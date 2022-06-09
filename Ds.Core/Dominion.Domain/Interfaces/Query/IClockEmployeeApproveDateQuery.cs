using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockEmployeeApproveDateQuery : IQuery<ClockEmployeeApproveDate, IClockEmployeeApproveDateQuery>
    {
        /// <summary>
        /// Filters approval status for a single client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClockEmployeeApproveDateQuery ByClientId(int clientId);

        /// <summary>
        /// Filters approval status by one or more employees.
        /// </summary>
        /// <param name="employeeIds"></param>
        /// <returns></returns>
        IClockEmployeeApproveDateQuery ByEmployees(IEnumerable<int> employeeIds);

        /// <summary>
        /// Filters approval status for dates on or after a given day.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        IClockEmployeeApproveDateQuery ByEventDateFrom(DateTime fromDate);
        IClockEmployeeApproveDateQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters approval status for dates on or before a given day.
        /// </summary>
        /// <param name="toDate"></param>
        /// <returns></returns>
        IClockEmployeeApproveDateQuery ByEventDateTo(DateTime toDate);

        /// <summary>
        /// Filters records by clock client note id.
        /// </summary>
        /// <param name="clockClientNoteId"></param>
        /// <returns></returns>
        IClockEmployeeApproveDateQuery ByClockClientNoteId(int clockClientNoteId);

        /// <summary>
        /// Filters records by approval status.
        /// </summary>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        IClockEmployeeApproveDateQuery ByIsApproved(bool isApproved);

        IClockEmployeeApproveDateQuery ByEventDate(DateTime eventDate);
        IClockEmployeeApproveDateQuery ByCostCenterId(int costCenterId);
        IClockEmployeeApproveDateQuery ByNullCostCenter();
    }
}
