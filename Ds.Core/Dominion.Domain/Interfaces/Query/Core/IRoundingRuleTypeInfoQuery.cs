using Dominion.Domain.Entities.Core;
using Dominion.Utility.MathExt;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Core
{
    /// <summary>
    /// Query on <see cref="RoundingRuleTypeInfo"/>.
    /// </summary>
    public interface IRoundingRuleTypeInfoQuery : IQuery<RoundingRuleTypeInfo, IRoundingRuleTypeInfoQuery>
    {
        /// <summary>
        /// Filters the rules by one or more particular results.
        /// </summary>
        /// <param name="rules">One or more rules to filter by.</param>
        /// <returns></returns>
        IRoundingRuleTypeInfoQuery ByRules(params RoundingRule[] rules);
    }
}
