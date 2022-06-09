using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Labor;

namespace Dominion.LaborManagement.Dto.Clock
{
    public class ClockClientRulesDto
    {
        public int                ClockClientRulesId                          { get; set; }
        public string             Name                                        { get; set; }
        public int                ClientId                                    { get; set; }
        public byte?              WeeklyStartingDayOfWeekId                   { get; set; }
        public byte?              BiWeeklyStartingDayOfWeekId                 { get; set; }
        public byte?              SemiMonthlyStartingDayOfWeekId              { get; set; }
        public byte?              MonthlyStartingDayOfWeekId                  { get; set; }
        public DateTime?          StartTime                                   { get; set; }
        public DateTime?          StopTime                                    { get; set; }
        public byte?              InEarlyClockRoundingTypeId                  { get; set; }
        public byte?              OutEarlyClockRoundingTypeId                 { get; set; }
        public byte?              InLateClockRoundingTypeId                   { get; set; }
        public byte?              OutLateClockRoundingTypeId                  { get; set; }
        public byte?              ClockTimeFormatId                           { get; set; }
        public short              InEarlyGraceTime                            { get; set; }
        public short              OutEarlyGraceTime                           { get; set; }
        public short              InLateGraceTime                             { get; set; }
        public short              OutLateGraceTime                            { get; set; }
        public decimal?           ShiftInterval                               { get; set; }
        public decimal?           MaxShift                                    { get; set; }
        public byte?              PunchOption                                 { get; set; }
        public bool               IsHideCostCenter                            { get; set; }
        public bool               IsHideDepartment                            { get; set; }
        public bool               IsHidePunchType                             { get; set; }
        public byte               InEarlyAllowPunchTime                       { get; set; }
        public byte               OutEarlyAllowPunchTime                      { get; set; }
        public byte               InLateAllowPunchTime                        { get; set; }
        public byte               OutLateAllowPunchTime                       { get; set; }
        public PunchRoundingType? AllPunchesClockRoundingTypeId               { get; set; }
        public short              AllPunchesGraceTime                         { get; set; }
        public bool               IsEditPunches                               { get; set; }
        public byte               ApplyHoursOption                            { get; set; }
        public byte               ClockAllocateHoursFrequencyId               { get; set; }
        public byte               ClockAllocateHoursOptionId                  { get; set; }
        public bool               IsEditBenefits                              { get; set; }
        public bool               IsHideMultipleSchedules                     { get; set; }
        public bool               IsHideJobCosting                            { get; set; }
        public bool               IsHideEmployeeNotes                         { get; set; }
        public bool               IsIpLockout                                 { get; set; }
        public bool               IsHideShift                                 { get; set; }
        public bool               AllowInputPunches                           { get; set; }
        public byte?              InEarlyOutsideGraceTimeClockRoundingTypeId  { get; set; } 
	   public byte?              InLateOutsideGraceTimeClockRoundingTypeId   { get; set; } 
	   public byte?              OutEarlyOutsideGraceTimeClockRoundingTypeId { get; set; } 
	   public byte?              OutLateOutsideGraceTimeClockRoundingTypeId  { get; set; } 


        public virtual ICollection<ClockClientDailyRulesDto> ClockClientDailyRules { get; set; }


    }
}