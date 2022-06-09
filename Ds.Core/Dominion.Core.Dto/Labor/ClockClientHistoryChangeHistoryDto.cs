using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class ClockClientHolidayChangeHistoryDto
    {
        public int ChangeId { get; set; }
        public int ClockClientHolidayId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public int? ClientEarningId { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public double? Hours { get; set; }
        public int? HolidayWorkedClientEarningId { get; set; }
        public int WaitingPeriod { get; set; }
        public int HolidayWaitingPeriodDateId { get; set; }
        public string ChangeMode { get; set; }
    }
}
