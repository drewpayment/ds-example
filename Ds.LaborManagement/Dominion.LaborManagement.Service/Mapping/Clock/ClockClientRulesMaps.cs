using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Entities.TimeClock;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.Utility.Mapping;

namespace Dominion.LaborManagement.Service.Mapping.Clock
{
    public class ClockClientRulesMaps
    {
        public class ToClockClientRulesDto : ExpressionMapper<ClockClientRules, ClockClientRulesDto>
        {
            public override Expression<Func<ClockClientRules, ClockClientRulesDto>> MapExpression
            {
                get
                {
                      return x => new ClockClientRulesDto()
                    {
                        StartTime = x.StartTime,
                        StopTime = x.StopTime,
                        ClientId = x.ClientId,
                        Name = x.Name,
                        InLateClockRoundingTypeId = x.InLateClockRoundingTypeId,
                        ClockClientRulesId = x.ClockClientRulesId,
                        OutLateClockRoundingTypeId = x.OutLateClockRoundingTypeId,
                        AllPunchesGraceTime = x.AllPunchesGraceTime,
                        AllPunchesClockRoundingTypeId =(PunchRoundingType?) x.AllPunchesClockRoundingTypeId,
                        OutEarlyClockRoundingTypeId = x.OutEarlyClockRoundingTypeId,
                        InEarlyGraceTime = x.InEarlyGraceTime,
                        OutLateGraceTime = x.OutLateGraceTime,
                        InEarlyClockRoundingTypeId = x.InEarlyClockRoundingTypeId,
                        InLateGraceTime = x.InLateGraceTime,
                        OutEarlyGraceTime = x.OutEarlyGraceTime,
                        ApplyHoursOption = x.ApplyHoursOption,
                        BiWeeklyStartingDayOfWeekId = x.BiWeeklyStartingDayOfWeekId,
                        ClockAllocateHoursFrequencyId = x.ClockAllocateHoursFrequencyId,
                        ClockAllocateHoursOptionId = x.ClockAllocateHoursOptionId,
                        ClockTimeFormatId = x.ClockTimeFormatId,
                        InEarlyAllowPunchTime = x.InEarlyAllowPunchTime,
                        InLateAllowPunchTime = x.InLateAllowPunchTime,
                        IsEditBenefits = x.IsEditBenefits,
                        IsHideCostCenter = x.IsHideCostCenter,
                        IsEditPunches = x.IsEditPunches,
                        IsHideDepartment = x.IsHideDepartment,
                        IsHideEmployeeNotes = x.IsHideEmployeeNotes,
                        IsHideJobCosting = x.IsHideJobCosting,
                        IsHideMultipleSchedules = x.IsHideMultipleSchedules,
                        IsHidePunchType = x.IsHidePunchType,
                        IsHideShift = x.IsHideShift,
                        IsIpLockout = x.IsIpLockout,
                        IsImportBenefits = x.IsImportBenefits,
                        IsImportPunches = x.IsImportPunches,
                        MaxShift = x.MaxShift,
                        MonthlyStartingDayOfWeekId = x.MonthlyStartingDayOfWeekId,
                        OutEarlyAllowPunchTime = x.OutEarlyAllowPunchTime,
                        OutLateAllowPunchTime = x.OutLateAllowPunchTime,
                        PunchOption = x.PunchOption,
                        ShiftInterval = x.ShiftInterval,
                        SemiMonthlyStartingDayOfWeekId = x.SemiMonthlyStartingDayOfWeekId,
                        WeeklyStartingDayOfWeekId = x.WeeklyStartingDayOfWeekId,
                        AllowInputPunches = x.AllowInputPunches,
                        IsAllowMobilePunch = x.IsAllowMobilePunch,
                        InEarlyOutsideGraceTimeClockRoundingTypeId = x.InEarly_OutsideGraceTimeClockRoundingTypeId,
                        OutEarlyOutsideGraceTimeClockRoundingTypeId = x.OutEarly_OutsideGraceTimeClockRoundingTypeId,
                        InLateOutsideGraceTimeClockRoundingTypeId = x.InLate_OutsideGraceTimeClockRoundingTypeId,
                        OutLateOutsideGraceTimeClockRoundingTypeId = x.OutLate_OutsideGraceTimeClockRoundingTypeId,
                      };
                }
            }
        }

