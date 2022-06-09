using System;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity representation of a dbo.bpOpenEnrollmentPlans_ChangeHistory record.
    /// </summary>
    public class OpenEnrollmentPlanChangeHistory : Entity<OpenEnrollmentPlanChangeHistory>, IHasModifiedOptionalData, IHasChangeHistoryData, IOpenEnrollmentPlanEntity
    {
        public virtual int       ChangeId         { get; set; }
        public virtual int       OpenEnrollmentId { get; set; }
        public virtual int       PlanId           { get; set; }
        public virtual DateTime? Modified         { get; set; }
        public virtual int?      ModifiedBy       { get; set; }
        public virtual string    ChangeMode       { get; set; }
    }
}