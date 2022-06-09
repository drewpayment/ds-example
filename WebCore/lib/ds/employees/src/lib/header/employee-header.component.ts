import { AddEmployeeModalService } from '@ajs/employee/add-employee/add-modal/add-modal.service';
import { IEmployeeSearchSetting } from '@ajs/employee/search/shared/models';
import { JobProfileModalService } from '@ajs/job-profiles/job-profile-modal/job-profile-modal.service';
import { IJobProfileBasicInfoData } from '@ajs/job-profiles/shared/models/job-profiles-detail-data.interface';
import {
  Component,
  Input,
  OnInit,
  ViewChild,
  ViewEncapsulation,
  Output,  EventEmitter,
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AccountService } from '@ds/core/account.service';
import { IContact } from '@ds/core/contacts';
import {
  EmployeeApiService,
  SearchFilterDialogComponent,
} from '@ds/core/employees';
import { IEmployeeAvatars } from '@ds/core/employees/shared/employee-avatars.model';
import { UserInfo, UserType } from '@ds/core/shared';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';
import { DsContactAutocompleteComponent } from '@ds/core/ui/ds-autocomplete/ds-contact-autocomplete/ds-contact-autocomplete.component';
import { Store } from '@ngrx/store';
import * as moment from 'moment';
import { Observable, Subject, BehaviorSubject, forkJoin, of } from 'rxjs';
import {
  debounceTime,
  map,
  shareReplay,
  startWith,
  switchMap,
  take,
  takeUntil,
  withLatestFrom,
} from 'rxjs/operators';
import { tap } from 'rxjs/operators';
import { JobProfilesService } from '../common/shared/job-profiles.service';
import { IEmployeeSearchFilter } from '../search/shared/models/employee-search-filter';
import { EmployeeSearchFilterType } from '../search/shared/models/employee-search-filter-type';
import { EmployeeSearchOptions, EmployeeSearchOptionsUpdate } from '../search/shared/models/employee-search-options';
import {
  IEmployeeSearchResult,
  IEmployeeSearchResultResponseData,
} from '../search/shared/models/employee-search-result';
import { DbStoreService } from './db-store/db-store.service';
import {
  GetEmployeeSearch,
  GoToNextEmployee,
  GoToPreviousEmployee,
  SetEmployee,
  UpdateSearchOptions,
  UpdateSearchOptionsSuccess,
  EmployeeActionTypes,
} from './ngrx/actions';
import {
  EmployeeState,
  getEmployeeState,
  getSearchOptions,
} from './ngrx/reducer';

export function addEmployeeModalServiceFactory(i: any) {
  return i.get(AddEmployeeModalService.SERVICE_NAME);
}

export const addEmployeeModalServiceProvider = {
  provide: AddEmployeeModalService,
  useFactory: addEmployeeModalServiceFactory,
  deps: ['$injector'],
};

export function jobProfileModalServiceFactory(i: any) {
  return i.get(JobProfileModalService.SERVICE_NAME);
}
export const JobProfileServiceProvider = {
  provide: JobProfileModalService,
  useFactory: jobProfileModalServiceFactory,
  deps: ['$injector'],
};

