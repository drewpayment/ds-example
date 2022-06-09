import { ReferenceDate } from './schedule-type.enum';

export const treatHardCodedRangeAsCalendarYear = (value: number) => {
    if(value === ReferenceDate.HardCodedRange){
      return ReferenceDate.CalendarYear;
  } else {
    return value;
  }
  }