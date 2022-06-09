using Dominion.Domain.Entities.Benefit;
using Dominion.Domain.Interfaces.Query.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    /// <summary>
    /// Query on <see cref="BenefitResource"/> data.
    /// </summary>
    public interface IBenefitResourceQuery : IQuery<BenefitResource, IBenefitResourceQuery>
    {
        /// <summary>
        /// Filters by resources belonging to a particular client.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IBenefitResourceQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by resources belonging to a particular benefit plan.
        /// </summary>
        /// <param name="planId">Benefit plan to filter by.</param>
        /// <returns></returns>
        IBenefitResourceQuery ByPlanId(int planId);

        /// <summary>
        /// Filters by a single resource.
        /// </summary>
        /// <param name="resourceId">ID of resource to get.</param>
        /// <returns></returns>
        IBenefitResourceQuery ByResourceId(int resourceId);
    }
}
