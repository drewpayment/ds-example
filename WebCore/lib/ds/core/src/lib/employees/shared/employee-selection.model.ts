import { EmployeeSearchOptions } from "@ajs/employee/search/shared/models";

export interface IEmployeeSelection {
    searchOptions?:EmployeeSearchOptions,
    includeAllSearchResults:boolean,
    exclude?:number[],
    include?:number[]
}