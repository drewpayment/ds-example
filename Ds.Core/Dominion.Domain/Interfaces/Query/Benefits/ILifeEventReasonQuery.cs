using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    /// <summary>
    /// Query on <see cref="LifeEventReasonInfo"/> data.
    /// </summary>
    public interface ILifeEventReasonQuery : IQuery<LifeEventReasonInfo, ILifeEventReasonQuery>
    {
    }
}
