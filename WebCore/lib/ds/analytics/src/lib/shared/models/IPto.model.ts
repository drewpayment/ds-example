import { PTOInfo } from './Pto.model';
import { DateRange } from './DateRange.model';

export interface IPto {
    ptoInfo: PTOInfo[];
    featureName: string;
    dateRange: DateRange;
}
