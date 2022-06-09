import { Directive, forwardRef } from '@angular/core';
import { Validator, FormControl, ValidationErrors, NG_VALIDATORS } from '@angular/forms';

@Directive({
  selector: '[dsCustomControlValidator][ngModel],[validateEmail][formControl]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: CustomControlValidatorDirective, multi: true }
  ]
})
export class CustomControlValidatorDirective implements Validator {
  validate(c: FormControl): ValidationErrors | null {
    return CustomControlValidatorDirective.validator(c);
  }

  static validator(control: FormControl): ValidationErrors | null {
    if(control.value == null || control.value == false || control.value == 0) {
      return {required: 'This field is required.'}
    }
    return null;
  }

  constructor() { }

}
