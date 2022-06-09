using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity representation of a reason for an employee to waive benefits. Maps to [dbo].[bpPlanWaiveReasons].
    /// </summary>
    public class PlanWaiveReason : Entity<PlanWaiveReason>
    {
        public virtual int    PlanWaiveReasonId { get; set; }
        public virtual string Description       { get; set; }
        public virtual int    Sequence          { get; set; }
        public virtual bool   IsActive          { get; set; }
    }
}
