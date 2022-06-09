import { ActiveEmployee } from './ActiveEmployeeData.model'
import { DateRange } from './DateRange.model';

export interface IActiveEmployee {
    activeEmployee: ActiveEmployee[];
    featureName: string;
    dateRange: DateRange;
    title: string;
}