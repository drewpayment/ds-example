import { FormGroup, FormControl, Validators, ValidatorFn, ValidationErrors } from '@angular/forms';

/**
 * Validates an address block form group's value to make sure zip/postal code is Validated correctly based on the country code for US/Canada
 */
export function zipCodeValidator(): ValidatorFn{
    return (fg: FormGroup): ValidationErrors | null => {
        const code = fg.controls['zip'].value;
        const countryObj = fg.controls['country'].value;

        const countryId = countryObj == null ? 0 : countryObj.countryId;

        const usRegex = new RegExp(/^\d{5}(-\d{4})?$/);
        const caRegex = new RegExp(/^([ABCEGHJKLMNPRSTVXY]|[abceghjklmnprstvxy]){1}\d{1}([ABCEGHJKLMNPRSTVWXYZ]|[abceghjklmnprstvwxyz]){1}( |-){0,1}\d{1}([ABCEGHJKLMNPRSTVWXYZ]|[abceghjklmnprstvwxyz]){1}\d{1}$/)


        if(countryId == 1){ //US
            if(code == "") return {'zipRequired': true}
            return usRegex.test(code) ? null : {'usZipError':true}
        }

        else if(countryId == 7){ //Canada
            if(code == "") return {'zipRequired': true}
            return caRegex.test(code) ? null : {'caZipError':true}
        }

        else{ //if the country is something other than US or Canada, zip Code is not required
            return null;
        }
    }
}
