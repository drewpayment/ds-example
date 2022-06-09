using System.Collections.Generic;

namespace Dominion.Core.Dto.Labor
{
    public class ClockClientHolidayDto : IHasClockClientHolidayValidation
    {
        public int ClockClientHolidayId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public int? ClientEarningId { get; set; }
        public double? Hours { get; set; }
        public int? HolidayWorkedClientEarningId { get; set; }
        public int WaitingPeriod { get; set; }
        public int HolidayWaitingPeriodDateId { get; set; }
        public List<int> RuleYears { get; set; } // only passed to front end, not a table column

        public IEnumerable<ClockClientHolidayDetailDto> ClientHolidayDetails { get; set; }

    }
}