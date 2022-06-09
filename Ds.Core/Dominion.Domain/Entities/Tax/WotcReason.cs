using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Tax
{
    public class WotcReason : Entity<WotcReason>
    {
        public int WotcReasonId { get; set; }
        public string Description { get; set; }
    }
}
