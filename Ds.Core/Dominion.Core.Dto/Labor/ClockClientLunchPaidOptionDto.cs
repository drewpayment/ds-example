using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public partial class ClockClientLunchPaidOptionDto
    {
        public int ClockClientLunchPaidOptionId { get; set; }
        public int ClientId { get; set; }
        public int? ClockClientLunchId { get; set; }
        public double? FromMinutes { get; set; }
        public double? ToMinutes { get; set; }
        public byte? ClockClientLunchPaidOptionRulesId { get; set; }
        public double? OverrideMinutes { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
