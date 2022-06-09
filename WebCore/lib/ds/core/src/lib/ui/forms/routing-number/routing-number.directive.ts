import { Directive } from '@angular/core';
import { Validator, ValidatorFn, AbstractControl, ValidationErrors, NG_VALIDATORS } from '@angular/forms';

/**
 * Derived from: https://github.com/DrShaffopolis/bank-routing-number-validator
 */
function routingNumberIsValid(routingNumberToTest) {
    if (!routingNumberToTest) { //all 0's is technically a valid routing number, but it's inactive
        return false;
    }
  
    var routing = routingNumberToTest.toString();
    // while (routing.length < 9) {
    //   routing = '0' + routing; //I refuse to import left-pad for this
    // }
  
    //gotta be 9  digits
    var match = routing.match("^\\d{9}$");
    if (!match) {
        return false;
    }
  
    //The first two digits of the nine digit RTN must be in the ranges 00 through 12, 21 through 32, 61 through 72, or 80.
    //https://en.wikipedia.org/wiki/Routing_transit_number
    const firstTwo = parseInt(routing.substring(0, 2));
    const firstTwoValid =  (0 <= firstTwo && firstTwo <= 12)
                        || (21 <= firstTwo && firstTwo <= 32)
                        || (61 <= firstTwo && firstTwo <= 72)
                        || firstTwo === 80;
    if (!firstTwoValid) {
        return false;
    }
  
    //this is the checksum
    //http://www.siccolo.com/Articles/SQLScripts/how-to-create-sql-to-calculate-routing-check-digit.html
    const weights = [3, 7 ,1];
    var sum = 0;
    for (var i=0 ; i<8; i++) {
        sum += parseInt(routing[i]) * weights[i % 3];
    }
  
    return (10 - (sum % 10)) % 10 === parseInt(routing[8]);
}

/**
 * Validates a form-control's value matches a valid Routing Transit Number
 */
export function routingNumberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        return routingNumberIsValid(control.value) ? null : {'routingNumber': {value: control.value}};
    };
}

@Directive({
    selector: '[dsRoutingNumber]',
    providers: [{provide: NG_VALIDATORS, useExisting: RoutingNumberDirective, multi: true}]
})
export class RoutingNumberDirective implements Validator {
    validator: ValidatorFn;
    constructor() { 
        this.validator = routingNumberValidator();
    }
    validate(control: AbstractControl): ValidationErrors {
        return this.validator(control);
    }
}
