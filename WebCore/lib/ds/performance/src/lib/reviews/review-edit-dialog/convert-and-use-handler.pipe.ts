import { Pipe, PipeTransform } from '@angular/core';
import * as moment from "moment";

@Pipe({
  name: 'convertAndUseHandler'
})
export class ConvertAndUseHandlerPipe implements PipeTransform {

  transform(
    date1: moment.Moment | string,
        date2: moment.Moment | string,
        defaultMoment: moment.Moment,
        handler: (moment1: moment.Moment, moment2: moment.Moment) => moment.Moment
  ): moment.Moment {
    if (date1 == null && date2 == null) {
      return defaultMoment;
  }

  var date1ToCompare: moment.Moment;
  if (typeof date1 === 'string') {
      date1ToCompare = moment(new Date(date1));
  } else {
      date1ToCompare = date1
  }
  var date2ToCompare: moment.Moment;
  if (typeof date2 === 'string') {
      date2ToCompare = moment(new Date(date2));
  } else {
      date2ToCompare = date2
  }

  if (date1 != null && date2 == null) {
      return date1ToCompare;
  }

  if (date1 == null && date2 != null) {
      return date2ToCompare;
  }

  return handler(date1ToCompare, date2ToCompare);
  }

}
