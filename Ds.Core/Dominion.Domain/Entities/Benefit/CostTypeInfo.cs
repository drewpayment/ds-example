using Dominion.Benefits.Dto.Plans;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity representation of a dbo.bpCostType record.
    /// </summary>
    public class CostTypeInfo : Entity<CostTypeInfo>
    {
        public virtual CostType CostTypeId  { get; set; }
        public virtual string   Description { get; set; }
    }
}