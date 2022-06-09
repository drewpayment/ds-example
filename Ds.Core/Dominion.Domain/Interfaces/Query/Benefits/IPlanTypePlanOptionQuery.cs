using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    public interface IPlanTypePlanOptionQuery : IQuery<PlanTypePlanOption, IPlanTypePlanOptionQuery>
    {
        /// <summary>
        /// Filters PlanTypePlanOptions by plan type ID.
        /// </summary>
        /// <param name="planTypeId">ID of plan type to filter by.</param>
        /// <returns></returns>
        IPlanTypePlanOptionQuery ByPlanType(int planTypeId);

        /// <summary>
        /// Filters PlanTypePlanOptions by plan option ID.
        /// </summary>
        /// <param name="planOptionId">ID of plan option to filter by.</param>
        /// <returns></returns>
        IPlanTypePlanOptionQuery ByPlanOption(int planOptionId);
    }
}
