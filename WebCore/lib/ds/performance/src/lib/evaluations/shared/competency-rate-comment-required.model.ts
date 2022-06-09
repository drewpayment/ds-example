import { IReviewRating } from '@ds/performance/ratings/shared/review-rating.model';

export interface ICompetencyRateCommentRequired {
    optionId: number,
    reviewRatingId: number,
    reviewRating: IReviewRating
}