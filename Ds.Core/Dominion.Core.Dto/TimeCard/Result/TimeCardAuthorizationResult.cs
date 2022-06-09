using Dominion.Core.Dto.Core.Search;
using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.TimeCard.Result
{
    public class TimeCardAuthorizationResult : IHasPaginationPage
    {
        public IEnumerable<TimeCardAuthorizationEmployeeResult> Employees { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? TotalEmployees { get; set; }
        public int? TotalPages { get; set; }
    }

    public class TimeCardAuthorizationEmployeeResult
    {
        public string Name { get; set; }
        public string EmployeeNumber { get; set; }
        public int EmployeeId { get; set; }
        public int? ClockClientTimePolicyId { get; set; }
        public string ClockClientTimePolicyName { get; set; }
        public IEnumerable<TimeCardAuthorizationResult> Rows { get; set; }
    }

    public class TimeCardAuthorizationRowResult
    {
        public TimeCardAuthorizationResultType Type { get; set; }
        public string DayColumnLabel { get; set; }
        public string DateColumnLabel { get; set; }
        public string Hours { get; set; }
        public string Schedule { get; set; }
        public int ClockEmployeeScheduledId { get; set; }
        public int? EmployeeId { get; set; }
        public PunchOptionType? PunchOption { get; set; }
        public DateTime EventDate { get; set; }
        public int AllocateHoursOptionId { get; set; }
        public int AllocateHoursFrequencyId { get; set; }
        public string Notes { get; set; }
        public bool IsTotalRow { get; set; }
        public bool IsScheduleReal { get; set; }
        public bool HideApprovalCheckbox { get; set; }
        public bool IsApproved { get; set; }
        public int? EmployeeActivity { get; set; }
        public bool IsBenefit { get; set; }
        public int? ClockClientTimePolicyId { get; set; }
        public string ClockClientTimePolicyName { get; set; }
        public int? ClientCostCenterId { get; set; }
        public string InCostCenterDesc { get; set; }
        public string In2CostCenterDesc { get; set; }
        public string OutCostCenterDesc { get; set; }
        public string Out2CostCenterDesc { get; set; }
        public string CostCenterDesc { get; set; }
        public string DivisionDesc { get; set; }
        public string DepartmentDesc { get; set; }
        public string ApprovingUser { get; set; }
        public string InDepartmentDesc { get; set; }
        public string In2DepartmentDesc { get; set; }
        public string OutDepartmentDesc { get; set; }
        public string Out2DepartmentDesc { get; set; }
        public string InDivisionDesc { get; set; }
        public string In2DivisionDesc { get; set; }
        public string OutDivisionDesc { get; set; }
        public string Out2DivisionDesc { get; set; }
        public int? InJobCostAssignId_1 { get; set; }
        public int? InJobCostAssignId_2 { get; set; }
        public int? InJobCostAssignId_3 { get; set; }
        public int? InJobCostAssignId_4 { get; set; }
        public int? InJobCostAssignId_5 { get; set; }
        public int? InJobCostAssignId_6 { get; set; }
        public int? In2JobCostAssignId_1 { get; set; }
        public int? In2JobCostAssignId_2 { get; set; }
        public int? In2JobCostAssignId_3 { get; set; }
        public int? In2JobCostAssignId_4 { get; set; }
        public int? In2JobCostAssignId_5 { get; set; }
        public int? In2JobCostAssignId_6 { get; set; }
        public int? OutJobCostAssignId_1 { get; set; }
        public int? OutJobCostAssignId_2 { get; set; }
        public int? OutJobCostAssignId_3 { get; set; }
        public int? OutJobCostAssignId_4 { get; set; }
        public int? OutJobCostAssignId_5 { get; set; }
        public int? OutJobCostAssignId_6 { get; set; }
        public int? Out2JobCostAssignId_1 { get; set; }
        public int? Out2JobCostAssignId_2 { get; set; }
        public int? Out2JobCostAssignId_3 { get; set; }
        public int? Out2JobCostAssignId_4 { get; set; }
        public int? Out2JobCostAssignId_5 { get; set; }
        public int? Out2JobCostAssignId_6 { get; set; }
        public int? JobCostAssignId_1 { get; set; }
        public int? JobCostAssignId_2 { get; set; }
        public int? JobCostAssignId_3 { get; set; }
        public int? JobCostAssignId_4 { get; set; }
        public int? JobCostAssignId_5 { get; set; }
        public int? JobCostAssignId_6 { get; set; }
        public decimal? EmployeeRate { get; set; }
        public string InClass { get; set; }
        public string In2Class { get; set; }
        public string OutClass { get; set; }
        public string Out2Class { get; set; }
        public string LnkDateClass { get; set; }
        public string LblDateClass { get; set; }
        public string LnkScheduleClass { get; set; }
        public string InToolTipContent { get; set; }
        public string In2ToolTipContent { get; set; }
        public string OutToolTipContent { get; set; }
        public string Out2ToolTipContent { get; set; }
        public string LnkDateToolTipContent { get; set; }
        public string LblDateToolTipContent { get; set; }
        public string LnkScheduleToolTipContent { get; set; }
        public string InModal { get; set; }
        public string In2Modal { get; set; }
        public string OutModal { get; set; }
        public string Out2Modal { get; set; }
        public string LnkDateModal { get; set; }
        public string LblDateModal { get; set; }
        public string LnkScheduleModal { get; set; }
        public string LnkSchedule2Modal { get; set; }
        public string Schedule3Modal { get; set; }
        public string InTitle { get; set; }
        public string In2Title { get; set; }
        public string OutTitle { get; set; }
        public string Out2Title { get; set; }
        public string LnkDateTitle { get; set; }
        public string LblDateTitle { get; set; }
        public string LnkScheduleTitle { get; set; }
        public string InDisabled { get; set; }
        public string In2Disabled { get; set; }
        public string OutDisabled { get; set; }
        public string Out2Disabled { get; set; }
        public string LnkDateDisabled { get; set; }
        public string LblDateDisabled { get; set; }
        public string LnkScheduleDisabled { get; set; }
        public string LnkDate { get; set; }
        public bool? SelectHoursDisabled { get; set; }
        public bool SelectHoursShowing { get; set; }
        public bool? LnkDateShowing { get; set; }
        public bool? LblDateShowing { get; set; }
        public bool SelectHoursChecked { get; set; }
        public string SelectHoursTooltip { get; set; }
        public bool DisplayExceptionAsApproved { get; set; }
        public string DateModal { get; set; }
        public bool? ChangesNotAllowed { get; set; }
        public bool? AddAllocationVisible { get; set; }
        public string AddPunchPopUp { get; set; }
        public string AddBenefitPopUp { get; set; }
        public string AddAllocationPopUp { get; set; }
        public bool LblNotesVisible { get; set; }
        public string Exceptions { get; set; }
        public string ExceptionStyle { get; set; }
        public int SetClockClientNoteId { get; set; }
        public bool? SetClockClientNoteDisabled { get; set; }
        public bool ImgClockNameVisible { get; set; }
        public bool ImgGeofencingVisible { get; set; }
        public bool ClientId { get; set; }
        public string Schedule2Modal { get; set; }
    }

    public enum TimeCardAuthorizationResultType
    {
        Header,
        DayDetail,
        DailyTotal,
        WeeklyTotal,
        GrandTotal
    }

    public class PunchActivitySprocResults
    {
        public IEnumerable<EmployeePunchActivityDto> Activity { get; set; }
        public IEnumerable<PunchActivityEmployeeInfoDto> Employees { get; set; }
    }

    public class PunchActivityEmployeeInfoDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNumber { get; set; }
        public int? EmployeeActivity { get; set; }
        public DateTime EmployeeHireDate { get; set; }
        public DateTime EmployeeSeparationDate { get; set; }
        public DateTime EmployeeRehireDate { get; set; }
        public int ClockClientTimePolicyId { get; set; }
        public string ClockClientTimePolicyName { get; set; }
    }

    public class EmployeePunchActivityDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime ModifiedPunch { get; set; }
        public int ClockEmployeePunchId { get; set; }
        public int ClockClientLunchId { get; set; }
        public DateTime DateOfPunch { get; set; }
        public string EmployeeName { get; set; }
        public string ShiftDateTime { get; set; }
        public string ShiftDateString { get; set; }
        public DateTime OriginalShiftDate { get; set; }
        public string Comment { get; set; }
        public string ClockName { get; set; }
        public int TimeZoneId { get; set; }
        public int ClientCostCenterId { get; set; }
        public bool IsPendingBenefit { get; set; }
        public string EmployeeComment { get; set; }
        public int ClientDepartmentId { get; set; }
        public int ClientDivisionId { get; set; }
        public int ClientJobCostingAssignmentId1 { get; set; }
        public int ClientJobCostingAssignmentId2 { get; set; }
        public int ClientJobCostingAssignmentId3 { get; set; }
        public int ClientJobCostingAssignmentId4 { get; set; }
        public int ClientJobCostingAssignmentId5 { get; set; }
        public int ClientJobCostingAssignmentId6 { get; set; }
        public bool HasGeofencing { get; set; }
    }
}
