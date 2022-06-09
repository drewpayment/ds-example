import { Directive } from '@angular/core';
import { Validator, ValidatorFn, AbstractControl, ValidationErrors, NG_VALIDATORS, FormGroup } from '@angular/forms';
import { valdateUsingRegExp } from '@util/ds-common';


function isAmountAndAmountTypeValid(amount: number, amountType: string) {

    if (amountType.includes('ercent') || amountType.includes('%')){ //if the amount type is set to percent of something, make sure the amount is an actual percent
        if(amount < 0 || amount > 100) {
            return false;
        }
    }
    return true;
}

/**
 * Validates a form-control's value makes sense for a given amount type value
 */
export function amountNumberValidator(amountTypeValue: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        return isAmountAndAmountTypeValid(control.value, amountTypeValue) ? null : {'amount': {value: control.value}};
    };
}

export function amountAndAmountTypeValidator(multipleHundreds: boolean): ValidatorFn{
    return (control: FormGroup): ValidationErrors | null => {
        const amount = control.controls['amount'].value;
        const amountType = control.controls['amountType'].value;
        const isActive = control.controls['isActive'].value;
        if (amountType.includes('ercent') || amountType.includes('%')){ //if the amount type is set to percent of something, make sure the amount is an actual percent
            if(amount < 0 || amount > 100) {
                return {'amount': true};
            }
        }
        if(amountType == "Percent of Net" && amount == 100 && multipleHundreds && isActive == true){
            return {'multipleHundreds': true};
        }
        return null;
    };
}

// export const amountAndAmountTypeValidator: ValidatorFn = (control: FormGroup): ValidationErrors | null => {
//     const amount = control.controls['amount'].value;
//     const amountType = control.controls['amountType'].value;
//     if (amountType.includes('ercent') || amountType.includes('%')){ //if the amount type is set to percent of something, make sure the amount is an actual percent
//         if(amount < 0 || amount > 100) {
//             return {'amount': true};
//         }
//     }
//     if(amountType == "Percent of Net" && amount == 100 && multipleHundreds == true){
//         return {'multipleHundreds': true};
//     }
//     return null;
// };

export function numberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        return isNaN(control.value) ? {'notANumber': control.value} : null;
    };
}

export function greaterThen16Validator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        if (control.value){
            const l = control.value.length;
            return (l > 16) ? {'greaterThen16digits': control.value} : null;
        }
        else
        {
            return null;
        }
    };
}


