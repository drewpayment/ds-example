import { Pipe, PipeTransform } from '@angular/core';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';
import { ICompanyResource } from '@ds/employees/resources/shared/company-resource.model';

@Pipe({
  name: 'filterResource',
  pure: false
})
export class FilterResourcePipe implements PipeTransform {

  transform(items: ICompanyResource[], filters?:{type?:number, searchText: string}): any {
    if(!items){
      return [];
    }

    if(!filters.type && !filters.searchText) {
      return items;
    }

    return items.filter(item => {
      if(filters.type === 0)
          return (<number>item.resourceTypeId === filters.type || item.resourceTypeId == null) && (!filters.searchText || item.resourceName.toLocaleLowerCase().indexOf(filters.searchText.toLocaleLowerCase()) != -1 );

      return item.resourceTypeId === filters.type && (!filters.searchText || item.resourceName.toLocaleLowerCase().indexOf(filters.searchText.toLocaleLowerCase()) != -1 );
    });
  }
}
