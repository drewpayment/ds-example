import { IReviewRating } from '@ds/performance/ratings/shared/review-rating.model';

export interface IGoalRateCommentRequired {
    optionId: number,
    reviewRatingId: number,
    reviewRating: IReviewRating
}
