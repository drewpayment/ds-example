using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class ClockClientScheduleDto
    {
        public int ClockClientScheduleId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public byte? RepeatInterval { get; set; }
        public bool? IsSunday { get; set; }
        public bool? IsMonday { get; set; }
        public bool? IsTuesday { get; set; }
        public bool? IsWednesday { get; set; }
        public bool? IsThursday { get; set; }
        public bool? IsFriday { get; set; }
        public bool? IsSaturday { get; set; }
        public int? ClientShiftId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public bool IsIncludeOnHolidays { get; set; }
        public byte RecurrenceOption { get; set; }
        public byte RecurEveryOption { get; set; }
        public byte? ClientStatusId { get; set; }
        public byte? PayType { get; set; }
        public bool IsOverrideSchedules { get; set; }
        public bool IsActive { get; set; }
    }
}
