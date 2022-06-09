import { Pipe, PipeTransform } from '@angular/core';
import { IOnboardingWorkflowTask } from '@models';

@Pipe({
  name: 'filterCustomPages',
  pure: true,
})
export class FilterCustomPagesPipe implements PipeTransform {
  transform(
    items: IOnboardingWorkflowTask[],
    filters?: {
      type?: string;
      excludeInactiveTasks?: boolean;
      searchText?: string;
    }
  ): any {
    if (!items) {
      return [];
    }

    return items.filter((item) => {
      if (!filters.type)
        return (
          (item.route1 === 'Document' ||
            item.route1 === 'Link' ||
            item.route1 === 'Video') &&
          (!filters.excludeInactiveTasks || item.isDeleted != true) &&
          (!filters.searchText ||
            item.workflowTitle
              .toLocaleLowerCase()
              .indexOf(filters.searchText.toLocaleLowerCase()) != -1)
        );

      return (
        item.route1 === filters.type &&
        (!filters.excludeInactiveTasks || item.isDeleted != true) &&
        (!filters.searchText ||
          item.workflowTitle
            .toLocaleLowerCase()
            .indexOf(filters.searchText.toLocaleLowerCase()) != -1)
      );
    });
  }
}
