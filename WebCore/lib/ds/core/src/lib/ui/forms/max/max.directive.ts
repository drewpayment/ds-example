import { Directive, Input, forwardRef, Provider, OnChanges, SimpleChanges } from '@angular/core'
import { NG_VALIDATORS, Validator, AbstractControl, Validators, ValidatorFn, ValidationErrors } from '@angular/forms'

export const MAX_VALIDATOR: Provider = {
  provide: NG_VALIDATORS,
  useExisting: forwardRef(() => MaxDirective),
  multi: true
};

@Directive({
  // added 'ds-' to make sure we don't attach this directive to something we 
  // don't want to (like when we want to use angular material datepicker's max)
  selector: '[dsMax]',
  providers: [MAX_VALIDATOR],
  host: {'[attr.max]': 'max ? max : null'}
})
export class MaxDirective implements Validator,
  OnChanges {
    private _validator: ValidatorFn;
    private _onChange: () => void;
  
    @Input('dsMax') max: string;
    ngOnChanges(changes: SimpleChanges): void {
      if ('max' in changes) {
        this._createValidator();
        if (this._onChange) this._onChange();
      }
    }
  
    validate(c: AbstractControl): ValidationErrors|null { return this._validator(c); }
  
    registerOnValidatorChange(fn: () => void): void { this._onChange = fn; }
  
    private _createValidator(): void { this._validator = Validators.max(parseInt(this.max, 10)); }
  }
  