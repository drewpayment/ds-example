using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    public partial class PlanOptionCostFactorConfiguration : Entity<PlanOptionCostFactorConfiguration>
    {
        public virtual int  PlanOptionId { get; set; } 
        public virtual int  CostFactorId { get; set; } 
        public virtual bool IsActive     { get; set; }

        //FOREIGN KEYS
        public virtual CostFactor CostFactor { get; set; } 
        public virtual PlanOption PlanOption { get; set; }
    }
}