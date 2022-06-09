import { ParamMap } from '@angular/router';
import { Moment } from 'moment';
import * as moment from 'moment';

export interface ScheduleDetailsDates {
    shiftDate: Moment;
    startDate: Moment;
    endDate: Moment;
}

export class ScheduleDetailsDatesImpl {
    shiftDate: Moment;
    startDate: Moment;
    endDate: Moment;

    constructor(params: ParamMap) {
        // Fall through - handle both routes to this component:
        // { path: 'schedule/:shiftDate' }
        // { path: 'schedule/:startDate/:endDate' }
        if (params.has('shiftDate')) {
            this.shiftDate = moment(params.get('shiftDate'));
            this.startDate = this.shiftDate;
        }

        if (params.has('startDate'))
            this.startDate = moment(params.get('startDate'));

        if (params.has('endDate'))
            this.endDate = moment(params.get('endDate'));

        // If we didn't pass an endDate, we only care about a single day (shiftDate).
        if (!this.endDate)
            this.endDate = this.startDate.clone().add(1, 'day');
    }
}
