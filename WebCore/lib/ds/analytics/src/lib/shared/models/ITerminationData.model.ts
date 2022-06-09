import { EmployeeTermination } from "./TerminationData.model";
import { DateRange } from './DateRange.model';

export interface ITerminationData {
    TerminationData: EmployeeTermination[];
    featureName: string;
    dateRange: DateRange;
}