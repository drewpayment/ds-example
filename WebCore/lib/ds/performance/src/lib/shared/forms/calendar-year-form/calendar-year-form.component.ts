import { Component, OnInit, Input, Inject, OnDestroy, Output, EventEmitter, ViewContainerRef, ViewChild, ComponentFactoryResolver, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ValidatorFn, AbstractControl, FormControl } from '@angular/forms';
import { merge, Subject, combineLatest, Observable, defer } from 'rxjs';
import { takeUntil, tap, map, startWith, debounceTime } from 'rxjs/operators';
import * as moment from "moment";
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { ICalendarYearForm, IFormItem, IFormItemComponent } from './calendar-year-form.model';


import { Directive } from '@angular/core';
import { DsContactAutocompleteComponent } from '@ds/core/ui/ds-autocomplete/ds-contact-autocomplete/ds-contact-autocomplete.component';
import { IContact } from '@ds/core/contacts/shared/contact.model';
import { JustBoolean } from '../date-range/date-range.component';
import { Maybe } from '@ds/core/shared/Maybe';
import { IReviewProfileSetup } from '@ds/performance/review-profiles/shared/review-profile-setup.model';
import { findMinDate, findMaxDate } from '@ds/core/date-comparison/date-comparison';
import { ValidateDatesWhenSourcesValueChange, SetRequiredWhenSourceGainsValue } from '../../shared-performance-fns';
import { datepicker_validator_builder_default, MIN_DATEPICKER_VALIDATOR, MAX_DATEPICKER_VALIDATOR } from '@ds/core/ui/forms/datepicker-validator/datepicker-validators';

@Directive({
  selector: '[ds-ContactInput]'
})
export class ContactInputDirective {
  constructor(public viewContainerRef: ViewContainerRef) { }
 }

 export interface SupervisorComp {
  multiple: boolean;
  contacts: IContact[];
  inputControl: FormControl;
  submitted: () => boolean;
 }

 export interface DateRangeComp {
  startDate?: FormControl;
  dueDate?: FormControl;
  processStartDate?: FormControl;
  processEndDate?: FormControl;
  defaultMinMoment: moment.Moment;
  defaultMaxMoment: moment.Moment;
  submitted: boolean | JustBoolean;
 }

 export interface EmployeeDateRangeComp extends DateRangeComp {
   employeeContact: IContact;
 }

 export interface MeritIncreaseComp {
   payrollRequestDate: FormControl;
   submitted: () => boolean;
 }

 export interface ReviewMeetingComp {
   /** This will be set inside the CalendarYearFormComponent */
   isMeetingRequiredCtrl?: FormControl;
   submitted: () => boolean;
 }

 export interface EmployeeNamecomp {
   contact: IContact;
 }

const defaultFormVal = '';

@Component({
  selector: 'ds-calendar-year-form',
  templateUrl: './calendar-year-form.component.html',
  styleUrls: ['./calendar-year-form.component.scss']
})
export class CalendarYearFormComponent implements OnInit, OnDestroy {

  private readonly unsubscriber = new Subject();

  ngForm: FormGroup;

  readonly momentFormatString = 'MM/DD/YYYY';

