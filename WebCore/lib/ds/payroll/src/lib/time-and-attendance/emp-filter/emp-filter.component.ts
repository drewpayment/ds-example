import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
  Output,
  EventEmitter,
  OnDestroy,
  ChangeDetectorRef,
  ViewChild,
  ElementRef,
} from '@angular/core';
import { PayPeriod } from '../shared/PayPeriod.model';
import { ApproveHourOption } from '../shared/ApproveHourOption.model';
import { ClockFilter } from '../shared/ClockFilter.model';
import {
  FormGroup,
  FormBuilder,
  FormControl,
  Validators,
} from '@angular/forms';
import {
  tap,
  distinctUntilChanged,
  startWith,
  switchMap,
  map,
  first,
} from 'rxjs/operators';
import { Observable, merge, of, Subscription, BehaviorSubject } from 'rxjs';
import { Maybe } from '@ds/core/shared/Maybe';
import { FilterOption } from '../shared/filter-option.model';
import {
  coerceBooleanProperty,
  coerceNumberProperty,
} from '@angular/cdk/coercion';
import { ActivatedRoute, Params } from '@angular/router';
import { MOMENT_FORMATS, UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import * as moment from 'moment';
import { ApproveHourSettings } from '../shared/ApproveHourSettings.model';
import { InitData } from '../shared/InitData.model';
import { IContact } from '@ds/core/contacts';
import { TimeAndAttendanceService } from '../time-and-attendance.service';
import { CalculateSize } from '@ds/core/ui/ui-helper';
import {
  MatAutocomplete,
  MatAutocompleteSelectedEvent,
} from '@angular/material/autocomplete';
import { MatInput } from '@angular/material/input';
import { convertToMoment } from '../../../../../core/src/lib/shared/convert-to-moment.func';
import { isNotUndefinedOrNull } from '@util/ds-common';

@Component({
  selector: 'ds-emp-filter',
  templateUrl: './emp-filter.component.html',
  styleUrls: ['./emp-filter.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EmpFilterComponent implements OnInit, OnDestroy {
  f: FormGroup = this.createForm();
  @Input() initData: InitData;
  urlParams: Params;

  private _payPeriods: PayPeriod[];
  @Input()
  public get payPeriods(): PayPeriod[] {
    return this._payPeriods;
  }
  public set payPeriods(value: PayPeriod[]) {
    if (!this._payPeriods) {
      this.totalPayPeriodOptions.setValue(
        new Maybe(value).map((x) => x.length).valueOr(0)
      );
      this.payPeriod.setValue(
        new Maybe(this.payPeriod.value).valueOr(
          new Maybe(value).map((x) => x[0]).value()
        )
      );
      this._payPeriods = value;
    }
  }

  private _approveHourOptions: ApproveHourOption[];
  @Input()
  public get approveHourOptions(): ApproveHourOption[] {
    return this._approveHourOptions;
  }
  public set approveHourOptions(value: ApproveHourOption[]) {
    if (!this._approveHourOptions) this._approveHourOptions = value;
  }

  private _clockFilters1: ClockFilter[];
  @Input()
  public get clockFilters1(): ClockFilter[] {
    return this._clockFilters1;
  }
  public set clockFilters1(value: ClockFilter[]) {
    if (!this._clockFilters1) this._clockFilters1 = value;
  }

  private _clockFilters2: ClockFilter[];
  @Input()
  public get clockFilters2(): ClockFilter[] {
    return this._clockFilters2;
  }
  public set clockFilters2(value: ClockFilter[]) {
    if (!this._clockFilters2) this._clockFilters2 = value;
  }

  private _filter1Values: {
    [categoryId: number]: FilterOption[];
  };
  @Input()
  public get filter1Values(): {
    [categoryId: number]: FilterOption[];
  } {
    return new Maybe(this._filter1Values).valueOr({});
  }
  public set filter1Values(value: { [categoryId: number]: FilterOption[] }) {
    if (!this._filter1Values) {
      this._filter1Values = value;
      this.hideFilterWhenListEmpty(
        this.category1Value.value,
        value,
        (val) => (this.showFilter1 = val)
      );
      this.hideFilterWhenListEmpty(
        this.category2Value.value,
        value,
        (val) => (this.showFilter2 = val)
      );
    }
  }
  filterValues$ = new BehaviorSubject<{ [key: number]: FilterOption[] }>(null);

  private _hideDailyTotals: boolean;
  @Input()
  public get hideDailyTotals(): boolean {
    return this._hideDailyTotals;
  }
  public set hideDailyTotals(value: boolean) {
    this._hideDailyTotals = value;
    this.daily.setValue(!value, { emitEvent: false });
  }

  private _hideWeeklyTotals: boolean;
  @Input()
  public get hideWeeklyTotals(): boolean {
    return this._hideWeeklyTotals;
  }
  public set hideWeeklyTotals(value: boolean) {
    this._hideWeeklyTotals = value;
    this.weekly.setValue(!value, { emitEvent: false });
  }

  private _hideGrandTotals: boolean;
  @Input()
  public get hideGrandTotals(): boolean {
    return this._hideGrandTotals;
  }
  public set hideGrandTotals(value: boolean) {
    this._hideGrandTotals = value;
    this.f
      .get('persistedSearchSettings.grand')
      .setValue(!value, { emitEvent: false });
  }

  private _defaultDaysFilter: number;
  @Input()
  public get defaultDaysFilter(): number {
    return this._defaultDaysFilter;
  }
  public set defaultDaysFilter(value: number) {
    const valueAsNumber = value == null ? 1 : +value;
    if (!isNaN(+valueAsNumber)) {
      this._defaultDaysFilter = valueAsNumber;
      this.days.setValue(valueAsNumber, { emitEvent: false });
    }
  }

  private _employeesPerPage: number;
  @Input()
  public get employeesPerPage(): number {
    return this._employeesPerPage;
  }
  public set employeesPerPage(value: number) {
    const valueAsNumber = value == null ? 10 : +value;

    if (!isNaN(+valueAsNumber)) {
      this._employeesPerPage = valueAsNumber;
      this.empsPerPage.setValue(valueAsNumber, { emitEvent: false });
    }
  }

  private _clockEmployeeApproveHoursSettingsID: number;
  @Input()
  public get clockEmployeeApproveHoursSettingsID(): number {
    return this._clockEmployeeApproveHoursSettingsID;
  }
  public set clockEmployeeApproveHoursSettingsID(value: number) {
    this._clockEmployeeApproveHoursSettingsID = value;
    this.clockEmployeeApproveHoursSettingsIDCtrl.setValue(value, {
      emitEvent: false,
    });
  }

  private _showFilter1 = false;
  public get showFilter1(): boolean {
    return this._showFilter1;
  }
  public set showFilter1(value: boolean) {
    this.filter1Visible.setValue(value);
    this._showFilter1 = value;
  }
  private _showFilter2 = false;
  public get showFilter2(): boolean {
    return this._showFilter2;
  }
  public set showFilter2(value: boolean) {
    this.filter2Visible.setValue(value);
    this._showFilter2 = value;
  }

  private _showCategory2 = false;
  public get showCategory2(): boolean {
    return this._showCategory2;
  }
  public set showCategory2(value: boolean) {
    this.category2Visible.setValue(value);
    this._showCategory2 = value;
  }

  private _setPage: number;
  @Input()
  public get setPage(): number {
    return this._setPage;
  }
  public set setPage(value: number) {
    this._setPage = value;
    this.CurrentPage.setValue(value);
  }

  selectEmployeeCtrl = new FormControl();

  submitted = false;

  @Output()
  searchParamsUpdated: EventEmitter<any> = new EventEmitter();

  @Output() filterCategoryChanged: EventEmitter<any> = new EventEmitter();
  @Output() filterCategory2Changed: EventEmitter<any> = new EventEmitter();
  @Output() persistedSearchSettingsChanged: EventEmitter<ApproveHourSettings> =
    new EventEmitter();

  categoryDropdownChanged$: Observable<any>;

  get payPeriod() {
    return this.f.get('payPeriod') as FormControl;
  }
  get days() {
    return (this.f.get('persistedSearchSettings') as FormGroup).get(
      'days'
    ) as FormControl;
  }
  get approvalStatus() {
    return this.f.get('approvalStatus') as FormControl;
  }
  get empFilter() {
    return this.f.get('empFilter') as FormControl;
  }
  get category1Value() {
    return this.f.get('category1Value') as FormControl;
  }
  get category2Value() {
    return this.f.get('category2Value') as FormControl;
  }
  get filter1Value() {
    return this.f.get('filter1Value') as FormControl;
  }
  get filter2Value() {
    return this.f.get('filter2Value') as FormControl;
  }
  get category1Visible() {
    return this.f.get('category1Visible') as FormControl;
  }
  get category2Visible() {
    return this.f.get('category2Visible') as FormControl;
  }
  get filter1Visible() {
    return this.f.get('filter1Visible') as FormControl;
  }
  get filter2Visible() {
    return this.f.get('filter2Visible') as FormControl;
  }
  get startDate() {
    return this.f.get('startDate') as FormControl;
  }
  get endDate() {
    return this.f.get('endDate') as FormControl;
  }
  get totalPayPeriodOptions() {
    return this.f.get('totalPayPeriodOptions') as FormControl;
  }
  get empsPerPage() {
    return (this.f.get('persistedSearchSettings') as FormGroup).get(
      'empsPerPage'
    ) as FormControl;
  }
  get daily() {
    return (this.f.get('persistedSearchSettings') as FormGroup).get(
      'daily'
    ) as FormControl;
  }
  get weekly() {
    return (this.f.get('persistedSearchSettings') as FormGroup).get(
      'weekly'
    ) as FormControl;
  }
  get persistedSearchSettings() {
    return this.f.get('persistedSearchSettings') as FormGroup;
  }
  get clockEmployeeApproveHoursSettingsIDCtrl() {
    return (this.f.get('persistedSearchSettings') as FormGroup).get(
      'clockEmployeeApproveHoursSettingsID'
    ) as FormControl;
  }
  get CurrentPage() {
    return this.f.get('currentPage') as FormControl;
  }

  get isCustomDateRange() {
    // return (this.payPeriod.value?.payrollId == 2);
    return (
      isNotUndefinedOrNull(this.payPeriod.value) &&
      this.payPeriod.value.payrollId == 2
    );
  }
  get startOrEndDatesHaveError() {
    return (this.startDateValidation.hasError() ||
      this.endDateValidation.hasError()) as boolean;
  }
  get isFilterSubmitDisabled() {
    return this.isCustomDateRange && this.startOrEndDatesHaveError;
  }

  private readonly _dateRangeNumberOfDaysBoundInclusive = 90;
  private readonly _dateRangeNumberOfDaysBoundExclusive =
    this._dateRangeNumberOfDaysBoundInclusive - 1;

  startDateValidation: IDateValidation = {
    getControl: () => this.startDate,
    hasError: () => this.dateHasError(this.startDate),
    errorMessage: () => this.getStartOrEndDateErrorMessage(true),
    minDate: () => {
      // no more than 90 days including the start and end dates, so subtract 89 days
      return convertToMoment(this.endDate.value).subtract(
        this._dateRangeNumberOfDaysBoundExclusive,
        'days'
      );
    },
    maxDate: () => this.endDate.value,
  };

  endDateValidation: IDateValidation = {
    getControl: () => this.endDate,
    hasError: () => this.dateHasError(this.endDate),
    errorMessage: () => this.getStartOrEndDateErrorMessage(false),
    minDate: () => this.startDate.value,
    maxDate: () => {
      // no more than 90 days including the start and end dates so add 89 days
      return convertToMoment(this.startDate.value).add(
        this._dateRangeNumberOfDaysBoundExclusive,
        'days'
      );
    },
  };

  hasExistingCookie = false;

  displaySettingsSubscription: Subscription;
  user: UserInfo;

  private _filter1Options: FilterOption[];
  private _filter2Options: FilterOption[];
  filter1ValueOptions$: Observable<FilterOption[]>;
  filter2ValueOptions$: Observable<FilterOption[]>;
  subs: Subscription[] = [];
  formValueCache: { [key: string]: any; value: any } = {} as {
    [key: string]: any;
    value: any;
  };
  filter1SearchValue: string;
  filter2SearchValue: string;
  filter1ControlLabel: string;
  filter2ControlLabel: string;

  filter1ErrorMessage: string;

  isUrlParamsChecked = false;

  @ViewChild('filter1Auto', { static: false }) filter1Auto: MatAutocomplete;
  @ViewChild('filter1AutoInput', { static: false })
  filter1AutoInput: ElementRef;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private accountService: AccountService,
    private taService: TimeAndAttendanceService,
    private cd: ChangeDetectorRef
  ) {}

  private _normalizeSearchValue(value: string) {
    return value ? value.trim().toLowerCase().replace(/\s/g, '') : '';
  }

  ngOnInit() {
    this.subs.push(
      this.accountService
        .getUserInfo()
        .pipe(
          map((user) => (this.user = user)),
          switchMap((_) =>
            merge(
              this.category1Value.valueChanges.pipe(
                tap((x) => this.handleCategory1ValueChange(x))
              ),
              this.category2Value.valueChanges.pipe(
                tap((x) => this.handleCategory2ValueChange(x))
              ),
              this.payPeriod.valueChanges.pipe(
                tap(() => {
                  const s = this.prepareSearchSettingsModel();
                  this.persistedSearchSettingsChanged.emit(s);
                })
              ),
              this.persistedSearchSettings.valueChanges.pipe(
                tap((search) => {
                  if (this.persistedSearchSettings.valid) {
                    const s = this.prepareSearchSettingsModel();
                    this.persistedSearchSettingsChanged.emit(s);
                  }
                })
              )
            )
          )
        )
        .subscribe()
    );

    this.filter1ValueOptions$ = this.filter1Value.valueChanges.pipe(
      startWith(''),
      map((search) => {
        this.filter1ErrorMessage = null;

        if (this._isObject(search)) {
          return search;
        } else {
          this.filter1SearchValue = this._normalizeSearchValue(search);
        }
      }),
      switchMap((_) =>
        this.taService.data$.pipe(
          switchMap((data) => {
            const categoryValue = +this.f.get('category1Value').value;
            return categoryValue != null
              ? of(data.filterValues[categoryValue] || [])
              : of([]);
          })
        )
      ),
      map((filterOptions: FilterOption[]) =>
        filterOptions.filter((option) =>
          option && option.filter
            ? this._normalizeSearchValue(option.filter).includes(
                this.filter1SearchValue
              )
            : false
        )
      ),
      tap((val) => {
        this._filter1Options = val;

        if (
          !this.isUrlParamsChecked &&
          this._filter1Options &&
          this._filter1Options.length
        ) {
          this.isUrlParamsChecked = true;
          this._applyQueryParameters(val);
        }
      })
    );

    this.filter2ValueOptions$ = this.filter2Value.valueChanges.pipe(
      startWith(''),
      map((search) => {
        if (this._isObject(search)) {
          return search;
        } else {
          this.filter2SearchValue = this._normalizeSearchValue(search);
        }
      }),
      switchMap((_) =>
        this.taService.data$.pipe(
          switchMap((data) => {
            const categoryValue = +this.f.get('category2Value').value;
            return categoryValue != null
              ? of(data.filterValues[categoryValue] || [])
              : of([]);
          })
        )
      ),
      map((filterOptions: FilterOption[]) =>
        filterOptions.filter((option) =>
          option && option.filter
            ? this._normalizeSearchValue(option.filter).includes(
                this.filter2SearchValue
              )
            : false
        )
      ),
      tap((val) => (this._filter2Options = val))
    );

    this.updateFormWithUrlParams();
  }

  /**
   * Unsubscribe from observables on the page to avoid memory leaks or unintentional page behavior.
   */
  ngOnDestroy() {
    if (this.displaySettingsSubscription)
      this.displaySettingsSubscription.unsubscribe();

    if (this.subs && this.subs.length)
      this.subs.forEach((s) => {
        if (!s.closed) s.unsubscribe();
      });
  }

  focusOnAutocomplete(key: number) {
    const formControlName = `filter${key}Value`;
    const formControl = this.f.get(formControlName);
    this.formValueCache[formControlName] = formControl.value;

    if (formControl) formControl.setValue(null);
  }

  focusOffAutocomplete(key: number) {
    const formControlName = `filter${key}Value`;
    const formControl = this.f.get(formControlName);

    if (
      !this._isObject(formControl.value) &&
      this._isObject(this.formValueCache[formControlName])
    ) {
      formControl.setValue(this.formValueCache[formControlName]);
    }
  }

  matAutocompleteEnterPress(event) {
    console.dir(event);
    event.preventDefault();
    event.stopPropagation();
  }

  private _applyQueryParameters(options: FilterOption[]) {
    const category1FormValue = this.f.get('category1Value').value;
    const categoryValue = coerceNumberProperty(category1FormValue);

    if (categoryValue && this.urlParams.supervisorId != null) {
      if (this.urlParams.supervisorId > 0) {
        const supervisor = options.find(
          (opt) => opt.id == this.urlParams.supervisorId
        );
        this.f.get('filter1Value').setValue(supervisor);
      }

      console.dir(this.persistedSearchSettings);

      if (this.persistedSearchSettings.valid) {
        const s = this.prepareSearchSettingsModel();

        this.subs.push(
          this.taService.data$
            .pipe(
              first(),
              tap((_) => {
                this.sendSearch();
              })
            )
            .subscribe()
        );

        this.persistedSearchSettingsChanged.emit(s);
      }
    }
  }

  /**
   * @param formValue The FormGroup value.
   * @param fieldNumber Either 1 OR 2. This is used to grab the correct category values and
   * filter form control values.
   */
  shouldUseAutocompleteField(categoryValue: number): boolean {
    return [1, 2, 8, 3, 6, 7, 13, 14].includes(
      coerceNumberProperty(categoryValue)
    );
  }

  private createForm(): FormGroup {
    const frmGroup = this.fb.group({
      payPeriod: this.fb.control(null),
      approvalStatus: this.fb.control(1),
      empFilter: this.fb.control(0),
      startDate: this.fb.control(moment(), { updateOn: 'blur' }),
      endDate: this.fb.control(moment(), { updateOn: 'blur' }),
      employee: this.fb.control(''),
      category1Visible: this.fb.control(true),
      category2Visible: this.fb.control(false),
      category1Value: this.fb.control(0),
      category2Value: this.fb.control(0),
      filter1Visible: this.fb.control(false),
      filter2Visible: this.fb.control(false),
      filter1Value: this.fb.control(''),
      filter2Value: this.fb.control(''),
      totalPayPeriodOptions: this.fb.control(0),
      persistedSearchSettings: this.fb.group({
        daily: this.fb.control(null),
        weekly: this.fb.control(null),
        grand: this.fb.control(null),
        empsPerPage: this.fb.control(10, {
          updateOn: 'blur',
          validators: [Validators.max(25), Validators.min(1)],
        }),
        days: this.fb.control(1),
        clockEmployeeApproveHoursSettingsID: this.fb.control(null),
      }),
      currentPage: this.fb.control(0),
    });

    return frmGroup;
  }

  /**
   * Get boolean representing the error state of a given MatDatepicker.
   *
   * @param fc A formControl, which is assumed to be a MatDatepicker.
   */
  private dateHasError(fc: FormControl): boolean {
    const hasErrorMin = fc.hasError('matDatepickerMin');
    const hasErrorMax = fc.hasError('matDatepickerMax');
    const hasErrorParse = fc.hasError('matDatepickerParse');
    return hasErrorMin || hasErrorMax || hasErrorParse;
  }

  /**
   * Completely dependent on {start,end}DateValidation.{min,max}Date() functions.
   * Implemented in such a way that makes assumptions about what those are set as.
   * If you change those later, this function will probably break.
   *
   * @param isStartDate toggle for whether we're getting the error message for the startDate (if true),
   *                    or the endDate (if false).
   */
  private getStartOrEndDateErrorMessage(isStartDate: boolean): string {
    let msg = '';

    const dateValidationBeingValidated = isStartDate
      ? this.startDateValidation
      : this.endDateValidation;
    const controlBeingValidated = dateValidationBeingValidated.getControl();
    // const dateBeingValidated = dateValidationBeingValidated.getControl().value;

    const hasErrorMin = controlBeingValidated.hasError('matDatepickerMin');
    const hasErrorMax = controlBeingValidated.hasError('matDatepickerMax');
    const hasErrorParse = controlBeingValidated.hasError('matDatepickerParse');
    if (hasErrorParse) {
      msg += `${
        controlBeingValidated.getError('matDatepickerParse').text
      } is not a valid date`;

      // if (hasErrorMin || hasErrorMax) {
      //     msg += '\n';
      // }
    } else if (hasErrorMin || hasErrorMax) {
      // let dateError: string | Date | moment.Moment;
      // dateError = hasErrorMin ? controlBeingValidated.errors.matDatepickerMin.min :
      //                           controlBeingValidated.errors.matDatepickerMax.max;

      const date = isStartDate
        ? this.startDateValidation.minDate()
        : this.endDateValidation.maxDate();
      const preposition = isStartDate ? 'after' : 'before';
      const subjectStr = isStartDate ? 'end' : 'start';

      const dateStr = convertToMoment(date).format('MM/DD/YYYY');
      const numberOfDays = this._dateRangeNumberOfDaysBoundInclusive;
      msg += `Please choose a date within ${numberOfDays} days of the ${subjectStr}, on or ${preposition} ${dateStr}`;
    }

    return msg;
  }

  /**
   * Pass in instance of the form's formgroup 'persistedSearchSettings' and it will return DTO ready to
   * be sent to API.
   *
   * @param search FormGroup's persistendSearchSettings child FormGroup
   */
  private prepareSearchSettingsModel(): ApproveHourSettings {
    const search = this.persistedSearchSettings.value;
    return {
      clockEmployeeApproveHoursSettingsID:
        search.clockEmployeeApproveHoursSettingsID,
      clientId: search.clientId || this.user.selectedClientId(),
      userId: search.userId || this.user.userId,
      defaultDaysFilter: search.days,
      hideDailyTotals: !search.daily,
      hideWeeklyTotals: !search.weekly,
      hideGrandTotals: !search.grand,
      employeesPerPage: search.empsPerPage,
      showAllDays: search.days == 1 || search.days == 2 || search.days == 3,
      hideNoActivity: search.days == 4 || search.days == 5,
      payPeriod:
        this.payPeriod.value != null ? this.payPeriod.value.payrollId : 0,
    };
  }

  private updateCategory1Value(value: number, shouldEmit = true): void {
    value = coerceNumberProperty(value);
    this.category1Value.setValue(value, { emitEvent: false });
    if (shouldEmit)
      this.filterCategoryChanged.emit({ newVal: value, form: this.f.value });
    this.showCategory2 = value != 0 && value != -2 && value != 15;
    this.showFilter1 = value != 0 && value != -2;
    this.filter1ControlLabel = this._getFilterLabel(value);
  }

  private updateCategory2Value(value: number, shouldEmit = true): void {
    value = coerceNumberProperty(value);
    this.category2Value.setValue(value, { emitEvent: false });
    if (shouldEmit)
      this.filterCategory2Changed.emit({ newVal: value, form: this.f.value });
    this.showFilter2 = this.showCategory2 && value > 0;
    this.filter2ControlLabel = this._getFilterLabel(value);
  }

  private updateFormWithUrlParams() {
    this.urlParams = this.route.snapshot.params;
    const qParams = this.route.snapshot.queryParams;
    const fPayPeriod =
      this.payPeriod.value && this.payPeriod.value.payrollId != 0
        ? this.payPeriod.value.payrollId
        : 0;

    // MUST HAPPEN BEFORE URL PARAMS ARE UPDATED IN THE FORM
    if (this.initData && fPayPeriod) {
      const payPeriod =
        this.initData.clockEmployeeApproveHoursSettings.length &&
        this.initData.clockEmployeeApproveHoursSettings[0]
          ? this.initData.clockEmployeeApproveHoursSettings[0].payPeriod
          : null;

      if (payPeriod) {
        this.payPeriod.setValue(
          {
            payrollId: payPeriod,
          },
          { emitEvent: false }
        );
      }
    }

    if (
      this.urlParams.isApproved != null ||
      this.urlParams.supervisorId != null
    ) {
      const category1Value = this.urlParams.supervisorId == 0 ? -2 : 6;
      const shouldSend = category1Value == -2;
      this.updateCategory1Value(category1Value);
      this.updateCategory2Value(0, false);
      const isApprovedDdlVal = `${this.urlParams.isApproved}`.toBoolean() ? 2 : 3;

      this.f.patchValue(
        {
          days: 1,
          startDate: qParams
            ? moment(qParams.StartDate, MOMENT_FORMATS.US)
            : null,
          endDate: qParams ? moment(qParams.EndDate, MOMENT_FORMATS.US) : null,
          payPeriod: {
            payrollId: qParams ? 2 : null,
          },
          approvalStatus: isApprovedDdlVal,
        },
        { emitEvent: shouldSend }
      );

      if (shouldSend) {
        const s = this.prepareSearchSettingsModel();

        this.subs.push(
          this.taService.data$
            .pipe(
              first(),
              tap((_) => {
                this.sendSearch();
              })
            )
            .subscribe()
        );

        this.persistedSearchSettingsChanged.emit(s);
      }
    }
  }

  private handleCategory2ValueChange(x: number) {
    this.clearFilterValue(2);

    if (x == 0) {
      this.filter2Value.setValue(0, { emitEvent: false });
      this.showFilter2 = false;
    } else {
      this.showFilter2 = true;
      this.filterCategory2Changed.emit({ newVal: x, form: this.f.value });
    }

    this.filter2ControlLabel = this._getFilterLabel(x);
  }

  private _getFilterLabel(value: number): string {
    switch (coerceNumberProperty(value)) {
      case 1:
        return 'Cost Center';
      case 2:
        return 'Department';
      case 3:
        return 'Time Policy';
      case 4:
        return 'Shift';
      case 6:
        return 'Supervisor';
      case 7:
        return 'Employee';
      case 8:
        return 'Group';
      case 14:
        return 'Home Cost Center';
      case 13:
        return 'Home Department';
      case 15:
        return 'Home Group';
      default:
        return '';
    }
  }

  private handleCategory1ValueChange(x: number) {
    this.clearFilterValue(1);

    if (x == 0 || x == -2) {
      this.showFilter1 = false;
      this.showFilter2 = false;
      this.showCategory2 = false;

      this.f.patchValue(
        {
          filter1Value: '',
          category2Value: 0,
        },
        { emitEvent: false }
      );
    } else if (x == 7) {
      this.showCategory2 = false;
      this.showFilter2 = false;
      this.showFilter1 = true;

      this.category2Value.setValue(0, { emitEvent: false });
    } else if (
      x == 1 ||
      x == 2 ||
      x == 3 ||
      x == 4 ||
      x == 8 ||
      x == 6 ||
      x == 13 ||
      x == 14 ||
      x == 15
    ) {
      this.showCategory2 = true;
      this.showFilter2 = false;
      this.showFilter1 = true;

      this.category2Value.setValue(0, { emitEvent: false });
    } else {
      this.showCategory2 = true;
      this.showFilter2 = false;
      this.showFilter1 = false;
    }

    if (!this.showCategory2) this.clearFilterValue(2);

    if (x != 0) {
      this.filterCategoryChanged.emit({ newVal: x, form: this.f.value });
    }

    this.filter1ControlLabel = this._getFilterLabel(x);
  }

  getEmployeeContacts(employeeFilters: FilterOption[]): IContact[] {
    if (!employeeFilters || !employeeFilters.length) return [];
    return employeeFilters.map((ef) => {
      const nameArr = ef.filter.split(', ');
      return {
        firstName: nameArr[1],
        lastName: nameArr[0],
        employeeId: ef.id,
      } as IContact;
    });
  }

  sendSearch(): void {
    if (this.isFilterSubmitDisabled) {
      // Prevents form submission on "Enter" key while a form input is selected.
      // return;
    }

    const fv = this.f.value;
    if (this.showFilter1 && !fv.filter1Value) {
      const label = this._getFilterLabel(fv.category1Value);
      this.filter1ErrorMessage = `Please select a ${label.toLowerCase()}.`;
      return;
    }

    this.submitted = true;
    if (!this.persistedSearchSettings.valid) return;
    this.setPage = 0;
    this.searchParamsUpdated.emit(this._prepareSearchModel());
  }

  private hideFilterWhenListEmpty(
    categoryValue: any,
    filterItemLists: { [id: number]: FilterOption[] },
    toggleVisibility: (isVisible: boolean) => void
  ) {
    toggleVisibility(
      new Maybe(filterItemLists)
        .map((x) => x[categoryValue])
        .map((x) => x.length > 0)
        .valueOr(false)
    );
  }

  comparePayPeriod(periodOne: PayPeriod, periodTwo: PayPeriod): boolean {
    return periodOne.payrollId === periodTwo.payrollId;
  }

  displayFilterValue(selected: FilterOption): string {
    return selected != null ? selected.filter : '';
  }

  clearFilterValue(option: number) {
    if (option != 1 && option != 2) return;
    const formControlName = `filter${option}Value`;
    this[formControlName].setValue('', { emitEvent: false });
    this.formValueCache[formControlName] = '';
  }

  getPanelWidth(filterNumber: number, inputWidth: number): string {
    const filterOptions =
      filterNumber == 1 ? this._filter1Options : this._filter2Options;
    if (!filterOptions) return null;

    let longestString = '';
    for (let i = 0; i < filterOptions.length; i++) {
      const opt = filterOptions[i];
      if (!opt || !opt.filter) continue;
      if (opt.filter.length > longestString.length) longestString = opt.filter;
    }

    if (!longestString.length) return null;

    const size = CalculateSize.getSizeOfText(longestString);

    return size.width > inputWidth ? size.width + 16 + 'px' : null;
  }

  private _prepareSearchModel() {
    const fv = this.f.value;
    const isFv1Ac = this.shouldUseAutocompleteField(fv.category1Value);
    const isFv2Ac = this.shouldUseAutocompleteField(fv.category2Value);

    if (isFv1Ac && this._isObject(fv.filter1Value)) {
      fv.filter1Value = fv.filter1Value.id;
    } else if (isFv1Ac) {
      fv.filter1Value = fv.filter1Value;
    }

    if (isFv2Ac && this._isObject(fv.filter2Value)) {
      fv.filter2Value = fv.filter2Value.id;
    } else if (isFv2Ac) {
      fv.filter2Value = fv.filter2Value;
    }

    fv.startDate = moment(fv.startDate).format('YYYY-MM-DD');
    fv.endDate = moment(fv.endDate).format('YYYY-MM-DD');

    return fv;
  }

  private _isObject(target): boolean {
    return typeof target === 'object' && target != null;
  }
}

interface IDateValidation {
  getControl(): FormControl;
  hasError(): boolean;
  errorMessage(): string;
  minDate(): any;
  maxDate(): any;
}
