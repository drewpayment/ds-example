import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, AbstractControl, ValidatorFn, Validators, FormControl } from '@angular/forms';
import { Observable, combineLatest, of, merge } from 'rxjs';
import { DateUnit, ConvertToDays } from '../../../../../core/src/lib/shared/time-unit.enum';
import { map, tap, startWith } from 'rxjs/operators';
import { maxDateError } from '@ds/core/ui/forms/datepicker-validator/datepicker-validators';
import { Maybe } from '@ds/core/shared/Maybe';
import { IReviewProfileSetup } from '@ds/performance/review-profiles/shared/review-profile-setup.model';
import { EvaluationRoleType } from '@ds/performance/evaluations/shared/evaluation-role-type.enum';
import { IReviewTemplate, ReviewTemplateDialogFormData } from '@ds/performance/review-policy';
import { ReferenceDate } from '@ds/core/groups/shared/schedule-type.enum';
import { GetDayOfMonthDefaultValue } from '@ds/core/ui/forms/day-of-month-selector/day-of-month-selector.component';
import { PerformanceManagerService } from '@ds/performance/performance-manager/performance-manager.service';
import { IContact } from '@ds/core/contacts/shared/contact.model';
import { SetRequiredWhenSourceGainsValue } from '@ds/performance/shared/shared-performance-fns';

@Component({
  selector: 'ds-reference-date',
  templateUrl: './reference-date.component.html',
  styleUrls: ['./reference-date.component.scss']
})
export class ReferenceDateComponent implements OnInit {

private isSavedTemplateRecurring: boolean;
  private _rcr: ReviewTemplateDialogFormData;
  @Input()
  public get SetForm(): ReviewTemplateDialogFormData {
    return this._rcr;
  }
  public set SetForm(value: ReviewTemplateDialogFormData) {
    this._rcr = value;
    const rcrM = new Maybe(this.SetForm);
    this.refDateForm.patchValue({
      startDuration: rcrM.map(x => x.template).map(x => x.delayAfterReference).value(),
      startUnitType: rcrM.map(x => x.template).map(x => x.delayAfterReferenceUnitTypeId).value(),
      evalPrevDuration: rcrM.map(x => x.template).map(x => x.evaluationPeriodDuration).value(),
      evalPrevUnitType: rcrM.map(x => x.template).map(x => x.evaluationPeriodDurationUnitTypeId).value(),
      timeCompleteDuration: rcrM.map(x => x.template).map(x => x.reviewProcessDuration).value(),
      timeCompleteUnitType: rcrM.map(x => x.template).map(x => x.reviewProcessDurationUnitTypeId).value(),
      supervisorDuration: rcrM.map(x => x.template).map(x => x.evaluations).map(x => x.find((a) => a.role === EvaluationRoleType.Manager)).map(x => x.evaluationDuration).value(),
      supervisorUnitType: rcrM.map(x => x.template).map(x => x.evaluations).map(x => x.find((a) => a.role === EvaluationRoleType.Manager)).map(x => x.evaluationDurationUnitTypeId).value(),
      employeeDuration: rcrM.map(x => x.template).map(x => x.evaluations).map(x => x.find((a) => a.role === EvaluationRoleType.Self)).map(x => x.evaluationDuration).value(),
      employeeUnitType: rcrM.map(x => x.template).map(x => x.evaluations).map(x => x.find((a) => a.role === EvaluationRoleType.Self)).map(x => x.evaluationDurationUnitTypeId).value(),
      monthAndDate: rcrM.map(x => x.template).map(x => x.hardCodedAnniversary).valueOr(GetDayOfMonthDefaultValue()),
      payrollRequest: rcrM.map(x => x.template).map(x => x.payrollRequestEffectiveDate).valueOr(GetDayOfMonthDefaultValue()),
      supervisorEvalConductedBy: rcrM.map(x => x.template).map(x => x.evaluations).map(x => x.find(evaluation => evaluation.role === EvaluationRoleType.Manager)).map(x => x.conductedBy).valueOr(-1),
      reviewReminderId: rcrM.map(x => x.template).map(x => x.reviewReminders).map(x => x[0]).map(x => x.reviewReminderID).value(),
      reminderDurationPrior: rcrM.map(x => x.template).map(x => x.reviewReminders).map(x => x[0]).map(x => x.durationPrior).value(),
      reminderdurationUnitType: rcrM.map(x => x.template).map(x => x.reviewReminders).map(x => x[0]).map(x => x.durationPriorUnitTypeId).value()
    });
    this.isSavedTemplateRecurring = rcrM.map(x => x.template).map(x => x.isRecurring).value();
  }

