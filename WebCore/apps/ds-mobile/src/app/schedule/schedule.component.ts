import { Component, OnInit, OnDestroy } from '@angular/core';
import { UserInfo, MOMENT_FORMATS } from '@ds/core/shared';
import { ClockEmployeeSetup, EmployeeScheduleSetup } from '@ajs/labor/clock-employee/models';
import { EmployeeSchedule, EmployeeScheduleImpl } from './employee-schedule.model';
import { Moment } from 'moment';
import * as moment from 'moment';
import { forkJoin, Subscription, iif, of, Observable } from 'rxjs';
import { EmployeeServicesService } from '@ds/core/employee-services/employee-services.service';
import { ScheduledHoursWorkedResult, EmployeeTimePolicyRuleConfiguration, ScheduledHoursWorked } from '@ds/core/employee-services/models';
import { Router, ActivatedRoute } from '@angular/router';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';
import { ClockService } from '@ds/core/employee-services/clock.service';
import { switchMap, tap, map } from 'rxjs/operators';
import { AccountService } from '@ds/core/account.service';

@Component({
    selector: 'ds-schedule',
    templateUrl: './schedule.component.html',
    styleUrls: ['./schedule.component.scss']
})
export class ScheduleComponent implements OnInit {
    user: UserInfo;
    clockEmployeeSetup: ClockEmployeeSetup;
    isLoading = true;

    //   nextPunchDetail: CheckPunchTypeResultDto;
    //   punchRules: EmployeeTimePolicyRuleConfiguration;

    weeklyHoursWorked: ScheduledHoursWorkedResult;
    // Date stuff
    firstDayOfWeek: Moment = moment().startOf('week');
    lastDayOfWeek: Moment = moment().endOf('week');

    // Alias ^^^ these
    startDate: Moment = this.firstDayOfWeek;
    endDate: Moment = this.lastDayOfWeek;

    // Used as dataSource in template table.
    employeeSchedules: EmployeeSchedule[];
    displayedColumns: string[] = ['date', 'schedule', 'hours', 'action'];
    // {date: 'Tue Jul 23', schedule: '8:00 am - 5:00 pm', hours: 8.00},

    initializeComponent$: Observable<any>;


    constructor(
        private employeeServicesSvc: EmployeeServicesService,
        private router: Router,
        private route: ActivatedRoute,
        private clockService: ClockService,
        private accountService: AccountService
    ) {
        // Nothing.
    }

    ngOnInit() {
        this.initializeComponent$ = this.accountService.getUserInfo().pipe(
            tap(u => {
               this.user = u;
            }),
            switchMap(u => {
                const getWeeklySchedules$ = this.clockService.getWeeklyScheduleByEmployeeId(u.userEmployeeId).pipe(
                    tap(weeklySchedule => {
                        if(weeklySchedule.firstDayOfWeek != undefined && weeklySchedule.lastDayOfWeek != undefined){
                            this.firstDayOfWeek = moment(weeklySchedule.firstDayOfWeek);
                            this.startDate = moment(weeklySchedule.firstDayOfWeek);
                            this.lastDayOfWeek = moment(weeklySchedule.lastDayOfWeek);
                            this.endDate = moment(weeklySchedule.lastDayOfWeek);
                        }
                    })
                );
                const weeklySchedules$ = iif(() => u.userEmployeeId !== null, getWeeklySchedules$, of(null));
                const empLaborSetup$ = this.employeeServicesSvc.getEmployeeLaborSetup();
                return forkJoin(weeklySchedules$, empLaborSetup$).pipe(map(x => { return x[1] }));
            }),
            switchMap(employeeLaborSetup => {
                this.clockEmployeeSetup = employeeLaborSetup;
                return this.clockService.getWeeklyHoursWorked(this.user.clientId, this.user.userEmployeeId,
                    this.startDate.clone().format(MOMENT_FORMATS.API), this.endDate.clone().format(MOMENT_FORMATS.API),
                    this.clockEmployeeSetup.punchOption, true);
            }),
        tap(weeklyHoursWorked => {
            this.weeklyHoursWorked = weeklyHoursWorked;
            // Initialize employeeSchedules
            this.employeeSchedules = [];
            if (this.weeklyHoursWorked.days && this.weeklyHoursWorked.days.length) {
                for (let i = 0; i < this.weeklyHoursWorked.days.length; i++) {
                    const day = this.weeklyHoursWorked.days[i];
                    const schedules = this.clockEmployeeSetup.selectedSchedules &&
                        this.clockEmployeeSetup.selectedSchedules.length ? this.clockEmployeeSetup.selectedSchedules : null;
                    this.employeeSchedules.push(new EmployeeScheduleImpl(day, schedules));
                }
            }
            // Give the all clear to render the table.
            this.isLoading = false;
        }))
    }

    // Prepare the formatted strings for later use.
    // private getManualScheduleString(d: ScheduledHoursWorked): string[] {
    //     let startEndTimesCount = 0;

    //     if (d.startTime  && d.endTime ) startEndTimesCount++;
    //     if (d.startTime2 && d.endTime2) startEndTimesCount++;
    //     if (d.startTime3 && d.endTime3) startEndTimesCount++;

    //     const result = new Array<string>(startEndTimesCount);

    //     // TODO: For consistency we should consider refactoring {Start,End}Time to {Start,End}Time1
    //     const fixMeLater = 1;
    //     if (startEndTimesCount > 0 && d.startTime  && d.endTime) {
    //         result.push(this.getScheduleString(d.startTime, d.endTime));
    //     }
    //     for (let i = fixMeLater; i < startEndTimesCount; i++) {
    //         const startKey = `startTime${i+1}`;
    //         const endKey = `endTime${i+1}`;
    //         result.push(this.getScheduleString(d[startKey], d[endKey]));
    //     }

    //     return result;
    // }

    // private getScheduleString(start: string|Moment|Date, stop: string|Moment|Date) {
    //     const startTimeString = moment(start).format('h:mm A');
    //     const stopTimeString = moment(stop).format('h:mm A');
    //     return `${startTimeString} - ${stopTimeString}`;
    // }

    titleDateString(): string {
        const startMonth = this.startDate.format('MMM');
        const endMonth = this.endDate.format('MMM');
        const startDay = this.startDate.format('D');
        const endDay = this.endDate.format('D');
        const year = this.startDate.format('YYYY');
        const startMonthNotEqualEndMonth = () => `${startMonth} ${startDay} - ${endMonth} ${endDay} ${year}`;
        const startMonthEqualEndMonth = () => `${startMonth} ${startDay} - ${endDay} ${year}`;
        return startMonth !== endMonth ? startMonthNotEqualEndMonth() : startMonthEqualEndMonth();
    }

    showDetail(row: EmployeeScheduleImpl) {
        if (row.isScheduled() || row.hasHours()) {
            this.router.navigate(['./details', row.getYearMonthDayString()], { relativeTo: this.route });
        }
    }

}
