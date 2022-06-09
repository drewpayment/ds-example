import { IRemark, ViewRemark } from "@ds/core/shared";
import { IApprovalProcessStatusIdAndIsEditedByApprover } from './approval-process-status-id-and-is-edited-by-approver.interface';

export interface ICompetencyEvaluation extends IApprovalProcessStatusIdAndIsEditedByApprover {
    competencyId: number;
    evaluationId: number;
    name: string;
    groupName?: string | null;
    groupId?:number;
    description: string;
    difficultyLevel?: number;
    ratingValue?: number;
    comment?: IRemark;
    isCore: boolean;
    modified?: Date | string;
    isCommentRequired?: boolean;
    activityFeed?:ViewRemark[];
}
