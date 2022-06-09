using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class ClockClientRulesDto : IHasTimePolicyRuleShiftSettings, IHasClockClientRulesStartingDayOfWeekIds
    {
        public   int                ClockClientRulesId             { get; set; }
        public   string             Name                           { get; set; }
        public   int                ClientId                       { get; set; }
        public   byte?              WeeklyStartingDayOfWeekId      { get; set; }
        public   byte?              BiWeeklyStartingDayOfWeekId    { get; set; }
        public   byte?              SemiMonthlyStartingDayOfWeekId { get; set; }
        public   byte?              MonthlyStartingDayOfWeekId     { get; set; }
        public   DateTime?          StartTime                      { get; set; }
        public   DateTime?          StopTime                       { get; set; }
        public   PunchRoundingType? InEarlyClockRoundingTypeId     { get; set; }
        public   PunchRoundingType? OutEarlyClockRoundingTypeId    { get; set; }
        public   PunchRoundingType? InLateClockRoundingTypeId      { get; set; }
        public   PunchRoundingType? OutLateClockRoundingTypeId     { get; set; }
        public   byte?              ClockTimeFormatId              { get; set; }
        public   int?               ModifiedBy                     { get; set; }
        public   DateTime?          Modified                       { get; set; }
        public   short              InEarlyGraceTime               { get; set; }
        public   short              OutEarlyGraceTime              { get; set; }
        public   short              InLateGraceTime                { get; set; }
        public   short              OutLateGraceTime               { get; set; }
        public   decimal?           ShiftInterval                  { get; set; }
        public   decimal?           MaxShift                       { get; set; }
        public   PunchOptionType?   PunchOption                    { get; set; }
        public   bool               IsHideCostCenter               { get; set; }
        public   bool               IsHideDepartment               { get; set; }
        public   bool               IsHidePunchType                { get; set; }
        public   byte               InEarlyAllowPunchTime          { get; set; }
        public   byte               OutEarlyAllowPunchTime         { get; set; }
        public   byte               InLateAllowPunchTime           { get; set; }
        public   byte               OutLateAllowPunchTime          { get; set; }
        public   PunchRoundingType? AllPunchesClockRoundingTypeId  { get; set; }
        public   short              AllPunchesGraceTime            { get; set; }
        public   bool               IsEditPunches                  { get; set; }
        public   bool               IsImportPunches                { get; set; }
        public   bool               IsImportBenefits               { get; set; }
        public   byte               ApplyHoursOption               { get; set; }
        public   byte               ClockAllocateHoursFrequencyId  { get; set; }
        public   byte               ClockAllocateHoursOptionId     { get; set; }
        public   bool               IsEditBenefits                 { get; set; }
        public   bool               IsHideMultipleSchedules        { get; set; }
        public   bool               IsHideJobCosting               { get; set; }
        public   bool               IsHideEmployeeNotes            { get; set; }
        public   bool               IsIpLockout                    { get; set; }
        public   bool               IsAllowMobilePunch             { get; set; }
        public   bool               IsHideShift                    { get; set; }

        public bool AllowInputPunches { get; set; }
        public PunchRoundingType? InEarlyOutsideGraceTimeClockRoundingTypeId { get; set; }
        public PunchRoundingType? InLateOutsideGraceTimeClockRoundingTypeId { get; set; }
        public PunchRoundingType? OutEarlyOutsideGraceTimeClockRoundingTypeId { get; set; }
        public PunchRoundingType? OutLateOutsideGraceTimeClockRoundingTypeId { get; set; }

        //public byte? InEarlyOutsideGraceTimeClockRoundingTypeId { get; set; }
        //public byte? InLateOutsideGraceTimeClockRoundingTypeId { get; set; }
        //public byte? OutEarlyOutsideGraceTimeClockRoundingTypeId { get; set; }
        //public byte? OutLateOutsideGraceTimeClockRoundingTypeId { get; set; }

        public ICollection<ClockClientDailyRulesDto> ClockClientDailyRules { get; set; }
    }
}
