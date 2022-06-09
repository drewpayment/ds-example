import { EmployeeSearchOptions } from "@ajs/employee/search/shared/models";
import { Moment } from 'moment';

export const REVIEW_SEARCH_OPTIONS_KEY = "review-search-options";

export interface IReviewSearchOptions extends EmployeeSearchOptions {
    clientId?:number,
    reviewTemplateId?:number,
    startDate?: string | Date | Moment,
    endDate?: string | Date | Moment
}

export interface IReviewStatusSearchOptions extends IReviewSearchOptions {
    includeScores?: boolean,
}