using System;
using System.Collections.Generic;
using System.Data;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Geofence;
using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Employee;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.Clock.Misc;
using Dominion.LaborManagement.Dto.Department;
using Dominion.LaborManagement.Dto.EmployeeLaborManagement;
using Dominion.LaborManagement.Dto.JobCosting;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.LaborManagement.Dto.Sproc.JobCosting;
using Dominion.Utility.Mapping;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Api
{
    /// <summary>
    /// Interface for a service that will provide the ability to standard CRUD actions to a controller
    /// </summary>
    public interface IClockService
    {
        /// <summary>
        /// This replaces Sproc : [dbo].[spGetClockClientRulesByEmployeeID]
        /// Returns ClockClientRules and a few supplemental fields that contains additional information.
        /// If an employeeId is not provided, all results for active employees will be returned.
        /// If updating legacy code, ensure this is the best option.  If only needing a list of clockemployees, 
        /// use GetClockEmployees, etc
        /// </summary>
        /// <param name="employeeId">employee Id</param>
        /// <param name="clientId">client Id</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockClientRulesSummaryDto>> GetClockClientRulesSummary(int? employeeId = null, int? clientId = null);

        /// <summary>
        /// returns a ClockClientRules object.  
        /// </summary>
        /// <typeparam name="TMapper">Custom mapping object that allows for implementing specific properties</typeparam>
        /// <param name="mapper"></param>
        /// <param name="employeeId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockClientRulesSummaryDto>> GetClockClientRulesSummary<TMapper>(TMapper mapper, int? employeeId = null, int? clientId = null)
            where TMapper : ExpressionMapper<EmployeePay, ClockClientRulesSummaryDto>;

        /// <summary>
        /// Looks at the ClientAccountOptions to determine if the rule for whether the client requires their
        /// employees to select / enter a cost center when submitting a punch.  
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<int> GetCostCenterRequiredValue(int clientId);

        /// <summary>
        /// returns all employee punches based on the employee ID
        /// </summary>
        /// <param name="employeeId">Id for the employee</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockEmployeePunchDto>> GetEmployeePunches(int employeeId);

        /// <summary>
        /// Returns all employee punches for an employee in a given date range
        /// </summary>
        /// <param name="employeeId">employee's id number</param>
        /// <param name="startDate">start date for the query</param>
        /// <param name="endDate">start date for the query</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockEmployeePunchDto>> GetEmployeePunches(int employeeId, DateTime startDate,
            DateTime endDate);

        /// <summary>
        /// Returns all employee punches for an employee for a given shiftDate
        /// </summary>
        /// <param name="employeeId">employee's id number</param>
        /// <param name="shiftDate">start date for the query</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockEmployeePunchDto>> GetEmployeePunches(int employeeId, DateTime shiftDate);

        /// <summary>
        /// Gets departments that can be selected in the clock.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClientDepartmentDto>> GetAvailableDepartments(int clientId);
        IOpResult<CheckPunchTypeResultDto> GetNextPunchTypeDetail(int employeeId, DateTime? punchTime = null, string ipAddress = null, bool includeEmployeeClockConfig = false, bool isHwClockPunch = false);

        /// <summary>
        /// Attempts to add the given punch request to the system.
        /// </summary>
        /// <param name="request">The punch request.</param>
        /// <param name="ipAddress">The IP Address the user is punching from.</param>
        /// <returns></returns>
        IOpResult<RealTimePunchResultDto> ProcessRealTimePunch(RealTimePunchRequest request, string ipAddress);

        /// <summary>
        /// Attempts to add the given punch request to the system.
        /// </summary>
        /// <param name="request">The punch request.</param>
        /// <param name="ipAddress">The IP Address the user is punching from.</param>
        /// <returns></returns>
        IOpResult<RealTimePunchResultDto> ProcessRealTimePunchFromClock(RealTimePunchRequest request);

        /// <summary>
        /// Attempts to add the given pair of punch requests to the system. This operation is atomic, unlike the ProcessBatchPunches method. 
        /// If one punch fails, they both will fail and will not be added to the system.
        /// </summary>
        /// <param name="first">The first punch.</param>
        /// <param name="second">The second punch.</param>
        /// <param name="ipAddress">The IP Address the user is punching from.</param>
        /// <returns></returns>
        IOpResult<RealTimePunchPairResultDto> ProcessRealTimePunchPair(RealTimePunchPairRequest request, string ipAddress);

        /// <summary>
        /// Attempts to add the given pair of punch requests to the system. This operation is atomic, unlike the ProcessBatchPunches method. 
        /// If one punch fails, they both will fail and will not be added to the system.
        /// </summary>
        /// <param name="first">The first punch.</param>
        /// <param name="second">The second punch.</param>
        /// <param name="ipAddress">The IP Address the user is punching from.</param>
        /// <returns></returns>
        IOpResult<RealTimePunchPairResultDto> ProcessRealTimePunchPairWithHours(RealTimePunchPairRequestWithHours request, string ipAddress);

        /// <summary>
        /// This method is used to obtain the most recent pay period ended for an employee by calling into SQL
        /// Stored Procedure sp.GetClockEmployeePayPeriodEnded  
        /// </summary>
        /// <param name="clientId">Id for client for the employee</param>
        /// <param name="employeeId">Id for employee</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockEmployeePayPeriodEndedDto>> ClockEmployeePayPeriodEndedSproc(int clientId, int employeeId);

        /// <summary>
        /// Determines if the given employee is allowed to punch from the given IP Address.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        IOpResult<CanEmployeePunchDto> CanEmployeePunchFromIp(int employeeId, string ipAddress);

        /// <summary>
        /// Gets the list of enabled job costings for the client with the given ID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClientJobCostingDto>> GetClientJobCostingList(int clientId);

        /// <summary>
        /// Gets the list of available assignments for the given parameters.
        /// </summary>
        /// <param name="clientId">The ID of the client to search.</param>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <param name="clientJobCostingId">The ID of the job costing to get available assignments for.</param>
        /// <param name="commaSeparatedParentJobCostingIds">The comma separated list of Job Costing IDs that represent the set of parents for the job costing.</param>
        /// <param name="commaSeparatedParentJobCostingAssignmentIds">The comma separated list of Assignment IDs that represent the set of associations for the parents of the job costing.</param>
        /// <param name="searchText">The search for the assignments.</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClientJobCostingAssignmentSprocDto>> GetEmployeeJobCostingAssignmentList(int clientId, int employeeId, int clientJobCostingId, string commaSeparatedParentJobCostingIds, string commaSeparatedParentJobCostingAssignmentIds, string searchText);

        /// <summary>
        /// Gets the list of available assignments for each given job costing.
        /// </summary>
        /// <param name="clientId">The ID of the client to get job costings for.</param>
        /// <param name="employeeId">The ID of the employee to get job costings for.</param>
        /// <param name="jobCostings">The list of DTOs that represent the job costings that the available assignments should be retrieved for.</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClientJobCostingListDto>> GetEmployeeJobCostingAssignments(int clientId, int employeeId, AssociatedClientJobCostingDto[] jobCostings);

        /// <summary>
        /// Checks if the given Employee PIN is available and valid
        /// </summary>
        /// <param name="employeeId">The ID of the employee to check PIN for.</param>
        /// <param name="employeePin">The 4 digit number to check availability and validity for.</param>
        /// <param name="clientId">The ID of the client</param>
        /// <returns></returns>
        IOpResult<bool> CanAssignEmployeePinToEmployee(int employeeId, string employeePin, int clientId);


        IOpResult<PunchTypeItemResult> GetPunchTypeItems(int employeeId);

        IOpResult<ScheduledHoursWorkedResult> GetEmployeeWeeklyHoursWorked(int clientId, int employeeId, DateTime startDate, DateTime endDate);
        IOpResult<IEnumerable<ClockEmployeeOvertimeInformationDto>> GetOvertimeInformationByEmployeeIds(int clientId, DateTime startDate, DateTime endDate, int[] employeeIds);

        IOpResult<IEnumerable<ClientDepartmentDto>> GetEmployeeAvailableDepartments(int clientId, int employeeId);

        IOpResult<InputHoursPunchRequestResult> ProcessInputHoursPunch(ClockEmployeeBenefitDto request, string ipAddess);

        IOpResult<ScheduledHoursWorkedResult> GetInputHoursWorked(int clientId, int employeeId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Checks to see if employee has activity
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        IOpResult<bool> HasEmployeeClockActivity(int employeeId);
        IOpResult<IEnumerable<ClockTimeCardDto>> GetEmployeeWeeklyHoursWorkedTimeCard(int clientId, int employeeId);
        IOpResult<IEnumerable<ClockTimeCardDto>> GetEmployeeHoursWorkedTimeCard(int clientId, int employeeId, int payrollId, DateTime datWeeklyActivity, DateTime datEndingPayPeriod);
        IOpResult<ClockEmployeePunchDto> GetPunchById(int employeeId, int punchId);
        IOpResult<IEnumerable<ClockEmployeePunchDto>> GetPunchesByIdList(int employeeId, int[] punchId);
        IOpResult<ClientJobCostingListDto> GetEmployeeJobCostingAssignmentsLazy(int clientId, int employeeId, ClientJobCostingCustomDto jobCostingCustom );
        IOpResult<IEnumerable<ClockEmployeeScheduleDto>> GetEmployeeScheduleByEmployeeIdAndDateRange(int clientId, IEnumerable<int> employeeIds, DateTime startDate, DateTime endDate);
        IOpResult<RealTimePunchResultDto> ProcessRealTimePunchAttempt(ClockEmployeePunchAttemptDto request);
        IOpResult<WeeklyScheduleDto> GetWeeklyScheduleByEmployeeId(int employeeId);
        /// <summary>
        /// This is a Geolocation-Enabled app ClockService handler for Clock users who are punching
        /// through the mobile app with Time Policys that require Normal Punches.
        /// </summary>
        /// <param name="punchRequest"></param>
        /// <returns></returns>
        IOpResult<RealTimePunchWithHoursResultDto> ProcessAppPunchRequest(AppPunchRequest punchRequest);
        /// <summary>
        /// This is a Geolocation-Enabled app ClockService handler for Clock users who are punching
        /// through the mobile app with Time Policys that require Input Punches.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        IOpResult<RealTimePunchPairWithHoursResultDto> ProcessAppPunchPairRequest(RealTimePunchPairRequestWithHours request, string ipAddress);
        /// <summary>
        /// This is a Geolocation-Enabled app ClockService handler for Clock users who are punching
        /// through the mobile app with Time Policys that require Input Hours.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        IOpResult<InputHoursPunchResultWithDetail> ProcessInputHoursAppPunch(AppPunchRequest request, string ipAddess);
    }
}
