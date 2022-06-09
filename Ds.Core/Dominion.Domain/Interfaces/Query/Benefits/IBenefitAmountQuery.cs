using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    public interface IBenefitAmountQuery : IQuery<BenefitAmount, IBenefitAmountQuery>
    {
        /// <summary>
        /// Filters benefit amounts by plan option ID.
        /// </summary>
        /// <param name="planOptionId">ID of plan option to filter by.</param>
        /// <returns></returns>
        IBenefitAmountQuery ByPlanOption(int planOptionId);
    }
}
