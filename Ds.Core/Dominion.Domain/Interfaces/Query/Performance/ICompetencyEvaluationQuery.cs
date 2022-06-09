using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface ICompetencyEvaluationQuery : IQuery<CompetencyEvaluation, ICompetencyEvaluationQuery>
    {
        ICompetencyEvaluationQuery ByCompetencyEvaluationId(int competencyId, int EvaluationId);
        ICompetencyEvaluationQuery ByEvaluationId(int evaluationId);
    }
}
