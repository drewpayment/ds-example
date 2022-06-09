using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    public interface IPlanOptionPlanQuery : IQuery<PlanOptionPlan, IPlanOptionPlanQuery>
    {
        /// <summary>
        /// Filters PlanOptionPlans by plan ID.
        /// </summary>
        /// <param name="planId">ID of plan to filter by.</param>
        /// <returns></returns>
        IPlanOptionPlanQuery ByPlan(int planId);

        /// <summary>
        /// Filters PlanOptionPlans by plan option ID.
        /// </summary>
        /// <param name="planOptionId">ID of plan option to filter by.</param>
        /// <returns></returns>
        IPlanOptionPlanQuery ByPlanOption(int planOptionId);
    }
}
