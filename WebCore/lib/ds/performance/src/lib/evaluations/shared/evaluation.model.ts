import { EvaluationRoleType } from "./evaluation-role-type.enum";
import { IContact } from "@ds/core/contacts";
import { ISignature } from "@ds/core/signatures";
import { IFeedbackResponse } from "@ds/performance/feedback";
import { IUserInfo } from '@ajs/user';
import { ICompetencyEvaluation } from '@ds/performance/evaluations/shared/competency-evaluation.model';

export interface IEvaluation {
    evaluationId: number;
    reviewId: number;
    role: EvaluationRoleType;
    evaluatedByUserId: number;
    currentAssignedUserId: number;
    startDate?: Date | string;
    dueDate: Date | string;
    overallRatingValue?: number;
    completedDate?: Date | string;
    completedTime?: Date | string;
    evaluatedByContact?: IContact;
    signature?: ISignature;
    signatures?: ISignature[];
    competencyEvaluations? : Array<ICompetencyEvaluation>;
    feedbackResponses?: IFeedbackResponse[];
    isViewableByEmployee?: boolean;
    isApprovalProcess?: boolean;
    currentAssignedUser?: IUserInfo;
}
