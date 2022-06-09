import { IReviewStatus } from "..";
import { ApprovalStatus } from '@ds/performance/evaluations/shared/approval-status.enum';

export interface IReviewStatusWithApprovalStatus extends IReviewStatus {
    approvalStatus?: ApprovalStatus;
  }