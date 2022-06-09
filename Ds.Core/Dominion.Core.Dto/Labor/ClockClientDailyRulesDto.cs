using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public partial class ClockClientDailyRulesDto
    {
        public int ClockClientDailyRulesId { get; set; }
        public int ClockClientRulesId { get; set; }
        public byte DayOfWeekId { get; set; }
        public int? ClientEarningId { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public double? MinHoursWorked { get; set; }
        public bool IsApplyOnlyIfMinHoursMetPrior { get; set; }

        public ClockClientRulesDto ClockClientRule { get; set; }
    }
}
