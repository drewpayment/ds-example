import * as moment from 'moment';
/**
 * Takes a date as a javascript Date object, .NET date string, or a Moment and returns
 * an equivalent Moment instance.  Returns an invalid @type moment.Moment when the provided
 * date is null or undefined
 * @param date The date to convert
 */
export function convertToMoment(date: string | Date | moment.Moment): moment.Moment {
    if (typeof date === 'string') {
      return moment(date, 'YYYY-MM-DDTHH:mm:ss');
  }
  if(date === undefined){
    date = null;
  }
    return moment(date);
  }
