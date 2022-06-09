using System;
using System.Collections.Generic;
using Dominion.Core.Dto.LeaveManagement;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query on a set of <see cref="TimeOffRequest"/>(s).
    /// </summary>
    public interface ITimeOffRequestDetailQuery : IQuery<TimeOffRequestDetail, ITimeOffRequestDetailQuery>
    {
        /// <summary>
        /// Filters by time off with the specified status.
        /// </summary>
        /// <param name="status">Status filter by.</param>
        /// <returns></returns>
        ITimeOffRequestDetailQuery ByStatus(TimeOffStatusType status);

        /// <summary>
        /// Filters time off by the specified employees. If id-set is null, filter will be ignored.
        /// Note: Clustered index exists for ClientID | EmployeeID | ClientAccrualID | RequestTimeOffID.
        /// </summary>
        /// <param name="employeeIds">Employees to filter by.</param>
        /// <returns></returns>
        ITimeOffRequestDetailQuery ByEmployees(IEnumerable<int> employeeIds);

        /// <summary>
        /// Filters by time off for a single employee.
        /// Note: Clustered index exists for ClientID | EmployeeID | ClientAccrualID | RequestTimeOffID.
        /// </summary>
        /// <param name="employeeId">Employee to get time off records for.</param>
        /// <returns></returns>
        ITimeOffRequestDetailQuery ByEmployee(int employeeId);

        /// <summary>
        /// Filters time off by accrual policy.
        /// Note: Clustered index exists for ClientID | EmployeeID | ClientAccrualID | RequestTimeOffID.
        /// </summary>
        /// <param name="clientAccrualId">ID of the accrual policy to filter time off by.</param>
        /// <returns></returns>
        ITimeOffRequestDetailQuery ByClientAccrualId(int clientAccrualId);

        /// <summary>
        /// Filters by a specified client.
        /// Note: Clustered index exists for ClientID | EmployeeID | ClientAccrualID | RequestTimeOffID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        ITimeOffRequestDetailQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by requests whose <see cref="TimeOffRequestDetail.RequestDate"/> is greater-than or equal-to the specified
        /// date/time.
        /// </summary>
        /// <param name="fromDate">Date/time request date must be greater than or equal to.</param>
        /// <returns></returns>
        ITimeOffRequestDetailQuery ByDateRangeFrom(DateTime fromDate);

        /// <summary>
        /// Filters by requests whose <see cref="TimeOffRequestDetail.RequestDate"/> is less-than or equal-to the specified
        /// date/time.
        /// </summary>
        /// <param name="toDate">Date/time request date must be less than or equal to.</param>
        /// <returns></returns>
        ITimeOffRequestDetailQuery ByDateRangeTo(DateTime toDate);
        ITimeOffRequestDetailQuery ByTimeOffRequestId(int timeOffRequestId);
    }
}
