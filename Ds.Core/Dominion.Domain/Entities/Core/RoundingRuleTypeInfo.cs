using Dominion.Domain.Entities.Base;
using Dominion.Utility.MathExt;

namespace Dominion.Domain.Entities.Core
{
    /// <summary>
    /// Entity representation of the core.RoundingRuleType record.
    /// </summary>
    public class RoundingRuleTypeInfo : Entity<RoundingRuleTypeInfo>
    {
        public virtual RoundingRule RoundingRuleTypeId { get; set; }
        public virtual string       Name               { get; set; }
        public virtual string       Description        { get; set; }
    }
}
