import { Moment } from 'moment';

export interface LastPayrollDates {
    lastPayrollDate: Date | Moment | string;
    yearBefore: Date | Moment | string;    
}
