import { Moment } from 'moment';
import { DayOfWeek } from '../enums';

export interface ScheduledHoursWorkedResult {
    days: ScheduledHoursWorked[];
    hasSchedule: boolean;
    totalHoursScheduled: number;
    totalHoursWorked: number;
}

export interface ScheduledHoursWorked {
    date: Moment | Date | string;
    dayOfWeek: DayOfWeek;
    // TODO: For consistency we should consider refactoring {Start,End}Time to {Start,End}Time1
    startTime: Moment | Date | string;
    endTime: Moment | Date | string;
    startTime2: Moment | Date | string;
    endTime2: Moment | Date | string;
    startTime3: Moment | Date | string;
    endTime3: Moment | Date | string;
    scheduledHours: number;
    hoursWorked: number;

    // UI PROPERTIES - NOT APART OF DTO
    dayName?: string;
    isSystemDefault: boolean;
}