        /// <summary>
        /// This map coverts and EmployeePay object to a ClockClientRulesSummaryDto.  It is the result set that replaces legacy
        /// Sproc: [dbo].[spGetClockClientRulesByEmployeeId]
        /// NOTE:  This map will throw and InvalidOperation exception if used with a ExecuteQueryAs() method.
        /// The nested logic doe not support a nest operation tree.  
        /// This is similar to <see cref="ToClockClientRulesSummaryDto"/>
        /// However, not all items are loaded.  CaleendarOptions, DefaultCalendarView, SplitHolidayAmongShifts
        /// ShowSubCheckOnBenefitsScreen, 
        /// TODO: Finish list of removed items, remove possibly more, removed commented code
        /// </summary>
        public class ToClockClientRulesSummaryPartial : ExpressionMapper<EmployeePay, ClockClientRulesSummaryDto>

        {
            public override Expression<Func<EmployeePay, ClockClientRulesSummaryDto>> MapExpression => x => GetClockClientRulesSummaryDto(x);

            private ClockClientRulesSummaryDto GetClockClientRulesSummaryDto(EmployeePay ep)
            {
                Debug.WriteLine($"GETCLOCKCLIENTRULESSUMMARY : Start Map {DateTime.Now}");
                var dto = new ClockClientRulesSummaryDto();

                var timePolicy = ep.ClockEmployee.TimePolicy;
                var rules = ep.ClockEmployee.TimePolicy.Rules;
                var accountOptions = ep.ClockEmployee.Client.AccountOptions;

                switch (ep.PayFrequencyId)
                {
                    case PayFrequencyType.Weekly:
                        dto.WeeklyStartingDayOfWeekId = rules.WeeklyStartingDayOfWeekId;
                        break;
                    case PayFrequencyType.BiWeekly:
                    case PayFrequencyType.AlternateBiWeekly:
                        dto.WeeklyStartingDayOfWeekId = rules.BiWeeklyStartingDayOfWeekId;
                        break;
                    case PayFrequencyType.SemiMonthly:
                        dto.WeeklyStartingDayOfWeekId = rules.SemiMonthlyStartingDayOfWeekId;
                        break;
                    case PayFrequencyType.Monthly:
                        dto.WeeklyStartingDayOfWeekId = rules.MonthlyStartingDayOfWeekId;
                        break;
                }

                dto.ClockClientRulesId = rules.ClockClientRulesId;
                dto.Name = rules.Name;
                dto.ClientId = ep.ClientId;
                dto.BiWeeklyStartingDayOfWeekId = rules.BiWeeklyStartingDayOfWeekId;
                dto.SemiMonthlyStartingDayOfWeekId = rules.SemiMonthlyStartingDayOfWeekId;
                dto.MonthlyStartingDayOfWeekId = rules.SemiMonthlyStartingDayOfWeekId;
                dto.StartTime = rules.StartTime;
                dto.StopTime = rules.StopTime;
                dto.InEarlyClockRoundingTypeId = (byte?) rules.InEarlyClockRoundingTypeId;
                dto.OutEarlyClockRoundingTypeId = (byte?) rules.OutEarlyClockRoundingTypeId;
                dto.InLateClockRoundingTypeId = (byte?) rules.InLateClockRoundingTypeId;
                dto.OutLateClockRoundingTypeId = (byte?) rules.OutLateClockRoundingTypeId;
                dto.InEarly_OutsideGraceTimeClockRoundingTypeId = (byte?)rules.InEarly_OutsideGraceTimeClockRoundingTypeId;
                dto.OutEarly_OutsideGraceTimeClockRoundingTypeId = (byte?)rules.OutEarly_OutsideGraceTimeClockRoundingTypeId;
                dto.InLate_OutsideGraceTimeClockRoundingTypeId = (byte?)rules.InLate_OutsideGraceTimeClockRoundingTypeId;
                dto.OutLate_OutsideGraceTimeClockRoundingTypeId = (byte?)rules.OutLate_OutsideGraceTimeClockRoundingTypeId;
                dto.InEarly_OutsideGraceMinutes = rules.InEarlyOutsideGraceTimeClockRoundingType?.Minutes;
                dto.OutEarly_OutsideGraceMinutes = rules.OutEarlyOutsideGraceTimeClockRoundingType?.Minutes;
                dto.InLate_OutsideGraceMinutes = rules.InLateOutsideGraceTimeClockRoundingType?.Minutes;
                dto.OutLate_OutsideGraceMinutes = rules.OutLateOutsideGraceTimeClockRoundingType?.Minutes;
                dto.InEarly_OutsideGraceRoundDirection = rules.InEarlyOutsideGraceTimeClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.OutEarly_OutsideGraceRoundDirection = rules.OutEarlyOutsideGraceTimeClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.InLate_OutsideGraceRoundDirection = rules.InLateOutsideGraceTimeClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.OutLate_OutsideGraceRoundDirection = rules.OutLateOutsideGraceTimeClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.ShiftInterval = rules.ShiftInterval;
                dto.MaxShift = rules.MaxShift;
                dto.PunchOption = rules.PunchOption;
                dto.HideCostCenter = rules.IsHideCostCenter;
                dto.HideDepartment = rules.IsHideDepartment;
                dto.HidePunchType = rules.IsHidePunchType;
                dto.InEarlyGraceTime = rules.InEarlyGraceTime;
                dto.InEarlyAllowPunchTime = rules.InEarlyAllowPunchTime;
                dto.OutEarlyAllowPunchTime = rules.OutEarlyAllowPunchTime;
                dto.InLateAllowPunchTime = rules.InLateAllowPunchTime;
                dto.OutLateAllowPunchTime = rules.OutLateAllowPunchTime;
                dto.OutLateGraceTime = rules.OutLateGraceTime;
                dto.AllPunchesClockRoundingTypeId = (byte?) rules.AllPunchesClockRoundingTypeId;
                dto.AllPunchesGraceTime = rules.AllPunchesGraceTime;
                dto.EditPunches = rules.IsEditPunches;
                dto.ImportPunches = rules.IsImportPunches;
                dto.ImportBenefits = rules.IsImportBenefits;
                dto.ApplyHoursOption = rules.ApplyHoursOption;
                dto.ClockAllocateHoursFrequencyId = rules.ClockAllocateHoursFrequencyId;
                dto.ClockAllocateHoursOptionId = rules.ClockAllocateHoursOptionId;
                dto.EditBenefits = rules.IsEditBenefits;
                dto.HideMultipleSchedules = rules.IsHideMultipleSchedules;
                dto.HideJobCosting = rules.IsHideJobCosting;
                dto.IpLockout = rules.IsIpLockout;
                dto.AllowMobilePunch = rules.IsAllowMobilePunch;
                dto.HideShift = rules.IsHideShift;
                dto.AllowInputPunches = rules.AllowInputPunches;

                dto.ClockClientTimePolicyId = timePolicy.ClockClientTimePolicyId;
                dto.ClockClientExceptionId = timePolicy.ClockClientExceptionId;

                dto.InEarlyMinutes = rules.InEarlyClockRoundingType?.Minutes;
                dto.InLateMinutes = rules.InLateClockRoundingType?.Minutes;
                dto.OutEarlyMinutes = rules.OutEarlyClockRoundingType?.Minutes;
                dto.OutLateMinutes = rules.OutLateClockRoundingType?.Minutes;
                dto.AllPunchesMinutes = rules.AllPunchesClockRoundingType?.Minutes;

                dto.InEarlyRoundDirection = rules.InEarlyClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.InLateRoundDirection = rules.InLateClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.OutEarlyRoundDirection = rules.OutEarlyClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.OutLateRoundDirection = rules.OutLateClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.AllPunchesRoundDirection = rules.AllPunchesClockRoundingType?.RoundDirection ?? 1;
                dto.ClockTimeFormatId = rules.ClockTimeFormatId ?? 1;

                dto.EmployeeId = ep.EmployeeId; //taken from EP instead of E (as in sp)
                dto.ClientCostCenterId = ep.Employee.ClientCostCenterId ?? 0;
                dto.ClientDepartmentId = ep.Employee.ClientDepartmentId ?? 0;
                dto.HolidayWorkedClientEarningId = timePolicy.Holidays?.HolidayWorkedClientEarningId ?? 0;

                dto.IncludeInOvertimeCalcs = ep.Employee.Client.Earnings.FirstOrDefault(
                    e => e.ClientEarningId == dto.HolidayWorkedClientEarningId)?.IsIncludeInOvertimeCalcs ?? false; //TODO: holidayEarning is always null here

                //var numDays = accountOptions.FirstOrDefault
                //    (ao => ao.AccountOption == AccountOption.TimeClock_DaysToShowOnPunch)?.Value;
                //dto.NumberOfDaysToShow = string.IsNullOrEmpty(numDays) ? 6 : int.Parse(numDays) - 1;

                var shift = accountOptions.FirstOrDefault(ao => ao.AccountOption == AccountOption.TimeClock_UseShifts);
                dto.IncludeShifts = shift?.Value == 0.ToString() ? "false" : "true";

                //dto.ShiftOptions = shift.AccountOptionInfo.AccountOptionItems.FirstOrDefault(
                //        coi => coi.AccountOptionItemId.ToString() == shift.Value)?.Value ?? 0;

                //dto.BlockSupervisorAuthorization = (accountOptions.FirstOrDefault(
                //    ao => ao.AccountOption == AccountOption.TimeClock_BlockSupervisorFromAuthorizingTimecards)?.Value ?? "0") != "0" ? "true" : "false";
                //dto.ShowRateOverrideOnBenefitScreen = (accountOptions.FirstOrDefault(
                //    ao => ao.AccountOption == AccountOption.TimeClock_ShowRateOverrideOnBenefits)?.Value ?? "0") != "0" ? "true" : "false"; ;

                //var clientAccountOption = (accountOptions.FirstOrDefault(
                //        ao => ao.AccountOption == AccountOption.TimeClock_RequireEmployeeToPickCostCenter));

                //var accountOptionItem = clientAccountOption?.AccountOptionInfo.AccountOptionItems.FirstOrDefault(
                //        i => i.AccountOptionItemId.ToString().Equals(clientAccountOption.Value));

                //var value = accountOptionItem != null ? accountOptionItem.Value : 1;

                //if (value == 2 || (value == 3 && ep.Employee.CostCenter == null))
                //    dto.IsCostCenterRequired = 1;
                //else
                //    dto.IsCostCenterRequired = 0;

                //int returnValue;
                //int.TryParse(value.ToString(), out returnValue);

                //var exceptionDetails = ep.ClockEmployee.TimePolicy.Exceptions.ExceptionDetails;

                //dto.NoPunchesOnScheduledDayException = exceptionDetails.FirstOrDefault(
                //    ei => ei.ExceptionType == ClockExceptionType.NoPunchesOnScheduledDay && (ei.IsSelected ?? false))?.ClockExceptionId ?? 0;

                dto.TimeZoneId = timePolicy.TimeZoneId;

                //dto.NoPunchesBeforeHoliday = exceptionDetails.FirstOrDefault(
                //    ei => ei.ExceptionType == ClockExceptionType.DidNotWorkBeforeHoliday && (ei.IsSelected ?? false))?.ClockExceptionId ?? 0;

                //dto.NoPunchesAfterHoliday = exceptionDetails.FirstOrDefault(
                //    ei => ei.ExceptionType == ClockExceptionType.DidNotWorkAfterHoliday && (ei.IsSelected ?? false))?.ClockExceptionId ?? 0;

                //dto.NoPunchesOnScheduledDayBeforeHoliday = exceptionDetails.FirstOrDefault(
                //    ei => ei.ExceptionType == ClockExceptionType.DidNotWorkBeforeHolidayScheduled && (ei.IsSelected ?? false))?.ClockExceptionId ?? 0;

                //dto.NoPunchesOnScheduledDayAfterHoliday = exceptionDetails.FirstOrDefault(
                //    ei => ei.ExceptionType == ClockExceptionType.DidNotWorkAfterHolidayScheduled && (ei.IsSelected ?? false))?.ClockExceptionId ?? 0;

                //dto.PunchesOnBenefitDay = exceptionDetails.FirstOrDefault(
                //    ei => ei.ExceptionType == ClockExceptionType.PunchesOnBenefitDay && (ei.IsSelected ?? false))?
                //    .ClockExceptionId ?? 0;

                //var hundrethsOptionValue = clientAccountOption?.AccountOptionInfo.AccountOptionItems.FirstOrDefault(
                //    co => co.AccountOption == AccountOption.TimeClock_TimeShowHoursInHundreths)?.Value;
                //int hoursInHundrethsValue;
                //int.TryParse(hundrethsOptionValue.ToString(), out hoursInHundrethsValue);

                //dto.ShowHoursInHundreths = hoursInHundrethsValue == -1 ? "true" : "false";

                //var showClockDropDownOptionValue = clientAccountOption?.AccountOptionInfo.AccountOptionItems.FirstOrDefault(
                //        co => co.AccountOption == AccountOption.TimeClock_ShowClockDropdownList)?.Value;

                //int showClockDropDownValue;
                //int.TryParse(showClockDropDownOptionValue.ToString(), out showClockDropDownValue);

                //dto.ShowClockDropDownList = showClockDropDownValue == 1 ? "true" : "false"; ;

                dto.HolidayClientEarningId = timePolicy.Holidays?.ClientEarningId ?? 0;
                dto.ClientShiftId = ep.ClientShiftId ?? 0;
                dto.AddToOtherPolicy = timePolicy.IsAddToOtherPolicy;
                dto.AddToOtherPolicy = timePolicy.IsAddToOtherPolicy;

                Debug.WriteLine($"GETCLOCKCLIENTRULESSUMMARY : Stop Map {DateTime.Now}");
                return dto;
            }

        }

