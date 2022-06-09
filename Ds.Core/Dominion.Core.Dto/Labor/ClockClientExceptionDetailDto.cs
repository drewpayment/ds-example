using System;

namespace Dominion.Core.Dto.Labor
{
    public class ClockClientExceptionDetailDto : IHasClockClientExceptionDetailValidation
    {
        public int ClockClientExceptionDetailId { get; set; }
        public int ClockClientExceptionId { get; set; }
        public ClockExceptionType ClockExceptionId { get; set; }
        public double? Amount { get; set; }
        public bool? IsSelected { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public int? ClockClientLunchId { get; set; }
        public PunchType PunchTimeOption { get; set; }

        public ClockExceptionTypeInfoDto ClockException { get; set; }
        public ClockClientLunchDto ClockClientLunch { get; set; }
        public ClockClientExceptionDto ClockClientException { get; set; }
    }
}
