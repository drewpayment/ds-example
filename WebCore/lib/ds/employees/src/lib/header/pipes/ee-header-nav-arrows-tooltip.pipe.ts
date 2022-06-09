import { Pipe, PipeTransform } from '@angular/core';
import { IEmployeeSearchResultResponseData } from '@ds/employees/search/shared/models/employee-search-result';


@Pipe({
  name: 'eeSearchResultToName',
})
export class EmployeeHeaderNavArrowsTooltipPipe implements PipeTransform {

  transform(value: IEmployeeSearchResultResponseData, ...args: ('current'|'next'|'prev'|'first'|'last')[]) {
    if (!value || !value.nav) return '';
    const navKey = args && args.length ? args[0] : 'current';

    const target = value.nav[navKey];
    if (!target) return '';

    return `${target.lastName}, ${target.firstName}`;
  }

}
