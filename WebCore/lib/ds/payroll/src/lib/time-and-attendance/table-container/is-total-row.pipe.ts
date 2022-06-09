import { Pipe, PipeTransform } from '@angular/core';
import { Maybe } from '@ds/core/shared/Maybe';

@Pipe({
  name: 'isTotalRow'
})
export class IsTotalRowPipe implements PipeTransform {
// add memoization

  transform = isTotalRow;

}

export function isTotalRow(Day: string): any {
  return new Maybe(Day)
  .map(x => x.trim())
  .map(x =>
    'Regular'.localeCompare(x) === 0 ||
    'OverTime'.localeCompare(x) === 0 ||
    'Daily Total'.localeCompare(x) === 0 ||
    'Weekly Total'.localeCompare(x) === 0 ||
    'Grand Total Hours'.localeCompare(x) === 0 ||
    'Holiday'.localeCompare(x) === 0).valueOr(false);
}
