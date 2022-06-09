import { PunchName } from './PunchType.model'
import { DateRange } from './DateRange.model';

export interface IPunchNames {
    punchNames: PunchName[];
    featureName: string;
    dateRange: DateRange;
}
