import { IProfileImageDto } from '@ds/core/resources/shared/profile-image.model';
import { IEmployeeAvatars } from '@ds/core/employees/shared/employee-avatars.model';
import {
  IEmployeeImage,
  IImage,
} from '@ds/core/resources/shared/employee-image.model';
import { IEmployeeSearchFilterOption } from '@ds/employees/search/shared/models/employee-search-filter-option';
import { EmployeeSearchFilterType } from '@ds/employees/search/shared/models/employee-search-filter-type';
import {
  EmployeeSearchOptions,
  EmployeeSearchOptionsUpdate,
} from '@ds/employees/search/shared/models/employee-search-options';
import {
  IEmployeeSearchResult,
  IEmployeeSearchResultResponseData,
} from '@ds/employees/search/shared/models/employee-search-result';
import { createSelector } from '@ngrx/store';
import { EmployeeAction, EmployeeActionTypes } from './actions';

export const employeeSearchStoreKey = '__employeeSearch__';
export interface EmployeeState {
  selectedEmployee: IEmployeeSearchResult;
  searchResults: IEmployeeSearchResultResponseData;
  searchSettings: EmployeeSearchOptions;
  loading: boolean;
  error: string | any;
}

const initialState: EmployeeState = {
  selectedEmployee: null,
  searchResults: null,
  searchSettings: {
    isActiveOnly: true,
    isExcludeTemps: false,
    filters: [],
  } as EmployeeSearchOptions,
  loading: true,
  error: '',
};

export const employeeState = (state) => state[employeeSearchStoreKey];
export function getEmployeeState<T>(fn: (state: EmployeeState) => {}): T {
  return createSelector(employeeState, fn) as unknown as T;
}
export function getSearchOptions<EmployeeSearchOptions>(): EmployeeSearchOptions {
  return createSelector(employeeState, x => x.searchSettings) as unknown as EmployeeSearchOptions;
}
export function getEmployeeFilterGroup(
  type: EmployeeSearchFilterType
): IEmployeeSearchFilterOption {
  return createSelector(employeeState, (state: EmployeeState) =>
    state.selectedEmployee.groups.find((g) => g.filterType === type)
  ) as unknown as IEmployeeSearchFilterOption;
}

//#region Utilities

export function mergeSearchSettingIntoSearchOptions(
  req: EmployeeSearchOptionsUpdate,
  state: EmployeeSearchOptions
): EmployeeSearchOptions {
  const result: EmployeeSearchOptions = {
    ...state,
    filters: state.filters.map((filter) => {
      if (filter.$selected) {
        const selected =
          filter.filterOptions.find(
            (x) =>
              x.filterType === filter.$selected.filterType &&
              x.id === filter.$selected.id
          ) || ({ name: '' } as IEmployeeSearchFilterOption);
        filter.$selected.name = selected.name;
      }
      return filter;
    }),
  };

  result.isActiveOnly = req.isActiveOnly;
  result.isExcludeTemps = req.isExcludeTemps;

  return result;
}

export function mergeFullState(
  current: EmployeeState,
  newState: EmployeeState,
  updateSearchResults: boolean
): EmployeeState {
  let result = { ...current } as EmployeeState;

  if (newState.error != null) result.error = newState.error;
  result.searchSettings = {
    ...current.searchSettings,
    ...newState.searchSettings,
  };
  result.selectedEmployee = {
    ...current.selectedEmployee,
    ...newState.selectedEmployee,
  };

  if (updateSearchResults) result.searchResults = newState.searchResults;

  result.loading = false;

  return result;
}

//#endregion

export function EmployeeReducer(
  state: EmployeeState = initialState,
  action: EmployeeAction
) {
  switch (action.type) {
    case EmployeeActionTypes.UpdateState:
      return mergeFullState(state, action.payload, action.updateSearchResults);

    case EmployeeActionTypes.SetEmployee:
      return { ...state, loading: true };
    case EmployeeActionTypes.SetEmployeeFail:
      return { ...state, loading: false };
    case EmployeeActionTypes.SetEmployeeProfileImage:
      return {
        ...state,
        selectedEmployee: {
          ...state.selectedEmployee,
          profileImage: {
            ...state.selectedEmployee.profileImage,
            extraLarge: {
              ...state.selectedEmployee.profileImage.extraLarge,
              ...action.employeeImage.extraLarge,
            } as IImage,
            profileImageInfo:
              action.employeeImage.profileImageInfo &&
              action.employeeImage.profileImageInfo.length
                ? ([
                    {
                      ...state.selectedEmployee.profileImage
                        .profileImageInfo[0],
                      ...action.employeeImage.profileImageInfo[0],
                    },
                  ] as IProfileImageDto[])
                : [],
            _employeeAvatar: {
              ...state.selectedEmployee.employeeAvatar,
              ...action.employeeImage._employeeAvatar,
            } as IEmployeeAvatars,
          } as IEmployeeImage,
          employeeAvatar: {
            ...state.selectedEmployee.employeeAvatar,
            ...action.employeeImage._employeeAvatar,
          } as IEmployeeAvatars,
        } as IEmployeeSearchResult,
      } as EmployeeState;

    case EmployeeActionTypes.GetEmployeeSearch:
      return { ...state, loading: false };
    case EmployeeActionTypes.GetEmployeeSearchSuccess:
      return {
        ...state,
        searchResults: action.payload,
        selectedEmployee: action.payload.nav.current,
        loading: false,
      };
    case EmployeeActionTypes.GetEmployeeSearchFail:
      return { ...state, error: action.payload, loading: false };

    case EmployeeActionTypes.GetSearchFilters:
      return { ...state, loading: true };
    case EmployeeActionTypes.GetSearchFiltersSuccess:
      return {
        ...state,
        searchSettings: { ...state.searchSettings, filters: action.payload },
        loading: false,
      };
    case EmployeeActionTypes.GetSearchFiltersFail:
      return { ...state, error: action.payload, loading: false };

    case EmployeeActionTypes.UpdateSearchOptionsSuccess:
      return {
        ...state,
        searchSettings: { ...state.searchSettings, ...action.payload },
        // searchSettings: mergeSearchSettingIntoSearchOptions(
        //   action.payload,
        //   state.searchSettings
        // ),
        loading: false,
      };

    case EmployeeActionTypes.GoToPreviousEmployee:
    case EmployeeActionTypes.GoToNextEmployee:
      return { ...state, loading: true };

    default:
      return state;
  }
}
