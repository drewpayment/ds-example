import { UserInfo } from '@ds/core/shared';
import { PayTypeEnum } from '@ajs/employee/hiring/shared/models/employee-hire-data.interface';

export interface IPayrollRequestItem {
    requestType: number,
    foreignKeyId: number,
    proposalId: number,
    reviewId: number,
    clientEarningId: number,
    basedOne: number,
    employeeClientRateId: number,
    employeeFirstName: string,
    employeeLastName: string,
    employeeNumber: string,
    payFrequencyId: number,
    increaseType: number,
    increaseAmount: number,
    annualPayPeriodCount: number,
    annualizedIncreaseAmount: number,
    annualizedTotalAmount: number,
    effectiveDate: Date,
    approvalStatusId: number,
    payoutFrom: number,
    payoutTo: number,
    score: number,
    percent: number,
    directSupervisorName: string,
    employeeJobTitle: string,
    modifiedBy: number,
    modified: Date,
    awardDescription: string,
    earningDescription: string,
    directSupervisor: UserInfo,
    isSelectedOnTableView: boolean,
    isEnabledOnProposal: boolean,
    completedGoals?: number,
    totalGoals?: number,
    processedByPayroll: boolean,
    basePay: number;
    _isProposalOpen: boolean;
    payType: number;
    canViewPayout: boolean;
    target?: number;
    department: string;
    division: string;
    approvedBy?: string;
    approvedOn?: Date;
    employeePayType?: PayTypeEnum
}
