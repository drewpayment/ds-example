import { Moment } from 'moment';

export interface WeeklySchedule {
    firstDayOfWeek: Date | Moment | string;
    lastDayOfWeek: Date | Moment | string;
}
