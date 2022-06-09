import { IEmployeeSearchSetting } from '@ajs/employee/search/shared/models';
import { Injectable } from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { EmployeeApiService } from '@ds/core/employees';
import { IEmployeeAvatars } from '@ds/core/employees/shared/employee-avatars.model';
import { UserInfo } from '@ds/core/shared';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';
import { SortDirection } from '@ds/core/shared/sort-direction.enum';
import { UpdateLastEmployee } from '@ds/core/users/store/user.actions';
import { getUser, UserState } from '@ds/core/users/store/user.reducer';
import { IEmployeeSearchFilter } from '@ds/employees/search/shared/models/employee-search-filter';
import { IEmployeeSearchFilterOption } from '@ds/employees/search/shared/models/employee-search-filter-option';
import { EmployeeSearchOptions, EmployeeSearchOptionsUpdate } from '@ds/employees/search/shared/models/employee-search-options';
import {
  IEmployeeSearchResult,
  IEmployeeSearchResultResponseData,
} from '@ds/employees/search/shared/models/employee-search-result';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { combineLatest, EMPTY, forkJoin, NEVER, Observable, of, throwError } from 'rxjs';
import {
  catchError,
  map,
  merge,
  mergeMap,
  switchMap,
  take,
  tap,
  withLatestFrom,
} from 'rxjs/operators';
import { DbStoreService } from '../db-store/db-store.service';
import {
  EmployeeAction,
  EmployeeActionTypes,
  GetDefaultSearchSettings,
  GetDefaultSearchSettingsFail,
  GetEmployeeSearch,
  GetEmployeeSearchFail,
  GetEmployeeSearchSuccess,
  GetSearchFiltersFail,
  GetSearchFiltersSuccess,
  GoToFail,
  SetEmployee,
  SetEmployeeFail,
  UpdateSearchOptions,
  UpdateSearchOptionsFail,
  UpdateSearchOptionsSuccess,
  UpdateState,
} from './actions';
import { EmployeeState, getEmployeeState } from './reducer';

@Injectable()
export class EmployeeEffects {
  setEmployee$ = createEffect(() =>
    this.actions$.pipe(
      ofType<SetEmployee>(EmployeeActionTypes.SetEmployee),
      mergeMap((action) =>
        this.service
          .updateLastEmployeeFromEmployeeSearchResult(action.payload)
          .pipe(
            map(() => action.payload),
            withLatestFrom(
              this.store.select(
                getEmployeeState((x) => x)
              ) as Observable<EmployeeState>
            ),
            switchMap(
              ([emp, state]: [IEmployeeSearchResult, EmployeeState]) => {
                let options = {
                  ...state.searchSettings,
                  employeeId: emp.employeeId,
                };
                const pageOpts = { page: 1, pageSize: 50 };

                if (action.fromSearch) {
                  options = { ...options, ...pageOpts, searchText: '' };
                }

                return this.service.search(options).pipe(
                  switchMap((searchResult) => {
                    return forkJoin([
                      of(searchResult),
                      this.service.getEmployeeAvatar(
                        searchResult.nav.current.employeeId
                      ),
                    ]);
                  })
                );
              }
            ),
            tap(
              ([result, avatar]: [
                IEmployeeSearchResultResponseData,
                IEmployeeAvatars
              ]) =>
                this.userStore.dispatch(
                  new UpdateLastEmployee({
                    ...result.nav.current,
                    profileImage: {...result.nav.current.profileImage, _employeeAvatar: avatar},
                  })
                )
            ),
            map(
              ([result, avatar]: [
                IEmployeeSearchResultResponseData,
                IEmployeeAvatars
              ]) =>
                new UpdateState(
                  {
                    searchResults: result,
                    selectedEmployee: {
                      ...result.nav.current,
                      profileImage: {...result.nav.current.profileImage, _employeeAvatar: avatar},
                    },
                  } as EmployeeState,
                  true
                )
            ),
            catchError((err) => of(new SetEmployeeFail(err)))
          )
      )
    )
  );

