using System;

namespace Dominion.Core.Dto.Labor
{
    public interface IHasClockClientOvertimeValidation
    {
        int ClockClientOvertimeId { get; set; }
        string Name { get; set; }
        int ClientId { get; set; }
        int ClientEarningId { get; set; }
        byte? ClockOvertimeFrequencyId { get; set; }
        double? Hours { get; set; }
        int? ModifiedBy { get; set; }
        DateTime? Modified { get; set; }
        bool IsSunday { get; set; }
        bool IsMonday { get; set; }
        bool IsTuesday { get; set; }
        bool IsWednesday { get; set; }
        bool IsThursday { get; set; }
        bool IsFriday { get; set; }
        bool IsSaturday { get; set; }
    }
}
