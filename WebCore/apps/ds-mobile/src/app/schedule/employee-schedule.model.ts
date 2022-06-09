import { Moment } from 'moment';
import * as moment from 'moment';
import { ScheduledHoursWorked } from '@ds/core/employee-services/models';
import { EmployeeScheduleSetup } from '@ajs/labor/clock-employee/models';
import { DayOfWeek } from '@ds/core/employee-services/enums';

export interface EmployeeSchedule {
    date: Moment; //|Date|string;
    schedule: string[];
    hours: number;
    isScheduled(): boolean;
    // Format: DayOfWeekAbbreviation MonthAbbreviation DayOfMonth
    getTitleDateString(): string;
    getYearMonthDayString(): string;
    hasHours(): boolean;
    isToday(): boolean;
}

export class EmployeeScheduleImpl implements EmployeeSchedule {
    static readonly notScheduledString = 'Not scheduled';
    date: Moment;
    private _schedule: string[] = [];
    get schedule(): string[] {
        if (this.day && this.day.isSystemDefault && !this._schedule) return [];
        return this._schedule;
    }
    set schedule(value: string[]) {
        this._schedule = value;
    }
    hours: number;
    
    constructor(private day: ScheduledHoursWorked, private schedules: EmployeeScheduleSetup[]) {
        if (!this.day) return;
        
        this.date = moment(this.day.date);
        this.hours = this.day.hoursWorked;        
        
        if (this.hasScheduleToday()) {
            if (this.day.startTime) 
                this.schedule.push(this.getScheduleString(moment(this.day.startTime), moment(this.day.endTime)));
                
            if (this.day.startTime2)
                this.schedule.push(this.getScheduleString(moment(this.day.startTime2), moment(this.day.endTime2)));
                
            if (this.day.startTime3) 
                this.schedule.push(this.getScheduleString(moment(this.day.startTime3), moment(this.day.endTime3)));
        }
    }

    isScheduled() {
        return this.day && !this.day.isSystemDefault;
    }

    // Format: DayOfWeekAbbreviation MonthAbbreviation DayOfMonth
    getTitleDateString() {
        return this.date.format('ddd MMM D');
    }

    getYearMonthDayString() {
        return this.date.format('YYYY-MM-DD');
    }

    hasHours() {
        return this.hours > 0;
    }

    isToday() {
        return moment().isSame(this.getYearMonthDayString(), 'day');
    }
    
    private getScheduleString(start: Moment, stop: Moment) {
        const startTimeString = start.format('h:mm A');
        const stopTimeString = stop.format('h:mm A');
        return `${startTimeString} - ${stopTimeString}`;
    }
    
    private hasScheduleToday(): boolean {
        if (!this.schedules) return false;
        let hasSchedule = false;
        const dow = this.date.weekday();
        
        for(let i = 0; i < this.schedules.length; i++) {
            if (hasSchedule) break;
            const s = this.schedules[i];
            
            switch(dow) {
                case DayOfWeek.Sunday:
                    hasSchedule = s.scheduleDetails['isSunday'];
                    break;
                case DayOfWeek.Monday:
                    hasSchedule = s.scheduleDetails['isMonday'];
                    break;
                case DayOfWeek.Tuesday:
                    hasSchedule = s.scheduleDetails['isTuesday'];
                    break;
                case DayOfWeek.Wednesday:
                    hasSchedule = s.scheduleDetails['isWednesday'];
                    break;
                case DayOfWeek.Thursday:
                    hasSchedule = s.scheduleDetails['isThursday'];
                    break;
                case DayOfWeek.Friday:
                    hasSchedule = s.scheduleDetails['isFriday'];
                    break;
                case DayOfWeek.Saturday:
                    hasSchedule = s.scheduleDetails['isSaturday'];
                    break;
            }
        }
        
        return hasSchedule;
    }
}
