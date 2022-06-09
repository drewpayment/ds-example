export interface ScheduledWorkedData {
    data: ScheduleWorked[];
}

export interface ScheduleWorked {
    actualHours: number;
    employeeName: string;
    firstName: string;
    hoursScheduled: number;
    shortDate: string;
    lastName: string;
    scheduleStart: Date;
    scheduleEnd: Date;
    scheduledHours: number;
    startSchedule: string;
    stopSchedule: string;
    workedHours: number;
}