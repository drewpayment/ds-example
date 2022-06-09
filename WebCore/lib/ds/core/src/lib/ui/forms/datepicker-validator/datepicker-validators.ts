import * as moment from 'moment';
import { ValidatorFn, AbstractControl } from '@angular/forms';
import { InjectionToken } from '@angular/core';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';

type compareResultHandler = (isInvalid: boolean, onError: any) => any;
type getOtherDateFn = () => moment.Moment | Date | string;
type compareDatesFn = (controlMoment: moment.Moment, otherMoment: moment.Moment) => boolean;
export const maxDateError = Object.freeze({'maxDateError': 'The value cannot be after the max date'});
export const minDateError = Object.freeze({'minDateError': 'The value cannot be before the min date'});

/**
 * A function that will allow you to apply all the values to the validator function except the abstract control
 * which is provided by Angular.  A custom date validator is necessary because when you call 'setValidators' on an abstract control,
 * Angular Material's datepicker date validators are removed.  It doesn't look like you can manually get those
 * validator functions and add them to the control again (unless you do some potentially hacky stuff).
 * 
 * @param compareFn The function used to determine whether the provided date meets the validation requirement.  The provided moments have 
 * time of day removed.  When it does meet the requirement, true is returned.  Otherwise false is returned
 * @param getOtherDate The function used to get the date that the input's date must be compared with.
 * @param defaultFormVal The default value that the input was initialized with.
 * @param handleCompareResult Takes the result of the compareFn and determines the return value
 * @param onError The default value to return when the input's date does not meet the validation requirement.  
 * This parameter is necessary to keep the handleCompareResult function pure
 */
export type datepicker_validator_builder = (
    compareFn: compareDatesFn,
    getOtherDate: getOtherDateFn,
    defaultFormVal: any,
    handleCompareResult: compareResultHandler,
    onError: any
) => ValidatorFn;

/**
 * A datepicker validator that already has a compareFn, handleCompareResult, and onError arguments supplied.  This is 
 * the validator that will satisfy 90% of the scenarios a developer might encounter.  When you encounter a scenario 
 * that this function does not work @see datepicker_validator
 */
export type datepicker_validator_builder_default = (
    getOtherDate: getOtherDateFn,
    defaultFormVal: any
) => ValidatorFn;

export function DPVDefaultCompare(
    convertToMoment: (date: string | Date | moment.Moment) => moment.Moment,
    compareFn: compareDatesFn,
    handleCompareResult: compareResultHandler,
    onError: any
): datepicker_validator_builder_default {
    return (
        getOtherDate: getOtherDateFn,
        defaultFormVal: any
    ) =>
        (control) => {
            return  validateDate(control, compareFn, getOtherDate, defaultFormVal, handleCompareResult, convertToMoment, onError);
        }
}

export function DatePickerValidator(
    convertToMoment: (date: string | Date | moment.Moment) => moment.Moment
): datepicker_validator_builder {
    return (
        compareFn: compareDatesFn,
        getOtherDate: getOtherDateFn,
        defaultFormVal: any,
        handleCompareResult: compareResultHandler,
        onError: any
    ) =>
        (control) => {
           return  validateDate(control, compareFn, getOtherDate, defaultFormVal, handleCompareResult, convertToMoment, onError);
        }
}

/**
 * 
 * @param control The control passed to this function by Angular when validation is run
 * @param compareFn @see datepicker_validator
 * @param getOtherDate @see datepicker_validator
 * @param defaultFormVal @see datepicker_validator
 * @param handleCompareResult @see datepicker_validator
 * @param convertToMoment @see datepicker_validator
 * @param onError @see datepicker_validator
 */
function validateDate(control: AbstractControl,
    compareFn: compareDatesFn,
    getOtherDate: getOtherDateFn,
    defaultFormVal: any,
    handleCompareResult: compareResultHandler,
    convertToMoment: (date: string | Date | moment.Moment) => moment.Moment,
    onError: any): null | object {
    if (control.value == null || control.value == defaultFormVal) return null;
    const otherMoment = convertToMoment(getOtherDate()).startOf('day');
    const controlMoment = convertToMoment(control.value).startOf('day');
    if (!controlMoment.isValid()) return null;
    return handleCompareResult(compareFn(controlMoment, otherMoment), onError);
}

/**
 * The token used to inject a validator builder which can create a validation function that is much more customizable than the 
 * validation functions you can create with the other DATEPICKER_VALIDATOR tokens
 */
export const DATEPICKER_VALIDATOR = new InjectionToken<datepicker_validator_builder>('datepicker_validator', {
    providedIn: 'root',
    factory: () => DatePickerValidator(convertToMoment)
});

/**
 * The token used to inject a validator builder which can create a validation function that validates the input against a maximum date
 */
export const MAX_DATEPICKER_VALIDATOR = new InjectionToken<datepicker_validator_builder_default>('max_datepicker_validator', {
    providedIn: 'root',
    factory: () => DPVDefaultCompare(convertToMoment, (a, b) => a.isAfter(b), (isInvalid, maxDateError) => isInvalid ? maxDateError : null, maxDateError )
});

/**
 * The token used to inject a validator builder which can create a validation function that validates the input against a minimum date
 */
export const MIN_DATEPICKER_VALIDATOR = new InjectionToken<datepicker_validator_builder_default>('min_datepicker_validator', {
    providedIn: 'root',
    factory: () => DPVDefaultCompare(convertToMoment, (a, b) => a.isBefore(b), (isInvalid, minDateError) => isInvalid ? minDateError : null, minDateError)
});