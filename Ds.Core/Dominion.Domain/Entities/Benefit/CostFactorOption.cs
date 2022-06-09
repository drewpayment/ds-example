using System.Collections.Generic;

using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    public partial class CostFactorOption : Entity<CostFactorOption>
    {
        public virtual int    CostFactorOptionId { get; set; } 
        public virtual int    CostFactorId       { get; set; } 
        public virtual int?   PlanOptionId       { get; set; } 
        public virtual string Description        { get; set; } 
        public virtual bool?  IsYesNoValue       { get; set; } 
        public virtual int?   MinValue           { get; set; } 
        public virtual int?   MaxValue           { get; set; } 
        public virtual int?   SystemValue        { get; set; }
        
        public virtual ICollection<PlanOptionCost> LinkedCosts { get; set; } 
        public virtual PlanOption                  PlanOption  { get; set; }
        public virtual CostFactor                  CostFactor  { get; set; } 
    }
}