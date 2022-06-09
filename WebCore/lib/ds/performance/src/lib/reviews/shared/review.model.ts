import { IReviewMeeting } from "./review-meeting.model";
import { IEvaluation } from "@ds/performance/evaluations";
import { IMeritIncreaseCurrentPayAndRates } from "@ds/performance/evaluations/shared/merit-increase-current-pay-and-rates.model";
import { IReviewContact, IContact } from '@ds/core/contacts/shared/contact.model';
import { IProposal } from '@ds/performance/evaluations/shared/proposal.model';
import { IUserInfo } from '@ajs/user';
import { EvaluationGroupReview } from './evaluation-group-review.model';

export interface IReview extends ReviewBase {
    reviewedEmployeeId: number; 
    reviewedEmployeeContact?: IReviewContact;
}

export interface IReviewWithEmployees extends ReviewBase {
    reviewedEmployeeIds:number[],
    reviewedEmployeeContacts?:IReviewContact[]
}

interface ReviewBase {
    reviewId: number;
    clientId: number;
    reviewProfileId?: number;
    reviewTemplateId?: number;
    reviewOwnerUserId?: number;
    reviewOwnerContact?: IContact;
    
    title: string;
    evaluationPeriodFromDate?: Date | string;
    evaluationPeriodToDate?: Date | string;
    reviewProcessStartDate?: Date | string;
    reviewProcessDueDate?: Date | string;
    overallRatingValue?: number;
    instructions?: string;
    reviewCompletedDate?: Date | string;
    evaluations?: IEvaluation[];
    meetings?: IReviewMeeting[];
    isGoalsWeighted?: boolean;
    isReviewMeetingRequired?: boolean;
    payrollRequestEffectiveDate?: Date | string;
    meritIncreases?: IMeritIncreaseCurrentPayAndRates[];
    overallScore?: number;
    proposal?: IProposal;
    directSupervisor?: IUserInfo;
    completedGoals?: number;
    totalGoals?: number;
    evaluationGroupReviews: EvaluationGroupReview[];
    employeeNotes?: IReviewRemark[];
    //reviewProfileEvaluations?: IReviewProfileEvaluation[];
}

export interface IReviewRemark {
    remarkId: number;
    description: string;
    addedDate: Date | string;
    addedBy: number;
    addedByFirstName: string;
    addedByLastName: string;
}

//export interface IReviewProfileEvaluation {
//    reviewProfileEvaluationId: number;
//    role: EvaluationRoleType;
//    isActive: boolean;
//}

