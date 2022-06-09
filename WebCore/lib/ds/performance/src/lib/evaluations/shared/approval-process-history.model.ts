import { ApprovalProcessHistoryAction } from './approval-process-history-action.enum';

export interface IApprovalProcessHistory {
    approvalProcessHistoryId: number;
    toUserId: number;
    fromUserId: number;
    evaluationId: number;
    action: ApprovalProcessHistoryAction;
}
