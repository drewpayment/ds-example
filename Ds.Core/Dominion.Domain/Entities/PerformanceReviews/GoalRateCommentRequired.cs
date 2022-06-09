using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    /// <summary>
    /// Goals with the associated <see cref="PerformanceReviews.ReviewRating"/> must have a
    /// comment.  This is enforced when the evaluator is filling out an evaluation.
    /// </summary>
    public class GoalRateCommentRequired : Entity<GoalRateCommentRequired>
    {
        public int OptionId { get; set; }
        public int ReviewRatingId { get; set; }

        public GoalOptions GoalOptions { get; set; }
        public ReviewRating ReviewRating { get; set; }
    }
}
