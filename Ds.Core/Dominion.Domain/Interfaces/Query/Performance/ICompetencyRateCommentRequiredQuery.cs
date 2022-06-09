using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface ICompetencyRateCommentRequiredQuery : IQuery<CompetencyRateCommentRequired, ICompetencyRateCommentRequiredQuery>
    {
        ICompetencyRateCommentRequiredQuery ByOptionId(int optionId);
        ICompetencyRateCommentRequiredQuery ByReviewRatingId(int reviewRatingId);
    }
}
