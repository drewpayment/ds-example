import { IContact } from "./contact.model";

export interface IContactSearchResult {
    results: IContact[];
    totalCount: number;
    filterCount: number;
    pageCount?: number;
    page?: number;
    pageSize?: number;
}