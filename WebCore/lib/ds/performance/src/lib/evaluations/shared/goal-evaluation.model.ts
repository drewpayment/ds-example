import { IRemark, ViewRemark } from "@ds/core/shared";
import * as moment from "moment";
import { checkboxComponent } from "@ajs/applicantTracking/application/inputComponents";
import { ITask, CompletionStatusType } from "@ds/core/shared";
import { IApprovalProcessStatusIdAndIsEditedByApprover } from './approval-process-status-id-and-is-edited-by-approver.interface';

export interface IGoalEvaluation extends IApprovalProcessStatusIdAndIsEditedByApprover {
    goalId: number;
    evaluationId: number;
    title: string;
    description: string;
    startDate?: string | Date | moment.Moment;
    dueDate?: string | Date | moment.Moment;
    completionDate?: string | Date | moment.Moment;
    progress?: number;
    ratingValue?: number;
    tasks?:ITask[];
    comment?: IRemark;
    modified?: Date | string;
    weight?: number;
    onReview: boolean;
    isCommentRequired?: boolean;
    activityFeed?:ViewRemark[];
    oneTimeEarningName: string;
}