  private _rcr: ICalendarYearForm;
  @Input()
  public get SetForm(): ICalendarYearForm {
    return this._rcr;
  }
  public set SetForm(value: ICalendarYearForm) {
    this._rcr = value;
    const rcrM = new Maybe(value);
    this.ngForm.patchValue(<ICalendarYearForm>{
      evaluationPeriodFromDate: '',
      evaluationPeriodToDate: '',
      reviewProcessStartDate: '',
      reviewProcessDueDate: '',
      supervisorEvaluationStartDate: '',
      supervisorEvaluationDueDate: '',
      employeeEvaluationStartDate: '',
      employeeEvaluationDueDate: '',
      payrollRequestDate: rcrM.map(x => x.payrollRequestDate).value(),
      employeeEvaluationId: rcrM.map(x => x.employeeEvaluationId).valueOr(this.ngForm.controls.employeeEvaluationId.value),
      employeeRCRReviewProfileEvalId: rcrM.map(x => x.employeeRCRReviewProfileEvalId).value(),
      hasReviewMeeting: rcrM.map(x => x.hasReviewMeeting).value(),
      supervisorEvaluationId: rcrM.map(x => x.supervisorEvaluationId).valueOr(this.ngForm.controls.supervisorEvaluationId.value),
      supervisorRCRReviewProfileEvalId: rcrM.map(x => x.supervisorRCRReviewProfileEvalId).value(),
      hasEmployeeEval: rcrM.map(x => x.hasEmployeeEval).valueOr(false),
      hasSupervisorEval: rcrM.map(x => x.hasSupervisorEval).valueOr(false),
      empEvalCompleteDate: rcrM.map(x => x.empEvalCompleteDate).value(),
      supEvalCompleteDate: rcrM.map(x => x.supEvalCompleteDate).value(),
      supEvalSignatures: rcrM.map(x => x.supEvalSignatures).value(),
      supEvalSignatureId: rcrM.map(x => x.supEvalSignatureId).value(),
      empEvalSignatureId: rcrM.map(x => x.empEvalSignatureId).value(),
      currentAssignedSupervisor: rcrM.map(x => x.currentAssignedSupervisor).value()
    });

    this.setEnabled(this.ngForm.controls.supervisorEvaluationStartDate, rcrM.map(x => x.hasSupervisorEval).valueOr(false));
    this.setEnabled(this.ngForm.controls.supervisorEvaluationDueDate, rcrM.map(x => x.hasSupervisorEval).valueOr(false));
    this.setEnabled(this.ngForm.controls.employeeEvaluationStartDate, rcrM.map(x => x.hasEmployeeEval).valueOr(false));
    this.setEnabled(this.ngForm.controls.employeeEvaluationDueDate, rcrM.map(x => x.hasEmployeeEval).valueOr(false));
  }

  private setEnabled(control: AbstractControl, enable: boolean): void {
    if(enable){
      control.enable();
    } else {
      control.disable();
    }
  }

  @Output() FormValue: EventEmitter<ICalendarYearForm> = new EventEmitter();

  private child: DsContactAutocompleteComponent;

  @Input()
  public dateRangesAreRequired: string;





  private _submitted: boolean;
  @Input()
  public get Submitted(): boolean {
    return this._submitted;
  }
  public set Submitted(value: boolean) {
    this._submitted = value;
  }
  private _reviewProfile: IReviewProfileSetup;
  @Input()
  public get ReviewProfile(): IReviewProfileSetup {
    return this._reviewProfile;
  }
  public set ReviewProfile(value: IReviewProfileSetup) {
    this._reviewProfile = value;
  }

  @Input() defaultMaxMoment: moment.Moment;
  @Input() defaultMinMoment: moment.Moment;
 private _supervisorComp: IFormItem<SupervisorComp>;
  @Input()
  public get supervisorComp(): IFormItem<SupervisorComp> {
    return this._supervisorComp;
  }
  public set supervisorComp(value: IFormItem<SupervisorComp>) {
    this._supervisorComp = value;
    this.instance = this.buildComponent(new Maybe(this.supervisorInput), new Maybe(this.supervisorComp), this.instance);
  }

  private _payrollRequestComp: IFormItem<MeritIncreaseComp>;
  @Input()
  public get payrollRequestComp(): IFormItem<MeritIncreaseComp> {
    return this._payrollRequestComp;
  }
  public set payrollRequestComp(value: IFormItem<MeritIncreaseComp>) {
    new Maybe(value).map(x => x.data).map(x => x.payrollRequestDate = this.payrollRequestDate);
    this.payrollRequestDate.clearValidators();
    new Maybe(this.payrollRequestComp).map(() => this.payrollRequestDate.setValidators(Validators.required));
    this._payrollRequestComp = value;
    this.meritIncreaseInstance = this.buildComponent(new Maybe(this.supervisorInput), new Maybe(this.payrollRequestComp), this.meritIncreaseInstance);
  }
  private meritIncreaseInstance: Maybe<IFormItem<any>> = new Maybe(null);

  private _reviewMeetingComp: IFormItem<ReviewMeetingComp>;
  @Input()
  public get reviewMeetingComp(): IFormItem<ReviewMeetingComp> {
    return this._reviewMeetingComp;
  }
  public set reviewMeetingComp(value: IFormItem<ReviewMeetingComp>) {
    new Maybe(value).map(x => x.data).map(x => x.isMeetingRequiredCtrl = this.hasReviewMeeting)
    this._reviewMeetingComp = value;
    this.reviewMeetingInstance = this.buildComponent(new Maybe(this.reviewMeetingInput), new Maybe(this.reviewMeetingComp), this.reviewMeetingInstance);
  }
  private reviewMeetingInstance: Maybe<IFormItem<ReviewMeetingComp>> = new Maybe(null);

