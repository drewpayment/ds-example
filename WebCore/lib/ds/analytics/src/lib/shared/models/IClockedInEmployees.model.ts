import { ClockedInEmployees } from './EmployeesClockedInData.model';
import { DateRange } from './DateRange.model';


export interface IEmployeesClockedIn {
    clockedInEmployees: ClockedInEmployees[];
    featureName: string;
    dateRange: DateRange;
}