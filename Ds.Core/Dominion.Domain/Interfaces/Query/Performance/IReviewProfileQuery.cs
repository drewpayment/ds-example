using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IReviewProfileQuery : IQuery<ReviewProfile, IReviewProfileQuery>
    {
        /// <summary>
        /// Filter profiles belonging to a specific client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IReviewProfileQuery ByClientId(int clientId);

        /// <summary>
        /// Filters profiles by the provided archive status.
        /// </summary>
        /// <param name="isArchived"></param>
        /// <returns></returns>
        IReviewProfileQuery ByIsArchived(bool isArchived);

        /// <summary>
        /// Filters by a specific profile.
        /// </summary>
        /// <param name="reviewProfileId"></param>
        /// <returns></returns>
        IReviewProfileQuery ByReviewProfileId(int reviewProfileId);
        IReviewProfileQuery ByReviewProfileIds(ICollection<int> reviewProfileIds);
    }
}
