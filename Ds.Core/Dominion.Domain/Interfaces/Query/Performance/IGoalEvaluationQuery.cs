using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IGoalEvaluationQuery : IQuery<GoalEvaluation, IGoalEvaluationQuery>
    {
        IGoalEvaluationQuery ByGoalEvaluationId(int goalId, int evaluationId);
        IGoalEvaluationQuery ByEvaluationId(int evaluationId);
    }
}
