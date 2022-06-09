import { PayFrequencyTypeEnum } from '@ajs/employee/hiring/shared/models';
export interface IPayrollHistoryInfo {
    payrollId: number;
    periodEnded?: Date;
    periodStart?: Date;
    checkDate: Date;
    isOpen: boolean;
    runDate?: Date;
    runBy: string;
    payrollHistoryId?: number;
    totalGross?: number;
    totalNet?: number;
    payrollRunDescription?: string;
    payrollRunId: number;
    isFrequencyWeekly: boolean;
    isFrequencyBiWeekly: boolean;
    isFrequencyAltBiWeekly: boolean;
    isFrequencySemiMonthly: boolean;
    isFrequencyMonthly: boolean;
    isFrequencyQuarterly: boolean;
    frequencyBiWeeklyPeriodStart: Date;
    frequencyBiWeeklyPeriodEnded: Date;
    frequencyAltBiWeeklyPeriodStart: Date;
    frequencyAltBiWeeklyPeriodEnded: Date;
    frequencySemiMonthlyPeriodStart: Date;
    frequencySemiMonthlyPeriodEnded: Date;
    frequencyMonthlyPeriodStart: Date;
    frequencyMonthlyPeriodEnded: Date;
    frequencyQuarterlyPeriodStart: Date;
    frequencyQuarterlyPeriodEnded: Date;
}

export interface IBasicPayrollHistory {
    payrollId: number;
    checkDate: Date;
    periodEnded?: Date;
    periodStart?: Date;
    runDate: Date;
    payrollRunId: IPayrollRun;
    payFrequencyType?: PayFrequencyTypeEnum;
}

export enum IPayrollRun {
    normalPayroll     = 1,
    specialPayroll    = 2,
    adjustment        = 3,
    parallel          = 4,
    taxcheckRun       = 5,
    realTimeRun       = 6,
    yearEndAdjustment = 7
}

export interface IPayrollRunType {
    payrollRunId: IPayrollRun;
    description: string;
}