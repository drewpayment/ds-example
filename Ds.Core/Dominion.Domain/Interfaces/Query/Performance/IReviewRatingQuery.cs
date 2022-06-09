using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IReviewRatingQuery : IQuery<ReviewRating, IReviewRatingQuery>
    {
        /// <summary>
        /// Filters a list of rating entities by the client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IReviewRatingQuery ByClient(int clientId);

        /// <summary>
        /// Filters a list of rating entities by the primary key.
        /// </summary>
        /// <param name="reviewRatingId"></param>
        /// <returns></returns>
        IReviewRatingQuery ByReviewRating(int reviewRatingId);
    }
}
