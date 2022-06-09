using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.TimeClock;

namespace Dominion.Core.Dto.Labor
{
    public class ClockClientLunchViewDto
    {
        public List<ClientCostCenterDto> CostCenters { get; set; }
        public List<ClockClientLunchDto> LunchRules { get; set; }
        public List<ClockRoundingTypeDto> ClockRoundingTypeList { get; set; }
        public List<ClockClientLunchPaidOptionRulesDto> PaidOptionRulesTypeList { get; set; } 
    }
}
