import { Component, OnInit, Input, ChangeDetectorRef, ChangeDetectionStrategy, OnDestroy } from '@angular/core';
import { IFormItemComponent } from '../calendar-year-form/calendar-year-form.model';
import { DateRangeComp } from '../calendar-year-form/calendar-year-form.component';
import { FormControl } from '@angular/forms';
import { Moment } from 'moment';
import { Subject } from 'rxjs';
import { Maybe } from '@ds/core/shared/Maybe';
import { RunChangeDetectionWhenAnyControlUpdates } from '@ds/core/shared/shared-api-fn';


export type JustBoolean = () => boolean;
export function convertToJustBoolean(submitted: boolean | JustBoolean): JustBoolean {
  if(typeof submitted === "function"){
    return submitted;
  } else {
  return () => submitted;
  }
}

@Component({
  selector: 'ds-date-range',
  templateUrl: './date-range.component.html',
  styleUrls: ['./date-range.component.scss']
})
export class DateRangeComponent implements OnInit, IFormItemComponent<DateRangeComp> {
  _startDate: FormControl
  _dueDate: FormControl;
  _processStartDate: FormControl;
  _processEndDate: FormControl;
  _defaultMinMoment: Moment;
  _defaultMaxMoment: Moment;
  _submitted: JustBoolean;

  readonly momentFormatString = 'MM/DD/YYYY';

  @Input()
  set data(data: DateRangeComp) {
    const dataM = new Maybe(data);
    this._startDate =  dataM.map(x => x.startDate).value();
    this._dueDate =  dataM.map(x => x.dueDate).value();
    this._processStartDate =  dataM.map(x => x.processStartDate).value();
    this._processEndDate =  dataM.map(x => x.processEndDate).value();
    this._defaultMinMoment =  dataM.map(x => x.defaultMinMoment).value();
    this._defaultMaxMoment =  dataM.map(x => x.defaultMaxMoment).value();
    this._submitted =  dataM.map(x => x.submitted).map(convertToJustBoolean).value();
  }

  constructor() { }

  ngOnInit() {
  }

}

@Component({
  selector: 'ds-date-range-on-push',
  templateUrl: './date-range.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DateRangeOnPushComponent extends DateRangeComponent implements OnInit, OnDestroy, DateRangeComp {

  private readonly _unsubscriber = new Subject();
  
  @Input()
  set startDate(employeeEvalStartDate: FormControl) {
    this._startDate = employeeEvalStartDate;
  }
  @Input()
  set dueDate(employeeEvalDueDate: FormControl) {
    this._dueDate = employeeEvalDueDate;
  }
  @Input()
  set processStartDate(employeeEvalDueDate: FormControl) {
    this._processStartDate = employeeEvalDueDate;
  }
  @Input()
  set processEndDate(employeeEvalDueDate: FormControl) {
    this._processEndDate = employeeEvalDueDate;
  }
  @Input()
  set defaultMinMoment(defaultMinMoment: Moment) {
    this._defaultMinMoment = defaultMinMoment;
  }
  @Input()
  set defaultMaxMoment(defaultMaxMoment: Moment) {
    this._defaultMaxMoment = defaultMaxMoment;
  }
  @Input()
  set submitted(submitted: boolean | JustBoolean) {
    this._submitted = convertToJustBoolean(submitted);
  }
  constructor(private ref: ChangeDetectorRef) {
    super()
   }

  ngOnInit(): void {
    RunChangeDetectionWhenAnyControlUpdates(
      [this._startDate, this._dueDate],
      [{dependency: this._processStartDate, invalidControl: this._startDate},
        {dependency: this._processStartDate, invalidControl: this._dueDate},
        {dependency: this._processEndDate, invalidControl: this._startDate},
        {dependency: this._processEndDate, invalidControl: this._dueDate}],
      this.ref,
      this._unsubscriber
    )
   }

   ngOnDestroy(): void {
    this._unsubscriber.next();
  }  
}

