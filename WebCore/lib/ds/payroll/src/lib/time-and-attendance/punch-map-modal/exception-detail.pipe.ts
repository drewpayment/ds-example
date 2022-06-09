import { Pipe, PipeTransform } from '@angular/core';
import { ClockExceptionEnum } from '@ajs/labor/models/client-exception-detail.model';

@Pipe({
  name: 'hasExceptionDetail'
})
export class HasExceptionDetailPipe implements PipeTransform {
  // add memoization

  transform = hasExceptionDetail;

}

export function hasExceptionDetail(exception: ClockExceptionEnum, clockName: string): string {
  if (ClockExceptionEnum.BadLocation === exception) return 'Punch outside geofence';

  if (ClockExceptionEnum.NoLocation === exception) return 'No location';

  if (ClockExceptionEnum.GoodLocation === exception) return 'Geofence punch';

  if (!!clockName) return clockName;
  return null;
}
