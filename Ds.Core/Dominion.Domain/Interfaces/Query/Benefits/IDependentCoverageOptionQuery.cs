using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    /// <summary>
    /// Query on <see cref="DependentCoverageOptionTypeInfo"/> data.
    /// </summary>
    public interface IDependentCoverageOptionQuery : IQuery<DependentCoverageOptionTypeInfo, IDependentCoverageOptionQuery>
    {
    }
}
