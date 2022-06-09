import { EmployeeSearchFilterType } from "./employee-search-filter-type";
import { IEmployeeSearchFilterOption } from "./employee-search-filter-option";

export interface IEmployeeSearchFilter {
    filterType:    EmployeeSearchFilterType;
    description:   string;
    $selected?:    IEmployeeSearchFilterOption;
    filterOptions: IEmployeeSearchFilterOption[];
}
