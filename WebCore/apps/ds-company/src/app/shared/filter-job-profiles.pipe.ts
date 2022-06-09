import { IJobDetailData } from 'apps/ds-company/src/app/models/job-profile.model';
import { Pipe, PipeTransform } from '@angular/core';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';

@Pipe({
  name: 'filterJobProfiles',
  pure: true
})
export class FilterJobProfilesPipe implements PipeTransform {

  transform(items: IJobDetailData[], filters?:{excludeInactiveJobProfiles?: boolean, searchText?: string}): any {
    if(!items){
      return [];
    }
    
    return items.filter(item => {
        return (!filters.excludeInactiveJobProfiles || item.isActive == true) && (!filters.searchText || item.description.toLocaleLowerCase().indexOf(filters.searchText.toLocaleLowerCase()) != -1);
    });
  }
}