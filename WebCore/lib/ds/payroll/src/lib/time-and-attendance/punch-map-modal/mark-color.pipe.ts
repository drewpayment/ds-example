import { Pipe, PipeTransform } from '@angular/core';
import { ClockExceptionEnum } from '@ajs/labor/models/client-exception-detail.model';

@Pipe({
  name: 'hasExceptionColor'
})
export class HasExceptionColorPipe implements PipeTransform {
// add memoization

  transform = hasExceptionColor;

}

export function hasExceptionColor(exception: ClockExceptionEnum): any {
  if (ClockExceptionEnum.BadLocation === exception) return 'text-danger';

  if (ClockExceptionEnum.NoLocation === exception) return 'text-warning';

  if (ClockExceptionEnum.GoodLocation === exception) return 'text-success';
}
