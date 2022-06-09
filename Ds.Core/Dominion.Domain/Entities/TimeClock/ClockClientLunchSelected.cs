using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Labor;

namespace Dominion.Domain.Entities.TimeClock
{
    public class ClockClientLunchSelected : Entity<ClockClientLunchSelected>
    {
        public int ClockClientTimePolicyId { get; set; }
        public int ClockClientLunchId { get; set; }


        // ENTITY RELATIONSHIPS
        public virtual ClockClientTimePolicy TimePolicy { get; set; }
        public virtual ClockClientLunch Lunch { get; set; }
    }
}
