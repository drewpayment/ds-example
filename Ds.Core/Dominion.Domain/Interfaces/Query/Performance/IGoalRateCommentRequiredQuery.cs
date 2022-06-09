using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IGoalRateCommentRequiredQuery : IQuery<GoalRateCommentRequired, IGoalRateCommentRequiredQuery>
    {
        IGoalRateCommentRequiredQuery ByOptionId(int optionId);
        IGoalRateCommentRequiredQuery ByReviewRatingId(int reviewRatingId);
    }
}
