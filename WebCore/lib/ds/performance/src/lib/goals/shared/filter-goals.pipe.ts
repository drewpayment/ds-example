import { Pipe, PipeTransform } from '@angular/core';
import { IGoal } from '..';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';
import { CompletionStatusType } from '@ds/core/shared';

@Pipe({
  name: 'filterGoals',
  pure: false
})
export class FilterGoalsPipe implements PipeTransform {

  transform(items: IGoal[], status?: CompletionStatusType): any {
    if(!items){
      return [];
    }
    if(status == null) {
      return items;
    }

    return items.filter(item => {
      if (status === CompletionStatusType.NotStarted)
          return item.completionStatus === status || item.completionStatus == null;
      return item.completionStatus === status;
    }).sort((a, b) => {
      const dueDateA = convertToMoment(a.dueDate).valueOf();
      const dueDateB = convertToMoment(b.dueDate).valueOf();
      const titleA = a.title || '';
      const titleB = b.title || '';

      return dueDateA - dueDateB || b.priority - a.priority || titleA.toLowerCase().localeCompare(titleB.toLowerCase());
    });
  }

}
