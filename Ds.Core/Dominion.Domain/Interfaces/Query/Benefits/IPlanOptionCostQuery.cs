using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    /// <summary>
    /// Query on <see cref="PlanOptionCost"/> data.
    /// </summary>
    public interface IPlanOptionCostQuery : IQuery<PlanOptionCost, IPlanOptionCostQuery>
    {
        /// <summary>
        /// Filters by a specific benefit plan.
        /// </summary>
        /// <param name="planId">ID of plan to filter by.</param>
        /// <returns></returns>
        IPlanOptionCostQuery ByPlanId(int planId);

        /// <summary>
        /// Filters by a specific plan option.
        /// </summary>
        /// <param name="planOptionId">ID of plan option to filter by.</param>
        /// <returns></returns>
        IPlanOptionCostQuery ByPlanOptionId(int planOptionId);

        /// <summary>
        /// Filters by one or more specified costs.
        /// </summary>
        /// <param name="ids">ID(s) of costs to get.</param>
        /// <returns></returns>
        IPlanOptionCostQuery ByPlanOptionCostIds(params int[] ids);

        /// <summary>
        /// Filters by costs belonging to the plan option's base pay frequency.
        /// </summary>
        /// <returns></returns>
        IPlanOptionCostQuery ByPlanOptionPayFrequency();

        /// <summary>
        /// Filters costs belonging to a specific pay type.
        /// </summary>
        /// <param name="freq">Pay frequency to get costs for.</param>
        /// <returns></returns>
        IPlanOptionCostQuery ByPayFrequency(PayFrequencyType freq);
    }
}
