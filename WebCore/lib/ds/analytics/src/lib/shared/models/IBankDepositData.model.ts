import { BankDeposit } from './BankDepositData.model'
import { DateRange } from './DateRange.model';

export interface IBankDepositData {
    bankDepositData: BankDeposit[];
    featureName: string;
    dateRange: DateRange;
    checkDate: string;
}