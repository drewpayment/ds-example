using Dominion.Benefits.Dto.Plans;
using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    /// <summary>
    /// Queries benefit plan cost factor information.
    /// </summary>
    public interface ICostFactorQuery : IQuery<CostFactor, ICostFactorQuery>
    {
        /// <summary>
        /// Filters cost factors by one or more system-level types.
        /// </summary>
        /// <param name="systemFactors">System factor types to filter by.</param>
        /// <returns></returns>
        ICostFactorQuery BySystemType(params SystemCostFactorType[] systemFactors);
    }
}
