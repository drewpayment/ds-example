import { IReviewMeeting } from "../shared/review-meeting.model";
import { IReview } from "../shared/review.model";

export interface IReviewMeetingDialogResult {
    review?: IReview;
    reviewMeeting: IReviewMeeting
}