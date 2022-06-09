using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Labor
{
    public class ClockClientOvertimeDto : IHasClockClientOvertimeValidation
    {
        public int ClockClientOvertimeId { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }
        public int ClientEarningId { get; set; }
        public byte? ClockOvertimeFrequencyId { get; set; }
        public double? Hours { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
        public bool IsSunday { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }
    }
}
