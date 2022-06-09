import { Component, OnInit, Input, Self, Optional, OnDestroy } from '@angular/core';
import { MonthType } from '@ds/core/shared/month-type.enum';
import * as moment from 'moment';
import { ControlValueAccessor, FormGroup, FormBuilder, ValidatorFn, FormControl, AbstractControl, Validators, NgControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { tap, startWith } from 'rxjs/operators';
import { Maybe } from '@ds/core/shared/Maybe';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';

@Component({
  selector: 'ds-day-of-month-selector',
  templateUrl: './day-of-month-selector.component.html',
  styleUrls: ['./day-of-month-selector.component.scss']
})
export class DayOfMonthSelectorComponent implements OnInit, OnDestroy {
  ngOnDestroy(): void {
    this.parentForm.removeControl(this.ctrlName);
    this.parentForm.setValidators(this.oldValidators);
  }

  @Input() targetControlName: string;

  constructor(
    private fb: FormBuilder) {

      
     }

     private getNotNullCtrl(ctrl: AbstractControl): AbstractControl {
       return new Maybe(ctrl).valueOr(new FormControl())
     }

  private februaryValidator: FebruaryValidationData = new IgnoreLeapYearStrategy();
  private ctrlName: string;
  private oldValidators: ValidatorFn;

  ngOnInit(): void {

    this.months = [
      { name: 'January', id: MonthType.January, noOfDays: 31},
      { name: 'February', id: MonthType.February, noOfDays: this.februaryValidator.maxDate },
      { name: 'March', id: MonthType.March, noOfDays: 31 },
      { name: 'April', id: MonthType.April, noOfDays: 30 },
      { name: 'May', id: MonthType.May, noOfDays: 31 },
      { name: 'June', id: MonthType.June, noOfDays: 30 },
      { name: 'July', id: MonthType.July, noOfDays: 31 },
      { name: 'August', id: MonthType.August, noOfDays: 31 },
      { name: 'September', id: MonthType.September, noOfDays: 30 },
      { name: 'October', id: MonthType.October, noOfDays: 31 },
      { name: 'November', id: MonthType.November, noOfDays: 30 },
      { name: 'December', id: MonthType.December, noOfDays: 31 }
    ];
    
    const wholeNumber = /^-?[0-9,]*$/;

    const nonNullInput = new Maybe(this.savedDate).map(this.convertToFormModel).valueOr(GetDayOfMonthDefaultValue());
    (this.parentForm.controls[this.targetControlName] as FormControl).setValue(nonNullInput, {emitEvent: false})
    const form: DayOfMonthSelectorForm = {
      month: this.fb.control(nonNullInput.month, { validators: Validators.required }),
      date: this.fb.control(nonNullInput.date, { updateOn: 'blur', validators: [Validators.min(1), Validators.pattern(wholeNumber), Validators.required] })
    }
    this._form = this.fb.group(form);
    const uniqueName = this.getUniqueControlName(this.parentForm);
    this.ctrlName = uniqueName;
    this.oldValidators = this.parentForm.validator;
    this.parentForm.addControl(uniqueName, this._form);
    const maxDateValidation = this.maxDate(this.months, form.month, () => this.februaryValidator);
    this._form.setValidators(maxDateValidation);
    this.parentForm.setValidators([this.parentForm.validator, this.setTargetValidity(this.form)].filter(x => !!x))
    this.subscriptionManager$ =
      this.form.valueChanges.pipe(
      tap(x => {
        this.parentForm.controls[this.targetControlName].setValue(x);
        this.onChange(x);
        this.onTouched();
      }));

      this.form

    this.februaryValidator = this.accomodateForLeapYear ? new AccomodateLeapYearStrategy() : new IgnoreLeapYearStrategy();
    this.months.find(x => x.id === MonthType.February).noOfDays = this.februaryValidator.maxDate;
  }

  private getUniqueControlName(group: FormGroup): string {
    var i = 0;
    while(group.controls[i] != null){ i++; }
    return i + "";
  }

  private convertToFormModel(input: string | Date | moment.Moment): DayOfMonthSelectorForm {
    const inputAsMoment = convertToMoment(input);
if(!inputAsMoment.isValid()){
return null;
}
    return {
      date: inputAsMoment.date(),
      month: inputAsMoment.month() + 1
    }
  }

  get month(): FormControl { return this.form.controls.month as FormControl; }
  get date(): FormControl { return this.form.controls.date as FormControl; }

  private _form: FormGroup;
  get form(): FormGroup{
    return this._form;
  }
  public get value(): DayOfMonthSelectorValue {
    return this.form.value;
  }
  public set value(value: DayOfMonthSelectorValue) {
    this.form.patchValue(value);
  }
  onChange: any = () => { };
  onTouched: any = () => { };
  subscriptionManager$: Observable<any>;
  private _accomodateForLeapYear: any;
  @Input()
  public get accomodateForLeapYear(): any {
    return !!this._accomodateForLeapYear;
  }
  public set accomodateForLeapYear(value: any) {
    this._accomodateForLeapYear = value;
  }
  @Input() parentForm: FormGroup;

  private _savedDate: string | Date | moment.Moment;
  @Input()
  public get savedDate(): string | Date | moment.Moment {
    return this._savedDate;
  }
  public set savedDate(value: string | Date | moment.Moment) {
    this._savedDate = value;
  }

  @Input() submitted: boolean;
  writeValue(obj: any): void {
    if(obj == null){
      return;
    }

    const missingProp = this.modelIsMissingProperty(obj);
    if(missingProp){
      throw missingProp;
    }

    this.value = obj;
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

months: MonthListItem[] = [];



private modelIsMissingProperty(model: DayOfMonthSelectorValue): string {
  if(model.date === undefined){
    return 'The passed in model is missing the "date" property!'
  }

  if(model.month === undefined){
    return  'The passed in model is missing the "month" property!'
  }
}



  private maxDate(months: MonthListItem[], selectedMonth: FormControl, febValidator: () => FebruaryValidationData): ValidatorFn {
    return (ctrl: FormGroup) => {
      const currentMonth = new Maybe(this.getCurrentMonth(months, selectedMonth));
        const isInvalidDate = currentMonth.map(x => ctrl.controls.date.value > x.noOfDays).valueOr(false);
        const isFebruary = currentMonth.map(x => x.id === MonthType.February).valueOr(false);
  
        if (isInvalidDate && isFebruary) {
          return {
            maxDate: febValidator().getErrorMsg(currentMonth.map(x => x.name).value(), currentMonth.map(x => x.noOfDays).value(), ctrl.controls.date.value)
          }
        } else if(isInvalidDate) {
          return {
            maxDate: standardErrorMsg(currentMonth.map(x => x.name).value(), currentMonth.map(x => x.noOfDays).value(), ctrl.controls.date.value)
          }
        }
  
        return null;
    }
  }

  private setTargetValidity(form: AbstractControl): ValidatorFn {
return ctrl => {
return form.errors;
}
  }

/**
 * Calls the provided validator and applies the result to the control we get from `getAffectedCtrl` before returning.
 * @param getAffectedCtrl This is the control we want to merge the validation result with.  The target control may not exist upfront so this function attempts to get it every time.
 * @param validate The function that has the validator functionality we want
 */
  private mergeValidity(getAffectedCtrl: () => AbstractControl, validate: ValidatorFn): ValidatorFn {
      return (ctrl) => {
        const result = validate(ctrl);
        const affectedCtrl = getAffectedCtrl();
        if(affectedCtrl.invalid || (affectedCtrl.valid && result != null)){
          affectedCtrl.setErrors(Object.assign({}, affectedCtrl.errors ? affectedCtrl.errors : {}, result));
        }
        return result;
      }
  }

  getCurrentMonth(months: MonthListItem[], selectedMonth: FormControl): MonthListItem {
    return months.find(month => month.id === +selectedMonth.value);
  }

}

class AccomodateLeapYearStrategy implements FebruaryValidationData {
  getErrorMsg = standardErrorMsg;
  maxDate = moment().isLeapYear() ? 29 : 28;

}

class IgnoreLeapYearStrategy implements FebruaryValidationData {
  getErrorMsg(name, days, inputDays: number): string {
    const part1 = inputDays > 29 ? 'does' : 'may';
    return `${name} ${part1} not have that many days!  When the current year is not a leap year ${name} will have ${days} days.`
  };
  maxDate = 28;

}

function standardErrorMsg(name: string, days: number, inputDays: number) {
  return `${name} does not have that many days!  The last day of ${name} is ${days}.`
}

interface FebruaryValidationData {
  getErrorMsg: (name: string, days: number, inputDays: number) => string;
  maxDate: number;
}

interface MonthListItem {name: string, id: MonthType, noOfDays: number};

/**
 * Defines the default value that should be used for this control.
 */
export function GetDayOfMonthDefaultValue(): DayOfMonthSelectorValue{
  return {
    month: null,
    date: ''
  }
}

interface DayOfMonthSelectorForm {
  month: any,
  date: any
}


export interface DayOfMonthSelectorValue extends DayOfMonthSelectorForm {
  month: number | string,
  date: number | string
}