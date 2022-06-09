using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    public interface IPlanOptionCostFactorQuery : IQuery<PlanOptionCostFactorConfiguration, IPlanOptionCostFactorQuery>
    {
        /// <summary>
        /// Filters cost factor configurations by a specific plan option.
        /// </summary>
        /// <param name="planOptionId">Plan option to filter by.</param>
        /// <returns></returns>
        IPlanOptionCostFactorQuery ByPlanOption(int planOptionId);
    }
}
