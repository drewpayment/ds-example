using System;

namespace Dominion.Domain.Entities.TimeClock
{
    public partial class ClockClientScheduleChangeHistory
    {
        public virtual int ChangeId { get; set; }
        public virtual int? ClockClientScheduleId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime? StartTime { get; set; }
        public virtual DateTime? StopTime { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual byte? RepeatInterval { get; set; }
        public virtual bool? IsSunday { get; set; }
        public virtual bool? IsMonday { get; set; }
        public virtual bool? IsTuesday { get; set; }
        public virtual bool? IsWednesday { get; set; }
        public virtual bool? IsThursday { get; set; }
        public virtual bool? IsFriday { get; set; }
        public virtual bool? IsSaturday { get; set; }
        public virtual int? ClientShiftId { get; set; }
        public virtual int? ClientDepartmentId { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int? ChangeMode { get; set; }
        public virtual bool IsIncludeOnHolidays { get; set; }
        public virtual byte? RecurrenceOption { get; set; }
        public virtual byte RecurEveryOption { get; set; }
        public virtual byte? ClientStatusId { get; set; }
        public virtual byte? PayType { get; set; }
        public virtual bool IsOverrideSchedules { get; set; }

        //Reference Entities 
        public virtual ClockClientSchedule Schedule { get; set; }

        public ClockClientScheduleChangeHistory()
        {
        }
    }
}