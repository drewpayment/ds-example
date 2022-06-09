import { IContactWithClient } from "@ds/core/contacts";
import { UserInfo } from "@ds/core/shared";
import { IReview } from '@ds/performance/reviews';
import { IReviewTemplate } from '..';

export interface IReviewTemplateEditDialogData {
    employee: IContactWithClient;
    review?: IReview;
    currentUser: UserInfo;
}

export interface IReviewTemplateEditDialogResult {
    review: IReviewTemplate;
}
