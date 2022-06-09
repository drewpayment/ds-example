using Dominion.Benefits.Dto.Plans;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Type/enum info for <see cref="PlanEligibilityType"/>
    /// </summary>
    public class PlanEligibilityTypeInfo : Entity<PlanEligibilityTypeInfo>
    {
        public PlanEligibilityType PlanEligibilityTypeId { get; set; }
        public string Name { get; set; }
    }
}
