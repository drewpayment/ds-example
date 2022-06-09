import { IncreaseType } from './increase-type';
import { ApprovalStatus } from '@ds/performance/evaluations/shared/approval-status.enum';
import { Moment } from 'moment';
import { RecommendedBonus } from '@ds/performance/evaluations/shared/recommended-bonus';

export interface OneTimeEarning {
    oneTimeEarningId?: number;
    employeeId?: number;
    increaseType?: IncreaseType;
    increaseAmount?: number;
    mayBeIncludedInPayroll?: Date | string | Moment;
    approvalStatusID?: ApprovalStatus;
    clientEarningId?: number;
    proposedTotalAmount?: number;
}

export interface OneTimeEarningWithRecommendation extends OneTimeEarning {
    recommendation: RecommendedBonus;
    earningDesc: string;
}