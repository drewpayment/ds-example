import { IEmployeeSearchFilter } from '@ds/employees/search/shared/models/employee-search-filter';
import { EmployeeSearchOptions } from '@ds/employees/search/shared/models/employee-search-options';

export interface ISearchFilterDialogData {
    options: EmployeeSearchOptions;
    filters?: IEmployeeSearchFilter[];
}
