import { IEvaluation } from "./evaluation.model";
import { IContact } from "@ds/core/contacts";
import { ICompetencyEvaluation } from "./competency-evaluation.model";
import { IGoalEvaluation } from "./goal-evaluation.model";
import { IFeedbackResponse } from "@ds/performance/feedback";
import { IMeritIncreaseView } from "./merit-increase-view.model";
import { IApprovalProcessHistory } from './approval-process-history.model';
import { IReviewRating } from '@ds/performance/ratings';
import { ICompetencyRateCommentRequired } from './competency-rate-comment-required.model';
import { IGoalRateCommentRequired } from './goal-rate-comment-required.model';
import { ApprovalProcessHistoryAction } from './approval-process-history-action.enum';

export interface IEvaluationDetail extends IEvaluation {
    reviewedEmployeeContact: IContact;
    competencyEvaluations: ICompetencyEvaluation[];
    goalEvaluations: IGoalEvaluation[];
    ratings: IReviewRating[];
    feedbackResponses: IFeedbackResponse[];
    allowGoalWeightAssignment: boolean;
    meritIncreaseInfo: IMeritIncreaseView;
    approvalProcessHistory: IApprovalProcessHistory[];
    isApprovalProcess: boolean;
    approvalProcessAction: ApprovalProcessHistoryAction;
    competencyRateCommentRequired?: ICompetencyRateCommentRequired[];
    goalRateCommentRequired?: IGoalRateCommentRequired[];

}
