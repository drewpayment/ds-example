using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IEvaluationGroupQuery : IQuery<EvaluationGroup, IEvaluationGroupQuery>
    {
        IEvaluationGroupQuery ByClientId(int clientId);
    }
}