  private _reviewProfile: IReviewProfileSetup;
  @Input()
  public get ReviewProfile(): IReviewProfileSetup {
    return this._reviewProfile;
  }
  public set ReviewProfile(value: IReviewProfileSetup) {
    this._reviewProfile = value;

    const profile = new Maybe(this.ReviewProfile);
    const evals = profile.map(x => x.evaluations);
    const hasEmployeeEval = evals.map(x => x.some(a => a.role === EvaluationRoleType.Self)).value();
    const hasSupervisorEval = evals.map(x => x.some(a => a.role === EvaluationRoleType.Manager)).value();
    const hasPayrollRequest = profile.map(x => x.includePayrollRequests).valueOr(false);

    this.setEnabled(this.employeeDuration, hasEmployeeEval);
    this.setEnabled(this.employeeUnitType, hasEmployeeEval);
    this.setEnabled(this.supervisorDuration, hasSupervisorEval);
    this.setEnabled(this.supervisorUnitType, hasSupervisorEval);
    this.setEnabled(this.payrollRequest, hasPayrollRequest);

    this.refDateForm.patchValue({
      supervisorRCRReviewProfileEvalId: evals.map(x => x.find(y => y.role === EvaluationRoleType.Manager)).map(x => x.reviewProfileEvaluationId).value(),
      employeeRCRReviewProfileEvalId: evals.map(x => x.find(y => y.role === EvaluationRoleType.Self)).map(x => x.reviewProfileEvaluationId).value()
    });
  }
  private _openRecurringView: boolean;
  @Input()
  public get openRecurringView(): boolean {
    return this._openRecurringView;
  }
  public set openRecurringView(value: boolean) {
    this._openRecurringView = value;
    
  }

  @Input() Submitted: boolean;
  private _referenceDate: ReferenceDate;
  @Input()
  public get referenceDate(): ReferenceDate {
    return this._referenceDate;
  }
  public set referenceDate(value: ReferenceDate) {
    this._referenceDate = value;
    this.setEnabled(this.startDuration, value === ReferenceDate.DateOfHire);
    this.setEnabled(this.startUnitType, value === ReferenceDate.DateOfHire);
    this.setEnabled(this.payrollRequest, value === ReferenceDate.CalendarYear);
  }
  @Output() FormValue = new EventEmitter<IReviewTemplate>();
  updateParent$: Observable<any>;

  get ReferenceDate() {
    return ReferenceDate;
  }

  supervisors$: Observable<IContact[]>;

  private setEnabled(control: AbstractControl, enable: boolean): void {
    if(enable){
      control.enable();
    } else {
      control.disable();
    }
  }

  get startDuration() { return this.refDateForm.controls.startDuration as FormControl; }
  get startUnitType() { return this.refDateForm.controls.startUnitType as FormControl; }
  get evalPrevDuration() { return this.refDateForm.controls.evalPrevDuration as FormControl; }
  get evalPrevUnitType() { return this.refDateForm.controls.evalPrevUnitType as FormControl; }
  get timeCompleteDuration() { return this.refDateForm.controls.timeCompleteDuration as FormControl; }
  get timeCompleteUnitType() { return this.refDateForm.controls.timeCompleteUnitType as FormControl; }
  get supervisorDuration() { return this.refDateForm.controls.supervisorDuration as FormControl; }
  get supervisorUnitType() { return this.refDateForm.controls.supervisorUnitType as FormControl; }
  get employeeDuration() { return this.refDateForm.controls.employeeDuration as FormControl; }
  get employeeUnitType() { return this.refDateForm.controls.employeeUnitType as FormControl; }
  get supervisorRCRReviewProfileEvalId() { return this.refDateForm.controls.supervisorRCRReviewProfileEvalId as FormControl; }
  get employeeRCRReviewProfileEvalId() { return this.refDateForm.controls.employeeRCRReviewProfileEvalId as FormControl; }
  get monthAndDate() { return this.refDateForm.controls.monthAndDate as FormControl; }
  get supervisorEvalConductedBy() { return this.refDateForm.controls.supervisorEvalConductedBy as FormControl; }
  get reminderDurationPrior() { return this.refDateForm.controls.reminderDurationPrior as FormControl; }
  get reminderdurationUnitType() { return this.refDateForm.controls.reminderdurationUnitType as FormControl; }
  get reviewReminderId() { return this.refDateForm.controls.reviewReminderId as FormControl; }
  get isRecurring() { return this.refDateForm.controls.isRecurring as FormControl; }
  get payrollRequest() { return this.refDateForm.controls.payrollRequest as FormControl; }


refDateForm: FormGroup;
formValidation$: Observable<any>;
startDate:string = "";
payrollStartDate:string = "";

