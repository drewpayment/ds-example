using System;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Interface defining common open enrollment plan properties.
    /// </summary>
    public interface IOpenEnrollmentPlanEntity
    {
        int       OpenEnrollmentId { get; set; }
        int       PlanId           { get; set; }
        DateTime? Modified         { get; set; }
        int?      ModifiedBy       { get; set; }
    }
}