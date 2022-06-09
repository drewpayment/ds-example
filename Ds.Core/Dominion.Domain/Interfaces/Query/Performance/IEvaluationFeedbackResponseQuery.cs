using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IEvaluationFeedbackResponseQuery : IQuery<EvaluationFeedbackResponse, IEvaluationFeedbackResponseQuery>
    {
        IEvaluationFeedbackResponseQuery ByEvaluationFeedbackReponse(int responseId, int evaluationId);
        IEvaluationFeedbackResponseQuery ByEvaluationId(int evaluationId);
    }
}
