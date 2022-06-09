import { Pipe, PipeTransform } from '@angular/core';
import { Maybe } from '@ds/core/shared/Maybe';

@Pipe({
  name: 'getCellTitle'
})
export class GetCellTitlePipe implements PipeTransform {

  transform(groupId: number, titles: {[id: number]: string[] | {value1: any, value2: any}[]}, headerIndex: number): any {
    return new Maybe(titles)
    .map(x => x[groupId])
    .map(x => x[headerIndex])
    .value();
  }

}
