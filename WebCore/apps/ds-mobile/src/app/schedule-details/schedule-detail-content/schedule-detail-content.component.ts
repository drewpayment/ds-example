import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ClockEmployeePunchDtoImpl } from '@ds/core/employee-services/models/clock-employee-punch-dto';
import { Moment } from 'moment';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';
import { ScheduleDetailsDay } from '../schedule-details-day.model';
import { EmployeeServicesService } from '@ds/core/employee-services/employee-services.service';
import { tap } from 'rxjs/operators';
import { ScheduleDetailsDates } from '../schedule-details-dates.model';
import { ScheduledHoursWorkedResult } from '@ds/core/employee-services/models';

@Component({
    selector: 'ds-schedule-detail-content',
    templateUrl: './schedule-detail-content.component.html',
    styleUrls: ['./schedule-detail-content.component.scss']
})
export class ScheduleDetailContentComponent implements OnInit, OnDestroy {

    @Input() scheduleDetailsDay: ScheduleDetailsDay;
    @Input() dates: ScheduleDetailsDates;
    isLoading = {
        inOutPunchPairs: true,
        hoursWorked: true
    };

    // Make this function available in the template
    punchExists = ClockEmployeePunchDtoImpl.punchExists;

    getPunchTimeString(punch: Moment | Date | string): string {
        return convertToMoment(punch).format('hh:mm A');
    }

    constructor(
        private service: EmployeeServicesService
    ) {
        // Nothing.
    }

    ngOnInit() {
        const date = convertToMoment(this.scheduleDetailsDay.date); // .format('YYYY-MM-DD');

        // const dateInDates = date.isBetween(this.dates.startDate, this.dates.endDate);
        const dateInServiceDates = date.isBetween(this.service.startDate, this.service.endDate);

        const callback = (result: ScheduledHoursWorkedResult) => {
            const scheduledHoursWorked = result.days.find(d => {
                return  convertToMoment(d.date).isSame(date, 'day');
            });
            if (scheduledHoursWorked)
                this.scheduleDetailsDay.hoursWorked = scheduledHoursWorked.hoursWorked;
            else
                this.scheduleDetailsDay.hoursWorked = 0;
            this.isLoading.hoursWorked = false;
        };

        if (dateInServiceDates) {
            // this.service.getWeeklyHoursWorked(date)
            this.service.getWeeklyHoursWorked()
                .pipe(tap(result => callback(result)))
                .subscribe();
        } else {
            this.service.getWeeklyHoursWorked(this.service.startDate, this.service.endDate)
                .pipe(tap(result => callback(result)))
                .subscribe();
        }

        this.scheduleDetailsDay.getInOutPunchPairs();
        this.isLoading.inOutPunchPairs = false;

        // this.scheduleDetailsDay.getHoursWorked();
        // this.isLoading.hoursWorked = false;
    }

    ngOnDestroy(): void {

    }

}
