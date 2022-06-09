import { Component, OnInit, Input } from '@angular/core';
import * as moment from 'moment';
import { ScheduleDetailsDay } from '../schedule-details-day.model';
import { ScheduleDetailsDates } from '../schedule-details-dates.model';

@Component({
  selector: 'ds-schedule-detail-card',
  templateUrl: './schedule-detail-card.component.html',
  styleUrls: ['./schedule-detail-card.component.scss']
})
export class ScheduleDetailCardComponent implements OnInit {

    @Input() scheduleDetailsDay: ScheduleDetailsDay;
    @Input() dates: ScheduleDetailsDates;

    constructor() { }

    ngOnInit() {
    }

    titleDateString(): string {
      return moment(this.scheduleDetailsDay.date).format('ddd MMM D');
    }

}
