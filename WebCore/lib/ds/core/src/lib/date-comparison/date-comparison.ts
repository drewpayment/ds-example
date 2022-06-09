import { Pipe, PipeTransform } from '@angular/core';
import * as moment from "moment";
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';

@Pipe({
  name: 'compareDatePipe'
})
export class CompareDatePipe implements PipeTransform {

  readonly transform: (
    date1: moment.Moment | string | Date,
    date2: moment.Moment | string | Date,
    defaultResult: any,
    handler: (moment1: moment.Moment, moment2: moment.Moment, defaultResult: any) => any
  ) => any = DatePipeTransform

}

/**
 * A pipe that just calls @see findMaxDate
 */
@Pipe({ name: 'findMaxDate' })
export class FindMaxDatePipe implements PipeTransform {
  readonly transform = findMaxDate;
}

/**
 * A pipe which just calls @see findMinDate
 */
@Pipe({ name: 'findMinDate' })
export class FindMinDatePipe implements PipeTransform {
  readonly transform = findMinDate
}

/**
 * Returns the latest date based on the ones provided.  When one of the dates is invalid, the other is returned.  
 * The result is usually a @type moment.Moment.  The one time the result is not a @type moment.Moment is when 
 * both dates are invalid and defaultResult is not a @type moment.Moment
 * @param firstDate A date to compare
 * @param secondDate A date to compare
 * @param defaultResult Returned when both provided dates are invalid as determined by Moment.js
 */
export function findMinDate(
  firstDate: moment.Moment | string | Date,
  secondDate: moment.Moment | string | Date,
  defaultResult: any): any {
  const handler = handleInvalid((moment1: moment.Moment, moment2: moment.Moment) => moment1.isBefore(moment2) ? moment1 : moment2);

  return DatePipeTransform(firstDate, secondDate, defaultResult, handler)
}

/**
 * Returns the latest date based on the ones provided.  When one of the dates is invalid, the other is returned.  
 * The result is usually a @type moment.Moment.  The one time the result is not a @type moment.Moment is when 
 * both dates are invalid and defaultResult is not a @type moment.Moment
 * @param firstDate A date to compare
 * @param secondDate A date to compare
 * @param defaultResult Returned when both provided dates are invalid as determined by Moment.js
 */
export function findMaxDate(
  firstDate: moment.Moment | string | Date,
  secondDate: moment.Moment | string | Date,
  defaultResult: any): any {
  const handler = handleInvalid((moment1: moment.Moment, moment2: moment.Moment) => moment1.isAfter(moment2) ? moment1 : moment2);

  return DatePipeTransform(firstDate, secondDate, defaultResult, handler)
}

/**
 * Ensures that both provided dates are valid before attempting to call the provided handler.  When one of the 
 * dates is invalid, the other valid one is returned.  When both are invalid the defaultResult is returned
 * @param fn A handler to call when the both provided dates are valid
 */
function handleInvalid(fn: (moment1: moment.Moment, moment2: moment.Moment, defaultResult: moment.Moment) => moment.Moment): (moment1: moment.Moment, moment2: moment.Moment, defaultResult: any) => any {
  return (moment1, moment2, defaultResult) => {

    if (!(moment1.isValid() || moment2.isValid())) {
      return defaultResult;
    }

    if (moment1.isValid() && !moment2.isValid()) {
      return moment1;
    }

    if (!moment1.isValid() && moment2.isValid()) {
      return moment2;
    }

    return fn(moment1, moment2, defaultResult);
  }
}

function DatePipeTransform(
  firstDate: moment.Moment | string | Date,
  secondDate: moment.Moment | string | Date,
  defaultResult: any,
  handler: (moment1: moment.Moment, moment2: moment.Moment, defaultResult: any) => any): any {
  return handler(convertToMoment(firstDate), convertToMoment(secondDate), defaultResult);
}
