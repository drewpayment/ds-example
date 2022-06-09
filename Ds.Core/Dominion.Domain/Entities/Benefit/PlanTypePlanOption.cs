using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    public class PlanTypePlanOption : Entity<PlanTypePlanOption>
    {
        public virtual int PlanTypeId   { get; set; }
        public virtual int PlanOptionId { get; set; }


        public virtual PlanType     PlanType    { get; set; }

        public virtual PlanOption   PlanOption  { get; set; }
    }
}

