import { ExceptionData } from './ExceptionData.model'
import { DateRange } from './DateRange.model';

export interface IExceptionData {
    employeeExceptions: ExceptionData[];
    featureName: string;
    dateRange: DateRange;
    scheduleData: any;
}