  constructor(fb: FormBuilder,
    private manager: PerformanceManagerService) {
    const wholeNumber = /^-?[0-9,]*$/;
    const durationValidators = [Validators.required, Validators.min(0), Validators.pattern(wholeNumber)];

    this.refDateForm = fb.group({
      startDuration: fb.control(null, {updateOn: 'blur', validators: durationValidators }),
      startUnitType: fb.control(null, {validators: Validators.required}),
      evalPrevDuration: fb.control(null, {updateOn: 'blur', validators: durationValidators}),
      evalPrevUnitType: fb.control(null, {validators: Validators.required}),
      timeCompleteDuration: fb.control(null, {updateOn: 'blur', validators: durationValidators}),
      timeCompleteUnitType: fb.control(null, { validators: Validators.required }),
      supervisorDuration: fb.control(null, {updateOn: 'blur' }),
      supervisorUnitType: fb.control(null, { validators: Validators.required }),
      supervisorRCRReviewProfileEvalId: fb.control(null),
      employeeDuration: fb.control(null, {updateOn: 'blur' }),
      employeeUnitType: fb.control(null, { validators: Validators.required }),
      employeeRCRReviewProfileEvalId: fb.control(null),
      monthAndDate: fb.control(null),
      supervisorEvalConductedBy: fb.control(-1),
      reminderDurationPrior: fb.control(null, {updateOn: 'blur', validators: durationValidators }),
      reminderdurationUnitType: fb.control(null, { validators: Validators.required }),
      reviewReminderId: fb.control(null),
      isRecurring: fb.control(false),
      payrollRequest: fb.control(null)
    });

    this.formValidation$ = merge(
      this.dateSelectorChangeStream(this.timeCompleteDuration, this.timeCompleteUnitType, this.supervisorUnitType, this.supervisorDuration),
    this.dateSelectorChangeStream(this.timeCompleteDuration, this.timeCompleteUnitType, this.employeeUnitType, this.employeeDuration),
    SetRequiredWhenSourceGainsValue(this.reminderDurationPrior.valueChanges, this.reminderdurationUnitType, [], null),
    SetRequiredWhenSourceGainsValue(this.reminderdurationUnitType.valueChanges, this.reminderDurationPrior, [], null)
    );

    this.supervisors$ = this.manager.getDirectSupervisors();

    
    const supDurationValidator = this.getEvaluationValidator(this.supervisorUnitType, this.timeCompleteDuration, this.timeCompleteUnitType);
    const empDurationValidator = this.getEvaluationValidator(this.employeeUnitType, this.timeCompleteDuration, this.timeCompleteUnitType);

    this.supervisorDuration.setValidators(durationValidators.concat([supDurationValidator]));
    this.employeeDuration.setValidators(durationValidators.concat([empDurationValidator]));

   }

  ngOnInit() {

    this.refDateForm.patchValue({
      isRecurring: new Maybe(this.isSavedTemplateRecurring).valueOr(!!this.openRecurringView)
    })

    this.updateParent$ = combineLatest(this.refDateForm.valueChanges, this.refDateForm.statusChanges).pipe(
      startWith([this.refDateForm.value, this.refDateForm.status]),
      map(x => ({value: x[0], status: x[1]})),
      tap(x => this.FormValue.emit("VALID" === x.status ? x.value : null)));

    // These are temp properties used to avoid expression errors due to value changes
    this.startDate = this.monthAndDate ? this.monthAndDate.value : "" ;
    this.payrollStartDate = this.payrollRequest ? this.payrollRequest.value : "" ;
  }

  private dateSelectorChangeStream(
    sourceDurationCtrl: AbstractControl, 
    sourceUnitTypeCtrl: AbstractControl, 
    affectedUnitTypeCtrl: AbstractControl,
    affectedDurationCtrl: AbstractControl): Observable<any> {
    return combineLatest(
      sourceDurationCtrl.valueChanges.pipe(startWith(sourceDurationCtrl.value)), 
      sourceUnitTypeCtrl.valueChanges.pipe(startWith(sourceUnitTypeCtrl.value)), 
      affectedUnitTypeCtrl.valueChanges.pipe(startWith(affectedUnitTypeCtrl.value)), 
      of(affectedDurationCtrl)).pipe(
      tap(x => x[3].updateValueAndValidity())
    )
  }

  private getEvaluationValidator(affectedUnitTypeCtrl: AbstractControl, sourceDurationCtrl: AbstractControl, sourceUnitTypeCtrl: AbstractControl): ValidatorFn {
    return (affedtedDurationCtrl) => {
      if(typeof affedtedDurationCtrl.value !== "number" || typeof sourceDurationCtrl.value !== "number" || DateUnit[affectedUnitTypeCtrl.value] == null || DateUnit[sourceUnitTypeCtrl.value] == null){
        return null;
      }
      const actualDays = ConvertToDays(affectedUnitTypeCtrl.value, affedtedDurationCtrl.value);
      const maximumDays = ConvertToDays(sourceUnitTypeCtrl.value, sourceDurationCtrl.value);

      return actualDays > maximumDays ? maxDateError : null;
    }
  }

}
