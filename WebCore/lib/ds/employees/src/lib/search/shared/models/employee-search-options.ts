import { SearchSortByType } from "@ajs/employee/search/shared/models";
import { SortDirection } from "@ds/core/shared/sort-direction.enum";
import { IEmployeeSearchFilter } from "./employee-search-filter";
import { IEmployeeSearchFilterOption } from "./employee-search-filter-option";

export interface EmployeeSearchOptions {
  employeeId?: number;
  page?: number;
  pageSize?: number;
  searchText?: string;
  sortBy?: string;
  sortDirection?: SortDirection;
  withSupervisors?: boolean;
  isActiveOnly?: boolean;
  isExcludeTemps?: boolean;
  filters?: IEmployeeSearchFilter[];
  selected?: number[];
}

export interface EmployeeSearchOptionsUpdate {
  isActiveOnly: boolean;
  isExcludeTemps: boolean;
  filters: IEmployeeSearchFilter[];
  sortOrder?: SearchSortByType;
}
