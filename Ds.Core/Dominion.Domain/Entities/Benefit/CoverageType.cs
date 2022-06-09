using System.Collections.Generic;

using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity for dbo.bpCoverageType table.
    /// </summary>
    public partial class CoverageType : Entity<CoverageType>
    {
        public virtual int    CoverageTypeId               { get; set; }
        public virtual string Description                  { get; set; }
        public virtual bool   IsEmployeeGrouping           { get; set; }
        public virtual byte?  MaxEmployeeSelectionsAllowed { get; set; }
        public virtual bool   IsWaiverReasonRequired       { get; set; }
        public virtual byte?  EmployeeSelectionSequence    { get; set; }
        public virtual string CssClass                     { get; set; }

        public virtual ICollection<PlanType> LinkedPlanTypes { get; set; }
    }
}
