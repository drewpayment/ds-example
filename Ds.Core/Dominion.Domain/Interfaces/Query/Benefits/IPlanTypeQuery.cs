using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    public interface IPlanTypeQuery : IQuery<PlanType, IPlanTypeQuery>
    {
        /// <summary>
        /// Filters plan types by plan type ID.
        /// </summary>
        /// <param name="planTypeId">ID of plan to filter by.</param>
        /// <returns></returns>
        IPlanTypeQuery ByPlanType(int planTypeId);

        /// <summary>
        /// Filters plan types by category ID.
        /// </summary>
        /// <param name="categoryId">ID of category to filter by.</param>
        /// <returns></returns>
        IPlanTypeQuery ByCategory(int categoryId);
    }
}
