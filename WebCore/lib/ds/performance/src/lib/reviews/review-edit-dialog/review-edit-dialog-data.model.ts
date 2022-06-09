import { IReview } from "../shared/review.model";
import { UserInfo } from "@ds/core/shared";
import { IContactWithClient } from '@ds/core/contacts';
import { IReviewProfileBasic } from '@ds/performance/review-profiles';

export interface IReviewEditDialogData {
    employee: IContactWithClient;
    review?: IReview;
    reviewProfile?: IReviewProfileBasic;
    currentUser: UserInfo;
}