  private _employeeNameComp: IFormItem<EmployeeNamecomp>;
  @Input()
  public get employeeNameComp(): IFormItem<EmployeeNamecomp> {
    return this._employeeNameComp;
  }
  public set employeeNameComp(value: IFormItem<EmployeeNamecomp>) {
    this._employeeNameComp = value;
    this._employeeNameInstance = this.buildComponent(new Maybe(this.employeeName), new Maybe(this.employeeNameComp), this._employeeNameInstance);
  }

  private _empDateRangeComp: IFormItem<DateRangeComp>;
  @Input()
  public get empDateRangeComp(): IFormItem<DateRangeComp> {
    return this._empDateRangeComp;
  }
  public set empDateRangeComp(value: IFormItem<DateRangeComp>) {
    const newVal = new Maybe(value).map(x => x.data);
    newVal.map(x => x.processStartDate = <FormControl>this.ngForm.controls.reviewProcessStartDate);
    newVal.map(x => x.processEndDate = <FormControl>this.ngForm.controls.reviewProcessDueDate);
    newVal.map(x => x.startDate = <FormControl>this.ngForm.controls.employeeEvaluationStartDate);
    newVal.map(x => x.dueDate = <FormControl>this.ngForm.controls.employeeEvaluationDueDate);
    this._empDateRangeComp = value;
    this._empDateRangeCompInstance = this.buildComponent(new Maybe(this.empDateRange), new Maybe(this.empDateRangeComp), this._empDateRangeCompInstance);
  }

  private _empDateRangeCompInstance: Maybe<IFormItem<DateRangeComp>> = new Maybe(null);

  private _employeeNameInstance: Maybe<IFormItem<EmployeeNamecomp>> = new Maybe(null);


  private _supervisorInput: ContactInputDirective;
  @ViewChild('contactInput', { read: ContactInputDirective, static: false})
  public get supervisorInput(): ContactInputDirective {
    return this._supervisorInput;
  }
  public set supervisorInput(value: ContactInputDirective) {
    this._supervisorInput = value;
    this.instance = this.buildComponent(new Maybe(this.supervisorInput), new Maybe(this.supervisorComp), this.instance);
  }
  private instance: Maybe<IFormItem<any>> = new Maybe(null);

  private _payrollRequestInput: ContactInputDirective;
  @ViewChild('payrollRequest', { read: ContactInputDirective, static: false })
  public get payrollRequestInput(): ContactInputDirective {
    return this._payrollRequestInput;
  }
  public set payrollRequestInput(value: ContactInputDirective) {
    this._payrollRequestInput = value;
    this.meritIncreaseInstance = this.buildComponent(new Maybe(this.payrollRequestInput), new Maybe(this.payrollRequestComp), this.meritIncreaseInstance);
  }

  private _reviewMeetingInput: ContactInputDirective;
  @ViewChild('reviewMeeting', { read: ContactInputDirective, static: false })
  public get reviewMeetingInput(): ContactInputDirective {
    return this._reviewMeetingInput;
  }
  public set reviewMeetingInput(value: ContactInputDirective) {
    this._reviewMeetingInput = value;
    this.reviewMeetingInstance = this.buildComponent(new Maybe(this.reviewMeetingInput), new Maybe(this.reviewMeetingComp), this.reviewMeetingInstance)
  }

  private _employeeName: ContactInputDirective;
  @ViewChild('employeeName', {read: ContactInputDirective, static: false})
  public get employeeName(): ContactInputDirective {
    return this._employeeName;
  }
  public set employeeName(value: ContactInputDirective) {
    this._employeeName = value;
    this._employeeNameInstance = this.buildComponent(new Maybe(this.employeeName), new Maybe(this.employeeNameComp), this._employeeNameInstance);
  }

  private _empDateRange: ContactInputDirective;
  @ViewChild('empDateRange', {read: ContactInputDirective, static: false})
  public get empDateRange(): ContactInputDirective {
    return this._empDateRange;
  }
  public set empDateRange(value: ContactInputDirective) {
    this._empDateRange = value;
    this._empDateRangeCompInstance = this.buildComponent(new Maybe(this.empDateRange), new Maybe(this.empDateRangeComp), this._empDateRangeCompInstance);
  }


private formStream$: Observable<any>;

