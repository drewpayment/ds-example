import { Observable, combineLatest, of, merge, iif, Subscription } from "rxjs";

import {
  AbstractControl,
  ValidatorFn,
  Validators,
  FormControl,
} from "@angular/forms";

import {
  scan,
  map,
  tap,
  mergeMap,
  takeUntil,
  filter,
  withLatestFrom,
} from "rxjs/operators";
import { ChangeDetectorRef } from "@angular/core";
import { Maybe } from "@ds/core/shared/Maybe";

// if these are used outside of performance reviews in the future, it might make sense to move them into core?

/**
 * When the control we are reacting to had no value and now gains a value, make another control required.
 * @param sourceControlChanges The stream of values of the control we are reacting to.
 * @param affectedControl The form control to add the validator(s) to.
 * @param defaultValidators The collection of validators that are always set on the affectedControl.  When there are none, just pass an empty array.
 * @param defaultFormVal The default value that the form controls were initialized to.
 */
export function SetRequiredWhenSourceGainsValue(
  sourceControlChanges: Observable<any>,
  affectedControl: AbstractControl,
  defaultValidators: ValidatorFn[],
  defaultFormVal: any,
  mapper?: (val: any) => any
): Observable<any> {
  return sourceControlChanges.pipe(
    map((x) => {
      if (mapper) {
        return new Maybe(x).map(() => mapper(x));
      }
      return new Maybe(x);
    }),
    scan((acc, current) => {
      const isNotDefault = (x) => x != defaultFormVal;
      const prevHasVal = acc.map(isNotDefault).valueOr(false);
      const currHasVal = current.map(isNotDefault).valueOr(false);
      const shouldRemoveValidator = prevHasVal && !currHasVal;
      const shouldAddValidator = !prevHasVal && currHasVal;
      if (shouldRemoveValidator) {
        affectedControl.setValidators(Validators.compose(defaultValidators));
      } else if (shouldAddValidator) {
        affectedControl.setValidators(
          Validators.compose([Validators.required].concat(defaultValidators))
        );
      }
      affectedControl.updateValueAndValidity({ emitEvent: false }); // if we always emit an event then our form controls will be in a long cycle (not sure if it's
      // infinite, it does seem to stop at some point) of reacting to the other controls event emission
      // and then emitting an event itself
      return current;
    }, <Maybe<any>>new Maybe(null)),
    map((x) => x.value())
  );
}

/**
 * When the control we are reacting to had no value and now gains a value, set that new value on another control.
 * @param sourceControl The control that we are reacting to
 * @param sourceControlChanges The stream of changes from the control we want to react to (this should be referencing the same control as the isControlValid function)
 * @param affectedControl The control to update
 * @param defaultFormVal the default value that the controls were initialized with
 */
export function SetValueWhenSourceGainsValue<T>(
  sourceControl: AbstractControl,
  sourceControlChanges: Observable<T>,
  affectedControl: AbstractControl,
  defaultFormVal: any
) {
  return combineLatest(
    sourceControlChanges.pipe(map((x) => new Maybe(x))),
    of(affectedControl)
  ).pipe(
    map((x) => ({ changes: x[0], affectedControl: x[1] })),
    scan<
      { changes: Maybe<T>; affectedControl: AbstractControl },
      {
        changes: Maybe<T>;
        affectedControl: AbstractControl;
        shouldSetEndDate: boolean;
      }
    >(
      (previous, current) => {
        const hasNonDefaultValue = (change: Maybe<T>, defaultVal: any) =>
          change.value() == null || change.value() == defaultVal;

        const sourceControlDidNotHaveValue = hasNonDefaultValue(
          previous.changes,
          defaultFormVal
        );
        const sourceControlNowHasValue = !hasNonDefaultValue(
          current.changes,
          defaultFormVal
        );
        const affectedControlHasNoValue = hasNonDefaultValue(
          new Maybe<T>(current.affectedControl.value),
          defaultFormVal
        );
        previous.shouldSetEndDate =
          sourceControlDidNotHaveValue &&
          sourceControlNowHasValue &&
          sourceControl.valid &&
          affectedControlHasNoValue;
        previous.changes = current.changes;
        previous.affectedControl = current.affectedControl;

        return previous;
      },
      {
        changes: new Maybe<T>(sourceControl.value),
        affectedControl: affectedControl,
        shouldSetEndDate: false,
      }
    ),
    map((x) => ({
      newValue: x.changes.value(),
      affectedControl: x.affectedControl,
      shouldSetEndDate: x.shouldSetEndDate,
    })),
    mergeMap((x) => {
      const setEndDate$ = of(x).pipe(
        tap((y) => y.affectedControl.setValue(y.newValue))
      );
      const doNothing$ = of(x);

      return iif(() => x.shouldSetEndDate === true, setEndDate$, doNothing$);
    }),
    map((x) => x.newValue)
  );
}

