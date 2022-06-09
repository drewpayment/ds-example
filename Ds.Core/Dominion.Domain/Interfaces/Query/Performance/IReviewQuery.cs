using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Performance;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IReviewQuery : IQuery<Review, IReviewQuery>
    {
        /// <summary>
        /// Filter by a specific review.
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        IReviewQuery ByReviewId(int reviewId);

        /// <summary>
        /// Filter by multiple reviews.
        /// </summary>
        /// <param name="reviewIds"></param>
        /// <returns></returns>
        IReviewQuery ByReviewIds(ICollection<int> reviewIds);

        /// <summary>
        /// Filter by reviews for a specific employee.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        IReviewQuery ByReviewedEmployeeId(int employeeId);

        /// <summary>
        /// Filter by reviews for a specific client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IReviewQuery ByClientId(int clientId);

        /// <summary>
        /// Filter reviews that are associated with a particular review within a review-cycle.
        /// </summary>
        /// <param name="reviewTemplateId"></param>
        /// <returns></returns>
        IReviewQuery ByReviewTemplate(int reviewTemplateId);
        IReviewQuery ByDateRange(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Filter reviews associated with a particular review profile.
        /// </summary>
        /// <param name="reviewProfileId"></param>
        /// <returns></returns>
        IReviewQuery ByReviewProfile(int reviewProfileId);

        IReviewQuery ByHasReviewTemplate(int reviewTemplateId);
        IReviewQuery ByReferenceDate(ReferenceDate referenceDate);
        /// <summary>
        /// Filter reviews associated with a particular owner.
        /// </summary>
        /// <param name="reviewOwnerId"></param>
        /// <returns></returns>
        IReviewQuery ByReviewOwnerId(int reviewOwnerId);
        /// <summary>
        /// Filter reviews by evaluation Id.
        /// </summary>
        /// <param name="evaluationId"></param>
        /// <returns></returns>
        IReviewQuery ByEvaluationId(int evaluationId);
        /// <summary>
        /// Filter reviews associated with a evaluator userId.
        /// </summary>
        /// <param name="evaluatorId"></param>
        /// <returns></returns>
        IReviewQuery ByEvaluatorId(int evaluatorId);
        /// <summary>
        /// Filter reviews by assigned but not evaluator userId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IReviewQuery ByAssignedUserId(int userId);
        /// <summary>
        /// Filter reviews associated with a evaluator role type.
        /// </summary>
        /// <param name="roleType"></param>
        /// <returns></returns>
        IReviewQuery ByEvaluationType(EvaluationRoleType roleType);
    }
}