  private _minValIsRPStartDate: ValidatorFn;
  public get minValIsRPStartDate(): ValidatorFn {
    return this._minValIsRPStartDate;
  }
  private _maxValIsRPDueDate: ValidatorFn;
  public get maxValIsRPDueDate(): ValidatorFn {
    return this._maxValIsRPDueDate;
  }
  private _supEvalStartDateMaxVal: ValidatorFn;
  public get supEvalStartDateMaxVal(): ValidatorFn {
    return this._supEvalStartDateMaxVal;
  }
  private _empEvalDueDateMinVal: ValidatorFn;
  public get empEvalDueDateMinVal(): ValidatorFn {
    return this._empEvalDueDateMinVal;
  }
  private _empEvalStartDateMaxVal: ValidatorFn;
  public get empEvalStartDateMaxVal(): ValidatorFn {
    return this._empEvalStartDateMaxVal;
  }
  private _supEvalDueDateMinVal: ValidatorFn;
  public get supEvalDueDateMinVal(): ValidatorFn {
    return this._supEvalDueDateMinVal;
  }
  private _evalPeriodStartMaxVal: ValidatorFn;
  public get evalPeriodStartMaxVal(): ValidatorFn {
    return this._evalPeriodStartMaxVal;
  }
  private _evalPeriodEndMinVal: ValidatorFn;
  public get evalPeriodEndMinVal(): ValidatorFn {
    return this._evalPeriodEndMinVal;
  }

  private buildComponent<T>(
    view: Maybe<{viewContainerRef: ViewContainerRef}>,
     component: Maybe<IFormItem<T>>,
     existing: Maybe<IFormItem<any>>){
    const container = view.map(x => x.viewContainerRef)
    container.map(x => x.clear());
    const factory = component.map(x => this.componentFactoryResolver.resolveComponentFactory(x.component));
    const instance = factory
    .map(x => container.map(y => y.createComponent(x)))
    .map(x => x.value())
    .map(x => x.instance);
    return instance.map(x => {
      component.map(y => {
        x.data = y.data;
        this.ref.detectChanges();
      })
      return component.value();
    });

  }

  get reviewProcessStartDate() { return this.ngForm.get('reviewProcessStartDate') as FormControl; }
  get reviewProcessDueDate() { return this.ngForm.get('reviewProcessDueDate') as FormControl; }
  get evaluationPeriodFromDate() { return this.ngForm.get('evaluationPeriodFromDate') as FormControl; }
  get evaluationPeriodToDate() { return this.ngForm.get('evaluationPeriodToDate') as FormControl; }
  get supervisorEvaluationStartDate() { return this.ngForm.get('supervisorEvaluationStartDate') as FormControl; }
  get supervisorEvaluationDueDate() { return this.ngForm.get('supervisorEvaluationDueDate') as FormControl; }
  get employeeEvaluationStartDate() { return this.ngForm.get('employeeEvaluationStartDate') as FormControl; }
  get employeeEvaluationDueDate() { return this.ngForm.get('employeeEvaluationDueDate') as FormControl; }
  get payrollRequestDate() { return this.ngForm.get('payrollRequestDate') as FormControl; }
  get hasReviewMeeting() { return this.ngForm.get('hasReviewMeeting') as FormControl; }
  get supervisorRCRReviewProfileEvalId() {return this.ngForm.get('supervisorRCRReviewProfileEvalId') as FormControl; }
  get employeeRCRReviewProfileEvalId() {return this.ngForm.get('employeeRCRReviewProfileEvalId') as FormControl; }
  get hasEmployeeEval() { return this.ngForm.get('hasEmployeeEval') as FormControl; }
  get hasSupervisorEval() { return this.ngForm.get('hasSupervisorEval') as FormControl; }