@Component({
  selector: 'ds-employee-header',
  templateUrl: './employee-header.component.html',
  styleUrls: ['./employee-header.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [addEmployeeModalServiceProvider, JobProfileServiceProvider],
})
export class EmployeeHeaderComponent implements OnInit {
  user: UserInfo;
  @ViewChild('ac', { static: false }) ac: DsContactAutocompleteComponent;
  searchInput = new FormControl();
  filteredEmployees: Observable<IEmployeeSearchResult[]>;
  destroy$ = new Subject();
  isLoading = true;
  searchResults$ = this.store.select(getEmployeeState((x) => x.searchResults));
  selectedEmployee$ = (
    this.store.select(
      getEmployeeState((x) => x.selectedEmployee)
    ) as Observable<IEmployeeSearchResult>
  ).pipe(tap((x) => {
    if(x){
      this.hasJobProfileLink(x);
      this.workHistoryText = this.workHistory(x);
      this.lengthOfServiceText = this.lengthOfService(x);
    }
  }));

  searchOptions$ = (
    this.store.select(
      getEmployeeState((x) => x.searchSettings)
    ) as Observable<EmployeeSearchOptions>
  ).pipe(tap((opts) => this.setHasEnabledFilters(opts)));

  hasEnabledFilters$ = new BehaviorSubject<boolean>(false);

  _hasEnabledFilters = false;
  jobProfileName: string;

  get hasEnabledFilters(): boolean {
    return this._hasEnabledFilters;
  }

  @Input() hideFilter = false;

  @Input() externalInput:IEmployeeSearchResultResponseData ;
  @Output() employeeNavigated = new EventEmitter();
  allowAddEmployee$ = new BehaviorSubject<boolean>(false);
  jobProfileData: IJobProfileBasicInfoData;
  showJobProfileLink: boolean = false;
  workHistoryText: string = '';
  lengthOfServiceText: string = '';

  jobProfilesData$ = this.jobProfileService
    .getJobProfilesBasicInfo()
    .pipe(shareReplay());

  mapEmpResToContact = (emps: IEmployeeSearchResult[]): any[] => {
    if (!emps || !emps.length) return [];

    return emps.map(
      (e) =>
        ({
          firstName: e.firstName,
          lastName: e.lastName,
          employeeId: e.employeeId,
          employeeNumber: e.employeeNumber,
          clientId: e.clientId,
          profileImage:
            e.profileImage != null
              ? {
                  extraLarge:
                    e.profileImage.profileImageInfo &&
                    e.profileImage.profileImageInfo.length
                      ? {
                          url: `${e.profileImage.profileImageInfo[0].source}${e.profileImage.sasToken}`,
                        }
                      : null,
                  employeeAvatar: e.profileImage._employeeAvatar
                    ? ({
                        employeeAvatarId:
                          e.profileImage._employeeAvatar.employeeAvatarId,
                        employeeId: e.employeeId,
                        clientId: e.clientId,
                        avatarColor: e.profileImage._employeeAvatar.avatarColor,
                      } as IEmployeeAvatars)
                    : null,
                }
              : null,
          extraLarge:
            e.profileImage.profileImageInfo &&
            e.profileImage.profileImageInfo.length
              ? {
                  hasImage: true,
                  url: `${e.profileImage.profileImageInfo[0].source}${e.profileImage.sasToken}`,
                }
              : null,
        } as any)
    );
  };

  searchOptionsLoad$ = new Subject();

  constructor(
    private store: Store<EmployeeState>,
    private dialog: MatDialog,
    private account: AccountService,
    private modalService: AddEmployeeModalService,
    private jpModalService: JobProfileModalService,
    private service: EmployeeApiService,
    private jobProfileService: JobProfilesService,
    private indexStore: DbStoreService
  ) {}

  ngOnInit() {
    this.store
      .select(getEmployeeState((x) => x.loading))
      .pipe(takeUntil(this.destroy$))
      .subscribe((x: boolean) => {
        this.isLoading = x;
      });

    const takeSearchSettings$ = this.service.getSearchSettings().pipe(take(1));
    const takeSearchFilters$ = this.service.getEmployeeSearchFilters().pipe(take(1));
    const takeBrowserCache$ = this.indexStore.getAll().pipe(take(1));
    const takeUserInfo$ = this.account.getUserInfo().pipe(take(1));

    this.filteredEmployees = this.searchInput.valueChanges.pipe(
      takeUntil(this.destroy$),
      startWith(''),
      debounceTime(350),
      switchMap(search => {
        return forkJoin([
          takeSearchSettings$,
          takeSearchFilters$,
          takeBrowserCache$,
          takeUserInfo$,
          of(search),
        ]);
      }),
      switchMap(([data, filters, indexData, user, search]: [IEmployeeSearchSetting, IEmployeeSearchFilter[], any, UserInfo, any]) => {
        this.user = user;
          this.allowAddEmployee$.next(
          this.user.isInMinimumRole(UserType.companyAdmin)
        );
        const optsUpdateRequest = this._mergeSearchSettingsWithCachedSearchOptions(data, filters, indexData, user.selectedClientId());

        this.store.dispatch(new UpdateSearchOptionsSuccess(optsUpdateRequest));

        const searchResults$ = this.searchResults$.pipe(map((x: IEmployeeSearchResultResponseData) => (!!x ? x.results : [])));

        if (typeof search === 'string' || typeof search === 'number') {
          let searchOptions: EmployeeSearchOptions = {};
          return this._getSearchOptions().pipe(
            switchMap(opts => {
              searchOptions = {...opts, searchText: search as any};
              return this.indexStore.getAndMergeSearchOptions(searchOptions, user.selectedClientId());
            }),
            tap(options => this._searchForEmployeeWithOptions(options, true)),
            switchMap(() => searchResults$),
          );
        } else if (
          typeof search === 'object' &&
          search !== null &&
          search.clientId &&
          search.employeeId
        ) {
          this.store.dispatch(new SetEmployee(search, true));
          if(this.employeeNavigated) this.employeeNavigated.emit(EmployeeActionTypes.SetEmployee);
        }
        return searchResults$;
      }),
    );

    if(this.externalInput && this.externalInput.results && this.externalInput.results.length > 0){
      this.loadExternalData()
    }
  }

  loadExternalData(){
    this.hideFilter = true;
    this.searchResults$ = of(this.externalInput);
    this.selectedEmployee$ = this.account.getUserInfo().pipe(tap(u => this.user = u), switchMap(u => of(this.externalInput.nav.current)));
    this.hasJobProfileLink(this.externalInput.nav.current);
    this.workHistoryText = this.workHistory(this.externalInput.nav.current);
    this.lengthOfServiceText = this.lengthOfService(this.externalInput.nav.current);
    this.filteredEmployees = this.searchResults$.pipe(map((x: IEmployeeSearchResultResponseData) => (!!x ? x.results : [])));
    this.isLoading = false;
  }

  selectedContact(selected: IContact) {
    if (selected != null && (selected as unknown as Event).returnValue) return;

    this.store
      .select(getEmployeeState((x) => x.searchResults))
      .pipe(take(1))
      .subscribe((res: IEmployeeSearchResultResponseData) => {
        const emp = res.results.find(
          (x) => x.employeeId === selected.employeeId
        );
        if (!emp) return;

        this.store.dispatch(new SetEmployee(emp, true));
        if(this.employeeNavigated) this.employeeNavigated.emit(EmployeeActionTypes.SetEmployee);
      });
  }

  setHasEnabledFilters(options: EmployeeSearchOptions) {
    if (options) {
      this._hasEnabledFilters =
        options.isActiveOnly ||
        options.isExcludeTemps ||
        options.filters.filter((x) => !!x.$selected).length != 0;

      this.hasEnabledFilters$.next(this._hasEnabledFilters);
    }
  }

  showFilterOptionModal(options: EmployeeSearchOptions) {
    this.dialog
      .open(SearchFilterDialogComponent, {
        data: {
          options,
          filters: options.filters,
        },
        width: '700px',
      })
      .afterClosed()
      .subscribe((result: { options: EmployeeSearchOptions }) => {
        if (!result) return;
        const updateRequest = this._mapSearchOptionsToSearchSettings(
          result.options
        );
        this.store.dispatch(new UpdateSearchOptions(updateRequest));

        this._searchForEmployee(this.searchInput.value, true);
      });
  }

  getEnabledFilters(options: EmployeeSearchOptions): IEmployeeSearchFilter[] {
    return options && options.filters && options.filters.length
      ? options.filters.filter((x) => !!x.$selected)
      : [];
  }

  clearFilterSelection(
    options: EmployeeSearchOptions,
    filter: IEmployeeSearchFilter
  ) {
    const filteredOptions = {
      ...options,
      filters: options.filters.map((x) => {
        if (x.$selected && x.$selected.id === filter.$selected.id) {
          delete x.$selected;
        }
        return x;
      }),
    } as EmployeeSearchOptions;

    const updateRequest =
      this._mapSearchOptionsToSearchSettings(filteredOptions);

    this.store.dispatch(new UpdateSearchOptions(updateRequest));
    this._searchForEmployee(this.searchInput.value, true);
  }

  removeActiveOnly() {
    this._updateSearchOptionsAndExecuteSearch('isActiveOnly', false);
  }

  removeExcludeTemps() {
    this._updateSearchOptionsAndExecuteSearch('isExcludeTemps', false);
  }

  private _updateSearchOptionsAndExecuteSearch(key: string, value: any) {
    this.store
      .select(getEmployeeState((x) => x.searchSettings))
      .pipe(take(1))
      .subscribe((search: EmployeeSearchOptions) => {
        search[key] = value;
        const req = this._mapSearchOptionsToSearchSettings(search);
        this.store.dispatch(new UpdateSearchOptions(req));

        this._searchForEmployee(this.searchInput.value);
      });
  }

  private _searchForEmployee(searchText?: string, getResults?: boolean) {
    getResults = getResults || searchText != null;
    return this._getSearchOptions().subscribe((opts) => {
      if (searchText && searchText.length) {
        opts = { ...opts, searchText };
      }

      this._searchForEmployeeWithOptions(opts, getResults);
    });
  }

  private _searchForEmployeeWithOptions(
    opts: EmployeeSearchOptions,
    getResults: boolean
  ) {
    this.store.dispatch(new GetEmployeeSearch(getResults, opts));
  }

  private _getSearchOptions(): Observable<EmployeeSearchOptions> {
    return this.store.select(getSearchOptions()).pipe(take(1));
  }

  previousEmployee() {
    this.store.dispatch(new GoToPreviousEmployee());
    if(this.employeeNavigated) this.employeeNavigated.emit(EmployeeActionTypes.GoToPreviousEmployee);
  }

  nextEmployee() {
    this.store.dispatch(new GoToNextEmployee());
    if(this.employeeNavigated) this.employeeNavigated.emit(EmployeeActionTypes.GoToNextEmployee);
  }

  addEmployee() {
    this.modalService
      .open(this.user.clientId)
      .result.then((ee: IEmployeeSearchResult) => {
        this.service
          .updateLastEmployeeFromEmployeeSearchResult(ee)
          .pipe(
            withLatestFrom(this.account.getSiteConfig(ConfigUrlType.Payroll))
          )
          .subscribe(([ee, payroll]) => {
            if (!payroll) {
              console.error(
                'Could not find base site URLs, please check web configs.'
              );
              return;
            }
            document.location.href = `${payroll.url}/Employee.aspx`;
          });
      });
  }

  hasJobProfileLink(emp: IEmployeeSearchResult) {
    if (!emp || !emp.groups || !emp.groups.length) return;
    const jobProfile = emp.groups.find(
      (groups) => groups.filterType === EmployeeSearchFilterType.JobTitle
    );
    if (!jobProfile) {
      this.jobProfileData = null;
      this.jobProfileName = '';
      this.showJobProfileLink = false;
      return;
    }

    this.jobProfileName = jobProfile.name;
    this.jobProfilesData$.subscribe((jobProfiles) => {
      const jp = jobProfiles.find((x) => x.jobProfileId === jobProfile.id);
      if (!jp) {
        this.jobProfileData = jp;
        this.showJobProfileLink = false;
        return;
      };
      this.jobProfileData = jp;
      this.showJobProfileLink = true;
    });
  }

  openJobProfile(emp: IEmployeeSearchResult) {
    const jobProfile = emp.groups.find(
      (groups) => groups.filterType === EmployeeSearchFilterType.JobTitle
    );
    if (!jobProfile) return;
    this.jobProfileService
      .getJobProfileBasicInfo(jobProfile.id)
      .subscribe((data) => this.jpModalService.open(data));
  }
  displayFn(emp: IEmployeeSearchResult) {
    if (!emp) return '';
    return `${emp.firstName} ${emp.lastName}`;
  }

  private _mapSearchOptionsToSearchSettings(
    options: EmployeeSearchOptions
  ): IEmployeeSearchSetting {
    let filters =
      options.filters
        .filter((x) => !!x.$selected)
        .map((x) => {
          const res = {} as any;
          res[x.$selected.filterType] = x.$selected.id;
          return res;
        }) || [];

    if (filters.length)
      filters = filters.reduce((prev, curr) => {
        for (let p in prev) {
          curr[p] = prev[p];
        }
        return curr;
      });

    return {
      clientId: this.user ? this.user.selectedClientId() : null,
      ...filters,
      isActiveOnly: options.isActiveOnly,
      isExcludeTemps: options.isExcludeTemps,
      userId: this.user ? this.user.userId : null,
    } as IEmployeeSearchSetting;
  }

  private _mergeSearchSettingsWithCachedSearchOptions(data: IEmployeeSearchSetting, filters: IEmployeeSearchFilter[], indexData: any, clientId: number): EmployeeSearchOptionsUpdate {
    const result: EmployeeSearchOptionsUpdate = {...data, filters};
    const hasIndexKey = indexData != null && (Object.keys(indexData).find(x => (x as any) == clientId) as any) > -1;
    const cached = hasIndexKey ? indexData[clientId] : null;

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

    return result;
  }

  workHistory(x) {
    let history = '';
    // let hireDate = new Date(x.hireDate).toLocaleDateString('en-US');
    let today = new Date(Date.now());

    let localeDate = (d:Date) => (d.getMonth()+1)+'/'+d.getDate()+'/'+d.getFullYear();

    let intervals:Array<any> = this.workHistoryIntervals(x);
    for( var i=0; i<intervals.length; i++ ) {
      if(i>0) history += '<br/>';

      let formattedHireDate = intervals[i].st ? localeDate(intervals[i].st) : "Unknown";

      if ( intervals[i].st && intervals[i].st > today) {
        history += 'Starts ' + formattedHireDate;
      }
      else {
        history += formattedHireDate + '-';

        if (intervals[i].en) {
          history += localeDate(intervals[i].en);
        }
        else if(i< (intervals.length-1)){
          history += "Unknown"
        }
        else {
          history += "Present"
        }
      }
    }
    return (history && history!='Unknown-Present') ? history : '';
  }

  workHistoryIntervals(x):Array<any> {
    // Any employee can only have a maximun of 2 intervals
    // 1. A hire to separation/present
    // 2. A rehire to present
    let hireDate = x.hireDate ? new Date(x.hireDate) : new Date();

    let separationDate = x.separationDate ? new Date(x.separationDate) : new Date();
    let rehireDate = x.rehireDate ? new Date(x.rehireDate) : new Date();
    let intervals = [];

    if ( !x.hireDate && !x.rehireDate && !x.separationDate ) {
      intervals.push({ st: null, en: null });
    }
    else if ( !x.hireDate && !x.rehireDate && x.separationDate) {
      intervals.push({ st: null, en: separationDate });
    }
    else if ( !x.hireDate && x.rehireDate && !x.separationDate) {
      intervals.push({ st: rehireDate, en: null });
    }
    else if ( !x.hireDate && x.rehireDate && x.separationDate ) {
      if(rehireDate < separationDate){
        intervals.push({ st: rehireDate, en: separationDate });
      } else {
        intervals.push({ st: null, en: separationDate });
        intervals.push({ st: rehireDate, en: null });
      }
    }
    else if ( x.hireDate && !x.rehireDate && !x.separationDate ){
      intervals.push({ st: hireDate, en: null });
    }
    else if ( x.hireDate && !x.rehireDate && x.separationDate ){
      intervals.push({ st: hireDate, en: separationDate });
    }
    else if ( x.hireDate && x.rehireDate && !x.separationDate ){
      intervals.push({ st: hireDate, en: null });
      intervals.push({ st: rehireDate, en: null });
    }
    else if ( x.hireDate && x.rehireDate && x.separationDate ){
      if(rehireDate < separationDate){
        intervals.push({ st: hireDate, en: null });
        intervals.push({ st: rehireDate, en: separationDate });
      } else {
        intervals.push({ st: hireDate, en: separationDate });
        intervals.push({ st: rehireDate, en: null });
      }
    }

    // Only the latest interval considered
    return intervals.length > 1 ? [intervals[1]] : intervals ;
  }

  lengthOfService(x) {
    let los: string;
    let diffInfo = {
      years: 0,
      months: 0,
      days: 0,
      hours: 0,
    };

    let intervals:Array<any> = this.workHistoryIntervals(x);
    // If no dates are returned, set N/A as the value
    for( var i=0; i< intervals.length; i++ ) {
      // First interval  without endate gets ignored
      if(i< (intervals.length-1) && !intervals[i].en) continue;

      if (intervals[i].st) {
        let mHire = moment(intervals[i].st).startOf('day');
        let mTo = intervals[i].en ? moment(intervals[i].en) : moment();

        mTo = mTo.startOf('day').add(1, 'day');

        if (mHire.isAfter(moment(), 'day')) {
          //onboarding
          if(i==0) los = 'New Hire';
          else los = 'Re-hire';
          return los;
        } else {
          let diff = moment.duration(mTo.diff(mHire));
          diffInfo.years  += diff.years();
          diffInfo.months += diff.months();
          diffInfo.days   += diff.days();
          diffInfo.hours  += diff.hours();

          if (diffInfo.hours >= 24) {
            diffInfo.days++;
            diffInfo.hours = diffInfo.hours - 24;
          }

          if (diffInfo.days >= 30) {
            diffInfo.months++;
            diffInfo.days = diffInfo.days - 30;
          }

          if (diffInfo.months >= 12) {
            diffInfo.years++;
            diffInfo.months = diffInfo.months - 12;
          }
        }
      }
    }

    //adjust for rounding
    if (diffInfo.hours >= 23) {
      diffInfo.days++;
      diffInfo.hours = 0;
    }

    if (diffInfo.days >= 29) {
      diffInfo.months++;
      diffInfo.days = 0;
    }

    if (diffInfo.months === 12) {
      diffInfo.years++;
      diffInfo.months = 0;
    }

    //post-adjustment ... output duration
    los = '';
    if (diffInfo.years)
      los += diffInfo.years + (diffInfo.years === 1 ? ' year' : ' years');
    if (diffInfo.months)
      los +=
        (diffInfo.years ? ', ' : '') +
        diffInfo.months +
        (diffInfo.months === 1 ? ' month' : ' months');
    if ((!diffInfo.years || !diffInfo.months) && diffInfo.days > 0)
      los += (diffInfo.years || diffInfo.months ? ", " : "") + diffInfo.days + (diffInfo.days === 1 ? ' day' : ' days');

    return los;
  }
}
