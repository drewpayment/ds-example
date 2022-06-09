import { Pipe, PipeTransform } from '@angular/core';
import { Maybe } from '@ds/core/shared/Maybe';

@Pipe({
  name: 'twoDecimalRound'
})
export class TwoDecimalRoundPipe implements PipeTransform {
// add memoization

  transform = twoDecimalRound;

}

export function twoDecimalRound(GeoPoint: number): any {
  return new Maybe(GeoPoint)
  .map(x => GeoPoint.toFixed(2)).value();
}
