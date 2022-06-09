using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;

namespace Dominion.Domain.Entities.Labor
{
    public partial class ClockClientLunchPaidOptionRules
    {
        public ClockClientLunchPaidOptionRulesType ClockClientLunchPaidOptionRulesId { get; set; }
        public string Description { get; set; }
    }
}
