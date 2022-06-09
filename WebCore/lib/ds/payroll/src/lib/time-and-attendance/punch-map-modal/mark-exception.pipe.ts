import { Pipe, PipeTransform } from '@angular/core';
import { ClockExceptionEnum } from '@ajs/labor/models/client-exception-detail.model';

@Pipe({
  name: 'isException'
})
export class IsExceptionPipe implements PipeTransform {
// add memoization

  transform = isException;

}

export function isException(exception: ClockExceptionEnum): any {
  if (ClockExceptionEnum.BadLocation === exception) return 'warning';

  if (ClockExceptionEnum.NoLocation === exception) return 'not_listed_location';

  if (ClockExceptionEnum.GoodLocation === exception) return 'where_to_vote';
}
