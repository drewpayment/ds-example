import { IReviewProfileEvaluation } from "./review-profile-evaluation.model";
import { IReviewProfileBasicSetup } from './review-profile-basic-setup.model';

export interface IReviewProfileSetup extends IReviewProfileBasicSetup {
    evaluations?: IReviewProfileEvaluation[];
}