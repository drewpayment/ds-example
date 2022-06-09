using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IEvaluationQuery : IQuery<Evaluation, IEvaluationQuery>
    {
        IEvaluationQuery ByEvaluationId(int evaluationId);
        IEvaluationQuery ByHasNotificationsToSend();
        IEvaluationQuery ByHasNotificationsToSendTodayOrShouldHaveBeenSentInThePast();
        IEvaluationQuery ByEvaluatingUserIsActive();
        IEvaluationQuery ByIncompleteReview();
        IEvaluationQuery ByIncompleteEvaluations();
        IEvaluationQuery ByReviewId(int reviewId);
        IEvaluationQuery ByReviewIds(ICollection<int> reviews);
        IEvaluationQuery ByRole(EvaluationRoleType evaluationRoleType);
        IEvaluationQuery ByShouldBeSynced();
    }
}
