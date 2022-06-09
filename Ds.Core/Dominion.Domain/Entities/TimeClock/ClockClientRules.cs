using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.TimeClock
{
    public partial class ClockClientRules : Entity<ClockClientRules>, IHasModifiedOptionalData, IHasClockClientRulesStartingDayOfWeekIds
    {
        public virtual   int                ClockClientRulesId             { get; set; }
        public virtual   string             Name                           { get; set; }
        public virtual   int                ClientId                       { get; set; }
        public virtual   byte?              WeeklyStartingDayOfWeekId      { get; set; }
        public virtual   byte?              BiWeeklyStartingDayOfWeekId    { get; set; }
        public virtual   byte?              SemiMonthlyStartingDayOfWeekId { get; set; }
        public virtual   byte?              MonthlyStartingDayOfWeekId     { get; set; }
        public virtual   DateTime?          StartTime                      { get; set; }
        public virtual   DateTime?          StopTime                       { get; set; }
        public virtual   PunchRoundingType? InEarlyClockRoundingTypeId     { get; set; }
        public virtual   PunchRoundingType? OutEarlyClockRoundingTypeId    { get; set; }
        public virtual   PunchRoundingType? InLateClockRoundingTypeId      { get; set; }
        public virtual   PunchRoundingType? OutLateClockRoundingTypeId     { get; set; }
        public virtual   PunchRoundingType? InEarly_OutsideGraceTimeClockRoundingTypeId { get; set; }
        public virtual   PunchRoundingType? OutEarly_OutsideGraceTimeClockRoundingTypeId { get; set; }
        public virtual   PunchRoundingType? InLate_OutsideGraceTimeClockRoundingTypeId { get; set; }
        public virtual   PunchRoundingType? OutLate_OutsideGraceTimeClockRoundingTypeId { get; set; }
        public virtual   byte?              ClockTimeFormatId              { get; set; }
        public virtual   int?               ModifiedBy                     { get; set; }
        public virtual   DateTime?          Modified                       { get; set; }
        public virtual   short              InEarlyGraceTime               { get; set; }
        public virtual   short              OutEarlyGraceTime              { get; set; }
        public virtual   short              InLateGraceTime                { get; set; }
        public virtual   short              OutLateGraceTime               { get; set; }
        public virtual   decimal?           ShiftInterval                  { get; set; }
        public virtual   decimal?           MaxShift                       { get; set; }
        public virtual   PunchOptionType?   PunchOption                    { get; set; }
        public virtual   bool               IsHideCostCenter               { get; set; }
        public virtual   bool               IsHideDepartment               { get; set; }
        public virtual   bool               IsHidePunchType                { get; set; }
        public virtual   byte               InEarlyAllowPunchTime          { get; set; }
        public virtual   byte               OutEarlyAllowPunchTime         { get; set; }
        public virtual   byte               InLateAllowPunchTime           { get; set; }
        public virtual   byte               OutLateAllowPunchTime          { get; set; }
        public virtual   PunchRoundingType? AllPunchesClockRoundingTypeId  { get; set; }
        public virtual   short              AllPunchesGraceTime            { get; set; }
        public virtual   bool               IsEditPunches                  { get; set; }
        public virtual   bool               IsImportPunches                { get; set; }
        public virtual   bool               IsImportBenefits               { get; set; }
        public virtual   byte               ApplyHoursOption               { get; set; }
        public virtual   byte               ClockAllocateHoursFrequencyId  { get; set; }
        public virtual   byte               ClockAllocateHoursOptionId     { get; set; }
        public virtual   bool               IsEditBenefits                 { get; set; }
        public virtual   bool               IsHideMultipleSchedules        { get; set; }
        public virtual   bool               IsHideJobCosting               { get; set; }
        public virtual   bool               IsHideEmployeeNotes            { get; set; }
        public virtual   bool               IsIpLockout                    { get; set; }
        public virtual   bool               IsAllowMobilePunch             { get; set; }
        public virtual   bool               IsHideShift                    { get; set; }
        public virtual   bool               AllowInputPunches              { get; set; }

        //Entity References
        public virtual ClockRoundingType InEarlyClockRoundingType { get; set; }
        public virtual ClockRoundingType OutEarlyClockRoundingType { get; set; }
        public virtual ClockRoundingType InLateClockRoundingType { get; set; }
        public virtual ClockRoundingType OutLateClockRoundingType { get; set; }
        public virtual ClockRoundingType AllPunchesClockRoundingType { get; set; }
        public virtual ClockRoundingType InEarlyOutsideGraceTimeClockRoundingType { get; set; }
        public virtual ClockRoundingType OutEarlyOutsideGraceTimeClockRoundingType { get; set; }
        public virtual ClockRoundingType InLateOutsideGraceTimeClockRoundingType { get; set; }
        public virtual ClockRoundingType OutLateOutsideGraceTimeClockRoundingType { get; set; }

        public virtual ICollection<ClockClientDailyRules> ClockClientDailyRules { get; set; }

        public ClockClientRules()
        {
        }
    }
}