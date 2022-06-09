import { EmployeeSearchFilterType } from '@ajs/employee/search/shared/models/employee-search-filter-type';

export enum SpecialFilter {
    ActiveOnly = 1,
    ExcludeTemps = 2
}
export class EmployeeFilter {
    filterName: string;
    specialFilterType: SpecialFilter;
    employeeFilterType: EmployeeSearchFilterType;
}