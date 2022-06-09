import { Directive, HostListener, Input, OnDestroy, Self } from '@angular/core';
import { Validator, ValidatorFn, AbstractControl, NG_VALIDATORS, ValidationErrors, NgControl } from '@angular/forms';
import { validateDateString } from '@util/dateUtilities';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Directive({
  selector: '[dsAccountAmount]'
})
export class AccountAmountDirective implements OnDestroy {
  destroy$ = new Subject();
  isMatch: boolean = true;
  /*
  * percent is default
  * dollar is optional
  */
  @Input('dsAccountAmount') _type: string;

  constructor(
    @Self() private ngControl: NgControl
  ) {}
  

  ngOnChanges() {
    this.ngControl
        .control
        .valueChanges
        .pipe(takeUntil(this.destroy$))
        .subscribe();
  }

  @HostListener('blur') onBlur() {
    this.match(this.ngControl.value);
    if (!this.isMatch) {
      this.ngControl.control.setValue(0, { emitEvent: false })
    }
  }
  @HostListener('keydown', ['$event'])onKeyDown(e: KeyboardEvent) { 
    if (  e.key == 'Backspace' || 
          e.key == 'Tab' ||
          e.key == '.' ||
          e.key == '-' ) {
      return true;
    }
    if ( e.key === ' ' || isNaN(Number(e.key)) ) {    
      e.preventDefault();  
    }
  }
  match(v) {
    let value = v;
    let dollarRegex = /^(\d+)(\.\d+)?$/;
    let percentRegex = /^(100|([1-9](\d)?|0)(\.\d{1,2})?)$/;

    if(this._type == 'dollar') {
      this.isMatch = dollarRegex.test(value);
    } else {
      this.isMatch = percentRegex.test(value);
    }
  }
  ngOnDestroy() {

  }
  accountNumberValidator(accountNumberToTest) {
    if(!accountNumberToTest) {
      return false;
    }

    let account = accountNumberToTest.toString();
    let percentRegex = /^(100|([1-9](\d)?|0)(\.\d{1,2})?)$/;

    let match = account.match(percentRegex);
    if (!match) {
      return false;
    }

  }
}
