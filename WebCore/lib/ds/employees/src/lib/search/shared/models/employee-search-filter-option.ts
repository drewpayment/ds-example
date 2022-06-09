import { EmployeeSearchFilterType } from "./employee-search-filter-type";

export interface IEmployeeSearchFilterOption {
    filterType: EmployeeSearchFilterType;
    id:        number;
    name:      string;
    parentOption?: IEmployeeSearchFilterOption;
}
