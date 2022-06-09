import { Directive, Self, HostBinding, Optional, Input } from '@angular/core';
import { NgControl, NgForm, FormGroupDirective } from '@angular/forms';

@Directive({
  selector: '[dsFormControlValidator]',
})
export class FormControlValidatorDirective {
  @Input()
  dsFormControlSubmitted: boolean;

  constructor(
    @Self() private ngControl: NgControl,
    @Optional() private parentForm: NgForm,
    @Optional() private parentFormGroup: FormGroupDirective
  ) {}

  @HostBinding('class.is-invalid')
  get isInvalidClass() {
    let isFormSubmitted = this.isBool(this.dsFormControlSubmitted)
      ? this.dsFormControlSubmitted
      : !!this.parentForm ? this.parentForm.submitted : false;

    return (
      this.ngControl.invalid && (this.ngControl.touched || isFormSubmitted)
    );
  }

  private isBool(val: any): boolean {
    return (val === true || val === false);
  }
}
