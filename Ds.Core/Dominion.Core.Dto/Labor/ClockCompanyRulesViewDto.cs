using System.Collections.Generic;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.TimeClock;

namespace Dominion.Core.Dto.Labor
{
    public class ClockCompanyRulesViewDto
    {
        public List<ClockClientRulesDto> ClockClientRules { get; set; }
        public List<ClockRoundingTypeDto> RoundingTypes { get; set; }
        public List<DayOfWeekDto> DaysOfWeek { get; set; }

        public List<ClientAccountFeatureDto> ClientFeatures { get; set; }
        
    }
}
