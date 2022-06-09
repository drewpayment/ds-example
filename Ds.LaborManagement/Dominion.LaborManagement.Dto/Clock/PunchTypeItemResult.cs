using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.Clock
{
    public class PunchTypeItemResult
    {
        public PunchTypeItemSource Source                  { get; set; }
        public int                 ClientId                { get; set; }
        public int                 ClockClientTimePolicyId { get; set; }
        public int                 ClockClientRulesId      { get; set; }

        public IEnumerable<PunchTypeItem> Items { get; set; }
    }
}