import { IReviewProfileBasic } from './review-profile-basic.model';

export interface IReviewProfileBasicSetup extends IReviewProfileBasic {
    defaultInstructions?: string;
    includeReviewMeeting?: boolean;
    includeScoring?: boolean;
    includePayrollRequests?: boolean;
}