using System;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity representation of a dbo.bpOpenEnrollmentPlans record.
    /// </summary>
    public class OpenEnrollmentPlan : Entity<OpenEnrollmentPlan>, IHasModifiedOptionalData, IHasChangeHistoryEntity<OpenEnrollmentPlanChangeHistory>, IOpenEnrollmentPlanEntity
    {
        public virtual int       OpenEnrollmentId { get; set; }
        public virtual int       PlanId           { get; set; }
        public virtual DateTime? Modified         { get; set; }
        public virtual int?      ModifiedBy       { get; set; }

        public virtual OpenEnrollment OpenEnrollment { get; set; }
        public virtual Plan           Plan           { get; set; }
    }
}
