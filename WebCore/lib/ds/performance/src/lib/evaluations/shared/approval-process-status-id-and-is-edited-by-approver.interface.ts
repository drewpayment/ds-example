import { ApprovalProcessStatus } from './approval-process-status.enum';

export interface IApprovalProcessStatusIdAndIsEditedByApprover {
    approvalProcessStatusId?: ApprovalProcessStatus;
    isEditedByApprover?: boolean;
}
