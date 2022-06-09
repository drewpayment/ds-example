import { UserInfo } from "@ds/core/shared";
import { IReviewProfileBasic } from "@ds/performance/review-profiles";
import { IContactWithClient } from "@ds/core/contacts";
import { IReview } from "../../reviews/shared/review.model";
import { IReviewTemplate } from "../../../../../core/src/lib/groups/shared/review-template.model";
import { ReferenceDate } from '../../../../../core/src/lib/groups/shared/schedule-type.enum';
import { Group } from '@ds/core/groups/shared/group.model';

export interface IReviewTemplateDialogData {
    reviewTemplateId: number;
    reviewProfileId: number;
    name: string;
    reviewProcessStartDate?: Date | string;
    reviewProcessEndDate?: Date | string;
    evaluationPeriodFromDate?: Date | string;
    evaluationPeriodToDate?: Date | string;
    payrollRequestEffectiveDate?: Date | string;
}

export interface IReviewTemplateBasicDialog {
    reviewTemplateId: number;
    reviewProfileId: number;
    name: string;
    reviewProcessStartDate?: Date | string;
    reviewProcessEndDate?: Date | string;
    evaluationPeriodFromDate?: Date | string;
    evaluationPeriodToDate?: Date | string;
    payrollRequestEffectiveDate?: Date | string;
}

export interface IReviewTemplateEditDialogData {
    employee: IContactWithClient;
    reviews?: IReview[];
    reviewTemplate?: IReviewTemplate;
    reviewProfile?: IReviewProfileBasic;
    currentUser: UserInfo;
    type: ReferenceDate;
    openRecurringView: boolean;
}

export interface IReviewTemplateForm {
    reviewProfile: string;
    title: string;
    evaluationPeriodFromDate: string;
    evaluationPeriodToDate: string;
    reviewProcessStartDate: string;
    reviewProcessEndDate: string;
    reviewProcessDueDate: string;
    employeeEvaluationDueDate: string;
    employeeEvaluationStartDate: string;
    supervisorEvaluationDueDate: string;
    supervisorEvaluationStartDate: string;
}