/**
 * When any control's value in the list of controls changes, call updateValueAndValidity on each control in another list
 * @param sourceControls When any of the controls in this list change value emit an event
 * @param affectedControls When an event is emitted call updateValueAndValidity on each of these controls
 */
export function ValidateDatesWhenSourcesValueChange(
  sourceControls: AbstractControl[],
  affectedControls: AbstractControl[]
): Observable<any> {
  return merge(...sourceControls.map((x) => x.valueChanges)).pipe(
    tap(() => {
      affectedControls.forEach((control) => control.updateValueAndValidity());
    })
  );
}

// export function CreateSaveHandler<T>(
//     element: ElementRef,
//     setSubmitted: () => void,
//     isValid: () => boolean,
//     SaveFn: () => Observable<T>,
//     onSuccessFn: (rcr: T) => void,
//     cancel$: Observable<any>,
//     msgSvc: DsMsgService,
//     operation: string,
//     continueStreamOnFailure?: boolean): Subscription {
//         const continueOnFailure = !!continueStreamOnFailure;

//     return fromEvent(element.nativeElement, 'click').pipe(
//         tap(setSubmitted),
//         exhaustMap(() => iif(isValid,
//             defer(() => of(null).pipe(
//                 tap(() => msgSvc.setMessage('Saving....', MessageTypes.warning)),
//                 concatMap(() => SaveFn()),
//                 tap(() => msgSvc.setTemporarySuccessMessage(operation + " completed successfully.")),
//                 catchError(e => httpError(msgSvc, operation, continueOnFailure, e, <T>null)))
//                 ).pipe(tap(onSuccessFn)),
//             empty())),
//         takeUntil(cancel$)).subscribe();
// }

// /**
//  * The token used to inject a validator builder which can create a validation function that validates the input against a minimum date
//  */
// export const MIN_DATEPICKER_VALIDATOR = new InjectionToken('min_datepicker_validator', {

// });

// function log(error: any, message: string): any {
//     // this is where we would wire up any logging we will do when we throw JS errors.
//     if (error) console.dir(error);

//     console.log(message); // for now, we are simply printing errors to the console.
// }

// export function httpError<T>(msgSvc: DsMsgService, operation = 'operation', continueOnFailure: boolean, error: any, result?: T) {
//         let errorMsg = error.error.errors != null && error.error.errors.length
//             ? error.error.errors[0].msg
//             : error.message;

//         log(error, `${operation} failed: ${errorMsg}`);

//         // TODO: better job of transforming error for user consumption
//         msgSvc.setTemporaryMessage(`Sorry, this operation failed: ${errorMsg}`, MessageTypes.error, 6000);

//         // let app continue by return empty result
//         return continueOnFailure ? of(result as T) : throwError(error);
// }

/**
 * Creates a stream that will run change detection on a component.  Change detection will be run
 * when the validity of any of the controls change.  It will also run when the value of a
 * dependency changes for a control that is invalid.
 * @param controls When the status of any of these controls change we need to run change detection
 * @param controlAndDependency A list of objects containing the target controls and their dependencies. When
 * any target control is invalid and the value of its dependency changes we need to run change detection
 * @param ref The service that will tell Angular that the component should be checked for changes
 * @param cancel$ Stop the stream when this emits
 */
export function RunChangeDetectionWhenAnyControlUpdates(
  controls: AbstractControl[],
  controlAndDependency: {
    invalidControl: AbstractControl;
    dependency: AbstractControl;
  }[],
  ref: ChangeDetectorRef,
  cancel$: Observable<any>
): Subscription {
  const updatedControls: Observable<any>[] = [];
  controls.forEach((control) => {
    updatedControls.push(
      control.statusChanges.pipe(
        scan<string, { status: string; changed: boolean }>(
          (acc, curr) => {
            acc.changed = (curr || "").localeCompare(acc.status) !== 0;
            acc.status = curr;
            return acc;
          },
          { status: "VALID", changed: false }
        ),
        map((x) => x.changed),
        filter((x) => x === true)
      )
    );
  });

  controlAndDependency.forEach((control) => {
    updatedControls.push(
      control.dependency.valueChanges.pipe(
        withLatestFrom(of(control.invalidControl)),
        filter((x) => x[1].invalid)
      )
    );
  });

  return merge(...updatedControls)
    .pipe(
      tap(() => ref.markForCheck()),
      takeUntil(cancel$)
    )
    .subscribe();
}

export function mustBePercentValidator(): ValidatorFn {
  return (control: FormControl): { [key: string]: any } | null => {
    return control.value >= -100.0 && control.value <= 100.0
      ? null
      : { mustBePercent: {} };
  };
}
