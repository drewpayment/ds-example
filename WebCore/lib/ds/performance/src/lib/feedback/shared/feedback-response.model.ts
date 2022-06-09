import { IContact } from "@ds/core/contacts";
import { checkboxComponent } from "@ajs/applicantTracking/application/inputComponents";
import { EvaluationRoleType } from '@ds/performance/evaluations/shared/evaluation-role-type.enum';
import { FieldType, ViewRemark } from '@ds/core/shared';
import { IFeedbackItem, IFeedbackResponseItem } from '@ds/performance/feedback/';
import { ApprovalProcessStatus } from '@ds/performance/evaluations/shared/approval-process-status.enum';
import { IApprovalProcessStatusIdAndIsEditedByApprover } from '@ds/performance/evaluations/shared/approval-process-status-id-and-is-edited-by-approver.interface';

export interface IFeedbackResponse extends IApprovalProcessStatusIdAndIsEditedByApprover {
    feedbackId: number;
    responseId: number;
    feedbackBody: string;
    responseByContact: IContact;
    fieldType: FieldType;
    isVisibleToEmployee: boolean;
    isRequired: boolean;
    orderIndex?: number | null;
    isActive: boolean;
    activityFeed?:ViewRemark[];
    hasResponse(): boolean;
    isLoading: boolean;
    oldVal: any;

}

export interface IFeedbackResponseData extends IFeedbackResponse {
    evaluationRoleType : EvaluationRoleType;
    feedbackItems: Array<IFeedbackItem>;
    responseItems: Array<IFeedbackResponseItemData>;
}
export interface IReviewIdWithFeedbackResponse {
    feedback: IFeedbackResponseData;
    reviewId: number;
}

export interface IFeedbackResponseItemData extends IFeedbackResponseItem {
    feedbackItemId?: number;
    booleanValue?: boolean;
    textValue:string;
    dateValue?:Date;
}