  getSearchFilters$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeActionTypes.GetSearchFilters),
      mergeMap(() =>
        this.service.getEmployeeSearchFilters().pipe(
          map(
            (data: IEmployeeSearchFilter[]) => new GetSearchFiltersSuccess(data)
          ),
          catchError((err) => of(new GetSearchFiltersFail(err)))
        )
      )
    )
  );

  getEmployeeSearch$ = createEffect(() =>
    this.actions$.pipe(
      ofType<GetEmployeeSearch>(EmployeeActionTypes.GetEmployeeSearch),
      switchMap((act) => {
        let opts = act.search as EmployeeSearchOptions;

        if (act.getResults) {
          opts = { ...opts, page: 1, pageSize: 50 } as EmployeeSearchOptions;
        }

        return forkJoin([
          of(opts),
          this.userStore.select(getUser).pipe(
            take(1),
            switchMap((user) =>
              !user.lastEmployeeId
                ? throwError({
                    redirectToEmployeeList: true,
                    message: 'No employee selected!',
                  })
                : of(user)
            )
          ),
        ]);
      }),
      catchError((err) => {
        if (err && err.redirectToEmployeeList) {
          this.account
            .getSiteConfig(ConfigUrlType.Payroll)
            .subscribe((config) => {
              const url = window.location.href;
              // unless if the current url is onboarding redirect to employee-picker
              if(url.toLowerCase().indexOf('/onboarding/') < 0)
                window.location.href = `${config.url}/ChangeEmployee.aspx?URL=${url}`;
            });
        }
        return NEVER;
      }),
      mergeMap(([options, user]: [EmployeeSearchOptions, UserInfo]) =>
        this.service
          .search({ ...options, employeeId: user.lastEmployeeId })
          .pipe(
            map((data: any) => new GetEmployeeSearchSuccess(data)),
            catchError((err) => of(new GetEmployeeSearchFail(err)))
          )
      )
    )
  );

  updateSearchOptions$ = createEffect(() =>
    this.actions$.pipe(
      ofType<UpdateSearchOptions>(EmployeeActionTypes.UpdateSearchOptions),
      mergeMap((action) =>
        this.service.setSearchSettings(action.payload).pipe(
          withLatestFrom(this.store.select(getEmployeeState(x => x.searchSettings))),
          map(
            ([data, searchSettings]: [IEmployeeSearchSetting, IEmployeeSearchSetting]) => {
              const payloadKeys = Object.keys(action.payload);
              const filterKeys = payloadKeys.filter((x: any) => !isNaN(x)) as unknown as number[];
              let dialogFilters = {};

              if (filterKeys && filterKeys.length) {
                filterKeys.forEach(key => {
                  dialogFilters[key] = action.payload[key];
                });
              } else {
                dialogFilters = null;
              }

              let result: EmployeeSearchOptionsUpdate = {
                ...searchSettings,
                ...data,
              } as EmployeeSearchOptionsUpdate;

              if (typeof dialogFilters === 'object' && dialogFilters !== null) {
                let hasChanges = false;
                searchSettings.filters.forEach(f => {
                  const selectedFilterValue = dialogFilters[f.filterType];

                  if (selectedFilterValue != null) {
                    f.$selected = {...f.$selected,
                      id: selectedFilterValue,
                    };
                    const fo = f.filterOptions.find(x => x.id == selectedFilterValue) ||
                        {name: ''} as IEmployeeSearchFilterOption;
                    f.$selected.name = fo.name;
                    hasChanges = true;
                  }
                });

                if (hasChanges)
                  result = {...result, filters: searchSettings.filters};
              }

              const cache = {
                userId: data.userId,
                isActiveOnly: result.isActiveOnly,
                isExcludeTemps: result.isExcludeTemps,
                sortDirection: result.sortOrder as unknown as SortDirection,
                filters: result.filters,
              } as EmployeeSearchOptions;
              this.indexStore.insertOrUpdate(cache, data.userId, data.clientId).subscribe();

              return new UpdateSearchOptionsSuccess(result);
            }
          ),
          catchError((err) => of(new UpdateSearchOptionsFail(err)))
        )
      )
    )
  );

  getDefaultSearchSettings$ = createEffect(() =>
    this.actions$.pipe(
      ofType<GetDefaultSearchSettings>(
        EmployeeActionTypes.GetDefaultSearchSettings
      ),
      mergeMap((action) =>
        forkJoin([
          this.service.getSearchSettings(),
          this.service.getEmployeeSearchFilters(),
          this.indexStore.getAll(),
        ]).pipe(
          map(
            ([data, filters, indexData]: [
              IEmployeeSearchSetting,
              IEmployeeSearchFilter[],
              any
            ]) => {
              const result: EmployeeSearchOptionsUpdate = {...data, filters} as EmployeeSearchOptionsUpdate;
              const hasIndexKey = indexData != null && (Object.keys(indexData).find(x => (x as any) == data.clientId) as any) > -1;
              const cached = hasIndexKey ? indexData[data.clientId] : null;

              if (typeof cached === 'object' && cached !== null) {
                if (cached.isActiveOnly !== null)
                  result.isActiveOnly = cached.isActiveOnly;

                if (cached.isExcludeTemps !== null)
                  result.isExcludeTemps = cached.isExcludeTemps;

                if (cached.sortOrder !== null)
                  result.sortOrder = cached.sortOrder;

                if (cached.filters && cached.filters.length) {
                  result.filters.forEach(filter => {
                    const cf = cached.filters[filter.filterType];

                    if (cf != null && cf > -1) {
                      const filterOption = filter.filterOptions.find(x => x.id == cf);

                      if (filterOption) {
                        filter.$selected = {
                          filterType: filterOption.filterType,
                          id: cf,
                          name: filterOption.name,
                          parentOption: null,
                        };
                      }
                    }
                  });
                }
              }

              return new UpdateSearchOptionsSuccess(result);
            }
          ),
          catchError((err) => of(new GetDefaultSearchSettingsFail(err)))
        )
      )
    )
  );

  goToPreviousEmployee$ = createEffect(() =>
    this.actions$.pipe(
      ofType<EmployeeAction>(EmployeeActionTypes.GoToPreviousEmployee),
      mergeMap(() =>
        this.store
          .select(getEmployeeState((x) => x.searchResults))
          .pipe(take(1))
          .pipe(
            switchMap((result: IEmployeeSearchResultResponseData) =>
              this.service
                .updateLastEmployeeFromEmployeeSearchResult(
                  result.nav.prev as any
                )
                .pipe(map(() => result.nav.prev))
            ),
            withLatestFrom(
              this.store.select(
                getEmployeeState((x) => x.searchSettings)
              ) as Observable<EmployeeSearchOptions>
            ),
            switchMap(
              ([emp, search]: [IEmployeeSearchResult, EmployeeSearchOptions]) =>
                this.service.search({ ...search, employeeId: emp.employeeId })
            ),
            tap((result) =>
              this.userStore.dispatch(
                new UpdateLastEmployee(result.nav.current)
              )
            ),
            map(
              (result) =>
                new UpdateState(
                  {
                    searchResults: result,
                    selectedEmployee: result.nav.current,
                  } as EmployeeState,
                  true
                )
            ),
            catchError((err) => of(new GoToFail(err)))
          )
      )
    )
  );

  goToNextEmployee$ = createEffect(() =>
    this.actions$.pipe(
      ofType<EmployeeAction>(EmployeeActionTypes.GoToNextEmployee),
      mergeMap(() =>
        this.store
          .select(getEmployeeState((x) => x.searchResults))
          .pipe(take(1))
          .pipe(
            switchMap((result: IEmployeeSearchResultResponseData) =>
              this.service
                .updateLastEmployeeFromEmployeeSearchResult(
                  result.nav.next as any
                )
                .pipe(map(() => result.nav.next))
            ),
            withLatestFrom(
              this.store.select(getEmployeeState((x) => x.searchSettings))
            ),
            switchMap(
              ([emp, search]: [IEmployeeSearchResult, EmployeeSearchOptions]) =>
                this.service.search({ ...search, employeeId: emp.employeeId })
            ),
            tap((result) =>
              this.userStore.dispatch(
                new UpdateLastEmployee(result.nav.current)
              )
            ),
            map(
              (result) =>
                new UpdateState(
                  {
                    searchResults: result,
                    selectedEmployee: result.nav.current,
                  } as EmployeeState,
                  true
                )
            ),
            catchError((err) => of(new GoToFail(err)))
          )
      )
    )
  );

  constructor(
    private store: Store<EmployeeState>,
    private actions$: Actions,
    private service: EmployeeApiService,
    private account: AccountService,
    private userStore: Store<UserState>,
    private indexStore: DbStoreService,
  ) {}
}
