import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, AbstractControl, ValidatorFn, Validators, FormControl } from '@angular/forms';
import { Observable, combineLatest, of, merge } from 'rxjs';
import { DateUnit, ConvertToDays } from '../shared/time-unit.enum';
import { map, tap, startWith } from 'rxjs/operators';
import { maxDateError } from '@ds/core/ui/forms/datepicker-validator/datepicker-validators';
import { Maybe } from '@ds/core/shared/Maybe';
import { IReviewProfileSetup } from '@ds/performance/review-profiles/shared/review-profile-setup.model';
import { EvaluationRoleType } from '@ds/performance/evaluations/shared/evaluation-role-type.enum';
import { IReviewCycleReview, IReviewCycleReviewDetail } from '../shared/review-cycle-review.model';
import { IReviewCycleReviewFormComponent } from '../shared/review-cycle-review-form';

@Component({
  selector: 'ds-reference-date',
  templateUrl: './reference-date.component.html',
  styleUrls: ['./reference-date.component.scss']
})
export class ReferenceDateComponent implements OnInit {


  private _rcr: IReviewCycleReviewDetail;
  @Input()
  public get SetForm(): IReviewCycleReviewDetail {
    return this._rcr;
  }
  public set SetForm(value: IReviewCycleReviewDetail) {
    this._rcr = value;
    const rcrM = new Maybe(this.SetForm);
    this.refDateForm.patchValue({
      startDuration: rcrM.map(x => x.delayAfterReference).value(),
      startUnitType: rcrM.map(x => x.delayAfterReferenceUnitTypeId).value(),
      evalPrevDuration: rcrM.map(x => x.evaluationPeriodDuration).value(),
      evalPrevUnitType: rcrM.map(x => x.evaluationPeriodDurationUnitTypeId).value(),
      timeCompleteDuration: rcrM.map(x => x.reviewProcessDuration).value(),
      timeCompleteUnitType: rcrM.map(x => x.reviewProcessDurationUnitTypeId).value(),
      supervisorDuration: rcrM.map(x => x.evaluations).map(x => x.find((a) => a.role === EvaluationRoleType.Manager)).map(x => x.evaluationDuration).value(),
      supervisorUnitType: rcrM.map(x => x.evaluations).map(x => x.find((a) => a.role === EvaluationRoleType.Manager)).map(x => x.evaluationDurationUnitTypeId).value(),
      employeeDuration: rcrM.map(x => x.evaluations).map(x => x.find((a) => a.role === EvaluationRoleType.Self)).map(x => x.evaluationDuration).value(),
      employeeUnitType: rcrM.map(x => x.evaluations).map(x => x.find((a) => a.role === EvaluationRoleType.Self)).map(x => x.evaluationDurationUnitTypeId).value()
    });
  }

  private _reviewProfile: IReviewProfileSetup;
  @Input()
  public get ReviewProfile(): IReviewProfileSetup {
    return this._reviewProfile;
  }
  public set ReviewProfile(value: IReviewProfileSetup) {
    this._reviewProfile = value;

    const evals = new Maybe(this.ReviewProfile).map(x => x.evaluations);
    const hasEmployeeEval = evals.map(x => x.some(a => a.role === EvaluationRoleType.Self)).value();
    const hasSupervisorEval = evals.map(x => x.some(a => a.role === EvaluationRoleType.Manager)).value();

    this.setEnabled(this.employeeDuration, hasEmployeeEval);
    this.setEnabled(this.employeeUnitType, hasEmployeeEval);
    this.setEnabled(this.supervisorDuration, hasSupervisorEval);
    this.setEnabled(this.supervisorUnitType, hasSupervisorEval);

    this.refDateForm.patchValue({
      supervisorRCRReviewProfileEvalId: evals.map(x => x.find(y => y.role === EvaluationRoleType.Manager)).map(x => x.reviewProfileEvaluationId).value(),
      employeeRCRReviewProfileEvalId: evals.map(x => x.find(y => y.role === EvaluationRoleType.Self)).map(x => x.reviewProfileEvaluationId).value()
    });
  }

  @Input() Submitted: boolean;
  @Output() FormValue = new EventEmitter<IReviewCycleReview>();
  updateParent$: Observable<any>;

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

refDateForm: FormGroup;
formValidation$: Observable<any>;

  constructor(fb: FormBuilder) {
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
      employeeRCRReviewProfileEvalId: fb.control(null)
    });

    this.formValidation$ = merge(this.dateSelectorChangeStream(this.timeCompleteDuration, this.timeCompleteUnitType, this.supervisorUnitType, this.supervisorDuration),
    this.dateSelectorChangeStream(this.timeCompleteDuration, this.timeCompleteUnitType, this.employeeUnitType, this.employeeDuration),
    );

    
    const supDurationValidator = this.getEvaluationValidator(this.supervisorUnitType, this.timeCompleteDuration, this.timeCompleteUnitType);
    const empDurationValidator = this.getEvaluationValidator(this.employeeUnitType, this.timeCompleteDuration, this.timeCompleteUnitType);

    this.supervisorDuration.setValidators(durationValidators.concat([supDurationValidator]));
    this.employeeDuration.setValidators(durationValidators.concat([empDurationValidator]));

   }

  ngOnInit() {

    this.updateParent$ = combineLatest(this.refDateForm.valueChanges, this.refDateForm.statusChanges).pipe(
      startWith([this.refDateForm.value, this.refDateForm.status]),
      map(x => ({value: x[0], status: x[1]})),
      tap(x => this.FormValue.emit("VALID" === x.status ? x.value : null)));
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
