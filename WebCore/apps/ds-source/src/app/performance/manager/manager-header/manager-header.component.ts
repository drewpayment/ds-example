import { Component, OnInit } from '@angular/core';
import {
  IEmployeeSearchResult,
  IEmployeeSearchResultResponseData,
} from '@ds/employees/search/shared/models/employee-search-result';
import { IEmployeeSearchFilter } from '@ds/employees/search/shared/models/employee-search-filter';
import { EmployeeSearchOptions } from '@ds/employees/search/shared/models/employee-search-options';
import { IEmployeeSearchSetting } from '@ajs/employee/search/shared/models';
import {
  Observable,
  Subject,
  throwError,
  zip,
  combineLatest,
  of,
  forkJoin,
  merge,
  iif,
} from 'rxjs';
import {
  EmployeeApiService,
  SearchFilterDialogService,
} from '@ds/core/employees';
import {
  debounceTime,
  distinctUntilChanged,
  map,
  concatMap,
  share,
  tap,
  filter,
  switchMap,
  startWith,
  withLatestFrom,
} from 'rxjs/operators';
import {
  FormControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { SortDirection } from '@ds/core/shared/sort-direction.enum';
import { UserType } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { IReviewProfileBasicSetup } from '@ds/performance/review-profiles/shared/review-profile-basic-setup.model';
import { Router, ActivatedRoute } from '@angular/router';
import { Maybe } from '@ds/core/shared/Maybe';
import {
  trigger,
  transition,
  style,
  animate,
  state,
} from '@angular/animations';
import {
  IReviewSearchOptions,
  REVIEW_SEARCH_OPTIONS_KEY,
} from '@ds/performance/performance-manager';
import { ReviewPolicyApiService } from '@ds/performance/review-policy/review-policy-api.service';
import { PerformanceManagerService } from '@ds/performance/performance-manager/performance-manager.service';
import {
  IReviewTemplate,
  SortByName,
  GetReviewTemplateName,
} from '@ds/core/groups/shared/review-template.model';
import { PayrollRequestReportArgsStore } from '@ds/performance/performance-manager/payroll-requests/payroll-request-report/payroll-request-report-args.store';
import { DsStorageService } from '@ds/core/storage/storage.service';
import * as moment from 'moment';
import { EmployeeSearchFilterType } from '@ds/employees/search/shared/models/employee-search-filter-type';

enum SpecialFilter {
  ActiveOnly = 1,
  ExcludeTemps = 2,
}
class EmployeeFilter {
  filterName: string;
  specialFilterType: SpecialFilter;
  employeeFilterType: EmployeeSearchFilterType;
  notMutable: boolean;
}

@Component({
  selector: 'ds-manager-header',
  templateUrl: './manager-header.component.html',
  styleUrls: ['./manager-header.component.scss'],
  animations: [
    trigger('searchText', [
      transition(':enter', [style({ opacity: 0 }), animate('.2s ease')]),
      transition(':leave', [animate('.2s ease', style({ opacity: 0 }))]),
      state('*', style({ opacity: 1 })),
    ]),
  ],
})
export class ManagerHeaderComponent implements OnInit {
  reviewTemplates: IReviewTemplate[];
  selectedReview: IReviewTemplate = null;
  options: IReviewSearchOptions = {};
  filtersEnabled: boolean = false;
  searchResponse$: Observable<IEmployeeSearchResultResponseData>;
  searchText = new FormControl('', { updateOn: 'change' });
  showFilterAndSearchFields: boolean = false;
  // keeps track of the filters and displays on the UI
  employeeFilters: EmployeeFilter[];
  employeeSearchFilterItems: IEmployeeSearchFilter[];
  userHasPermissiontoSeePayrollRequestsTab$: Observable<boolean>;
  reviewHasPayrollRequestsEnabled$: Observable<boolean>;
  getReviewProfiles$: Observable<{ [id: number]: IReviewProfileBasicSetup }>;
  reviewProfsAndHasPermission$: Observable<{
    reviewProfs: { [id: number]: IReviewProfileBasicSetup };
    hasPermission: boolean;
  }>;
  filterForm: FormGroup;
  activeRoute: string;
  isFiltered = false;
  readonly momentFormatString = 'MM/DD/YYYY';

  private applyFilterSubject: Subject<string> = new Subject<string>();
  private applyFilterAction = this.applyFilterSubject.asObservable();

  constructor(
    private reviewPolicySvc: ReviewPolicyApiService,
    private manager: PerformanceManagerService,
    private employeeSearchSvc: SearchFilterDialogService,
    private employeeService: EmployeeApiService,
    private acctSvc: AccountService,
    private store: DsStorageService,
    public router: Router,
    private route: ActivatedRoute,
    fb: FormBuilder,
    public reportParamSvc: PayrollRequestReportArgsStore
  ) {
    this.filterForm = fb.group({
      StartDate: fb.control(null, {
        updateOn: 'blur',
        validators: [Validators.required],
      }),
      EndDate: fb.control(null, {
        updateOn: 'blur',
        validators: [Validators.required],
      }),
      Review: fb.control(null, [Validators.required]),
      Filters: fb.control(null),
      ApplyFilter: fb.control(null),
    });
  }

  // don't want expose more private properties than we need to
  readonly UserInfoFn = this.acctSvc.getUserInfo;

  readonly ReviewTemplatesFn =
    this.reviewPolicySvc.getReviewTemplatesByClientId;

  readonly ReviewProfileSetupsFn = this.reviewPolicySvc.getReviewProfileSetups;

  readonly selectedRCR$ = this.manager.selectedReviewTemplate$;

  readonly doNothing = (x: boolean) => {};

  get ReviewCtrl() {
    return this.filterForm.get('Review') as FormControl;
  }
  get FiltersCtrl() {
    return this.filterForm.get('Filters') as FormControl;
  }
  get EndDate() {
    return this.filterForm.controls.EndDate as FormControl;
  }
  get StartDate() {
    return this.filterForm.controls.StartDate as FormControl;
  }
  get ApplyFilter() {
    return this.filterForm.controls.ApplyFilter as FormControl;
  }

  endDateParam: Date;
  startDateParam: Date;
  templateIdParam: number;

  ngOnInit() {
    var startDate = new Date();
    var endDate = new Date();

    this.route.queryParams.subscribe((params) => {
      if (params['ed']) {
        endDate = new Date(params['ed'].replace(/['"]+/g, ''));
      } else {
        endDate.setFullYear(endDate.getFullYear(), endDate.getMonth() + 1, 0);
      }
      if (params['sd']) {
        startDate = new Date(params['sd'].replace(/['"]+/g, ''));
      } else {
        startDate.setFullYear(
          startDate.getFullYear() - 1,
          startDate.getMonth(),
          1
        );
      }
      this.templateIdParam = params['rtid'] ? +params['rtid'] : 0;
    });

    this.StartDate.setValue(startDate);
    this.EndDate.setValue(endDate);

    this.filterForm.valueChanges.subscribe(() => {
      this.manager.filterOptionsFormGroup = this.filterForm;
    });

    // Retrieve the search options save is storage previously
    const storeResult = this.store.get(REVIEW_SEARCH_OPTIONS_KEY);
    storeResult.success = false;
    if (storeResult.success) {
      this.options = storeResult.data;
      this.manager.setReviewSearchOptions(storeResult.data);
    } else this.manager.setReviewSearchOptions({});

    this.UpdateSearchOptionsAndStartSearch(
      this.EmitWhenPageShouldLoad()
    ).subscribe((x) => {
      this.showFilterAndSearchFields = !!this.ReviewCtrl.value;
    });

    this.userHasPermissiontoSeePayrollRequestsTab$ = this.acctSvc
      .getUserInfo()
      .pipe(map((x) => x.userTypeId <= UserType.companyAdmin));

    this.employeeService
      .getEmployeeSearchFilters(true, true, true)
      .pipe(
        tap((filters) => {
          this.options.filters = filters;
          this.updateFilterForm();
        })
      )
      .subscribe();

    this.manager.activeReviewSearchOptions$.subscribe((opts) => {
      if (opts == null) return;

      this.options = opts;

      if (this.options.isActiveOnly == null) {
        this.options.isActiveOnly = true;
      }

      this.filtersEnabled = this.manager.filterEnabled;
      this.updateEmployeeFilters();
      this.manager.filterEnabled = !!this.employeeFilters.length;
    });

    this.searchResponse$ = this.manager.employeeSearchResponse$;

    const getReviewTemplates$ = this.acctSvc.PassUserInfoToRequest((x) => {
      return this.reviewPolicySvc
        .getReviewTemplatesByClientId(x.selectedClientId(), false, false)
        .pipe(
          tap((x) => {
            this.reviewTemplates = SortByName(GetReviewTemplateName, x);
            this.updateFilterForm();

            this.ReviewCtrl.setValue(
              this.reviewTemplates.find((rev) => {
                return rev.reviewTemplateId == this.templateIdParam;
              })
            );
          }),
          share()
        );
    });
    const getReviewProfiles$ = getReviewTemplates$.pipe(
      concatMap((cycles) => {
        const profileIds = cycles
          .map((x) => x.reviewProfileId)
          .reduce((x, y) => x.concat(y), []);
        const existing: { [id: number]: boolean } = {};
        const result: number[] = [];
        profileIds.forEach((x) => {
          if (existing[x] == null) {
            result.push(x);
          }

          existing[x] = true;
        });
        const profIds: { [id: number]: IReviewProfileBasicSetup } = {};
        return this.reviewPolicySvc.getReviewProfileSetups(result).pipe(
          map((x) => {
            (x || []).forEach((y) => (profIds[y.reviewProfileId] = y));
            return profIds;
          })
        );
      })
    );

    this.reviewProfsAndHasPermission$ = zip(
      getReviewProfiles$,
      this.userHasPermissiontoSeePayrollRequestsTab$
    ).pipe(map((x) => ({ reviewProfs: x[0], hasPermission: x[1] })));

    // set value on load for routing select on small screens
    this.activeRoute = this.router.url;
  }

  private updateEmployeeFilters() {
    // Update the UI to display selected items
    this.employeeFilters = [];
    if (this.options.isActiveOnly) {
      let activeOnlyFilter = new EmployeeFilter();
      activeOnlyFilter.specialFilterType = SpecialFilter.ActiveOnly;
      activeOnlyFilter.filterName = 'Active Only';
      activeOnlyFilter.notMutable = true;
      this.employeeFilters.push(activeOnlyFilter);
    }

    if (this.options.isExcludeTemps) {
      let excludeTempFilter = new EmployeeFilter();
      excludeTempFilter.specialFilterType = SpecialFilter.ExcludeTemps;
      excludeTempFilter.filterName = 'Temps Excluded';
      this.employeeFilters.push(excludeTempFilter);
    }

    if (this.options.filters) {
      this.options.filters.forEach(filter => {
        if (filter.$selected) {
          const eef = new EmployeeFilter();
          eef.employeeFilterType = filter.filterType;
          eef.filterName = filter.$selected.name;
          this.employeeFilters.push(eef);
        }
      });
    }
  }

  private updateFilterForm(): void {
    if (this.options && Object.keys(this.options).length > 0) {
      if (this.options.startDate && this.options.endDate) {
        this.filterForm.patchValue({
          StartDate: this.options.startDate,
          EndDate: this.options.endDate,
        });
      }

      this.searchText.setValue(this.options.searchText);

      if (this.reviewTemplates) {
        if (this.options.reviewTemplateId) {
          this.showFilterAndSearchFields = true;
          this.ReviewCtrl.setValue(
            this.reviewTemplates.find(
              (x) => x.reviewTemplateId == this.options.reviewTemplateId
            )
          );

          this.applyFilterSubject.next('initial');
        }
      }
      this.updateEmployeeFilters();
    }
  }

  applyFilter() {
    this.isFiltered = true;
    this.applyFilterSubject.next('apply');
  }

  private SearchTextInput(): Observable<string> {
    return this.searchText.valueChanges.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      switchMap((search) => {
        this.options.searchText = search;
        this.options.sortBy =
          this.options.sortBy || EmployeeSearchFilterType.Name.toString();
        this.options.sortDirection =
          this.options.sortDirection || SortDirection.ascending;

        return iif(
          () => this.options.reviewTemplateId == null,
          of(null),
          of(search)
        );
      })
    ); // make sure we get data when input is not visible on initial page load
  }

  private EmitWhenPageShouldLoad(): Observable<any[]> {
    return combineLatest(
      this.FiltersCtrl.valueChanges.pipe(startWith(null)),
      of(this.filterForm),
      of(this.options),
      this.SearchTextInput(),
      this.applyFilterAction
    );
  }

  private UpdateSearchOptionsAndStartSearch(source$: Observable<any[]>) {
    return source$.pipe(
      debounceTime(100),

      // Data is the form. We are checking if it's valid
      filter((data) => {
        return data[1].valid;
      }),
      map((data) => ({
        formValue: data[1],
        options: data[2],
      })),
      tap((formVal) => {
        this.manager.selectedReviewTemplate =
          formVal.formValue.controls['Review'].value;
      }),
      map<any, IReviewSearchOptions>((formVal) => {
        formVal.options.reviewTemplateId = 0;
        if (formVal.formValue.controls['Review'].value)
          formVal.options.reviewTemplateId =
            formVal.formValue.controls['Review'].value.reviewTemplateId;
        formVal.options.startDate = this.StartDate.value;
        formVal.options.endDate = this.EndDate.value;
        return formVal.options;
      }),
      tap((options) => {
        // Re-Update
        this.options.startDate = options.startDate;
        this.options.endDate = options.endDate;
        this.options.reviewTemplateId = options.reviewTemplateId;
        this.manager.setReviewSearchOptions(this.options);
      })
    );
  }

  selectedReviewChanged() {
    this.manager.selectedReviewTemplate = this.selectedReview;
  }

  showFilterDialog() {
    this.employeeSearchSvc
      .open(this.options, this.options.filters)
      .afterClosed()
      .pipe(
        tap((result) => {
          if (!result || !result.options) return;

          const filters = result.options.filters.filter(f => !!f.$selected);
          const filtersArray = new Array<number[]>(filters.length);
          filters.forEach(f => {
            filtersArray[f.filterType] = filtersArray[f.filterType] && filtersArray[f.filterType].length
              ? filtersArray[f.filterType] = [...filtersArray[f.filterType], f.$selected.id]
              : [f.$selected.id];
          });

          this.options = {
            ...this.options,
            isActiveOnly: result.options.isActiveOnly,
            isExcludeTemps: result.options.isExcludeTemps,
          };

          this.manager.setReviewSearchOptions(this.options);
        })
      )
      .subscribe();
  }

  clearFilterSelection(filter: EmployeeFilter) {
    if (filter.specialFilterType) {
      switch (filter.specialFilterType) {
        case SpecialFilter.ActiveOnly:
          this.options.isActiveOnly = false;
          break;
        case SpecialFilter.ExcludeTemps:
          this.options.isExcludeTemps = false;
          break;
      }
    } else {
      this.employeeFilters = this.employeeFilters.filter(x => x.employeeFilterType !== filter.employeeFilterType);

      this.options.filters = this.options.filters.map(x =>
        !!x.$selected && x.$selected.filterType == filter.employeeFilterType
          ? {
            description: x.description,
            filterType: x.filterType,
            filterOptions: x.filterOptions,
          } as IEmployeeSearchFilter
          : x);
    }

    this.manager.filterEnabled = !!this.employeeFilters.length;
    this.manager.setReviewSearchOptions(this.options);
  }

  // Routes on mobile
  navigateTo(value) {
    if (value) {
      this.router.navigate([value]);
    }

    return false;
  }
}
