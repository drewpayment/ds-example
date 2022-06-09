import { EarningsByPaydate } from './EarningsByPaydate.model';

export interface EarningsBreakdown {
    payDateList: Date[];
    description: string;
    clientEarningId: number;
    totalAmount: number;
    startDate: Date;
    endDate: Date;
    breakdownByPaydate: EarningsByPaydate[];
}