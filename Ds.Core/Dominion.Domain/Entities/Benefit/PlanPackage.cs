using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity representation of the dbo.bpPlanPackage table.
    /// </summary>
    public partial class PlanPackage : Entity<PlanPackage>, IHasModifiedData
    {
        public int       PlanId           { get; set; }
        public int       BenefitPackageId { get; set; }
        public DateTime  Modified         { get; set; }
        public int       ModifiedBy       { get; set; }

        public Plan           Plan           { get; set; }
        public BenefitPackage BenefitPackage { get; set; }
    }
}