namespace Dominion.Core.Dto.Labor
{
    public class ClientExceptionDetailDto
    {
        public string ExceptionName { get; set; }
        public ClockExceptionType ClockExceptionId { get; set; }
        public int? ClockClientLunchId { get; set; }
        public int? ClockClientExceptionDetailId { get; set; }
        public int? ClockClientExceptionId { get; set; }
        public double? Amount { get; set; }
        public bool IsSelected { get; set; }
        public bool IsHour { get; set; }
        public PunchType PunchTimeOption { get; set; }
        public bool HasPunchTimeOption { get; set; }
        public bool HasAmountTextBox { get; set; }
        public ClockExceptionGroupType ClockExceptionType { get; set; }
    }
}
