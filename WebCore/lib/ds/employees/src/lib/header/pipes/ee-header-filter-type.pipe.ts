import { Pipe, PipeTransform } from '@angular/core';
import { IEmployeeSearchResult } from '@ds/employees/search/shared/models/employee-search-result';

@Pipe({
  name: 'filterTypeId',
})
export class EmployeeHeaderFilterTypePipe implements PipeTransform {

  transform(value: IEmployeeSearchResult, ...args: number[]) {
    if (!value || !value.groups || !value.groups.length || !args || !args.length || args[0] < 0 ) return ''; 
    const filterType = args[0];
    const group = value.groups.find(g => g.filterType === filterType);
    if (!group) return '';
    return group.name;
  }

}