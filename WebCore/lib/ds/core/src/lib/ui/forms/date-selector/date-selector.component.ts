import { Component, OnInit, Input, ChangeDetectionStrategy, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Subject } from 'rxjs';
import { DateUnit } from '../../../shared/time-unit.enum';
import { RunChangeDetectionWhenAnyControlUpdates } from '@ds/core/shared/shared-api-fn';

@Component({
  selector: 'ds-date-selector',
  templateUrl: './date-selector.component.html',
  styleUrls: ['./date-selector.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DateSelectorComponent implements OnInit, OnDestroy {

  private readonly _unsubscriber = new Subject();
  private previousVal: DateUnit;

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

  /**
   * When the user clicks the same option again. Deselect the option.
   * @param newVal The value that the user just clicked.
   */
  optionClicked(newVal: DateUnit): void {
    if(newVal == this.previousVal){
      this.SelectedUnitType.reset();
      //make sure the option can be selected again
      newVal = this.SelectedUnitType.value;
    }
    this.previousVal = newVal;
  }

  ngOnDestroy(): void {
    this._unsubscriber.next();
  }
}
