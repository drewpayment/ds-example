using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Sprocs;
using Dominion.Core.Dto.TimeCard.Result;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Query.Labor;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.LaborManagement.EF.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repo methods for getting data for labor management.
    /// </summary>
    public interface ILaborManagementRepository : IRepository, IDisposable
    {
        /// <summary>
        /// pass:1
        /// Constructs a new <see cref="GroupSchedule"/> query.
        /// </summary>
        /// <returns></returns>
        IGroupScheduleQuery GroupScheduleQuery();

        /// <summary>
        /// highfix: jay: needs test.
        /// Constructs a new <see cref="GroupScheduleShift"/> query.
        /// </summary>
        /// <returns></returns>
        IGroupScheduleShiftQuery GroupScheduleShiftQuery();

        /// <summary>
        /// pass:1
        /// Constructs a query for client cost centers.
        /// TABLE: dbo.ClientCostCenter
        /// </summary>
        /// <returns></returns>
        IClientCostCenterQuery ClientCostCenterQuery();


        IClockClientScheduleQuery ClockClientScheduleQuery();
        IClockClientScheduleChangeHistoryQuery ClockClientScheduleChangeHistoryQuery();
        IClockEmployeeBenefitQuery ClockEmployeeBenefitQuery();
        IClockEmployeeCostCenterQuery ClockEmployeeCostCenterQuery();
        IClockEmployeeScheduleQuery ClockEmployeeScheduleQuery();

        IEmployeeDefaultShiftQuery EmployeeDefaultShiftQuery();
        IEmployeeSchedulePreviewQuery EmployeeSchedulePreviewQuery();

        //IEnumerable<GroupScheduleDtos.ScheduleGroupDto> GetScheduleGroupCostCenters(
        //    GetScheduleGroupCostCentersArgsDto args);
        IClientDepartmentQuery ClientDepartmentQuery();

        //sync-sept-14th
        //IApplicantsQuery ApplicantsQuery();
        //IApplicantPostingsQuery ApplicantPostingsQuery();

        IApplicantPostingCategoriesQuery ApplicantPostingCategoriesQuery();
        IClientDivisionQuery ClientDivisionQuery();
        IClientShiftQuery ClientShiftQuery();
        /// Query <see cref="ClockEmployeeExceptionHistory"/> data.
        /// </summary>
        /// <returns></returns>
        IClockEmployeeExceptionHistoryQuery ClockEmployeeExceptionHistoryQuery();

        /// <summary>
        /// Query <see cref="ClockEmployeeApproveDate"/> data.
        /// </summary>
        /// <returns></returns>
        IClockEmployeeApproveDateQuery ClockEmployeeApproveDateQuery();

        IClockEarningDesHistoryQuery ClockEarningDesHistoryQuery();

        IClockClientNotesQuery ClockClientNotesQuery();
        ILeaveManagementPendingAwardQuery LeaveManagementPendingAwardQuery();
        IClientAccrualQuery ClientAccrualQuery();
        IClientAccrualEarningQuery ClientAccrualEarningQuery();
        IClockClientRulesQuery ClockClientRulesQuery();

        IClockClientOvertimeQuery ClockClientOvertimeQuery();

        IClockClientOvertimeSelectedQuery ClockClientOvertimeSelectedQuery();

        IClockOvertimeFrequencyQuery ClockOvertimeFrequencyQuery();

        IClockExceptionQuery ClockExceptionQuery();

        IClockClientExceptionQuery ClockClientExceptionQuery();

        IClockClientExceptionDetailQuery ClockClientExceptionDetailQuery();
        IClockClientLunchQuery ClockClientLunchQuery();
        /// <summary>
        /// entity created previously via .tomap on entity, conflicting at this point. 
        /// </summary>
        /// <returns></returns>
//        IClockClientLunchSelectedQuery ClockClientLunchSelectedQuery();
        IClockClientLunchPaidOptionQuery ClockClientLunchPaidOptionQuery();
        IClockClientLunchPaidOptionRulesQuery ClockClientLunchPaidOptionRulesQuery();
        IClockClientAddHoursQuery ClockClientAddHoursQuery();
        IClockClientHolidayQuery ClockClientHolidayQuery();
        IClockClientHolidayDetailQuery ClockClientHolidayDetailQuery();
        IClockClientTimePolicyQuery ClockClientTimePolicyQuery();
        IHolidayQuery HolidayQuery();
        IHolidayDateQuery HolidayDateQuery();
        IClockClientHolidayChangeHistoryQuery ClockClientHolidayChangeHistoryQuery();

        IClockClientDailyRulesQuery ClockClientDailyRulesQuery();
        IClockEmployeeAllocateHoursDetailQuery ClockEmployeeAllocateHoursDetailQuery();
        ITimeOffRequestQuery TimeOffRequestQuery();

        #region TimeCardAuthorization
        IEnumerable<GetClockPayrollListByClientIDPayrollRunIDDto> GetClockPayrollListByClientIDPayrollRunID(GetClockPayrollListByClientIDPayrollRunIDArgsDto args);
        IEnumerable<GetClockFilterCategoryDto> GetClockFilterCategory(GetClockFilterCategoryArgsDto args);

        IEnumerable<GetClockEmployeeApproveHoursSettingsDto> GetClockEmployeeApproveHoursSettings(GetClockEmployeeApproveHoursSettingsArgsDto args);

        IEnumerable<GetClockEmployeeApproveHoursOptionsDto> GetClockEmployeeApproveHoursOptions(GetClockEmployeeApproveHoursOptionsArgsDto args);

        GetClientJobCostingInfoByClientIDResultsDto GetClientJobCostingInfoByClientID(GetClientJobCostingInfoByClientIDArgsDto args);

        int InsertClockEmployeeApproveDate(InsertClockEmployeeApproveDateArgsDto args);
        IEnumerable<GetClockEmployeeApproveDateDto> GetClockEmployeeApproveDate(GetClockEmployeeApproveDateArgsDto args);
        IEnumerable<GetClockEmployeePunchListByDateAndFilterDto> GetClockEmployeePunchListByDateAndFilter(GetClockEmployeePunchListByDateAndFilterArgsDto args);
        IEnumerable<GetClockFilterIdsDto> GetClockFilterIDs(GetClockFilterIdsArgsDto args);
        GetTimeClockCurrentPeriodDto GetTimeClockCurrentPeriod(GetTimeClockCurrentPeriodArgsDto args);
        IEnumerable<GetClockEmployeeAllocatedHoursDifferenceDto> GetClockEmployeeAllocatedHoursDifference(GetClockEmployeeAllocatedHoursDifferenceArgsDto args);
        EmployeePunchListCountAndResultLengthDto GetClockEmployeePunchListByDateAndFilterCount(ClockEmployeePunchListByDateAndFilterPaginatedCountArgs args);
        PunchActivitySprocResults GetClockEmployeePunchListByDateAndFilterPaginated(ClockEmployeePunchListByDateAndFilterPaginatedCountArgs args);
        #endregion

        #region ClientAccruals
        IServiceFrequencyQuery               ServiceFrequencyQuery();
        IServiceRenewFrequencyQuery          ServiceRenewFrequencyQuery();
        IServiceRewardFrequencyQuery         ServiceRewardFrequencyQuery();
        IServiceCarryOverFrequencyQuery      ServiceCarryOverFrequencyQuery();
        IServiceCarryOverTillFrequencyQuery  ServiceCarryOverTillFrequencyQuery();
        IServiceCarryOverWhenFrequencyQuery  ServiceCarryOverWhenFrequencyQuery();
        IServiceReferencePointFrequencyQuery ServiceReferencePointFrequencyQuery();
        IServiceStartEndFrequencyQuery       ServiceStartEndFrequencyQuery();
        IServicePlanTypeQuery                ServicePlanTypeQuery();
        IServiceTypeQuery                    ServiceTypeQuery();
        IServiceUnitQuery                    ServiceUnitQuery();

        IAccrualBalanceOptionQuery           AccrualBalanceOptionQuery();
        IClientAccrualEmployeeStatusQuery    ClientAccrualEmployeeStatusQuery();
        IAccrualCarryOverOptionQuery         AccrualCarryOverOptionQuery();
        IClientAccrualClearOptionQuery       AccrualClearOptionQuery();
        IServiceBeforeAfterQuery             ServiceBeforeAfterQuery();
        IAutoApplyAccrualPolicyQuery         AutoApplyAccrualPolicyQuery();
        #endregion ClientAccruals
    }
}