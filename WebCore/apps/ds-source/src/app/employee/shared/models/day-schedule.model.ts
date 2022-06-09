import { DayShift } from './day-shift.model';

export interface DaySchedule {
    clockEmployeeScheduleId: number,
    eventDate: Date, 
    clientId: number,
    employeeId: number,
    shift1: DayShift, 
    shift2: DayShift, 
    shift3: DayShift, 
    validShifts: DayShift[],
}