  constructor(
    fb: FormBuilder,
    @Inject(MAX_DATEPICKER_VALIDATOR) maxDateValidator: datepicker_validator_builder_default,
    @Inject(MIN_DATEPICKER_VALIDATOR) minDateValidator: datepicker_validator_builder_default,
    private componentFactoryResolver: ComponentFactoryResolver,
    private ref: ChangeDetectorRef) {

      this._minValIsRPStartDate = minDateValidator(() => this.reviewProcessStartDate.value, defaultFormVal);
      this._supEvalStartDateMaxVal = maxDateValidator(() => findMinDate(this.supervisorEvaluationDueDate.value, this.reviewProcessDueDate.value, this.defaultMaxMoment), defaultFormVal);
      this._maxValIsRPDueDate = maxDateValidator(() => this.reviewProcessDueDate.value, defaultFormVal);
      this._empEvalDueDateMinVal = minDateValidator(() => findMaxDate(this.employeeEvaluationStartDate.value, this.reviewProcessStartDate.value, this.defaultMinMoment), defaultFormVal);
      this._empEvalStartDateMaxVal = maxDateValidator(() => findMinDate(this.employeeEvaluationDueDate.value, this.reviewProcessDueDate.value, this.defaultMaxMoment), defaultFormVal);
      this._supEvalDueDateMinVal = minDateValidator(() => findMaxDate(this.supervisorEvaluationStartDate.value, this.reviewProcessStartDate.value, this.defaultMinMoment), defaultFormVal);
      this._evalPeriodStartMaxVal = maxDateValidator(() => this.evaluationPeriodToDate.value, defaultFormVal);
      this._evalPeriodEndMinVal = minDateValidator(() => this.evaluationPeriodFromDate.value, defaultFormVal);
      this._evalPeriodEndMinVal = maxDateValidator(() => this.reviewProcessDueDate.value, defaultFormVal);

    this.ngForm = fb.group(<ICalendarYearForm>{
      evaluationPeriodFromDate: fb.control(defaultFormVal, { updateOn: 'blur'}),
      evaluationPeriodToDate: fb.control(defaultFormVal, { updateOn: 'blur'}),
      reviewProcessStartDate: fb.control(defaultFormVal, { updateOn: 'blur'}),
      reviewProcessDueDate: fb.control(defaultFormVal, { updateOn: 'blur'}),
      supervisorEvaluationDueDate: fb.control(defaultFormVal, { updateOn: 'blur'}),
      supervisorEvaluationStartDate: fb.control(defaultFormVal, { updateOn: 'blur'}),
      supervisorEvaluationId: fb.control(defaultFormVal),
      supervisorRCRReviewProfileEvalId: fb.control(defaultFormVal),
      employeeEvaluationDueDate: fb.control(defaultFormVal, { updateOn: 'blur'}),
      employeeEvaluationStartDate: fb.control(defaultFormVal, { updateOn: 'blur'}),
      employeeEvaluationId: fb.control(defaultFormVal),
      employeeRCRReviewProfileEvalId: fb.control(defaultFormVal),
      payrollRequestDate: fb.control(defaultFormVal, { updateOn: 'blur'}),
      hasReviewMeeting: fb.control(defaultFormVal),
      hasEmployeeEval: fb.control(defaultFormVal),
      hasSupervisorEval: fb.control(defaultFormVal),
      empEvalCompleteDate: fb.control(defaultFormVal),
      supEvalCompleteDate: fb.control(defaultFormVal),
      supEvalSignatures: fb.control(defaultFormVal),
      supEvalSignatureId: fb.control(defaultFormVal),
      currentAssignedSupervisor: fb.control(defaultFormVal)
  });

   }

   private setRequired(control: FormControl, validatorFns: ValidatorFn[], required: boolean){
     const result = true === required ? validatorFns.concat([Validators.required]): validatorFns;
     control.setValidators(result);
   }

