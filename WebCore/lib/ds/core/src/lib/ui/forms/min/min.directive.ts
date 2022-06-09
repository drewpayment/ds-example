import { Directive, Input, forwardRef, HostBinding, Self, Provider, OnChanges, SimpleChanges } from '@angular/core'
import { NG_VALIDATORS, Validator, AbstractControl, Validators, NgControl, ValidatorFn, ValidationErrors } from '@angular/forms'

export const MIN_VALIDATOR: Provider = {
  provide: NG_VALIDATORS,
  useExisting: forwardRef(() => MinDirective),
  multi: true
};

@Directive({
  selector: '[min]',
  providers: [MIN_VALIDATOR],
  host: {'[attr.min]': 'min ? min : null'}
})
export class MinDirective implements Validator,
  OnChanges {
    private _validator: ValidatorFn;
    private _onChange: () => void;
  
    @Input() min: string;
    ngOnChanges(changes: SimpleChanges): void {
      if ('min' in changes) {
        this._createValidator();
        if (this._onChange) this._onChange();
      }
    }
  
    validate(c: AbstractControl): ValidationErrors|null { return this._validator(c); }
  
    registerOnValidatorChange(fn: () => void): void { this._onChange = fn; }
  
    private _createValidator(): void { this._validator = Validators.min(parseInt(this.min, 10)); }
  }
  