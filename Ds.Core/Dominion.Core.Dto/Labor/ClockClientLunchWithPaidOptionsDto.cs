using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class ClockClientLunchWithPaidOptionsDto
    {
        public ClockClientLunchDto LunchDto { get; set; }
        public List<ClockClientLunchPaidOptionDto> PaidOptionDtos { get; set; }
    }
}