  ngOnInit() {
      
    const shouldAlwaysBeRequired = new Maybe(this.dateRangesAreRequired).map(x => x.toLowerCase().trim()).map(x => "true".localeCompare(x) === 0).valueOr(false);

    this.setRequired(this.evaluationPeriodFromDate, [this.evalPeriodStartMaxVal], shouldAlwaysBeRequired);
    this.setRequired(this.evaluationPeriodToDate, [this.evalPeriodEndMinVal], shouldAlwaysBeRequired);
    this.setRequired(this.reviewProcessStartDate, [this.maxValIsRPDueDate], shouldAlwaysBeRequired);
    this.setRequired(this.reviewProcessDueDate, [this.minValIsRPStartDate], shouldAlwaysBeRequired);
    this.setRequired(this.supervisorEvaluationDueDate, [this.supEvalDueDateMinVal, this.maxValIsRPDueDate], shouldAlwaysBeRequired);
    this.setRequired(this.supervisorEvaluationStartDate, [this.minValIsRPStartDate, this.maxValIsRPDueDate], shouldAlwaysBeRequired);
    this.setRequired(this.employeeEvaluationDueDate, [this.empEvalDueDateMinVal, this.maxValIsRPDueDate], shouldAlwaysBeRequired);
    this.setRequired(this.employeeEvaluationStartDate, [this.minValIsRPStartDate, this.maxValIsRPDueDate], shouldAlwaysBeRequired);
  

    const controlsToReactTo = [this.reviewProcessStartDate, this.reviewProcessDueDate];
    const controlsToUpdate = [
        this.supervisorEvaluationStartDate,
        this.employeeEvaluationStartDate,
    ];
    const isValid = (status: any) => "valid" === new Maybe(status).map(x => x.toLowerCase()).value();

    const doNothing = (sourceControlChanges:Observable<MatDatepickerInputEvent<moment.Moment | string> | moment.Moment | string>, affectedControl: AbstractControl, defaultValidators: ValidatorFn[], defaultFormVal: any, mapper?: (val: any) => any) => sourceControlChanges;
    const setRequiredOnChanges = shouldAlwaysBeRequired ? doNothing : SetRequiredWhenSourceGainsValue;

    const mapper = (y) => {
      if (typeof y === 'string' || moment.isMoment(y)) {
        return y;
      } else {
        return y.value;
      }
    }
    
    // TODO figure out a way to make initialization of this component less expensive (maybe optimize stream?).

    this.formStream$ = merge(
      combineLatest(this.ngForm.valueChanges, this.ngForm.statusChanges).pipe(
        startWith([this.ngForm.value, this.ngForm.status]),
        map(x => ({value: x[0], status: x[1]})),
        tap(x => this.FormValue.emit(isValid(x.status) ? x.value : null))),
      ValidateDatesWhenSourcesValueChange(controlsToReactTo, controlsToUpdate),
      setRequiredOnChanges(this.reviewProcessStartDate.valueChanges,
        this.reviewProcessDueDate,
        [this.maxValIsRPDueDate],
        defaultFormVal),
      setRequiredOnChanges(this.reviewProcessDueDate.valueChanges,
          this.reviewProcessStartDate,
          [this.maxValIsRPDueDate],
          defaultFormVal),
      setRequiredOnChanges(this.evaluationPeriodToDate.valueChanges,
          this.evaluationPeriodFromDate,
          [this.evalPeriodStartMaxVal],
          defaultFormVal, mapper),
      setRequiredOnChanges(this.supervisorEvaluationDueDate.valueChanges,
          this.supervisorEvaluationStartDate,
          [this.minValIsRPStartDate, this.supEvalStartDateMaxVal],
          defaultFormVal, mapper),
      setRequiredOnChanges(this.employeeEvaluationDueDate.valueChanges,
          this.employeeEvaluationStartDate,
          [this.minValIsRPStartDate, this.empEvalStartDateMaxVal],
          defaultFormVal, mapper),
  ).pipe(takeUntil(this.unsubscriber));

    this.formStream$.subscribe();
    
    // Manually update dependent dates when updated after an initial value is added
    const time = 200;
    this.reviewProcessStartDate.valueChanges.pipe(debounceTime(time)).subscribe( val => {
      this.ngForm.get('reviewProcessDueDate').updateValueAndValidity();
      this.ngForm.get('evaluationPeriodFromDate').updateValueAndValidity();
      this.ngForm.get('supervisorEvaluationStartDate').updateValueAndValidity();
      this.ngForm.get('employeeEvaluationStartDate').updateValueAndValidity();
    });
    this.reviewProcessDueDate.valueChanges.pipe(debounceTime(time)).subscribe( val => {
      this.ngForm.get('reviewProcessStartDate').updateValueAndValidity();
    })
    this.evaluationPeriodFromDate.valueChanges.pipe(debounceTime(time)).subscribe( val => {
      this.ngForm.get('evaluationPeriodToDate').updateValueAndValidity();
    });
    this.supervisorEvaluationStartDate.valueChanges.pipe(debounceTime(time)).subscribe( val => {
      this.ngForm.get('supervisorEvaluationDueDate').updateValueAndValidity();
    });
    this.employeeEvaluationStartDate.valueChanges.pipe(debounceTime(time)).subscribe( val => {
      this.ngForm.get('employeeEvaluationDueDate').updateValueAndValidity();
    });
  }

  ngOnDestroy(): void {
    this.unsubscriber.next();
  }

}
