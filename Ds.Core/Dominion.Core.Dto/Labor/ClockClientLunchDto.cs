using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Labor
{
    public class ClockClientLunchDto
    {
        public int ClockClientLunchId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public double? Length { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsDoEmployeesPunch { get; set; }
        public bool? IsAutoDeducted { get; set; }
        public double? AutoDeductedWorkedHours { get; set; }
        public short? GraceTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public int? ClientCostCenterId { get; set; }
        public byte PunchType { get; set; }
        public bool IsShowPunches { get; set; }
        public bool IsMaxPaid { get; set; }
        public byte? InEarlyClockRoundingTypeId { get; set; }
        public byte? OutEarlyClockRoundingTypeId { get; set; }
        public byte? InLateClockRoundingTypeId { get; set; }
        public byte? OutLateClockRoundingTypeId { get; set; }
        public short? InEarlyGraceTime { get; set; }
        public short? OutEarlyGraceTime { get; set; }
        public short? InLateGraceTime { get; set; }
        public short? OutLateGraceTime { get; set; }
        public byte? AllPunchesClockRoundingTypeId { get; set; }
        public short? AllPunchesGraceTime { get; set; }
        public bool IsSunday { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }
        public bool IsUseStartStopTimes { get; set; }
        public bool IsAllowMultipleTimePeriods { get; set; }
        public int MinutesToRestrictLunchPunch { get; set; }

        // relationships

        public IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto> TimePolicies { get; set; }
    }
}