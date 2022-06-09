using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity for dbo.bpPlanOptionPlan table.
    /// </summary>
    public class PlanOptionPlan : Entity<PlanOptionPlan>
    {
        public virtual int PlanId       { get; set; }
        public virtual int PlanOptionId { get; set; }

        public virtual Plan       Plan       { get; set; }
        public virtual PlanOption PlanOption { get; set; }
    }
}
