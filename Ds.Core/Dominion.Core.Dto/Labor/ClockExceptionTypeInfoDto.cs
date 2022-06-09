namespace Dominion.Core.Dto.Labor
{
    public class ClockExceptionTypeInfoDto
    {
        public ClockExceptionType ClockExceptionId { get; set; }
        public string ClockException { get; set; }
        public bool? IsHours { get; set; }
        public bool HasPunchTimeOption { get; set; }
        public bool HasAmountTextBox { get; set; }
        public ClockExceptionGroupType ClockExceptionType { get; set; }
    }
}
