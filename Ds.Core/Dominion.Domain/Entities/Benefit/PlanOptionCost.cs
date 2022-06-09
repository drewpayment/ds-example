using System;
using System.Collections.Generic;

using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity representation of a single cost option of a benefit plan.  Maps to [dbo].[bpPlanOptionCost] table.
    /// </summary>
    public class PlanOptionCost : Entity<PlanOptionCost>, IHasModifiedData
    {
        public virtual int               PlanOptionCostId { get; set; }
        public virtual int               PlanOptionId     { get; set; }
        public virtual PayFrequencyType  PayFrequencyId   { get; set; }
        public virtual decimal?          Cost             { get; set; }
        public virtual decimal?          MinimumLimit     { get; set; }
        public virtual decimal?          MaximumLimit     { get; set; }
        public virtual DateTime          Modified         { get; set; }
        public virtual int               ModifiedBy       { get; set; }

        public virtual PlanOption   PlanOption   { get; set; }
        public virtual PayFrequency PayFrequency { get; set; }

        public virtual ICollection<CostFactorOption> CostFactorOptions { get; set; }

        public void SetValues(PlanOptionCost item)
        {
            this.PlanOptionCostId = item.PlanOptionCostId;
            this.PlanOptionId = item.PlanOptionId;
            this.PayFrequencyId = item.PayFrequencyId;
            this.Cost = item.Cost;
            this.MinimumLimit = item.MinimumLimit;
            this.MaximumLimit = item.MaximumLimit;
            this.Modified = item.Modified;
            this.ModifiedBy = item.ModifiedBy;
        }
    }
}