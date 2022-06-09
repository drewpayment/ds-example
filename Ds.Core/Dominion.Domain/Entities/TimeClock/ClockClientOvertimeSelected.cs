using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Labor;

namespace Dominion.Domain.Entities.TimeClock
{
    public class ClockClientOvertimeSelected : Entity<ClockClientOvertimeSelected>
    {
        public int ClockClientOvertimeSelectedId { get; set; }
        public int ClockClientTimePolicyId { get; set; }
        public int ClockClientOvertimeId { get; set; }

        public ClockClientTimePolicy ClockClientTimePolicy { get; set; }

        public ClockClientOvertime ClockClientOvertime { get; set; }
    }
}
