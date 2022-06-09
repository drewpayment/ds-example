import { MeritIncreaseType } from './merit-increase-type.enum';
import { IRemark } from '@ds/core/shared';
import { IProposal } from './proposal.model';
import { ApprovalStatus } from '@ds/performance/evaluations/shared/approval-status.enum';

export interface IMeritIncrease {
proposalId: number;
type: MeritIncreaseType;
increaseAmount: number;
rateName: string;
clientRateId?: number;
employeeClientRateId?: number;
employeePayId?: number;
currentAmount: number;
proposedAmount: number;
comments: IRemark[];
proposedBy: number;
meritIncreaseId:number;
applyMeritIncreaseOn?: Date | string;
payFrequencyId: number;
approvalStatusID?: ApprovalStatus;
effectiveDate? : any;
}

export interface IReviewMeritIncrease extends IMeritIncrease {
    reviewId: number;
    reviewTemplateId?:number;
    proposal: IProposal;
    processedByPayroll: boolean;
    acceptedByPayroll: boolean;
}
