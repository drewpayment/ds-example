import { Moment } from 'moment';

export interface ClockEmployeePayPeriodEnded {
    employeeId?:number;
    periodEnded?:Moment|Date|string;
    periodStartLocked?:Moment|Date|string;
    warningMessageLocked:string;
    warningMessageClosed:string;
    allowScheduleEdits:number;
}
