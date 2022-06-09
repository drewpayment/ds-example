using System;
using System.Data;
using Dominion.Utility.Query.LinqKit;

namespace Dominion.Core.Dto.Labor
{
    public class ClockClientRulesSummaryDto : IHasClockClientRulesStartingDayOfWeekIds
    {
        public byte? WeeklyStartingDayOfWeekId { get; set; }
        public byte? WeeklyStartingDayOfWeekIdForPayFrequency { get; set;}
        public byte? DefaultCalendarView { get; set; }
        public string SplitHolidayAmongShifts { get; set; }
        public string ShowSubcheckOnBenefitScreen { get; set; }

        public int ClockClientRulesId { get; set; } //start ClockClientRuleMemebers
        public string Name { get; set; }
        public int? ClientId { get; set; }
        public byte? BiWeeklyStartingDayOfWeekId { get; set; }
        public byte? SemiMonthlyStartingDayOfWeekId { get; set; }
        public byte? MonthlyStartingDayOfWeekId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public byte? InEarlyClockRoundingTypeId { get; set; }
        public byte? OutEarlyClockRoundingTypeId { get; set; }
        public byte? InLateClockRoundingTypeId { get; set; }
        public byte? OutLateClockRoundingTypeId { get; set; }
        public byte? OutLate_OutsideGraceTimeClockRoundingTypeId { get; set; }
        public byte? InEarly_OutsideGraceTimeClockRoundingTypeId { get; set; }
        public byte? OutEarly_OutsideGraceTimeClockRoundingTypeId { get; set; }
        public byte? InLate_OutsideGraceTimeClockRoundingTypeId { get; set; }
        public double? OutLate_OutsideGraceMinutes { get; set; }
        public double? InEarly_OutsideGraceMinutes { get; set; }
        public double? OutEarly_OutsideGraceMinutes { get; set; }
        public double? InLate_OutsideGraceMinutes { get; set; }
        public byte? OutLate_OutsideGraceRoundDirection { get; set; }
        public byte? InEarly_OutsideGraceRoundDirection { get; set; }
        public byte? OutEarly_OutsideGraceRoundDirection { get; set; }
        public byte? InLate_OutsideGraceRoundDirection { get; set; }
        public byte? ClockTimeFormatId { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
        public short InEarlyGraceTime { get; set; }
        public short OutEarlyGraceTime { get; set; }
        public short InLateGraceTime { get; set; }
        public short OutLateGraceTime { get; set; }
        public decimal? ShiftInterval { get; set; }
        public decimal? MaxShift { get; set; }
        public PunchOptionType? PunchOption { get; set; }
        public bool HideCostCenter { get; set; }
        public bool HideDepartment { get; set; }
        public bool HidePunchType { get; set; }
        public byte InEarlyAllowPunchTime { get; set; }
        public byte OutEarlyAllowPunchTime { get; set; }
        public byte InLateAllowPunchTime { get; set; }
        public byte OutLateAllowPunchTime { get; set; }
        public byte? AllPunchesClockRoundingTypeId { get; set; }
        public short AllPunchesGraceTime { get; set; }
        public bool EditPunches { get; set; }
        public bool ImportPunches { get; set; }
        public bool ImportBenefits { get; set; }
        public byte? ApplyHoursOption { get; set; }
        public byte? ClockAllocateHoursFrequencyId { get; set; }
        public byte? ClockAllocateHoursOptionId { get; set; }
        public bool EditBenefits { get; set; }
        public bool HideMultipleSchedules { get; set; }
        public bool HideJobCosting { get; set; }
        public bool HideEmployeeNotes { get; set; }
        public bool IpLockout { get; set; }
        public bool AllowMobilePunch { get; set; }
        public bool HideShift { get; set; } //END ClockClientRules

        public int? ClockClientTimePolicyId { get; set; }
        public int? ClockClientExceptionId { get; set; }
        public int? ClockClientLunchId { get; set; }
        public double? InEarlyMinutes { get; set; }
        public double? InLateMinutes { get; set; }
        public double? OutEarlyMinutes { get; set; }
        public double? OutLateMinutes { get; set; }
        public double? AllPunchesMinutes { get; set; }
        public byte? InEarlyRoundDirection { get; set; }
        public byte? InLateRoundDirection { get; set; }
        public byte? OutEarlyRoundDirection { get; set; }
        public byte? OutLateRoundDirection { get; set; }
        public byte? AllPunchesRoundDirection { get; set; }
        public int? EmployeeId { get; set; }
        public int? ClientCostCenterId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int? HolidayWorkedClientEarningId { get; set; }
        public bool IncludeInOvertimeCalcs { get; set; }
        public int? NumberOfDaysToShow { get; set; }
        public byte? ShiftOptions { get; set; }
        public string IncludeShifts { get; set; }
        public string BlockSupervisorAuthorization { get; set; }
        public string ShowRateOverrideOnBenefitScreen { get; set; }
        public int IsCostCenterRequired { get; set; }
        public int NoPunchesOnScheduledDayException { get; set; }
        public int? TimeZoneId { get; set; }
        public int NoPunchesBeforeHoliday { get; set; }
        public int NoPunchesAfterHoliday { get; set; }
        public int NoPunchesOnScheduledDayBeforeHoliday { get; set; }
        public int NoPunchesOnScheduledDayAfterHoliday { get; set; }
        public int PunchesOnBenefitDay { get; set; }
        public bool ShowHoursInHundreths { get; set; }
        public string ShowClockDropDownList { get; set; }
        public int ClientShiftId { get; set; }
        public int HolidayClientEarningId { get; set; }
        public bool AddToOtherPolicy { get; set; }
        public bool AllowInputPunches { get; set; }
    }

    //TODO: Make a Generic Version of this
    public static class ClockClientRulesSummaryExtensions
    {
        public static DataSet ToDataSet(this ClockClientRulesSummaryDto dto)
        {
            var ds = new DataSet();
            var table = new DataTable();

            var row = table.NewRow();
            
            typeof(ClockClientRulesSummaryDto).GetProperties().ForEach(p =>
            {
                table.Columns.Add((string) p.Name);
                var value = p.GetValue(dto, null);
                row[(string) p.Name] = value;
            });

            table.Rows.Add(row);
            ds.Tables.Add(table);

            return ds;
        }
    }

  
}
