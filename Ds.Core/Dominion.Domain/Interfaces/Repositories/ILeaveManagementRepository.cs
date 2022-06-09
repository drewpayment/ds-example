using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.LeaveManagement;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Interfaces.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository providing query access to Leave Management data.;
    /// </summary>
    public interface ILeaveManagementRepository : IRepository, IDisposable
    {
        /// <summary>
        /// Calls the 'spGetEmployeeWithLeaveManagementGrid' stored procedure.
        /// We're moving the employee leave management page to ESS.
        /// This is the sproc used to populate the grid showing the recent activity.
        /// </summary>
        GetActiveTimeOffEventsSprocDto TimeOffEventsSproc(EmployeeActiveTimeOffEventsArgsDto args);

        /// <summary>
        /// Calls the 'spGetEmployeeLeaveManagementListByClientID' stored procedure.
        /// Gets list of policies for client/employees.
        /// </summary>
        IEnumerable<TimeOffPolicyDto> TimeOffPolicyListSproc(EmployeeTimeOffPolicyListArgsDto args);

        /// <summary>
        /// Gets details about a single time off request.
        /// </summary>
        RequestTimeOffDetailsSprocDto RequestTimeOffDetailsSproc(RequestTimeOffDetailsArgsDto args);

        /// <summary>
        /// Creates a new query on Time-Off request data.
        /// </summary>
        /// <returns></returns>
        ITimeOffRequestDetailQuery QueryTimeOffRequestDetails();

        /// <summary>
        /// Creates a new query on employee benefit (vacation/holiday) data.
        /// </summary>
        /// <returns></returns>
        IEmployeeBenefitQuery QueryEmployeeBenefits();
        IEnumerable<ClientAccrualDto> GetEmployeeActiveLeaveManagementListByClientId(int clientId, int employeeId);
        EmployeeLeaveManagementLastPayrollDto GetEmployeeLeaveManagementLastPayrollDates(int clientId, PayFrequencyType payFrequencyId);
        IEnumerable<EmployeeLeaveManagementInfoDto> GetEmployeeLeaveManagementInfo(int clientId, int userId, int filterType, int filterId, int eeStatusFilter, PayFrequencyType payFrequencyId,
            int? planId = null, int? statusId = null, int? employeeId = null, DateTime? startDate = null, DateTime? endDate = null, bool? requireLeaveManagement = true);
        InsertRequestTimeOffResultDto InsertRequestTimeOff(int clientAccrualId, int employeeId, DateTime requestFrom, DateTime requestUntil, int modifiedBy, int amountInOneDay, string requestorNotes);
        InsertRequestTimeOffResultDto InsertRequestTimeOffDetail(int requestTimeOffId, DateTime requestFrom, int modifiedBy, decimal amountInOneDay, DateTime fromTime, DateTime toTime);
        IEnumerable<GetLeaveManagementHolidaysResultDto> GetLeaveManagementHolidays(int employeeId, DateTime from, DateTime until, int clientAccrualId);
        IEnumerable<TimeOffRequestDto> GetOverrlappingLeaveManagementRequests(int employeeId, DateTime from, DateTime until, int requestTimeOffId, decimal hours, int clientAccrualId);
        LeaveManagementMinimumHoursResultDto GetLeaveManagementMinimumRequestAllowed(int clientAccrualId, int clientId);
        EmployeeLeaveManagementBalanceDto GetEmployeeLeaveManagementBalance(int employeeId, int clientAccrualId, DateTime dateForComparison, int? requestTimeOffId = null);
    }
}
