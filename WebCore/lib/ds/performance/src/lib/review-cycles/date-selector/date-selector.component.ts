import { Component, OnInit, Input, ChangeDetectionStrategy, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { DateUnit } from "../shared/time-unit.enum";
import { Subject } from 'rxjs';
import { RunChangeDetectionWhenAnyControlUpdates } from '@ds/performance/shared/shared-performance-fns';

@Component({
  selector: 'ds-date-selector',
  templateUrl: './date-selector.component.html',
  styleUrls: ['./date-selector.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DateSelectorComponent implements OnInit, OnDestroy {

  private readonly _unsubscriber = new Subject();

  @Input() label: string;
  @Input() helpText: string;
  @Input() parentForm: FormGroup;
  @Input() durationControlName: string;
  @Input() unitTypeControlName: string;
  @Input() submitted: boolean;

  readonly errors


numOfUnits = 3

  get textInputWidth(){
    return ((.75 * ( 1 / (1 + this.numOfUnits))) * 100);
  }

  get TimeUnit() {
    return DateUnit;
  }

  get SelectedUnitType(){
    return this.parentForm.controls[this.unitTypeControlName];
  }

  get DurationCtrl(){
    return this.parentForm.controls[this.durationControlName];
  }

  constructor(private ref: ChangeDetectorRef) { }

  ngOnInit() {
    RunChangeDetectionWhenAnyControlUpdates(
      [this.DurationCtrl, this.SelectedUnitType],
      [],
      this.ref,
      this._unsubscriber
    );
  }

  ngOnDestroy(): void {
    this._unsubscriber.next();
  }
}
