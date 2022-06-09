import { Pipe, PipeTransform } from '@angular/core';
import { IRelativeTimeDifference } from '../reviews/shared/relative-time-difference';
import { RelativeTimeType } from '../reviews/shared/relative-time-type';
import * as moment from "moment";

@Pipe({
  name: 'toFriendlyTimeDiff'
})
export class ToFriendlyTimeDiffPipe implements PipeTransform {

  transform(date: string | Date | moment.Moment): IRelativeTimeDifference {
    if (!date)
    return <IRelativeTimeDifference>{};

let m = moment(date).startOf('day');
let today = moment().startOf('day');
let diff = m.diff(today, 'day', true);
let val = <IRelativeTimeDifference>{};

if (diff === 0) {
    val.displayText = "today";
    val.relativeTimeType = RelativeTimeType.Soon;
    val.isSoon = true;
}
else if (diff === 1) {
    val.displayText = "tomorrow";
    val.relativeTimeType = RelativeTimeType.Soon;
    val.isSoon = true;
}
else if (diff > 0) {
    val.displayText = m.from(today);
    val.relativeTimeType = RelativeTimeType.Upcoming;
    val.isUpcoming = true;
}
else {
    val.displayText = "Overdue";
    val.relativeTimeType = RelativeTimeType.InPast;
    val.isInPast = true;
}

return val;
  }

}
