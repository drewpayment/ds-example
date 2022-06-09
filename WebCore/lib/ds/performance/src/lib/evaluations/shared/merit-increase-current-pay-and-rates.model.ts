import { MeritIncreaseType } from "./merit-increase-type.enum";
import { IRemark } from "@ds/core/shared";
import { checkboxComponent } from '@ajs/applicantTracking/application/inputComponents';

export interface IMeritIncreaseCurrentPayAndRates{
    selected: boolean;
    rateDesc: string;
    payFrequencyId: number;
    payFrequencyDesc: string;
    currentAmount: number;
    increaseType: MeritIncreaseType;
    increaseAmount: number;
    amountIncreased: number;
    proposedTotal: number;
    comments: IRemark[];
    isSalaryRow: boolean;
    isRate: boolean;
    employeePayId: number;
    employeeClientRateId: number;
    canViewHourlyRates: boolean;
    canViewSalaryRates: boolean;
    proposalId:number;
    meritIncreaseId:number;
    processedByPayroll: boolean;
    acceptedByPayroll:boolean;
    applyMeritIncreaseOn?: Date | string;
}
