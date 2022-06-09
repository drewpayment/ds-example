using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Common;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.SftpUpload.CsvTemplates;
using Dominion.Core.Dto.TimeCard;
using Dominion.Core.Dto.TimeCard.Result;
using Dominion.Core.Dto.TimeClock;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.TimeClock;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public interface ILaborManagementProvider
    {
        /// <summary>
        /// Register new ClockClientNote entity to current UnitOfWork, commit to database and return updated dto.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientNoteDto> RegisterNewClockClientNote(ClockClientNote entity, ClockClientNoteDto dto);

        /// <summary>
        /// Get single ClockClientNote entity from database by primary id.
        /// </summary>
        /// <param name="clockClientNoteId"></param>
        /// <returns></returns>
        IOpResult<ClockClientNoteDto> GetClockClientNote(int clockClientNoteId);

        /// <summary>
        /// Determine if ClockClientNote entity has been used previously and cannot be removed from database.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        IOpResult CheckUsageForClockClientNote(ClockClientNoteDto current);

        /// <summary>
        /// Register changed properties of ClockClientNote entity, commit to database and then return updated dto.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientNoteDto> RegisterExistingClockClientNote(ClockClientNote entity, ClockClientNoteDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="current"></param>
        void RegisterModifiedClockClientNote(ClockClientNote entity, ClockClientNoteDto current);

        /// <summary>
        /// Register new ClockClientOvertime entity on UnitOfWork, commit to db and then return updated dto.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientOvertimeDto> RegisterNewClockClientOvertime(ClockClientOvertime entity, ClockClientOvertimeDto dto);

        /// <summary>
        /// Return single ClockClientOvertime entity mapped to dto.
        /// </summary>
        /// <param name="clockClientOvertimeId"></param>
        /// <returns></returns>
        IOpResult<ClockClientOvertimeDto> GetClockClientOvertime(int clockClientOvertimeId);

        /// <summary>
        /// Register changed properties of ClockClientOvertime entity to current UnitOfWork, commit and return updated dto.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientOvertimeDto> RegisterExistingClockClientOvertime(ClockClientOvertime entity, ClockClientOvertimeDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="current"></param>
        void RegisterModifiedClockClientOvertime(ClockClientOvertime entity, ClockClientOvertimeDto current);

        /// <summary>
        /// Determine if the ClockClientOvertime entity has been used previously and cannot be deleted.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        IOpResult CheckUsageForClockClientOvertimeSelected(ClockClientOvertimeDto current);

        /// <summary>
        /// Get ClientExceptionDetail entity list mapped to dto list;
        /// </summary>
        /// <returns></returns>
        IOpResult<IEnumerable<ClientExceptionDetailDto>> GetStandardExceptionsAsExceptionDetail();

        /// <summary>
        /// Return ClockClientExceptionDetail entity list mapped to dto list by the parent's id.
        /// </summary>
        /// <param name="clockClientExceptionId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockClientExceptionDetailDto>> GetClientExceptionDetailByClientException(int clockClientExceptionId);

        /// <summary>
        /// Register changed properties of ClockClientException entity to current UnitOfWork.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="current"></param>
        void RegisterModifiedClockClientException(ClockClientException entity, ClockClientExceptionDto current);

        /// <summary>
        /// Return single ClockClientException entity by primary id, mapped to dto.
        /// </summary>
        /// <param name="clockClientExceptionId"></param>
        /// <returns></returns>
        IOpResult<ClockClientExceptionDto> GetClockClientException(int clockClientExceptionId);

        /// <summary>
        /// Register new ClockClientException entity, commit to database and then return dto.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientExceptionDto> RegisterNewClockClientException(ClockClientException entity, ClockClientExceptionDto dto);

        /// <summary>
        /// Register changed properties of ClockClientException entity to UnitOfWork, commit and then return updated dto.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientExceptionDto> RegisterExistingClockClientException(ClockClientException entity, ClockClientExceptionDto dto);

        /// <summary>
        /// Register a new ClockClientExceptionDetail entity to the current UnitOfWork, 
        /// commit to database and then return the updated dto with primary id.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientExceptionDetailDto> RegisterNewClockClientExceptionDetail(
            ClockClientExceptionDetail entity, ClockClientExceptionDetailDto dto);

        /// <summary>
        /// Return single ClockClientExceptionDetail entity by primary id, mapped to dto.
        /// </summary>
        /// <param name="clockClientExceptionDetailId"></param>
        /// <returns></returns>
        IOpResult<ClockClientExceptionDetailDto> GetClockClientExceptionDetail(int clockClientExceptionDetailId);

        void RegisterModifiedClockClientExceptionDetail(ClockClientExceptionDetail entity, ClockClientExceptionDetailDto current);

        /// <summary>
        /// Register changed properties of a ClockClientExceptionDetail entity for update on the current UnitOfWork.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientExceptionDetailDto> RegisterExistingClockClientExceptionDetail(
            ClockClientExceptionDetail entity, ClockClientExceptionDetailDto dto);

        /// <summary>
        /// Determine if the ClockClientException entity has been used.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        IOpResult CheckUsageForClockClientException(ClockClientExceptionDto current);

        /// <summary>
        /// Determine if the ClockClientExceptionDetail entity has been used.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        IOpResult CheckUsageForClockClientExceptionDetail(ClockClientExceptionDetailDto current);

        /// <summary>
        /// Get a single ClockClientHoliday entity, return mapped to a dto.
        /// </summary>
        /// <param name="clockClientHolidayId"></param>
        /// <returns></returns>
        IOpResult<ClockClientHolidayDto> GetClockClientHoliday(int clockClientHolidayId);

        /// <summary>
        /// Register changed properties of a ClockClientHoliday entity to the current UnitOfWork.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientHolidayDto> RegisterExistingClockClientHoliday(ClockClientHoliday entity, ClockClientHolidayDto dto);

        /// <summary>
        /// Get a single ClockClientHolidayDetail by the primary id.
        /// </summary>
        /// <param name="clockClientHolidayDetailId"></param>
        /// <returns></returns>
        IOpResult<ClockClientHolidayDetailDto> GetClockClientHolidayDetail(int clockClientHolidayDetailId);

        /// <summary>
        /// Register changed properties of a ClockClientHolidayDetail entity to the current UnitOfWork.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientHolidayDetailDto> RegisterExistingClockClientHolidayDetail(ClockClientHolidayDetail entity, ClockClientHolidayDetailDto dto);

        /// <summary>
        /// Get available holiday years for the current year and years in the future.
        /// </summary>
        /// <returns>List of Holiday Dates</returns>
        IOpResult<IEnumerable<HolidayDateDto>> GetAvailableHolidayYears(int? year = null);

        /// <summary>
        /// Inserts ClockClientHolidayChangeHistory record for change tracking.
        /// </summary>
        /// <param name="hol"></param>
        /// <param name="changeMode"></param>
        /// <returns></returns>
        IOpResult<ClockClientHolidayChangeHistoryDto> InsertClockClientHolidayChangeHistory(ClockClientHolidayDto hol, ChangeModeType changeMode);

        /// <summary>
        /// Determine if the particular ClockClientHoliday is used on an existing ClockClientTimePolicy.
        /// </summary>
        /// <param name="curr"></param>
        /// <returns></returns>
        IOpResult CheckUsageForClockClientHoliday(ClockClientHolidayDto curr);

        /// <summary>
        /// Get a list of ClockClientHolidayDetails by its parent ClockClientHolidayId.
        /// </summary>
        /// <param name="clockClientHolidayId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockClientHolidayDetailDto>> GetClockClientHolidayDetailList(int clockClientHolidayId);

        /// <summary>
        /// Determine if the particular ClockClientHolidayDetail record is used on an active ClockClientHoliday.
        /// </summary>
        /// <param name="curr"></param>
        /// <returns></returns>
        IOpResult CheckUsageForClockClientHolidayDetail(ClockClientHolidayDetailDto curr);
        IOpResult CheckUsageForClockClientRules(ClockClientRulesDto current);

        /// <summary>
        /// Register deletes for related ClockEmployeeBenefit records from database.
        /// </summary>
        /// <param name="clockClientHolidayDetailId"></param>
        /// <param name="holdCommit">If true, method will not Commit current UnitOfWork, so developer can chain multiple sql statements.</param>
        /// <returns></returns>
        IOpResult DeleteRelatedClockEmployeeBenefitRecords(int clockClientHolidayDetailId, bool holdCommit = false);

        /// <summary>
        /// Save a ClockEmployeeBenefit entity and dynamically insert as a new record or update an existing record.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="holdCommit"></param>
        /// <returns></returns>
        IOpResult<ClockEmployeeBenefit> SaveClockEmployeeBenefit(ClockEmployeeBenefitDto dto, bool holdCommit = false);

        /// <summary>
        /// Get a ClockClientRule object from the database by the clockClientRulesId.
        /// </summary>
        /// <param name="clockClientRulesId"></param>
        /// <returns></returns>
        IOpResult<ClockClientRulesDto> GetClockClientRules(int clockClientRulesId);

        /// <summary>
        /// Register an entity's modified properties to the Unit of Work to be processed when Unit of Work's commit method
        /// is called.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientRulesDto> RegisterExistingClockClientRules(ClockClientRules entity, ClockClientRulesDto dto);

        /// <summary>
        /// Register a ClockClientDailyRules entity's modified properties to the UnitOfWork so that it can be pended for commit later.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult RegisterExistingClockClientDailyRule(ClockClientDailyRules entity, ClockClientDailyRulesDto dto);

        /// <summary>
        /// Get a list of client cost centers by client id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<List<ClientCostCenterDto>> GetClientCostCenterListByClient(int clientId);

        /// <summary>
        /// Get list of rounding types.
        /// </summary>
        /// <returns></returns>
        IOpResult<List<ClockRoundingTypeDto>> GetClockRoundingTypeList();

        /// <summary>
        /// Register a new clock client lunch paid option entity and return the dto.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientLunchPaidOptionDto> RegisterNewClockClientLunchPaidOption(ClockClientLunchPaidOptionDto dto);

        /// <summary>
        /// Register a changes to existing clock client lunch paid option entity and return dto.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientLunchPaidOptionDto> RegisterExistingClockClientLunchPaidOption(ClockClientLunchPaidOptionDto dto);

        /// <summary>
        /// Registers existing changes on an entity to the Unit of Work and prepares it for committing above.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientLunchDto> RegisterExistingClockClientLunch(ClockClientLunchDto dto);

        /// <summary>
        /// Registers a new clock client lunch entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IOpResult<ClockClientLunchDto> RegisterNewClockClientLunch(ClockClientLunch entity);

        /// <summary>
        /// Registers a new clock client lunch entity. Does NOT commit to SQL.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientLunchDto> RegisterNewClockClientLunch(ClockClientLunchDto dto);

        /// <summary>
        /// Registers existing changes on an entity to the Unit of Work and prepares it for committing above.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientAddHoursDto> RegisterExistingClockClientAddHours(ClockClientAddHoursDto dto);

        /// <summary>
        /// Registers a new ClockClientAddHours entity. Does NOT commit to SQL.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientAddHoursDto> RegisterNewClockClientAddHours(ClockClientAddHoursDto dto);

        /// <summary>
        /// Gets list of time policies by client id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<List<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>> GetClockClientTimePolicies(int clientId);

        /// <summary>
        /// Get list of clock client rules by client id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<List<ClockClientRulesDto>> GetClockClientRulesByClient(int clientId);

        /// <summary>
        /// Get list of clock client exceptions by client id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<List<ClockClientExceptionDto>> GetClockClientExceptionsByClient(int clientId);

        /// <summary>
        /// Get a list of clock client holidays by client id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<List<ClockClientHolidayDto>> GetClockClientHolidaysByClient(int clientId);

        /// <summary>
        /// Get a list of time zones from the database
        /// </summary>
        /// <returns></returns>
        IOpResult<List<TimeZoneDto>> GetTimeZones();

        /// <summary>
        /// Get list of selected overtime records by the time policy.
        /// </summary>
        /// <param name="clockClientTimePolicyId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<List<ClockClientOvertimeSelectedDto>> GetClockClientOvertimeSelectedList(int clockClientTimePolicyId, int clientId);

        /// <summary>
        /// Get list of overtime by time policy id
        /// </summary>
        /// <param name="clockClientTimePolicyId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<List<ClockClientOvertimeDto>> GetClockClientOvertimeList(int clientId);

        /// <summary>
        /// Get list of lunch break entities associated with the client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<List<ClockClientLunchDto>> GetClockClientLunchBreakList(int clientId);

        /// <summary>
        /// Get list of AddHours entities associated with the client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<List<ClockClientAddHoursDto>> GetClockClientAddHoursList(int clientId);

        /// <summary>
        /// Get list of client shift entities associated with the client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<List<ClientShiftDto>> GetClientShiftList(int clientId);

        /// <summary>
        /// Get clock client lunch entities related to the passed time policy.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <returns></returns>
        IOpResult<List<ClockClientLunchDto>> GetClockClientLunchListByTimePolicy(int clientId, int clockClientTimePolicyId);

        /// <summary>
        /// Get clock client add hours entities related to the passed time policy.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <returns></returns>
        IOpResult<List<ClockClientAddHoursDto>> GetClockClientAddHoursListByTimePolicy(int clientId, int clockClientTimePolicyId);

        /// <summary>
        /// Get clock client overtime entities by related time policy.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <returns></returns>
        IOpResult<List<ClockClientOvertimeDto>> GetClockClientOvertimeByTimePolicy(int clientId, int clockClientTimePolicyId);

        /// <summary>
        /// Get a time policy by id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <returns></returns>
        IOpResult<ClockClientTimePolicyDtos.ClockClientTimePolicyDto> GetClockClientTimePolicy(int clientId, int clockClientTimePolicyId);

        /// <summary>
        /// Get a list 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <returns></returns>
        IOpResult<List<ClientShiftDto>> GetClientShiftListByTimePolicy(int clientId, int clockClientTimePolicyId);

        /// <summary>
        /// Registers a new Time Policy entity on the Business Session waiting to commit to sql. 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientTimePolicyDtos.ClockClientTimePolicyDto> RegisterNewClockClientTimePolicy(ClockClientTimePolicyDtos.ClockClientTimePolicyDto dto);

        /// <summary>
        /// Registers a modified Time Policy entity on the session waiting to commit.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientTimePolicyDtos.ClockClientTimePolicyDto> RegisterExistingClockClientTimePolicy(ClockClientTimePolicyDtos.ClockClientTimePolicyDto dto);

        /// <summary>
        /// Registers a modified Overtime entity. 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult RegisterExistingClockClientOvertime(ClockClientOvertimeDto dto);

        /// <summary>
        /// Registers new Overtime entity.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClockClientOvertimeDto> RegisterNewClockClientOvertime(ClockClientOvertimeDto dto);

        /// <summary>
        /// Registers a modified client shift entity. 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult RegisterExistingClientShift(ClientShiftDto dto);

        /// <summary>
        /// Registers a new client shift entity.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult<ClientShiftDto> RegisterNewClientShift(ClientShiftDto dto);

        /// <summary>
        /// Delete all lunch entities related to the passed time policy id. 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <param name="lunches"></param>
        /// <returns></returns>
        IOpResult DeleteRelatedClockClientLunches(int clientId, int clockClientTimePolicyId, List<ClockClientLunchSelectedDto> lunches);

        /// <summary>
        /// Delete all related selected lunch entities from ClockClientLunchSelected table based on relation
        /// to a time policy.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <returns></returns>
        IOpResult DeleteRelatedClockClientLunches(int clientId, int clockClientTimePolicyId);

        /// <summary>
        /// Delete all AddHours entities related to the passed time policy id. 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <param name="lunches"></param>
        /// <returns></returns>
        IOpResult DeleteRelatedClockClientAddHours(int clientId, int clockClientTimePolicyId, List<ClockClientAddHoursSelectedDto> lunches);

        /// <summary>
        /// Delete all related selected AddHours entities from ClockClientAddHoursSelected table based on relation
        /// to a time policy.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <returns></returns>
        IOpResult DeleteRelatedClockClientAddHours(int clientId, int clockClientTimePolicyId);

        /// <summary>
        /// Delete all overtime entities related to the passed time policy id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <param name="overtimes"></param>
        /// <returns></returns>
        IOpResult DeleteRelatedClockClientOvertimes(int clientId, int clockClientTimePolicyId, List<ClockClientOvertimeSelectedDto> overtimes);

        /// <summary>
        /// Delete all related selected overtime entities from ClockClientOvertimeSelected table based on relation
        /// to a time policy.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <returns></returns>
        IOpResult DeleteRelatedClockClientOvertimes(int clientId, int clockClientTimePolicyId);

        /// <summary>
        /// Delete all shift entities related to the passed time policy id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <param name="shifts"></param>
        /// <returns></returns>
        IOpResult DeleteRelatedClientShifts(int clientId, int clockClientTimePolicyId, List<ClientShiftSelectedDto> shifts);

        /// <summary>
        /// Delete all related selected client shift entities from ClientShiftSelected table based on relation
        /// to a time policy.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <returns></returns>
        IOpResult DeleteRelatedClientShifts(int clientId, int clockClientTimePolicyId);

        /// <summary>
        /// Save a list of selected lunch entities by a particular time policy.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <param name="lunches"></param>
        /// <returns></returns>
        IOpResult SaveNewClockClientLunchSelectedList(int clientId, int clockClientTimePolicyId, List<ClockClientLunchSelected> lunches);

        /// <summary>
        /// Save a list of selected AddHours entities by a particular time policy.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <param name="addHours"></param>
        /// <returns></returns>
        IOpResult SaveNewClockClientAddHoursSelectedList(int clientId, int clockClientTimePolicyId, List<ClockClientAddHoursSelected> addHours);

        /// <summary>
        /// Save a list of selected overtime entities by a particular time policy.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <param name="overtimes"></param>
        /// <returns></returns>
        IOpResult SaveNewClockClientOvertimeSelectedList(int clientId, int clockClientTimePolicyId, List<ClockClientOvertimeSelected> overtimes);

        /// <summary>
        /// Save a list of selected shift entities by a particular time policy.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <param name="shifts"></param>
        /// <returns></returns>
        IOpResult SaveNewClientShiftSelectedList(int clientId, int clockClientTimePolicyId, List<ClientShiftSelected> shifts);

        /// <summary>
        /// Checks for existing clock employee benefits and deletes them if found. 
        /// </summary>
        /// <returns></returns>
        IOpResult DeleteClockEmployeeBenefit(ClockEmployeeBenefitDto dto);

        /// <summary>
        /// Delete a list of clock employee benefits.
        /// </summary>
        /// <param name="clockEmployeeBenefitIdList"></param>
        /// <param name="clientId"></param>
        void DeleteClockEmployeeBenefits(IEnumerable<int> clockEmployeeBenefitIdList, int clientId);

        /// <summary>
        /// Get a list of benefit records
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<OnShiftPunchesLayout>> GetBenefitRecordsByEmployeeDateRange(int employeeId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get schedule information for the employee.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockClientScheduleDto>> GetEmployeeSelectedSchedules(int employeeId);
        IOpResult<EmployeePunchListCountAndResultLengthDto> GetEmployeePagedResultLength(ClockEmployeePunchListByDateAndFilterPaginatedCountArgs args);
        IOpResult<EmployeePunchListCountAndResultLengthDto> GetClockEmployeePunchListPaginationCount(TimeCardAuthorizationSearchOptions options);
        IOpResult<PunchActivitySprocResults> GetTcaEmployeePunchActivityList(TimeCardAuthorizationSearchOptions options);
        IOpResult DeleteFutureHolidayHours(int timePolicyId, int clockClientHolidayId, DateTime startDate);
        IOpResult InsertFutureHolidayHours(int timePolicyId, int clockClientHolidayId, DateTime startDate);
    }
}
