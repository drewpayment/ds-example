using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.TimeClock
{
    public partial class ClockClientLunch : Entity<ClockClientLunch>, IHasModifiedData
    {
        public virtual int ClockClientLunchId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Name { get; set; }
        public virtual double? Length { get; set; }
        public virtual bool? IsPaid { get; set; }
        public virtual bool? IsDoEmployeesPunch { get; set; }
        public virtual bool? IsAutoDeducted { get; set; }
        public virtual double? AutoDeductedWorkedHours { get; set; }
        public virtual short? GraceTime { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual DateTime? StartTime { get; set; }
        public virtual DateTime? StopTime { get; set; }
        public virtual int? ClientCostCenterId { get; set; }
        public virtual byte PunchType { get; set; }
        public virtual bool IsShowPunches { get; set; }
        public virtual bool IsMaxPaid { get; set; }
        public virtual byte? InEarlyClockRoundingTypeId { get; set; }
        public virtual byte? OutEarlyClockRoundingTypeId { get; set; }
        public virtual byte? InLateClockRoundingTypeId { get; set; }
        public virtual byte? OutLateClockRoundingTypeId { get; set; }
        public virtual short? InEarlyGraceTime { get; set; }
        public virtual short? OutEarlyGraceTime { get; set; }
        public virtual short? InLateGraceTime { get; set; }
        public virtual short? OutLateGraceTime { get; set; }
        public virtual byte? AllPunchesClockRoundingTypeId { get; set; }
        public virtual short? AllPunchesGraceTime { get; set; }
        public virtual bool IsSunday { get; set; }
        public virtual bool IsMonday { get; set; }
        public virtual bool IsTuesday { get; set; }
        public virtual bool IsWednesday { get; set; }
        public virtual bool IsThursday { get; set; }
        public virtual bool IsFriday { get; set; }
        public virtual bool IsSaturday { get; set; }
        public virtual bool IsUseStartStopTimes { get; set; }
        public virtual bool IsAllowMultipleTimePeriods { get; set; }
        public virtual int MinutesToRestrictLunchPunch { get; set; }

        //Entity References
        public virtual ICollection<ClockClientTimePolicy> TimePolicies { get; set; } 

        public virtual ClientCostCenter ClientCostCenter { get; set; }

        public virtual ICollection<ClockClientLunchSelected> LunchSelected { get; set; }

        public ClockClientLunch()
        {
        }
    }
}