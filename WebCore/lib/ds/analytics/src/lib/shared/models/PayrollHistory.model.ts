export interface PayrollHistory {
    payrollId: number;
    periodStart?: Date;
    periodEnded?: Date;
    checkDate: Date;
    payrollHistoryId?: number;
    totalGross?: number;
    totalNet?: number;
    payrollRunDescription?: string;
    runDate?: Date;
}
