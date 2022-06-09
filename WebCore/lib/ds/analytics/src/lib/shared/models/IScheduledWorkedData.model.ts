import { ScheduledWorkedData } from './ScheduledWorkedData.model'
import { DateRange } from './DateRange.model';

export interface IScheduledWorkedData {
    dataObjects: ScheduledWorkedData[];
    featureName: string;
    dateRange: DateRange;
}