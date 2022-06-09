import { HttpParams } from "@angular/common/http";

export interface IContactSearchOptions {
    page?: number;
    pageSize?: number;
    searchText?: string;
    excludeTimeClockOnly:boolean;
    haveActiveEmployee:boolean;
    ifSupervisorGetSubords:boolean
}

export class ContactSearchOptions implements IContactSearchOptions {
    page?: number;
    pageSize?: number;
    searchText?: string;
    excludeTimeClockOnly:boolean;
    haveActiveEmployee:boolean;
    ifSupervisorGetSubords: boolean;
    
    constructor(options?: IContactSearchOptions) {
        if(options)
            Object.assign(this, options);
    }

    toHttpParams(params?: HttpParams): HttpParams {
        params = params || new HttpParams();

        if (this.pageSize)
            params = params.set('pagesize', this.pageSize.toString());
        if (this.page)
            params = params.set('page', this.page.toString());
        if (this.searchText && typeof this.searchText === "string" && this.searchText.trim())
            params = params.set('search', this.searchText.trim());
        if(this.excludeTimeClockOnly)
        params = params.set('excludeTimeClockOnly', this.excludeTimeClockOnly.toString());
        if(this.haveActiveEmployee)
        params = params.set('haveActiveEmployee', this.haveActiveEmployee.toString());
        if(this.ifSupervisorGetSubords)
        params = params.set('ifSupervisorGetSubords', this.ifSupervisorGetSubords.toString());

        return params;
    }
}