import {
  IEmployeeSearchResultResponseData,
  IEmployeeSearchSetting,
} from "@ajs/employee/search/shared/models";
import { IEmployeeImage } from "@ds/core/resources/shared/employee-image.model";
import { IEmployee } from "@ds/core/resources/shared/employee.model";
import { IEmployeeSearchFilter } from "@ds/employees/search/shared/models/employee-search-filter";
import {
  EmployeeSearchOptions,
  EmployeeSearchOptionsUpdate,
} from "@ds/employees/search/shared/models/employee-search-options";
import { IEmployeeSearchResult } from "@ds/employees/search/shared/models/employee-search-result";
import { Action } from "@ngrx/store";
import { EmployeeState } from './reducer';

export enum EmployeeActionTypes {
  SetEmployee = "[Employee] Add Employee",
  SetEmployeeProfileImage = "[Employee] Update Employee Image",
  SetEmployeeSuccess = "[Employee] Set Employee Success",
  SetEmployeeFail = "[Employee] Set Employee Fail",

  GetEmployeeSearch = "[Employee] Get Employee Search",
  GetEmployeeSearchSuccess = "[Employee] Get Employee Search Success",
  GetEmployeeSearchFail = "[Employee] Get Employee Search Fail",

  GetSearchFilters = "[Search Options] Get Search Options",
  GetSearchFiltersFail = "[Search Options] Get Search Options Fail",
  GetSearchFiltersSuccess = "[Search Options] Get Search Options Success",
  UpdateSearchOptions = "[Search Options] Update",
  UpdateSearchOptionsSuccess = "[Search Options] Update Success",
  UpdateSearchOptionsFail = "[Search Options] Update Fail",
  GetDefaultSearchSettings = "[Search Options] Get default search settings",
  GetDefaultSearchSettingsSuccess = "[Search Options] Get Success",
  GetDefaultSearchSettingsFail = "[Search Options] Get fail",

  GoToPreviousEmployee = "[EE Hdr Nav] Go to previous employee",
  GoToNextEmployee = "[EE Hdr Nav] Go to next employee",
  GoToFail = "[EE Hdr Nav] Failed to navigate to employee",
  SetPreviousOrNextEmployee = '[EE Hdr Nav] Setting the employee header nav selections',
  UpdateState = '[EE Hdr Nav] Update entire state',
}

export class UpdateState implements Action {
  readonly type = EmployeeActionTypes.UpdateState;
  constructor(public payload: EmployeeState, public updateSearchResults: boolean = false) {}
}

export class SetEmployee implements Action {
  readonly type = EmployeeActionTypes.SetEmployee;
  constructor(public payload: IEmployeeSearchResult, public fromSearch: boolean = false) {}
}
export class SetEmployeeProfileImage implements Action {
  readonly type = EmployeeActionTypes.SetEmployeeProfileImage;
  constructor(public employeeImage: IEmployeeImage) {}
}

export class SetEmployeeSuccess implements Action {
  readonly type = EmployeeActionTypes.SetEmployeeSuccess;
  constructor(public payload: IEmployeeSearchResult) {}
}

export class SetEmployeeFail implements Action {
  readonly type = EmployeeActionTypes.SetEmployeeFail;
  constructor(public err: any) {}
}

export class GetEmployeeSearch implements Action {
  readonly type = EmployeeActionTypes.GetEmployeeSearch;
  constructor(public getResults: boolean, public search?: EmployeeSearchOptions) {}
}

export class GetEmployeeSearchSuccess implements Action {
  readonly type = EmployeeActionTypes.GetEmployeeSearchSuccess;
  constructor(public payload: IEmployeeSearchResultResponseData) {}
}

export class GetEmployeeSearchFail implements Action {
  readonly type = EmployeeActionTypes.GetEmployeeSearchFail;
  constructor(public payload: any) {}
}

export class UpdateSearchOptions implements Action {
  readonly type = EmployeeActionTypes.UpdateSearchOptions;
  constructor(public payload: IEmployeeSearchSetting) {}
}

export class UpdateSearchOptionsSuccess implements Action {
  readonly type = EmployeeActionTypes.UpdateSearchOptionsSuccess;
  constructor(public payload: EmployeeSearchOptionsUpdate) {}
}

export class UpdateSearchOptionsFail implements Action {
  readonly type = EmployeeActionTypes.UpdateSearchOptionsFail;
  constructor(public err: any) {}
}

export class GetDefaultSearchSettings implements Action {
  readonly type = EmployeeActionTypes.GetDefaultSearchSettings;
}

export class GetDefaultSearchSettingsFail implements Action {
  readonly type = EmployeeActionTypes.GetDefaultSearchSettingsFail;
  constructor(public err: any) {}
}

export class GetSearchFilters implements Action {
  readonly type = EmployeeActionTypes.GetSearchFilters;
}

export class GetSearchFiltersSuccess implements Action {
  readonly type = EmployeeActionTypes.GetSearchFiltersSuccess;
  constructor(public payload: IEmployeeSearchFilter[]) {}
}

export class GetSearchFiltersFail implements Action {
  readonly type = EmployeeActionTypes.GetSearchFiltersFail;
  constructor(public payload: any) {}
}

export class GoToPreviousEmployee implements Action {
  readonly type = EmployeeActionTypes.GoToPreviousEmployee;
}

export class GoToNextEmployee implements Action {
  readonly type = EmployeeActionTypes.GoToNextEmployee;
}

export class GoToFail implements Action {
  readonly type = EmployeeActionTypes.GoToFail;
  constructor(public payload: any) {}
}

export type EmployeeAction =
  | SetEmployee
  | SetEmployeeProfileImage
  | SetEmployeeSuccess
  | SetEmployeeFail
  | GetEmployeeSearch
  | GetEmployeeSearchSuccess
  | GetEmployeeSearchFail
  | GetSearchFilters
  | GetSearchFiltersFail
  | GetSearchFiltersSuccess
  | UpdateSearchOptionsSuccess
  | UpdateSearchOptionsFail
  | GetDefaultSearchSettings
  | GetDefaultSearchSettingsFail
  | GoToPreviousEmployee
  | GoToNextEmployee
  | GoToFail
  | UpdateState;
