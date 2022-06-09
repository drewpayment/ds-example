import { DecimalPipe } from '@angular/common';
import { Directive, HostListener, Input, OnDestroy, Self } from '@angular/core';
import { NgControl } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Directive({
  selector: '[dsDecimalFormatter]'
})
export class DecimalFormatDirective implements OnDestroy {

  destroy$ = new Subject();
  /*
  * Types include percent (4), rate (4)
  */
  @Input('dsDecimalFormatter') _type: string;

  constructor(
    @Self() private ngControl: NgControl,
    private decimalPipe: DecimalPipe
  ) {}
   
   ngOnChanges() {
    this.setValue( this.formatDecimal(this.ngControl.value) );
    this.ngControl
        .control
        .valueChanges
        .pipe(takeUntil(this.destroy$))
        .subscribe();

  }

   @HostListener('blur') onBlur() {
     let v = this.ngControl.value;
     !!v && this.setValue( this.formatDecimal(v) );
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

   formatDecimal(v) {
      // Default value will be 2 decimals
      if (this._type == 'percent' || this._type == 'rate') {
        return this.decimalPipe.transform(v, '1.4-4');
      } else {
        return this.decimalPipe.transform(v, '1.2-2');
      }
   }

   setValue(v) {
    this.ngControl.control.setValue(v, { emitEvent: false })
  }
  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
