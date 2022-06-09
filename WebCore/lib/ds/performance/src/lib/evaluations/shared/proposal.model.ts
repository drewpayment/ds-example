import { ApprovalStatus } from './approval-status.enum';
import { IRemark } from '@ds/core/shared';
import { IMeritIncrease } from '../shared/merit-increase.model';
import { OneTimeEarning } from '@ds/performance/competencies/shared/one-time-earning.model';

export interface IProposal {
    proposalID: number;
    proposedByUserID: number;
    approvalStatus: ApprovalStatus;
    title: string;
    description: string;
    remarks: IRemark[];
    meritIncreases: IMeritIncrease[];
    oneTimeEarning: OneTimeEarning;
}
