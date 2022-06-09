import { IJobDetailData } from 'apps/ds-company/src/app/models/job-profile.model';
import { Pipe, PipeTransform } from '@angular/core';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';

@Pipe({
  name: 'filterOnboardingWorkflowTasks',
  pure: true
})
export class FilterOnboardingWorkflowTasksPipe implements PipeTransform {

  transform(items: any[], filters?:{adminMustSelect?: boolean, isNullclient?: boolean}): any {
    if(!items){
      return [];
    }
    
    return items.filter(item => {
        return (item.adminMustSelect == filters.adminMustSelect) && ((filters.isNullclient && item.clientId == null) || (!filters.isNullclient && item.clientId != null));
    });
  }
}