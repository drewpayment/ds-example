import { coerceNumberProperty } from "@angular/cdk/coercion";
import { Injectable } from "@angular/core";
import { FormArray, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from "@angular/forms";
import { AccountService } from "@ds/core/account.service";
import { ClientAccrual, ClientEarning, IClientAccrual } from "@ds/core/employee-services/models";
import { ClientAccrualEarning } from "@ds/core/employee-services/models/client-accrual-earning.model";
import { ClientAccrualFirstYearSchedule } from "@ds/core/employee-services/models/client-accrual-first-year-schedule.model";
import { ClientAccrualSchedule, ServiceFrequencyType, ServiceStartEndFrequencyType } from "@ds/core/employee-services/models/client-accrual-schedule.model";
import { convertToMoment } from "@ds/core/shared/convert-to-moment.func";
import * as moment from 'moment';
import { BehaviorSubject, combineLatest, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, shareReplay, switchMap, take, tap } from 'rxjs/operators';
import { ClientAccrualConstants } from "../../models/leave-management/client-accrual-constants";
import { LeaveManagementApiService } from "./leave-management-api.service";

@Injectable({
  providedIn: "root",
})
export class ClientAccrualsStoreService {

  ////////////////
  // Main form. //
  ////////////////
  private _form: FormGroup;
  get form(): FormGroup {
    return this._form || (this._form = this.createForm());
  }

  get formHasUnsavedChanges() {
    return (
      (this.form.touched && this.form.dirty)
      || (this._isEditingAccrualSchedules$.value && !this._isKeepOriginalSchedules$.value)
      || (this._isEditingFirstYearAccrualSchedules$.value && !this._isKeepOriginalProratedSchedules$.value)
    );
  }

  get clientAccrualSchedulesForm() {
    return this.form.get("accrual.clientAccrualSchedules") as FormArray;
  }

  get clientAccrualProratedSchedulesForm() {
    return this.form.get("accrual.clientAccrualProratedSchedules") as FormArray;
  }

  get clientAccrualProratedSchedulesValueChanges$() {
    const valueChanges = this.clientAccrualProratedSchedulesForm.valueChanges;
    return valueChanges as Observable<ClientAccrualFirstYearSchedule[]>;
  }

  get clientAccrualProratedSchedules() {
    const value = this.clientAccrualProratedSchedulesForm.value;
    return value as ClientAccrualFirstYearSchedule[];
  }

  get selectedClientAccrualIdForm() {
    return this.form.get('accrual.clientAccrualId');
  }

  get selectedClientAccrualId() {
    return coerceNumberProperty(this.selectedClientAccrualIdForm.value);
  }

  get isNewAccrual() {
    return (this.selectedClientAccrualId <= ClientAccrualConstants.NEW_ENTITY_ID);
  }

  private _selectedClientId$ = new BehaviorSubject<number>(-1);
  selectedClientId$ = this._selectedClientId$.asObservable();

  private _clientAccruals$ = new BehaviorSubject<IClientAccrual[]>([
    this.newClientAccrualKernel
  ]);
  clientAccruals$ = this._clientAccruals$.asObservable();

  get clientAccruals() { return this._clientAccruals$.value; }

  get newClientAccrualKernel() {
    // cleanFormModel should match values used in this.createForm().
    const cleanFormModel = {
      clientId: '',
      clientAccrualId: ClientAccrualConstants.NEW_ENTITY_ID.toString(),
      clientAccrualIdUI: ClientAccrualConstants.NEW_ENTITY_ID.toString(),
      clientEarningId: '',
      description: '',
      employeeStatusId: '',
      employeeTypeId: '',
      isShowOnStub: true,
      accrualBalanceOptionId: '',
      serviceReferencePointId: '',
      planType: '',
      units: '',
      isShowPreviewMassages: '',
      notes: '',
      beforeAfterId: '',
      beforeAfterDate: '',
      carryOverToId: '',
      isUseCheckDates: false,
      isLeaveManagment: false,
      leaveManagmentAdministrator: '',
      requestMinimum: '',
      requestMaximum: '',
      hoursInDay: '',
      atmInterfaceCode: '',
      isPeriodEnd: false,
      isPeriodEndPlusOne: false,
      showAccrualBalanceStartingFrom: '',
      isAccrueWhenPaid: true,
      accrualCarryOverOptionId: 1,
      isEmailSupervisorRequests: '',
      isCombineByEarnings: '',
      isLeaveManagementUseBalanceOption: '',
      isRealTimeAccruals: '',
      isActive: true,
      accrualClearOptionId: '',
      isStopLastPay: '',
      carryOverToBalanceLimit: false,
      requestIncrement: '',
      policyLink: '',
      projectAmount: '',
      projectAmountType: 1,
      isEmpEmailRequest: '',
      isEmpEmailApproval: '',
      isEmpEmailDecline: '',
      isSupEmailRequest: '',
      isSupEmailApproval: '',
      isCaEmailRequest: '',
      isCaEmailApproval: '',
      isCaEmailDecline: '',
      isLmaEmailRequest: '',
      isLmaEmailApproval: '',
      isLmaEmailDecline: '',
      balanceOptionAmount: '',
      lmMinAllowedBalance: '',
      isDisplay4Decimals: '',
      waitingPeriodValue: '',
      waitingPeriodTypeId: '',
      waitingPeriodReferencePoint: 1,
      allowAllDays: '',
      isPaidLeaveAct: false,
      hoursPerWeekAct: 0,
      allowHoursRollOver: '',
      proratedServiceReferencePointOverrideId: '',
      proratedWhenToAwardTypeId: ClientAccrualConstants.PRORATEDWHENTOAWARD,
      autoApplyAccrualPolicyOptionId: '',
      searchEarningsCtrl: '',
      clientEarnings: [],
      // FormArrays
      // clientAccrualSchedules: [],
      // clientAccrualProratedSchedules: [],
    };
    return {
      ...cleanFormModel,
      clientAccrualId: ClientAccrualConstants.NEW_ENTITY_ID.toString(),
      description: "-- Add Policy --",
      isActive: true,
      isShowOnStub: true,
      isLeaveManagment: false,
      accrualCarryOverOptionId: 1,
      waitingPeriodReferencePoint: 1,
      proratedWhenToAwardTypeId: ClientAccrualConstants.PRORATEDWHENTOAWARD,
    } as any;
  }

  /////////////////
  // Misc state. //
  /////////////////
  // Manually handling routerLinkActive, since we are no longer using routerLink for nav.
  private _isSetupTab$ = new BehaviorSubject<boolean>(true);
  public get isSetupTab() {
    return this._isSetupTab$.value;
  }
  public set isSetupTab(value) {
    this._isSetupTab$.next(value);
    this._isSchedulesTab$.next(!value);
    if (value) {
      this._currentTabName$.next('setup');
    } else {
      this._currentTabName$.next('schedules');
    }
  }
  private _isSchedulesTab$ = new BehaviorSubject<boolean>(false);
  public get isSchedulesTab() {
    return this._isSchedulesTab$.value;
  }
  public set isSchedulesTab(value) {
    this._isSchedulesTab$.next(value);
    this._isSetupTab$.next(!value);
    if (value) {
      this._currentTabName$.next('schedules');
    } else {
      this._currentTabName$.next('setup');
    }
  }

  private _currentTabName$ = new BehaviorSubject<string>(null);
  currentTabName$ = this._currentTabName$.asObservable();

  private _isEditingAccrualSchedules$ = new BehaviorSubject<boolean>(false);
  isEditingAccrualSchedules$ = this._isEditingAccrualSchedules$.asObservable();
  private _isEditingFirstYearAccrualSchedules$ = new BehaviorSubject<boolean>(false);
  isEditingFirstYearAccrualSchedules$ = this._isEditingFirstYearAccrualSchedules$.asObservable();

  canChangePolicy$: Observable<boolean> = combineLatest([
      this._isSetupTab$,
      this._isEditingAccrualSchedules$,
      this._isEditingFirstYearAccrualSchedules$
    ])
    .pipe(
      distinctUntilChanged(),
      map(([setupTab, editingSchedules, editingProrated]) => {
        return (setupTab || (!editingSchedules && !editingProrated));
      })
    );

  private _isKeepOriginalSchedules$ = new BehaviorSubject<boolean>(false);
  isKeepOriginalSchedules$ = this._isKeepOriginalSchedules$.asObservable();

  private _isKeepOriginalProratedSchedules$ = new BehaviorSubject<boolean>(false);
  isKeepOriginalProratedSchedules$ = this._isKeepOriginalProratedSchedules$.asObservable();


  private _isResetForm$ = new BehaviorSubject<boolean>(false);
  isResetForm$ = this._isResetForm$.asObservable();
  // Signals that the reset is triggered by change in selected accrual policy.
  // Currently used such that _isResetForm$ must also be true,
  // in order for _isResetFormViaPolicyChange$ to have any effect.
  private _isResetFormViaPolicyChange$ = new BehaviorSubject<boolean>(false);
  isResetFormViaPolicyChange$ = this._isResetFormViaPolicyChange$.asObservable();
  ////////////////

  private readonly _integerRegex = /^[0-9]\d*$/;
  private readonly _integerRegexValidator = Validators.pattern(this._integerRegex);

  private readonly _floatRegex = /^(?:[1-9]\d*|0)?(?:\.\d+)?$/;
  private readonly _floatRegexValidator = Validators.pattern(this._floatRegex);


  constructor(private fb: FormBuilder) {}

  updateIsKeepOriginalSchedules(value: boolean) { this._isKeepOriginalSchedules$.next(value); }

  updateIsKeepOriginalProratedSchedules(value: boolean) { this._isKeepOriginalProratedSchedules$.next(value); }

  unsetIsResetForm = () => this._isResetForm$.next(false);
  setIsResetForm = () => this._isResetForm$.next(true);
  unsetIsResetFormViaPolicyChange = () => this._isResetFormViaPolicyChange$.next(false);
  setIsResetFormViaPolicyChange = () => this._isResetFormViaPolicyChange$.next(true);

  resetFormChanges(): void {
    this.setIsResetForm();
    this.selectedClientAccrualIdForm.setValue(
      this.selectedClientAccrualId.toString()
    );
  }

  getClientAccrualIndex(clientAccrualId: number|string): number {
    const id = coerceNumberProperty(clientAccrualId);
    return this.clientAccruals.findIndex(x => x.clientAccrualId == id);
  }

  replaceClientAccrualAtIndex = (index: number, accrual: IClientAccrual) => {
    const clientAccruals = this._clientAccruals$.value;
    clientAccruals[index] = accrual;
    this._clientAccruals$.next(clientAccruals);
  }

  removeClientAccrualAtIndex = (index: number, clientAccrualId: number) => {
    const clientAccruals = this._clientAccruals$.value;
    if (clientAccrualId == clientAccruals[index].clientAccrualId) {
      // Set the form back to "--Add Policy--" option.
      this.setIsResetForm();
      this.selectedClientAccrualIdForm.setValue(clientAccruals[0].clientAccrualId);
      clientAccruals.splice(index, 1);
      this._clientAccruals$.next(clientAccruals);
    }
  }

  pushClientAccrual = (accrual: IClientAccrual) => {
    const clientAccruals = this._clientAccruals$.value;
    clientAccruals.push(accrual);
    this._clientAccruals$.next(clientAccruals);
  }

  loadClientAccruals = (
    accountService: AccountService,
    leaveManagementService: LeaveManagementApiService
  ) => {
    accountService
    .getUserInfo()
    .pipe(
      switchMap((user) => {
        this._selectedClientId$.next(user.selectedClientId());
        return leaveManagementService.getClientAccrualsDropdownList(this._selectedClientId$.value);
      }),
      take(1)
    )
    .subscribe((accruals: IClientAccrual[]) => {
      this._clientAccruals$.next([
        this.newClientAccrualKernel,
        ...accruals,
      ]);
    });
  }

  updateAccrualSchedulesEditStatus = (isEditing: boolean) => {
    this._isEditingAccrualSchedules$.next(isEditing);
  }

  updateFirstYearAccrualSchedulesEditStatus = (isEditing: boolean) => {
    this._isEditingFirstYearAccrualSchedules$.next(isEditing);
  }

  createClientAccrualScheduleForm(
    x: ClientAccrualSchedule = {} as ClientAccrualSchedule
  ): FormGroup {
    return this.fb.group({
      accrualBalanceLimit: this.fb.control(this._selfOrEmptyString(x.accrualBalanceLimit), [this._floatRegexValidator]),
      balanceLimit: this.fb.control(this._selfOrEmptyString(x.balanceLimit), [this._floatRegexValidator]),
      carryOver: this.fb.control(this._selfOrEmptyString(x.carryOver), [this._floatRegexValidator]),
      clientAccrualId: this.fb.control(this._selfOrEmptyString(x.clientAccrualId)),
      clientAccrualScheduleId: this.fb.control(this._selfOrNewEntityId(x.clientAccrualScheduleId)),
      rateCarryOverMax: this.fb.control(this._selfOrEmptyString(x.rateCarryOverMax), [this._floatRegexValidator]),
      renewEnd: this.fb.control(this._selfOrEmptyString(x.renewEnd), [this._floatRegexValidator]),
      reward: this.fb.control(this._selfOrEmptyString(x.reward), [Validators.required, Validators.maxLength(11), this._floatRegexValidator]),
      serviceCarryOverFrequencyId: this.fb.control(this._selfOrEmptyString(x.serviceCarryOverFrequencyId)),
      serviceCarryOverTill: this.fb.control(this._selfOrEmptyString(x.serviceCarryOverTill), [this._floatRegexValidator]),
      serviceCarryOverTillFrequencyId: this.fb.control(this._selfOrEmptyString(x.serviceCarryOverTillFrequencyId)),
      serviceCarryOverWhenFrequencyId: this.fb.control(this._selfOrEmptyString(x.serviceCarryOverWhenFrequencyId)),

      serviceEnd: this.fb.control(this._selfOrEmptyString(x.serviceEnd), [
        Validators.min(0), this._integerRegexValidator
      ]),
      serviceEndFrequencyId: this.fb.control(this._selfOrEmptyString(x.serviceEndFrequencyId), []),

      serviceFrequencyId: this.fb.control(this._selfOrEmptyString(x.serviceFrequencyId), [Validators.required]),
      serviceRenewFrequencyId: this.fb.control(this._selfOrEmptyString(x.serviceRenewFrequencyId)),
      serviceRewardFrequencyId: this.fb.control(this._selfOrEmptyString(x.serviceRewardFrequencyId), [Validators.required]),

      serviceStart: this.fb.control(this._selfOrEmptyString(x.serviceStart), [
        Validators.required, Validators.min(0), this._integerRegexValidator
      ]),
      serviceStartFrequencyId: this.fb.control(this._selfOrEmptyString(x.serviceStartFrequencyId), [Validators.required]),

      modified: this.fb.control(this._selfOrEmptyString(x.modified)),
      modifiedBy: this.fb.control(this._selfOrEmptyString(x.modifiedBy)),
    }, {
      validators: [
        this._serviceEndServiceFrequencyIdCrossValidator_serviceStartEndServiceMaxCrossValidator
      ]
    });
  }

  createClientAccrualPaidLeaveActScheduleForm(): FormGroup {
    const paidLeaveActScheduleDto = {
      accrualBalanceLimit: ClientAccrualConstants.COL_SVACCAMTLMT,
      balanceLimit: ClientAccrualConstants.COL_BALLMT,
      carryOver: ClientAccrualConstants.COL_CARROVR,
      clientAccrualId: ClientAccrualConstants.NEW_ENTITY_ID,
      clientAccrualScheduleId: ClientAccrualConstants.NEW_ENTITY_ID,
      rateCarryOverMax: ClientAccrualConstants.COL_RTCRROVRMX,
      renewEnd: ClientAccrualConstants.COL_RENWEND,
      reward: ClientAccrualConstants.COL_REWARD,
      serviceCarryOverFrequencyId: ClientAccrualConstants.COL_SVCRROVRFRQ,
      serviceCarryOverTill: ClientAccrualConstants.COL_SVCRROVRTIL,
      serviceCarryOverTillFrequencyId: ClientAccrualConstants.COL_SVCRROVRTILFQ,
      serviceCarryOverWhenFrequencyId: ClientAccrualConstants.COL_SVCRRWNFRQ,
      serviceEnd: ClientAccrualConstants.COL_SVEND,
      serviceEndFrequencyId: ClientAccrualConstants.COL_SVENDFRQ,
      serviceFrequencyId: ClientAccrualConstants.COL_SVFRQ,
      serviceRenewFrequencyId: ClientAccrualConstants.COL_SVRENEWFRQ,
      serviceRewardFrequencyId: ClientAccrualConstants.COL_SVREWFRQ,
      serviceStart: ClientAccrualConstants.COL_SVSTRT,
      serviceStartFrequencyId: ClientAccrualConstants.COL_STRTFRQ,
    } as ClientAccrualSchedule;
    return this.createClientAccrualScheduleForm(paidLeaveActScheduleDto);
  }

  createClientAccrualProratedScheduleForm(
    x: ClientAccrualFirstYearSchedule = {} as ClientAccrualFirstYearSchedule
  ): FormGroup {
    return this.fb.group({
      awardAfterDaysValue: this.fb.control(this._selfOrEmptyString(x.awardAfterDaysValue), [this._integerRegexValidator]),
      awardOnDate: this.fb.control(this._selfOrEmptyString(x.awardOnDate)),
      awardValue: this.fb.control(this._selfOrEmptyString(x.awardValue), [Validators.required, this._floatRegexValidator]),
      clientAccrualId: this.fb.control(
        this._selfOrEmptyString(this.form.get('accrual.clientAccrualId').value || x.clientAccrualId)
      ),
      clientAccrualProratedScheduleId: this.fb.control(this._selfOrNewEntityId(x.clientAccrualProratedScheduleId)),
      scheduleFrom: this.fb.control(this._selfOrEmptyString(x.scheduleFrom), [Validators.required]),
      scheduleTo: this.fb.control(this._selfOrEmptyString(x.scheduleTo), [Validators.required]),
      modified: this.fb.control(this._selfOrEmptyString(x.modified)),
      modifiedBy: this.fb.control(this._selfOrEmptyString(x.modifiedBy)),
      clientId: this.fb.control(
        this._selfOrEmptyString(this._selectedClientId$.value || x.clientId)
      ),
    });
  }

  /**
  * Gets row with earliest scheduleTo date
  * @param rows
  * @returns Row with earliest scheduleTo date
  */
  private _getProratedScheduleFirstRow(
    rows: ClientAccrualFirstYearSchedule[]
  ): ClientAccrualFirstYearSchedule|null {
    if (!Array.isArray(rows) || !rows.length) {
      return null;
    }
    const firstRow = rows.reduce((prev, curr, currIndex) => {
      let result: ClientAccrualFirstYearSchedule;

      const prevFrom = (!prev) ? null : this._discardTimeAndConvertToMoment(prev.scheduleFrom);
      const currFrom = this._discardTimeAndConvertToMoment(curr.scheduleFrom);

      result = (!prev || prevFrom > currFrom) ? curr : prev;
      return result;
    });

    return firstRow;
  }

  /**
   * Gets row with latest scheduleTo date
   * @param rows
   * @returns Row with latest scheduleTo date
   */
  private _getProratedScheduleLastRow(
    rows: ClientAccrualFirstYearSchedule[]
  ): ClientAccrualFirstYearSchedule|null {
    if (!Array.isArray(rows) || !rows.length) {
      return null;
    }
    const lastRow = rows.reduce((prev, curr, currIndex) => {
      let result: ClientAccrualFirstYearSchedule;

      const prevTo = (!prev) ? null : this._discardTimeAndConvertToMoment(prev.scheduleTo);
      const currTo = this._discardTimeAndConvertToMoment(curr.scheduleTo);

      result = (!prev || prevTo < currTo) ? curr : prev;
      return result;
    });

    return lastRow;
  }

  /**
   * Shifts forward by whole months if from/to spans one or more whole months.
   * Else shifts forward by the number of days between from/to.
   * @param scheduleFrom
   * @param scheduleTo
   * @returns
   */
  getShiftedTimespan(
      scheduleFrom: string|Date|moment.Moment,
      scheduleTo: string|Date|moment.Moment
  ): {scheduleFrom: moment.Moment, scheduleTo: moment.Moment} {
    const result = {
      scheduleFrom: null as moment.Moment ,
      scheduleTo: null as moment.Moment
    };

    const basisScheduleFrom = this._discardTimeAndConvertToMoment(scheduleFrom);
    const basisScheduleTo = this._discardTimeAndConvertToMoment(scheduleTo);

    const fromIsFirstDayOfMonth = (basisScheduleFrom.date() === 1);
    const toIsLastDayOfMonth = this._momentIsLastDayOfOwnMonth(basisScheduleTo);
    // If From is first day of a month,
    // and To is last day of a month,
    // assume they want to shift by months instead of the "raw days delta".
    const unit = (fromIsFirstDayOfMonth && toIsLastDayOfMonth)
      ? 'months'
      : 'days';
    // Get the number of units to shift dates by.
    let lastRowFromToUnitDelta = basisScheduleTo.diff(basisScheduleFrom, unit);
    // Add one to delta if shifting by months.
    if (unit === 'months') {
      lastRowFromToUnitDelta += 1;
    }

    result.scheduleFrom = basisScheduleFrom.clone()
      .add(lastRowFromToUnitDelta, unit);

    if (unit === 'days') {
      // Mutates moment directly.
      result.scheduleFrom.add(1, 'days');
    }

    result.scheduleTo = result.scheduleFrom.clone()
      .add(lastRowFromToUnitDelta, unit);

    if (unit === 'months') {
      // Mutates moment directly.
      result.scheduleTo.subtract(1, 'days');
    }

    return result;
  }

  // Generates the next form, based on the current existing prorated schedules rows.
  createNextClientAccrualProratedScheduleForm(): FormGroup {
    let nextRow: ClientAccrualFirstYearSchedule;

    const currentRows = this.clientAccrualProratedSchedules;

    // Check if this is the first row being created.
    if (!Array.isArray(currentRows) || !currentRows.length) {
      // Year doesn't actually matter.
      // But, need to pick one for the date,
      // so may as well just init it to this year.
      const currentYear = new Date().getFullYear();
      // Init the first row with {from: Jan 1, to: Jan 31}
      const firstDayOfCurrentYear = new Date(currentYear, 0, 1);
      const lastDayOfJanuaryOfCurrentYear = new Date(currentYear, 0, 31);
      nextRow = {
        awardValue: null,
        awardAfterDaysValue: null,
        awardOnDate: null,
        scheduleFrom: firstDayOfCurrentYear,
        scheduleTo: lastDayOfJanuaryOfCurrentYear,
      } as ClientAccrualFirstYearSchedule;
      return this.createClientAccrualProratedScheduleForm(nextRow);
    }

    // Row with earliest scheduleFrom date
    // const firstRow = this._getProratedScheduleFirstRow(currentRows);

    // Row with latest scheduleTo date
    const lastRow = this._getProratedScheduleLastRow(currentRows);

    const lastRowToMoment = this._discardTimeAndConvertToMoment(lastRow.scheduleTo);

    const nextRowFromMoment = lastRowToMoment.clone().add(1, 'days');
    let nextRowToMoment: moment.Moment;
    let tempMoment: moment.Moment;


    // Normalize tempMoment to be end of month of nextRowFromMoment.
    if (this._momentIsLastDayOfOwnMonth(nextRowFromMoment)) {
      tempMoment = nextRowFromMoment.clone();
    } else {
      tempMoment = nextRowFromMoment.clone().endOf('month');
    }

    // Shift forward to end of next month.
    tempMoment = tempMoment.clone()
      .add(1, 'days')
      .endOf('month');

    nextRowToMoment = this._discardTimeAndConvertToMoment(tempMoment);

    nextRow = {
      ...lastRow,
      awardValue: null,
      awardAfterDaysValue: null,
      awardOnDate: null,
      scheduleFrom: nextRowFromMoment,
      scheduleTo: nextRowToMoment,
    } as ClientAccrualFirstYearSchedule;

    return this.createClientAccrualProratedScheduleForm(nextRow);
  }


  prepareClientAccrualModel(form: FormGroup): IClientAccrual {
    const f = form.getRawValue();
    const empty = {} as ClientAccrual;

    // check for missing required properties...
    if (f.accrual == null || f.accrual.clientId == null) return empty;

    const dtoTemp = f.accrual as any;
    delete dtoTemp.clientAccrualIdUI; //this is not a real property
    const dto = dtoTemp as ClientAccrual;

    // Map ClientEarnings to ClientAccrualEarnings.
    const clientAccrualEarnings = ((f.accrual.clientEarnings || []) as ClientEarning[])
      .map(x => {
        const cae = {
            clientAccrualId: dto.clientAccrualId,
            clientAccrualEarningId: x.clientEarningId
        } as ClientAccrualEarning;
        return cae;
      });

    const schedules = (dto.clientAccrualSchedules || [] as ClientAccrualSchedule[])
      .map(x => {
        const cas = {
          ...x,
          clientAccrualId: coerceNumberProperty(dto.clientAccrualId),
          clientAccrualScheduleId: coerceNumberProperty(x.clientAccrualScheduleId),
        } as ClientAccrualSchedule;
        return cas;
      });

    const proratedSchedules = (
        dto.clientAccrualProratedSchedules
        || [] as ClientAccrualFirstYearSchedule[]
      )
      .map(x => {
        const caps = {
          ...x,
          clientAccrualId: coerceNumberProperty(dto.clientAccrualId),
          clientAccrualProratedScheduleId: coerceNumberProperty(x.clientAccrualProratedScheduleId),
        } as ClientAccrualFirstYearSchedule;
        return caps;
      });

    dto.clientId = dto.clientId || this._selectedClientId$.value;
    dto.description = dto.description || '';
    dto.isUseCheckDates = dto.isUseCheckDates || false;
    dto.clientAccrualEarnings = clientAccrualEarnings;
    dto.clientAccrualSchedules = schedules;
    dto.clientAccrualProratedSchedules = proratedSchedules;

    return dto;
  }

  private createForm(): FormGroup {
    return this.fb.group({
      accrual: this.fb.group({
        clientId: this.fb.control(''),
        clientAccrualId: this.fb.control(ClientAccrualConstants.NEW_ENTITY_ID.toString()),
        clientAccrualIdUI: this.fb.control(ClientAccrualConstants.NEW_ENTITY_ID.toString()),
        clientEarningId: this.fb.control("", [Validators.required]),
        description: this.fb.control("", [
          Validators.required,
          Validators.maxLength(35),
        ]),
        employeeStatusId: this.fb.control("", [Validators.required]),
        employeeTypeId: this.fb.control("", [Validators.required]), // PayType
        isShowOnStub: this.fb.control(true),
        accrualBalanceOptionId: this.fb.control("", [Validators.required]), // balance
        serviceReferencePointId: this.fb.control("", [Validators.required]), // reference method
        planType: this.fb.control("", [Validators.required]),
        units: this.fb.control("", [Validators.required]),
        isShowPreviewMassages: this.fb.control(""),
        notes: this.fb.control("", Validators.maxLength(4000)),
        beforeAfterId: this.fb.control(""),
        beforeAfterDate: this.fb.control(''),
        carryOverToId: this.fb.control(""),
        isUseCheckDates: this.fb.control(""),
        isLeaveManagment: this.fb.control(false),
        leaveManagmentAdministrator: this.fb.control({value: '', disabled: true}),
        requestMinimum: this.fb.control({value: '', disabled: true}),
        requestMaximum: this.fb.control({value: '', disabled: true}),
        hoursInDay: this.fb.control({value: '', disabled: true}, [Validators.required]),
        atmInterfaceCode: this.fb.control("", Validators.maxLength(4)),
        isPeriodEnd: this.fb.control(false),
        isPeriodEndPlusOne: this.fb.control(false),
        showAccrualBalanceStartingFrom: this.fb.control(""), // reference date
        isAccrueWhenPaid: this.fb.control(true),
        accrualCarryOverOptionId: this.fb.control(1, Validators.required),
        isEmailSupervisorRequests: this.fb.control(""),
        isCombineByEarnings: this.fb.control(""),
        isLeaveManagementUseBalanceOption: this.fb.control({value: '', disabled: true}),
        isRealTimeAccruals: this.fb.control({value: '', disabled: true}),
        isActive: this.fb.control(true),
        accrualClearOptionId: this.fb.control(""),
        isStopLastPay: this.fb.control(""),
        carryOverToBalanceLimit: this.fb.control(false),
        requestIncrement: this.fb.control({value: '', disabled: true}),
        policyLink: this.fb.control(""),
        projectAmount: this.fb.control({value: '', disabled: true}),
        projectAmountType: this.fb.control({value: 1, disabled: true}),
        isEmpEmailRequest: this.fb.control(""),
        isEmpEmailApproval: this.fb.control(""),
        isEmpEmailDecline: this.fb.control(""),
        isSupEmailRequest: this.fb.control({value: '', disabled: true}),
        isSupEmailApproval: this.fb.control(""),
        isCaEmailRequest: this.fb.control(""),
        isCaEmailApproval: this.fb.control(""),
        isCaEmailDecline: this.fb.control(""),
        isLmaEmailRequest: this.fb.control(""),
        isLmaEmailApproval: this.fb.control(""),
        isLmaEmailDecline: this.fb.control(""),
        balanceOptionAmount: this.fb.control(""),
        lmMinAllowedBalance: this.fb.control(""),
        isDisplay4Decimals: this.fb.control(""),
        waitingPeriodValue: this.fb.control("", Validators.maxLength(3)),
        waitingPeriodTypeId: this.fb.control(""),
        waitingPeriodReferencePoint: this.fb.control(1),
        allowAllDays: this.fb.control({value: '', disabled: true}),
        isPaidLeaveAct: this.fb.control({value: false, disabled: true}),
        hoursPerWeekAct: this.fb.control({value: 0, disabled: true}),
        allowHoursRollOver: this.fb.control({value: '', disabled: true}),
        proratedServiceReferencePointOverrideId: this.fb.control(""),
        proratedWhenToAwardTypeId: this.fb.control(ClientAccrualConstants.PRORATEDWHENTOAWARD),
        // autoApplyAccrualPolicyOptionId does not exist on the ClientAccrual record directly.
        // It is passed as a separate flag when saving a ClientAccrual.
        autoApplyAccrualPolicyOptionId: this.fb.control(''),
        searchEarningsCtrl: this.fb.control(''),
        clientEarnings: this.fb.control([]),

        clientAccrualSchedules: this.fb.array([]),
        clientAccrualProratedSchedules: this.fb.array([]),

      }, {validators: [this._proratedWhenToAwardTypeCrossValidator]}),
    });
  }

  /////////////////////////////////
  // Cross Validation Functions. //
  /////////////////////////////////

  /**
   * Cross-validation on the `this.form.get("accrual")` FormGroup.
   * Requires one of:
   * - `accrual.clientAccrualProratedSchedules[index].awardAfterDaysValue`
   * - `accrual.clientAccrualProratedSchedules[index].awardOnDate`
   * depending on value of `accrual.proratedWhenToAwardTypeId`.
   * @param accrualForm
   * @returns ValidationErrors
   */
  private _proratedWhenToAwardTypeCrossValidator: ValidatorFn = (
    accrualForm: FormGroup
  ): ValidationErrors | null => {
    const errors = {} as ValidationErrors;
    let error: ValidationErrors;

    const proratedWhenToAwardTypeIdForm = accrualForm.get("proratedWhenToAwardTypeId") as FormControl;
    const isAwardByDay = (proratedWhenToAwardTypeIdForm.value == 1);
    const isAwardByDate = (proratedWhenToAwardTypeIdForm.value == 2);

    const proratedSchedulesFormArray = accrualForm.get("clientAccrualProratedSchedules") as FormArray;
    const proratedSchedulesRows = proratedSchedulesFormArray.controls as FormGroup[];

    proratedSchedulesRows.forEach((proratedRow, index) => {
      const awardAfterDaysValueForm = proratedRow.get("awardAfterDaysValue") as FormControl;
      error = Validators.required(awardAfterDaysValueForm);

      if (error != null && isAwardByDay) {
        awardAfterDaysValueForm.setErrors(error);
        // awardAfterDaysValueForm.setErrors({...awardAfterDaysValueForm.errors, ...error});
        errors[`clientAccrualProratedSchedules[${index}].awardAfterDaysValue`] = error;
      } else {
        // Not required, so remove any errors from Validators.required,
        // but keep any other validation errors that may already exist on the control.
        const currentErrors = awardAfterDaysValueForm.errors;
        if (currentErrors != null) {
          delete currentErrors.required;
          awardAfterDaysValueForm.setErrors(
            this._isEmptyObject(currentErrors) ? null : currentErrors
          );
        }
      }

      const awardOnDateForm = proratedRow.get("awardOnDate") as FormControl;
      error = Validators.required(awardOnDateForm);

      if (error != null && isAwardByDate) {
        // Is required, so add error.
        awardOnDateForm.setErrors(error);
        // awardOnDateForm.setErrors({...awardOnDateForm.errors, ...error});
        errors[`clientAccrualProratedSchedules[${index}].awardOnDate`] = error;
      } else {
        // Not required, so remove any errors from Validators.required,
        // but keep any other validation errors that may already exist on the control.
        const currentErrors = awardOnDateForm.errors;
        if (currentErrors != null) {
          delete currentErrors.required;
          awardOnDateForm.setErrors(
            this._isEmptyObject(currentErrors) ? null : currentErrors
          );
        }
      }
    });

    // Discard errors on the parent accrual FormGroup,
    // if the the relevant control for cross-validation is disabled.
    return (this._isEmptyObject(errors) || proratedSchedulesFormArray.disabled)
      ? null
      : errors;
  }

  /**
   * Cross-validating for prorated schedules rows/tiers.
   * proratedNoGapsBetweenTiers
   * proratedRowsCompleteFullAnnualCycle
   *
   * This is a relatively expensive validation to run,
   * so we make it public in order to manually check this validation
   * before attempting to save.
   * @param proratedSchedulesFormArray
   * @returns ValidationErrors
   */
  proratedNoGapsBetweenTiers_proratedRowsCompleteFullAnnualCycle: ValidatorFn = (
    proratedSchedulesFormArray: FormArray
  ): ValidationErrors | null => {
    const errors = {} as ValidationErrors;
    const proratedSchedulesFormArrayCurrentErrors = proratedSchedulesFormArray.errors || {};
    let error: ValidationErrors;
    let errorKey: string;

    // const proratedSchedulesFormArray = accrualForm.get("clientAccrualProratedSchedules") as FormArray;
    const proratedSchedulesFormRows = proratedSchedulesFormArray.controls as FormGroup[];
    const proratedSchedulesRowsFormsAndMoments = proratedSchedulesFormRows.map(row => {
      const scheduleFromForm   = row.get("scheduleFrom") as FormControl;
      const scheduleToForm     = row.get("scheduleTo")   as FormControl;
      const model              = row.value as ClientAccrualFirstYearSchedule;
      model.scheduleFrom       = this._discardTimeAndConvertToMoment(model.scheduleFrom);
      model.scheduleTo         = this._discardTimeAndConvertToMoment(model.scheduleTo);
      const result = {
        model,
        scheduleFromForm,
        scheduleToForm,
        // Property getters bind this to `result`.
        // (As opposed to `this` the `ClientAccrualsStoreService`.)
        get scheduleFromMoment() { return this.model.scheduleFrom as moment.Moment; },
        get scheduleToMoment()   { return this.model.scheduleTo as moment.Moment; },
      };
      return result;
    });

    if (proratedSchedulesRowsFormsAndMoments.length === 0) {
      return null;
    }

    // Remove any errors from any previous round of
    // `proratedNoGapsBetweenTiers_proratedRowsCompleteFullAnnualCycle`
    // validation.
    if (proratedSchedulesFormArrayCurrentErrors != null) {
      const matchingKeys = Object.keys(proratedSchedulesFormArrayCurrentErrors).filter(key => {
        const matches = key.match(this._proratedSchedulesFormArrayErrorKeysRegex);
        return matches.length > 0;
      });
      if (matchingKeys.length > 0) {
        matchingKeys.forEach(key => {
          delete proratedSchedulesFormArrayCurrentErrors[key];
        });
        proratedSchedulesFormArray.setErrors(
          this._isEmptyObject(proratedSchedulesFormArrayCurrentErrors)
            ? null
            : proratedSchedulesFormArrayCurrentErrors
        );
      }
    }

    /////////////////////////////////////////////////
    // #region proratedRowsCompleteFullAnnualCycle
    const proratedSchedulesRows = proratedSchedulesRowsFormsAndMoments.map(x => x.model);
    // Row with earliest scheduleFrom date
    const firstRow = this._getProratedScheduleFirstRow(proratedSchedulesRows);
    // Row with latest scheduleTo date
    const lastRow = this._getProratedScheduleLastRow(proratedSchedulesRows);

    if (firstRow == null) {
      return null;
    }

    const firstRowScheduleFromMoment = this._discardTimeAndConvertToMoment(firstRow.scheduleFrom);
    const lastRowScheduleToMoment = this._discardTimeAndConvertToMoment(lastRow.scheduleTo);

    const diffInYears = (
      (lastRowScheduleToMoment.clone().add(1, 'days'))
      .diff(
        firstRowScheduleFromMoment,
        'years',
        true
      )
    );
    const isOneYearMinusADayLater = (diffInYears === 1);

    // Need to get the indexes to match to the form array...
    const firstRowFormIndex = proratedSchedulesRows.indexOf(firstRow);
    const lastRowFormIndex = proratedSchedulesRows.indexOf(lastRow);

    const firstRowForm = proratedSchedulesFormRows[firstRowFormIndex];
    const lastRowForm = proratedSchedulesFormRows[lastRowFormIndex];

    // const firstRowFormScheduleFrom = firstRowForm.get('scheduleFrom');
    const lastRowFormScheduleTo = lastRowForm.get('scheduleTo');

    // const firstRowFormScheduleFromErrors = firstRowFormScheduleFrom.errors;
    const lastRowFormScheduleToErrors = lastRowFormScheduleTo.errors;

    errorKey = `clientAccrualProratedSchedules[${lastRowFormIndex}].scheduleTo`;

    if (isOneYearMinusADayLater) {
      // Valid, so remove any errors from 'proratedDoesNotCoverFullYear' key,
      // but keep any other validation errors that may already exist on the control.

      if (lastRowFormScheduleToErrors != null) {
        delete lastRowFormScheduleToErrors.proratedDoesNotCoverFullYear;
        lastRowFormScheduleTo.setErrors(
          this._isEmptyObject(lastRowFormScheduleToErrors)
            ? null
            : lastRowFormScheduleToErrors
        );
      }
      delete proratedSchedulesFormArrayCurrentErrors[errorKey];
    } else {
      error = { 'proratedDoesNotCoverFullYear': true };
      lastRowFormScheduleTo.setErrors(error);
      errors[errorKey] = error;
    }
    // Unset any error that may exist on the temp error var.
    error = null;
    errorKey = null;
    // #endregion proratedRowsCompleteFullAnnualCycle
    /////////////////////////////////////////////////

    /////////////////////////////////////////////////
    // #region proratedNoGapsBetweenTiers
    proratedSchedulesRowsFormsAndMoments.forEach((row, index, rows) => {
      const nextRow = rows[index + 1];
      if (nextRow != null) {
        const rowToNextRowFromDaysDelta = nextRow
          .scheduleFromMoment
          .diff(row.scheduleToMoment, 'days', true);

        if (rowToNextRowFromDaysDelta <= 0 || rowToNextRowFromDaysDelta > 1) {
          error = { 'gapInCoverage': true };
        }

        errorKey = `clientAccrualProratedSchedules[${index + 1}].scheduleFrom`;

        if (error != null) {

          nextRow.scheduleFromForm.setErrors(error);
          nextRow.scheduleFromForm.markAsTouched();
          errors[errorKey] = error;
        } else {
          // Valid, so remove any errors from 'gapInCoverage' key,
          // but keep any other validation errors that may already exist on the control.
          let currentErrors: ValidationErrors;

          currentErrors = nextRow.scheduleFromForm.errors;
          if (currentErrors != null) {
            delete currentErrors.gapInCoverage;
            nextRow.scheduleFromForm.setErrors(
              this._isEmptyObject(currentErrors) ? null : currentErrors
            );
          }

          delete proratedSchedulesFormArrayCurrentErrors[errorKey];
        }

        // reset the error
        error = null;
      }
    });
    // #endregion proratedNoGapsBetweenTiers
    /////////////////////////////////////////////////

    if (errors != null) {
      // Add the new errors, while keeping any existing errors.
      const mergedErrors = {
        ...proratedSchedulesFormArrayCurrentErrors,
        ...errors
      };
      proratedSchedulesFormArray.setErrors(mergedErrors);
    } else {
      // Remove any errors from any previous round of
      // `proratedNoGapsBetweenTiers_proratedRowsCompleteFullAnnualCycle`
      // validation. (We've been deleting them from
      // `proratedSchedulesFormArrayCurrentErrors` throughout is validation cycle.)
      proratedSchedulesFormArray.setErrors(
        this._isEmptyObject(proratedSchedulesFormArrayCurrentErrors)
          ? null
          : proratedSchedulesFormArrayCurrentErrors
      );
    }

    proratedSchedulesFormArray.updateValueAndValidity();

    // Discard errors on the parent accrual FormGroup,
    // if the the relevant control for cross-validation is disabled.
    return (this._isEmptyObject(errors) || proratedSchedulesFormArray.disabled)
      ? null
      : errors;
  }

  // tslint:disable-next-line
  private readonly _proratedSchedulesFormArrayErrorKeysRegex =
    /clientAccrualProratedSchedules\[\d+\].schedule(To|From)/g;

  /**
   * Cross-validation on the clientAccrualScheduleRow.
   *
   * Conditionally requires
   * - `clientAccrualScheduleRow.serviceEnd`
   * - `clientAccrualScheduleRow.serviceEndFrequencyId`
   * depending on value of `clientAccrualScheduleRow.serviceFrequencyId`.
   *
   * See: https://dominionsystems.atlassian.net/browse/PAY-883
   * Remove validation within Accrual Schedule when Schedule Type is 'one time' or 'payroll'
   *
   * Cross-validation on the clientAccrualScheduleRow.
   * Only set max=99 if the service{start,end}FrequencyId is ServiceStartEndFrequencyType.Years.
   * @param clientAccrualScheduleRow
   * @returns ValidationErrors
   */
  private _serviceEndServiceFrequencyIdCrossValidator_serviceStartEndServiceMaxCrossValidator: ValidatorFn = (
    clientAccrualScheduleRow: FormGroup
  ): ValidationErrors | null => {
    const errors = {} as ValidationErrors;

    const cas = clientAccrualScheduleRow.getRawValue() as ClientAccrualSchedule;
    const isServiceStartRequired = this._isServiceStartRequired(cas);
    const isServiceEndRequired = this._isServiceEndRequired(cas);

    const serviceFormPairs = [
      {
        rowErrorKey: 'serviceStart',
        serviceValue: clientAccrualScheduleRow.get('serviceStart') as FormControl,
        serviceFreq: clientAccrualScheduleRow.get('serviceStartFrequencyId') as FormControl
      },
      {
        rowErrorKey: 'serviceEnd',
        serviceValue: clientAccrualScheduleRow.get('serviceEnd') as FormControl,
        serviceFreq: clientAccrualScheduleRow.get('serviceEndFrequencyId') as FormControl
      },
    ];

    const maxValidatorFn = Validators.max(99);

    serviceFormPairs.forEach(formPair => {
      let serviceValueMaxError: ValidationErrors = null;
      let serviceValueRequiredError: ValidationErrors = null;
      let serviceFreqRequiredError: ValidationErrors = null;

      serviceValueMaxError = maxValidatorFn(formPair.serviceValue);
      const serviceValueCurrentErrors = formPair.serviceValue.errors;
      const serviceFreqCurrentErrors = formPair.serviceFreq.errors;

      const isServiceStart_isServiceStartRequired = (formPair.rowErrorKey == 'serviceStart' && isServiceStartRequired);
      const isServiceEnd_isServiceEndRequired = (formPair.rowErrorKey == 'serviceEnd' && isServiceEndRequired);

      if (isServiceStart_isServiceStartRequired) {
        serviceValueRequiredError = Validators.required(formPair.serviceValue);
        serviceFreqRequiredError = Validators.required(formPair.serviceFreq);
      }
      if (isServiceEnd_isServiceEndRequired) {
        serviceValueRequiredError = Validators.required(formPair.serviceValue);
        serviceFreqRequiredError = Validators.required(formPair.serviceFreq);
      }

      // If ServiceStartEndFrequencyType.Years, cap the serviceValue to 99.
      const isServiceFreqYears = (formPair.serviceFreq.value == ServiceStartEndFrequencyType.Years);

      // Ensure we're adding/removing max and required errors appropriately.
      const setServiceValueErrorsFn = (
        currentErrors: ValidationErrors,
        pendingErrors: ValidationErrors,
        control: FormControl
      ): ValidationErrors => {
        if (currentErrors != null) {
          if (pendingErrors.max == null) {
            delete currentErrors.max;
          }
          if (pendingErrors.required == null) {
            delete currentErrors.required;
          }
          const e = this._isEmptyObject(currentErrors) ? null : currentErrors;
          control.setErrors(e);
          return e;
        }
      };

      // Set serviceStart errors
      if ((serviceValueMaxError != null && isServiceFreqYears) || isServiceStart_isServiceStartRequired) {
        let tempErrors = {};
        if (isServiceFreqYears) {
          tempErrors = {...tempErrors, ...serviceValueMaxError};
        }
        if (isServiceStart_isServiceStartRequired) {
          tempErrors = {...tempErrors, ...serviceValueRequiredError};
        }
        tempErrors = setServiceValueErrorsFn(
          {...serviceValueCurrentErrors, ...tempErrors},
          tempErrors,
          formPair.serviceValue
        );
        // errors[formPair.rowErrorKey] = tempErrors;
      }
      // Set serviceEnd errors
      else if ((serviceValueMaxError != null && isServiceFreqYears) || isServiceEnd_isServiceEndRequired) {
        let tempErrors = {};
        if (isServiceFreqYears) {
          tempErrors = {...tempErrors, ...serviceValueMaxError};
        }
        if (isServiceEnd_isServiceEndRequired) {
          tempErrors = {...tempErrors, ...serviceValueRequiredError};
        }
        tempErrors = setServiceValueErrorsFn(
          {...serviceValueCurrentErrors, ...tempErrors},
          tempErrors,
          formPair.serviceValue
        );
        // errors[formPair.rowErrorKey] = tempErrors;
      } else {
        setServiceValueErrorsFn(serviceValueCurrentErrors, {}, formPair.serviceValue);
      }

      if (serviceFreqRequiredError != null && isServiceEndRequired) {
        const tempErrors = {...serviceFreqCurrentErrors, ...serviceFreqRequiredError};
        formPair.serviceFreq.setErrors(tempErrors);
        // errors[`${formPair.rowErrorKey}FrequencyId`] = serviceFreqRequiredError;
      } 
      // Set serviceEndFrequencyId errors
      else if (serviceFreqRequiredError != null && isServiceEndRequired) {
        const tempErrors = {...serviceFreqCurrentErrors, ...serviceFreqRequiredError};
        formPair.serviceFreq.setErrors(tempErrors);
        // errors[`${formPair.rowErrorKey}FrequencyId`] = serviceFreqRequiredError;
      } else {
        if (serviceFreqCurrentErrors != null) {
          delete serviceFreqCurrentErrors.required;
          formPair.serviceFreq.setErrors(
            this._isEmptyObject(serviceFreqCurrentErrors) ? null : serviceFreqCurrentErrors
          );
        }
      }
    });

    // Discard errors on the parent accrual FormGroup,
    // if the the relevant control for cross-validation is disabled.
    return (this._isEmptyObject(errors) || clientAccrualScheduleRow.disabled)
      ? null
      : errors;
  }

  private _isServiceStartRequired(x: ClientAccrualSchedule = {} as ClientAccrualSchedule): boolean {
    // [{1: 'One Time'}, {4: 'First Payroll'}]
    return (
      x.serviceFrequencyId != null
      && !(
           x.serviceFrequencyId == ServiceFrequencyType["One Time"]
        || x.serviceFrequencyId == ServiceFrequencyType["First Payroll"]
      )
    );
  }

  private _isServiceEndRequired(x: ClientAccrualSchedule = {} as ClientAccrualSchedule): boolean {
    // [{1: 'One Time'}, {4: 'First Payroll'}]
    return (
      x.serviceFrequencyId != null
      && !(
           x.serviceFrequencyId == ServiceFrequencyType["One Time"]
        || x.serviceFrequencyId == ServiceFrequencyType["First Payroll"]
      )
    );
  }

  private _selfOrDefaultValue(x: any, defaultValue: any): any {
    return (x != null) ? x : defaultValue;
  }

  private _selfOrEmptyString(x: any): any {
    return this._selfOrDefaultValue(x, '');
  }

  private _selfOrNewEntityId(x: any): any {
    return this._selfOrDefaultValue(x, ClientAccrualConstants.NEW_ENTITY_ID);
  }

  /**
   * Checks whether object is equivalent to empty object.
   * See: https://stackoverflow.com/a/32108184/13188284
   * @param obj object to check if equal to empty object.
   * @returns result.
   */
  private _isEmptyObject(obj: any): boolean {
    // because Object.keys(new Date()).length === 0;
    // we have to do some additional check
    return (obj // null and undefined check
      && Object.keys(obj).length === 0 && obj.constructor === Object);
  }

  private _discardTimeAndConvertToMoment(
    value: string | Date | moment.Moment
  ): moment.Moment {
    // Double convertToMoment() to safely discard the time portion of the dates... Ugh.
    return convertToMoment(
      convertToMoment(value).format(moment.HTML5_FMT.DATE)
    );
  }

  private _momentIsLastDayOfOwnMonth(m: moment.Moment): boolean {
    return (m.date() === m.daysInMonth());
  }
}