        /// <summary>
        /// This map coverts and EmployeePay object to a ClockClientRulesSummaryDto.  It is the result set that replaces legacy
        /// Sproc: [dbo].[spGetClockClientRulesByEmployeeId]
        /// NOTE:  This map will throw and InvalidOperation exception if used with a ExecuteQueryAs() method.
        /// The nested logic doe not support a nest operation tree.  
        /// </summary>
        public class ToClockClientRulesSummaryDto : ExpressionMapper<EmployeePay, ClockClientRulesSummaryDto>
        {

            public override Expression<Func<EmployeePay, ClockClientRulesSummaryDto>> MapExpression => x => GetClockClientRulesSummaryDto(x);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ep"></param>
            /// <returns></returns>
            private ClockClientRulesSummaryDto GetClockClientRulesSummaryDto(EmployeePay ep)
            {
                var dto = new ClockClientRulesSummaryDto();

                var timePolicy = ep.ClockEmployee.TimePolicy;
                var rules = ep.ClockEmployee.TimePolicy.Rules;
                var accountOptions = ep.ClockEmployee.Client.AccountOptions;


                dto.WeeklyStartingDayOfWeekIdForPayFrequency = rules.GetStartingDayOfWeekTypeAsByte(ep.PayFrequencyId);


                var calendarOption = accountOptions.FirstOrDefault(
                    co => co.AccountOption == AccountOption.TimeClock_DefaultPunchCalendarView);

                dto.DefaultCalendarView = calendarOption?.AccountOptionInfo.AccountOptionItems.FirstOrDefault(
                    coi => coi.AccountOptionItemId.ToString() == calendarOption.Value)?.Value ?? 1;

                var splitOption = accountOptions.FirstOrDefault(
                        co => co.AccountOption == AccountOption.TimeClock_SplitHolidaysAmongShifts);

                dto.SplitHolidayAmongShifts = splitOption?.AccountOptionInfo.AccountOptionItems.FirstOrDefault(
                    coi => coi.AccountOptionItemId.ToString() == splitOption.Value)?.Value.ToString() ?? "0";

                var showSubCheck = accountOptions.FirstOrDefault(
                        co => co.AccountOption == AccountOption.TimeClock_ShowSubcheckOnBenefitScreen);
                dto.ShowSubcheckOnBenefitScreen = showSubCheck?.AccountOptionInfo.AccountOptionItems.FirstOrDefault(
                    coi => coi.AccountOptionItemId.ToString() == showSubCheck.Value)?.Value.ToString() ?? "0";

                dto.ClockClientRulesId = rules.ClockClientRulesId;
                dto.Name = rules.Name;
                dto.ClientId = ep.ClientId;
                dto.WeeklyStartingDayOfWeekId = rules.WeeklyStartingDayOfWeekId;
                dto.BiWeeklyStartingDayOfWeekId = rules.BiWeeklyStartingDayOfWeekId;
                dto.SemiMonthlyStartingDayOfWeekId = rules.SemiMonthlyStartingDayOfWeekId;
                dto.MonthlyStartingDayOfWeekId = rules.SemiMonthlyStartingDayOfWeekId;
                dto.StartTime = rules.StartTime;
                dto.StopTime = rules.StopTime;
                dto.InEarlyClockRoundingTypeId = (byte?) rules.InEarlyClockRoundingTypeId;
                dto.OutEarlyClockRoundingTypeId = (byte?) rules.OutEarlyClockRoundingTypeId;
                dto.InLateClockRoundingTypeId = (byte?) rules.InLateClockRoundingTypeId;
                dto.OutLateClockRoundingTypeId = (byte?) rules.OutLateClockRoundingTypeId;
                dto.InEarly_OutsideGraceTimeClockRoundingTypeId = (byte?)rules.InEarly_OutsideGraceTimeClockRoundingTypeId;
                dto.OutEarly_OutsideGraceTimeClockRoundingTypeId = (byte?)rules.OutEarly_OutsideGraceTimeClockRoundingTypeId;
                dto.InLate_OutsideGraceTimeClockRoundingTypeId = (byte?)rules.InLate_OutsideGraceTimeClockRoundingTypeId;
                dto.OutLate_OutsideGraceTimeClockRoundingTypeId = (byte?)rules.OutLate_OutsideGraceTimeClockRoundingTypeId;
                dto.InEarly_OutsideGraceMinutes = rules.InEarlyOutsideGraceTimeClockRoundingType?.Minutes;
                dto.OutEarly_OutsideGraceMinutes = rules.OutEarlyOutsideGraceTimeClockRoundingType?.Minutes;
                dto.InLate_OutsideGraceMinutes = rules.InLateOutsideGraceTimeClockRoundingType?.Minutes;
                dto.OutLate_OutsideGraceMinutes = rules.OutLateOutsideGraceTimeClockRoundingType?.Minutes;
                dto.InEarly_OutsideGraceRoundDirection = rules.InEarlyOutsideGraceTimeClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.OutEarly_OutsideGraceRoundDirection = rules.OutEarlyOutsideGraceTimeClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.InLate_OutsideGraceRoundDirection = rules.InLateOutsideGraceTimeClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.OutLate_OutsideGraceRoundDirection = rules.OutLateOutsideGraceTimeClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.ShiftInterval = rules.ShiftInterval;
                dto.MaxShift = rules.MaxShift;
                dto.PunchOption = rules.PunchOption;
                dto.HideCostCenter = rules.IsHideCostCenter;
                dto.HideDepartment = rules.IsHideDepartment;
                dto.HidePunchType = rules.IsHidePunchType;
                dto.InEarlyGraceTime = rules.InEarlyGraceTime;
                dto.InEarlyAllowPunchTime = rules.InEarlyAllowPunchTime;
                dto.OutEarlyAllowPunchTime = rules.OutEarlyAllowPunchTime;
                dto.InLateAllowPunchTime = rules.InLateAllowPunchTime;
                dto.OutLateAllowPunchTime = rules.OutLateAllowPunchTime;
                dto.OutLateGraceTime = rules.OutLateGraceTime;
                dto.AllPunchesClockRoundingTypeId = (byte?) rules.AllPunchesClockRoundingTypeId;
                dto.AllPunchesGraceTime = rules.AllPunchesGraceTime;
                dto.EditPunches = rules.IsEditPunches;
                dto.ImportPunches = rules.IsImportPunches;
                dto.ImportBenefits = rules.IsImportBenefits;
                dto.ApplyHoursOption = rules.ApplyHoursOption;
                dto.ClockAllocateHoursFrequencyId = rules.ClockAllocateHoursFrequencyId;
                dto.ClockAllocateHoursOptionId = rules.ClockAllocateHoursOptionId;
                dto.EditBenefits = rules.IsEditBenefits;
                dto.HideMultipleSchedules = rules.IsHideMultipleSchedules;
                dto.HideJobCosting = rules.IsHideJobCosting;
                dto.IpLockout = rules.IsIpLockout;
                dto.AllowMobilePunch = rules.IsAllowMobilePunch;
                dto.HideShift = rules.IsHideShift;
                dto.AllowInputPunches = rules.AllowInputPunches;

                dto.ClockClientTimePolicyId = timePolicy.ClockClientTimePolicyId;
                dto.ClockClientExceptionId = timePolicy.ClockClientExceptionId;

                dto.InEarlyMinutes = rules.InEarlyClockRoundingType?.Minutes;
                dto.InLateMinutes = rules.InLateClockRoundingType?.Minutes;
                dto.OutEarlyMinutes = rules.OutEarlyClockRoundingType?.Minutes;
                dto.OutLateMinutes = rules.OutLateClockRoundingType?.Minutes;
                dto.AllPunchesMinutes = rules.AllPunchesClockRoundingType?.Minutes;

                dto.InEarlyRoundDirection = rules.InEarlyClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.InLateRoundDirection = rules.InLateClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.OutEarlyRoundDirection = rules.OutEarlyClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.OutLateRoundDirection = rules.OutLateClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.AllPunchesRoundDirection = rules.AllPunchesClockRoundingType?.RoundDirection ?? 1; //default RoundDirection.UpTo
                dto.ClockTimeFormatId = rules.ClockTimeFormatId ?? 1;

                dto.EmployeeId = ep.EmployeeId; //taken from EP instead of E (as in sp)
                dto.ClientCostCenterId = ep.Employee.ClientCostCenterId ?? 0;
                dto.ClientDepartmentId = ep.Employee.ClientDepartmentId ?? 0;
                dto.HolidayWorkedClientEarningId = timePolicy.Holidays?.HolidayWorkedClientEarningId ?? 0;

                dto.IncludeInOvertimeCalcs = ep.Employee.Client.Earnings.FirstOrDefault(
                    e => e.ClientEarningId == dto.HolidayWorkedClientEarningId)?.IsIncludeInOvertimeCalcs ?? false; //TODO: holidayEarning is always null here

                var numDays = accountOptions.FirstOrDefault
                    (ao => ao.AccountOption == AccountOption.TimeClock_DaysToShowOnPunch)?.Value;
                dto.NumberOfDaysToShow = string.IsNullOrEmpty(numDays) ? 6 : int.Parse(numDays) - 1;

                var shift = accountOptions.FirstOrDefault(ao => ao.AccountOption == AccountOption.TimeClock_UseShifts);
                dto.IncludeShifts = shift?.Value == 0.ToString() ? "false" : "true";

                dto.ShiftOptions = shift?.AccountOptionInfo?.AccountOptionItems?.FirstOrDefault(
                        coi => coi.AccountOptionItemId.ToString() == shift.Value)?.Value ?? 0;

                dto.BlockSupervisorAuthorization = (accountOptions.FirstOrDefault(
                    ao => ao.AccountOption == AccountOption.TimeClock_BlockSupervisorFromAuthorizingTimecards)?.Value ?? "0") != "0" ? "true" : "false";
                dto.ShowRateOverrideOnBenefitScreen = (accountOptions.FirstOrDefault(
                    ao => ao.AccountOption == AccountOption.TimeClock_ShowRateOverrideOnBenefits)?.Value ?? "0") != "0" ? "true" : "false"; ;

                var clientAccountOption_RequireEmployeeToPickCostCenter = (accountOptions.FirstOrDefault(
                        ao => ao.AccountOption == AccountOption.TimeClock_RequireEmployeeToPickCostCenter));

                var accountOptionItem_RequireEmployeeToPickCostCenter = clientAccountOption_RequireEmployeeToPickCostCenter?.AccountOptionInfo.AccountOptionItems.FirstOrDefault(
                        i => i.AccountOptionItemId.ToString().Equals(clientAccountOption_RequireEmployeeToPickCostCenter.Value));

                var value = accountOptionItem_RequireEmployeeToPickCostCenter != null ? accountOptionItem_RequireEmployeeToPickCostCenter.Value : 1;

                if (value == 2 || (value == 3 && ep.Employee.CostCenter == null))
                    dto.IsCostCenterRequired = 1;
                else
                    dto.IsCostCenterRequired = 0;

                int returnValue;
                int.TryParse(value.ToString(), out returnValue);

                var exceptionDetails = ep.ClockEmployee.TimePolicy.Exceptions?.ExceptionDetails;
                dto.TimeZoneId = timePolicy.TimeZoneId;

                if (exceptionDetails != null)
                {
                    dto.NoPunchesOnScheduledDayException = (int)(exceptionDetails.FirstOrDefault(ei =>
                                                                     ei.ClockExceptionId == ClockExceptionType.NoPunchesOnScheduledDay &&
                                                                     (ei.IsSelected ?? false))?.ClockExceptionId ?? 0);
                    

                    dto.NoPunchesBeforeHoliday = (int)(exceptionDetails.FirstOrDefault(
                                                           ei => ei.ClockExceptionId == ClockExceptionType.DidNotWorkBeforeHoliday && (ei.IsSelected ?? false))?.ClockExceptionId ?? 0);

                    dto.NoPunchesAfterHoliday = (int)(exceptionDetails.FirstOrDefault(
                                                          ei => ei.ClockExceptionId == ClockExceptionType.DidNotWorkAfterHoliday && (ei.IsSelected ?? false))?.ClockExceptionId ?? 0);

                    dto.NoPunchesOnScheduledDayBeforeHoliday = (int)(exceptionDetails.FirstOrDefault(
                                                                         ei => ei.ClockExceptionId == ClockExceptionType.DidNotWorkBeforeHolidayScheduled && (ei.IsSelected ?? false))?.ClockExceptionId ?? 0);

                    dto.NoPunchesOnScheduledDayAfterHoliday = (int)(exceptionDetails.FirstOrDefault(
                                                                        ei => ei.ClockExceptionId == ClockExceptionType.DidNotWorkAfterHolidayScheduled && (ei.IsSelected ?? false))?.ClockExceptionId ?? 0);

                    dto.PunchesOnBenefitDay = (int)(exceptionDetails.FirstOrDefault(
                                                        ei => ei.ClockExceptionId == ClockExceptionType.PunchesOnBenefitDay && (ei.IsSelected ?? false))?.ClockExceptionId ?? 0);
                }
                else
                {
                    dto.NoPunchesOnScheduledDayException = 0;

                    dto.NoPunchesBeforeHoliday = 0;

                    dto.NoPunchesAfterHoliday = 0;

                    dto.NoPunchesOnScheduledDayBeforeHoliday = 0;

                    dto.NoPunchesOnScheduledDayAfterHoliday = 0;

                    dto.PunchesOnBenefitDay = 0;
                }



                var clientAccountOption_ShowHoursInHundreths = (accountOptions.FirstOrDefault(
                       ao => ao.AccountOption == AccountOption.TimeClock_TimeShowHoursInHundreths));

                var hundrethsOptionValue = clientAccountOption_ShowHoursInHundreths?.Value;
                int hoursInHundrethsValue;
                int.TryParse(hundrethsOptionValue.ToString(), out hoursInHundrethsValue);

                dto.ShowHoursInHundreths = hoursInHundrethsValue == -1 ? true : false;

                var showClockDropDownOptionValue = clientAccountOption_RequireEmployeeToPickCostCenter?.AccountOptionInfo.AccountOptionItems.FirstOrDefault(
                    co => co.AccountOption == AccountOption.TimeClock_ShowClockDropdownList)?.Value;

                int showClockDropDownValue;
                int.TryParse(showClockDropDownOptionValue.ToString(), out showClockDropDownValue);

                dto.ShowClockDropDownList = showClockDropDownValue == 1 ? "true" : "false"; ;

                dto.HolidayClientEarningId = timePolicy.Holidays?.ClientEarningId ?? 0;
                dto.ClientShiftId = ep.ClientShiftId ?? 0;
                dto.AddToOtherPolicy = timePolicy.IsAddToOtherPolicy;
                dto.AddToOtherPolicy = timePolicy.IsAddToOtherPolicy;

                return dto;
            }
        }
    }
}