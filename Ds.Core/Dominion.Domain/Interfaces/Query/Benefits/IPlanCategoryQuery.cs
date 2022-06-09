using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    /// <summary>
    /// Query on <see cref="PlanCategory"/> data.
    /// </summary>
    public interface IPlanCategoryQuery : IQuery<PlanCategory, IPlanCategoryQuery>
    {
        /// <summary>
        /// Filter by a single plan category.
        /// </summary>
        /// <param name="id">ID of category to get.</param>
        /// <returns></returns>
        IPlanCategoryQuery ByPlanCategoryId(int id);
    }
}
