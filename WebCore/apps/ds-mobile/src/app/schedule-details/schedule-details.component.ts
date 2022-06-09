import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import * as moment from 'moment';
import { EmployeeServicesService } from '@ds/core/employee-services/employee-services.service';
import { ClockEmployeePunchDto, ClockEmployeePunchDtoImpl } from '@ds/core/employee-services/models/clock-employee-punch-dto';
import { UserInfo, MOMENT_FORMATS } from '@ds/core/shared';
import { ScheduleDetailsDay, ScheduleDetailsDayImpl } from './schedule-details-day.model';
import { ScheduleDetailsDatesImpl, ScheduleDetailsDates } from './schedule-details-dates.model';
import { switchMap } from 'rxjs/operators';
import { PunchOptionType } from '@ds/core/employee-services/enums';
import { ClockService } from '@ds/core/employee-services/clock.service';
import { ClockEmployeeSetup } from '@ajs/labor/clock-employee/models';
import { ScheduledHoursWorkedResult, ScheduledHoursWorked } from '@ds/core/employee-services/models';
import { AccountService } from '@ds/core/account.service';

@Component({
  selector: 'ds-schedule-details',
  templateUrl: './schedule-details.component.html',
  styleUrls: ['./schedule-details.component.scss']
})
export class ScheduleDetailsComponent implements OnInit {

    user: UserInfo;
    employeeId: number; // = 125021;
    punchesByDay: ScheduleDetailsDay[] = [];
    isLoading = {
        titleDateString: true,
        content: true
    };

    // Date stuff
    dates: ScheduleDetailsDates;

    clockSetup: ClockEmployeeSetup;
    weeklyHoursWorked: ScheduledHoursWorkedResult;
    inputHoursShift: ScheduledHoursWorked;

    constructor(
        private route: ActivatedRoute,
        private accountSvc: AccountService,
        private employeeServicesSvc: EmployeeServicesService,
        private clockService: ClockService
    ) {
        // Nothing.
    }

    ngOnInit() {
        this.accountSvc.getUserInfo().pipe(
            switchMap(u => {
                this.user = u;
                this.employeeId = this.user.userEmployeeId;
                return this.route.paramMap;
            }),
            switchMap(params => {
                this.dates = new ScheduleDetailsDatesImpl(params);
                this.isLoading.titleDateString = false;

                return this.employeeServicesSvc.employeeLaborSetup$;
            }),
            switchMap(setup => {
                this.clockSetup = setup;
                if (this.clockSetup.punchOption === PunchOptionType.InputHours) {
                    return this.clockService.getWeeklyHoursWorked(this.user.clientId, this.user.userEmployeeId,
                        this.dates.startDate.format(MOMENT_FORMATS.API), this.dates.endDate.format(MOMENT_FORMATS.API),
                        this.clockSetup.punchOption);
                }

                return this.dates.shiftDate
                    ? this.employeeServicesSvc.getEmployeeShiftPunches(this.employeeId, this.dates.shiftDate.format(MOMENT_FORMATS.API))
                    : this.employeeServicesSvc.getEmployeePunches(this.employeeId, this.dates.startDate.format(MOMENT_FORMATS.API),
                        this.dates.endDate.format(MOMENT_FORMATS.API));
            })
        ).subscribe(payload => {
            if (this.clockSetup.punchOption === PunchOptionType.InputHours) {
                this.weeklyHoursWorked = payload as ScheduledHoursWorkedResult;
                this.inputHoursShift = this.weeklyHoursWorked.days
                    .find(d => moment(d.date).isSame(this.dates.shiftDate || this.dates.startDate, 'day'));
                this.isLoading.content = false;
            } else {
                this.getEmployeePunchesCallback(payload as ClockEmployeePunchDto[]);
            }
        });
    }

    inputHoursTitleString() {
        if (!this.inputHoursShift) return 'Punches';
        return `Hours for ${moment(this.inputHoursShift.date).format('ddd MMM D')}`;
    }

    private getEmployeePunchesCallback(punches: ClockEmployeePunchDto[]): void {
        // Partition by day
        this.partitionPunchesByDay(punches);
        // Party by night ;)
        this.isLoading.content = false;
    }

    private partitionPunchesByDay(punches: ClockEmployeePunchDto[]): void {
        const dayPartitionKeys = this.getDayPartitionKeys();

        // Partition by day
        dayPartitionKeys.forEach(key => {
            const scheduleDetailsDay = new ScheduleDetailsDayImpl(key);

            scheduleDetailsDay.punches = punches.filter(p => {
                const date = ClockEmployeePunchDtoImpl.coalesceShiftDate(p);
                return this.getYearMonthDayFromTimestamp(date as string) === key;
            });

            this.punchesByDay.push(scheduleDetailsDay);
        });

    }

    private getDayPartitionKeys(): string[] {
        const dayPartitionKeys: string[] = [];

        const daysCount = this.dates.endDate.diff(this.dates.startDate, 'days');

        // We don't want to mutate the OG startDate
        const startDateClone = this.dates.startDate.clone();

        // Generate array of day partition keys
        for (let i=0; i < daysCount; i++) {
            const key = startDateClone.format('YYYY-MM-DD');
            dayPartitionKeys.push(key);
            startDateClone.add(1, 'day');
        }

        // This populates keys for only the days returned via the api.
        // // Get array of day partition keys
        // punches.forEach(x => {
        //     const date = ClockEmployeePunchDtoImpl.coalesceShiftDate(x);
        //     // The api is going to return this in raw string format.
        //     const yearMonthDay = this.getYearMonthDayFromTimestamp(date as string);

        //     if (!dayPartitionKeys.includes(yearMonthDay)) {
        //         dayPartitionKeys.push(yearMonthDay);
        //     }
        // });

        return dayPartitionKeys;
    }

    private getYearMonthDayFromTimestamp(s: string): string {
        if (s.length > 0) {
            const  index        = s.indexOf('T');
            const  yearMonthDay = s.substring(0, index);
            return yearMonthDay;
        }
    }
}
