using System.Collections.Generic;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity representation of a benefit plan category (e.g. Medical, Dental, etc). Maps to [dbo].[bpPlanCategory].
    /// </summary>
    public class PlanCategory
    {
        public virtual int    PlanCategoryId              { get; set; }
        public virtual string Name                        { get; set; }
        public virtual byte?  Sequence                    { get; set; }
        public virtual bool   IsArchived                  { get; set; }

        public virtual ICollection<Plan> Plans { get; set; }
    }
}