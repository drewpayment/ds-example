namespace Dominion.Core.Dto.Labor
{
    public interface IHasClockClientExceptionDetailValidation
    {
        int ClockClientExceptionId { get; set; }
        ClockExceptionType ClockExceptionId { get; set; }
        double? Amount { get; set; }
        bool? IsSelected { get; set; }
        int? ClockClientLunchId { get; set; }
        PunchType PunchTimeOption { get; set; }
    }
}
