import { IMeeting } from '@ds/core/meetings';

export interface IReviewMeeting extends IMeeting {
    reviewId: number;
    completedDate?: Date | string;
}