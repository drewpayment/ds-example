using System;
using Dominion.Core.Dto.Labor;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.Utility.OpResult;
using System.Collections.Generic;
using System.Data;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Payroll;
using Dominion.Core.Dto.User;
using Dominion.Core.Services.Dto.Employee;
using Dominion.Pay.Dto.Earnings;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Core.Dto.TimeCard;
using Dominion.Core.Dto.TimeCard.Result;

namespace Dominion.LaborManagement.Service.Api
{
    public interface ILaborManagementService
    {
        IOpResult<IEnumerable<ClockClientNoteDto>> GetClientNotes(int clientId);

        IOpResult<ClockClientNoteDto> SaveClockClientNote(ClockClientNoteDto dte);

        /// <summary>
        /// Attempts to delete an existing note.  Currently does not check if note is in use.
        /// </summary>
        /// <param name="clockClientNoteId"></param>
        /// <returns></returns>
        IOpResult DeleteClockClientNote(int clockClientNoteId);

        IOpResult<ClockClientOvertimeDto> SaveClockClientOvertime(ClockClientOvertimeDto dto);

        IOpResult<IEnumerable<ClockClientOvertimeDto>> GetOvertimeRules(int clientId);

        IOpResult<IEnumerable<ClockOvertimeFrequencyDto>> GetOvertimeFrequencies(int clientId);

        IOpResult DeleteClockClientOvertime(int clockClientOvertimeId);

        IOpResult<IEnumerable<ClockExceptionTypeInfoDto>> GetExceptions();

        IOpResult<IEnumerable<ClientExceptionDetailDto>> GetStandardExceptionsDetail(int clientId);

        IOpResult<IEnumerable<GetClockEmployeeExceptionHistoryByEmployeeIDListDto>> GetStandardExceptionsDetailByRange(int clientId, DateTime startDate, DateTime endDate, IEnumerable<int> employeeIds);

        IOpResult<IEnumerable<ClockClientExceptionDto>> GetClientExceptionRules(int clientId);

        IOpResult<IEnumerable<ClientExceptionDetailDto>> GetClockClientExceptionDetailList(int clockClientExceptionId, int clientId);

        IOpResult<ClockClientExceptionDto> SaveClockClientException(ClockClientExceptionDto dto, int clientId);

        IOpResult DeleteClockClientException(int clientId, int clockClientExceptionId);

        IOpResult<CompanyHolidayDto> GetCompanyHolidayVmDto(int clientId, ClientEarningCategory clientEarningType);

        IOpResult<ClockClientHolidayDto> SaveClockClientHoliday(ClockClientHolidayDto dto);

        IOpResult<ClockClientHolidayDetailDto> SaveClockClientHolidayDetail(ClockClientHolidayDetailDto dto, int clientId, bool holdCommit = false);

        IOpResult<IEnumerable<ClockClientHolidayDetailDto>> SaveClockClientHolidayDetailList(IEnumerable<ClockClientHolidayDetailDto> dtos, int clientId);

        IOpResult DeleteCompanyHolidayRule(int clockClientHolidayId);

        IOpResult DeleteCompanyHolidayDetail(int clockClientHolidayId, bool holdCommit = false);

