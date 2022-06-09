import { UserType } from "./UserType.model";
import { DateRange } from './DateRange.model';

export interface IUserType {
    userType: UserType[];
    featureName: number;
    dateRange: DateRange;
}