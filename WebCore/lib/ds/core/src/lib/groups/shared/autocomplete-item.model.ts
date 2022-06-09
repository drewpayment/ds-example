import { IReviewTemplate } from './review-template.model';

export interface AutocompleteItem {
    display: string;
    value: any;
}

export interface AutocompleteItemWithRT extends AutocompleteItem {
template: IReviewTemplate
}