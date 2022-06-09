import { Pipe, PipeTransform } from '@angular/core';
import { Maybe } from '@ds/core/shared/Maybe';
import { treatHardCodedRangeAsCalendarYear } from './treat-hardcoded-date-range-as-calendar-year.fn';
import { IReviewTemplate } from './review-template.model';

/**
 * A model with a function to get a `IReviewTemplate` from whatever object is assigned to `model`
 */
export interface SomethingWithReviewTemplate<T> {
    getTemplate: () => IReviewTemplate;
    model: T
}

export function sortTemplates(templates: IReviewTemplate[]): IReviewTemplate[] {
    return baseSortTemplates(templates.map(x => ({ getTemplate: () => x, model: x } as SomethingWithReviewTemplate<IReviewTemplate>))).map(x => x.getTemplate())
}

export function baseSortTemplates<T>(templates: SomethingWithReviewTemplate<T>[]): SomethingWithReviewTemplate<T>[] {
    return templates.sort((a, b) => {
        const safeA = new Maybe(a).map(x => x.getTemplate());
        const safeB = new Maybe(b).map(x => x.getTemplate());

        const getTypeFromMaybe = (val: Maybe<IReviewTemplate>) => val.map(x => x.isRecurring).map(x => x === true ? 1 : 0).valueOr(0);
        const getNameFromMaybe = (val: Maybe<IReviewTemplate>) => val.map(x => x.name).map(x => x.toLowerCase().replace(/\s/g, '')).valueOr('')

        const typeA = getTypeFromMaybe(safeA);
        const typeB = getTypeFromMaybe(safeB);
        const nameA = getNameFromMaybe(safeA);
        const nameB = getNameFromMaybe(safeB);

        return typeB - typeA || nameA.localeCompare(nameB);
    });
}

@Pipe({name: 'sortTemplates'})
export class SortTemplatesPipe implements PipeTransform {
    transform = sortTemplates
}