        IOpResult DeleteCompanyHolidayDetail(int clockClientHolidayDetailId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult ApplyHolidayRule(ClockClientHolidayDto dto, int selectedYear);

        /// <summary>
        /// Get list of clock client rules by client id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<List<ClockClientRulesDto>> GetClockClientRulesList(int clientId);

        /// <summary>
        /// Gets CompanyRules objects by client id, days of week list and clock rounding types for 
        /// frontend page load in angularjs
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<ClockCompanyRulesViewDto> GetCompanyRulesViewDto(int clientId);

        /// <summary>
        /// Save new/updated ClockClientRules entity and return updated dto.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientRulesDto> SaveClockClientRules(ClockClientRulesDto dto);

        /// <summary>
        /// Delete existing ClockClientRules entity from database and return void result.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientRulesId"></param>
        /// <returns></returns>
        IOpResult DeleteClockClientRules(int clientId, int clockClientRulesId);

        /// <summary>
        /// Get list of ClockClientDailyRules entities by ClockClientDailyRulesId
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientDailyRulesId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockClientDailyRulesDto>> GetClockClientDailyRules(int clientId, int clockClientDailyRulesId);

        /// <summary>
        /// Get list client earnings by the clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClientEarningDto>> GetClientEarnings(int clientId);

        /// <summary>
        /// Takes a dto, either commits to EF as new entity or updates existing entity with EF and returns the updated dto.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<ClockClientDailyRulesDto> SaveClockClientDailyRule(ClockClientDailyRulesDto dto, int clientId);

        /// <summary>
        /// Deletes ClockClientDailyRules entity from database.
        /// </summary>
        /// <param name="clockClientDailyRulesId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult DeleteClockClientDailyRule(int clockClientDailyRulesId, int clientId);

        /// <summary>
        /// Get page load data view model data for angular frontend, client lunches, cost centers,
        /// paid option rules, and rounding type list.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<ClockClientLunchViewDto> GetClockCompanyLunchViewData(int clientId);

        /// <summary>
        /// Get paid options for the current ClockClientLunch by the clock client lunch id and the client id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientLunchId"></param>
        /// <returns></returns>
        IOpResult<List<ClockClientLunchPaidOptionDto>> GetClockClientLunchPaidOptions(int clientId, int clockClientLunchId);

        /// <summary>
        /// Save a list of lunch paid options that will save them as new/existing entities.
        /// </summary>
        /// <param name="dtoList"></param>
        /// <param name="clientId"></param>
        /// <param name="clockClientLunchId"></param>
        /// <returns></returns>
        IOpResult<List<ClockClientLunchPaidOptionDto>> SaveClockClientLunchPaidOptions(List<ClockClientLunchPaidOptionDto> dtoList, int clientId, int clockClientLunchId = 0);

        /// <summary>
        /// Delete an existing lunch paid option entity.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientLunchId"></param>
        /// <param name="clockClientLunchPaidOptionId"></param>
        /// <returns></returns>
        IOpResult DeleteClockClientPaidOption(int clientId, int clockClientLunchId, int clockClientLunchPaidOptionId);

        /// <summary>
        /// Save clock client lunch entity to the database or updates an existing if passed an existing record.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientLunchDto> SaveClockClientLunch(ClockClientLunchDto dto);

        /// <summary>
        /// Delete an existing clock client lunch. Also deletes child LunchPaidOption entities.
        /// </summary>
        /// <param name="clockClientLunchId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult DeleteClockClientLunch(int clockClientLunchId, int clientId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<ClockClientAddHoursViewDto> GetClockClientAddHoursView(int clientId);

        /// <summary>
        /// Get all Attendance Award policies for a client by clientId
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<List<ClockClientAddHoursDto>> GetClockClientAddHoursByClient(int clientId);

        /// <summary>
        /// Save ClockClientAddHours entity to the database or updates an existing one if passed an existing record.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientAddHoursDto> SaveClockClientAddHours(ClockClientAddHoursDto dto);

        /// <summary>
        /// Delete an existing ClockClientAddHours entity.
        /// </summary>
        /// <param name="clockClientAddHoursId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult DeleteClockClientAddHours(int clockClientAddHoursId, int clientId);

        /// <summary>
        /// Get list of clock company time policies, clock company rules, clock company exceptions, clock company holidays,
        /// and time zones based on the currently selected client id. It wraps all of these list in DTO that we use on page 
        /// of the clock company time view.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<ClockClientTimePolicyDtos.ClockClientTimePolicyViewModels> GetClockClientTimePolicyViewData(int clientId);

        /// <summary>
        /// Get entity lists of the following associated by client id: 
        /// Overtime rules
        /// Lunch/break rules
        /// Client shifts
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<ClockClientTimePolicySearchLists> GetTimePolicySearchLists(int clientId);

        /// <summary>
        /// Get entity lists related to the current time policy:
        /// Selected overtime rules
        /// Selected lunch/break rules 
        /// Selected shifts
        /// </summary>
        /// <param name="clockClientTimePolicyId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<ClockClientTimePolicySearchLists> GetTimePolicySelectedLists(int clockClientTimePolicyId, int clientId);

        /// <summary>
        /// Check if the user has permission to read time policies. 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult CanReadTimePolicy(int clientId);

        /// <summary>
        /// Filter a set of employee entities by optional parameters specifically for use in the Time Policies.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="payType"></param>
        /// <param name="employeeStatusType"></param>
        /// <param name="departmentId"></param>
        /// <param name="clientShiftId"></param>
        /// <returns></returns>
        IOpResult<List<EmployeeDto>> GetFilteredEmployees(int clientId, PayType? payType, EmployeeStatusType? employeeStatusType, int? departmentId, int? clientShiftId);

        /// <summary>
        /// Save a new or existing time policy entity.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientTimePolicyDtos.ClockClientTimePolicyDto> SaveClockClientTimePolicy(int clientId, ClockClientTimePolicyDtos.ClockClientTimePolicyDto dto);

        /// <summary>
        /// Iterates through all three lists and then either updates existing ones or inserts new records where applicable.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <param name="lists"></param>
        /// <returns></returns>
        IOpResult<ClockClientTimePolicySearchLists> SaveTimePolicySelectedLists(int clientId, int clockClientTimePolicyId, ClockClientTimePolicySearchLists lists);

        /// <summary>
        /// Calls VB service that fires sproc and returns integer value.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <returns></returns>
        IOpResult AutoApplyClockClientTimePolicy(int clientId, int clockClientTimePolicyId);

        /// <summary>
        /// Delete an existing clock client time policy.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <returns></returns>
        IOpResult DeleteClockClientTimePolicy(int clientId, int clockClientTimePolicyId);

        /// <summary>
        /// :p
        /// </summary>
        /// <returns></returns>
        IOpResult<GetTimeApprovalTableDto> GetTimeApprovalTable(int? supervisorId, TcaSearchOptions justin);
        IOpResult<EmployeePunchListCountAndResultLengthDto> GetEmployeePagedResultLength(ClockEmployeePunchListByDateAndFilterPaginatedCountArgs args);
        IOpResult<TimeCardAuthorizationResult> GetTimeCardAuthorizationPaged(int employeeId, TimeCardAuthorizationSearchOptions options);
        IOpResult<GetTimeClockCurrentPeriodDto> GetCurrentPayPeriod(int clientId);
        IOpResult<IEnumerable<GetReportClockEmployeeOnSiteDto>> GetReportClockEmployeeOnSite(int clientId, IEnumerable<int> employeeIds);
        IOpResult<IEnumerable<GetClockEmployeeHoursComparisonDto>> GetClockEmployeeHoursComparisonSproc(int clientId, DateTime startDate, DateTime endDate, IEnumerable<int> employeeIds);
    }
}
