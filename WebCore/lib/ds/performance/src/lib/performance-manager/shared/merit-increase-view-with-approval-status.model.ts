import { IMeritIncreaseView } from "@ds/performance/evaluations/shared/merit-increase-view.model";
import { ApprovalStatus } from '@ds/performance/evaluations/shared/approval-status.enum';

export interface MeritIncreaseViewWithApprovalStatus extends IMeritIncreaseView {
    approvalStatus?: ApprovalStatus;
    }