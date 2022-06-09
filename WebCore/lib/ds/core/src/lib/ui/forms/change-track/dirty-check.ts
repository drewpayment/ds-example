import { AbstractControl, FormArray, FormGroup } from '@angular/forms';
import { combineLatest, defer, fromEvent, merge, Observable, of } from 'rxjs';
import {
  debounceTime,
  distinctUntilChanged,
  map,
  shareReplay,
  startWith,
  withLatestFrom,
} from 'rxjs/operators';

interface DirtyCheckConfig {
  debounce?: number;
  withDisabled?: boolean;
}

function mergeConfig(config: DirtyCheckConfig): DirtyCheckConfig {
  return {
    debounce: 300,
    withDisabled: true,
    ...config,
  };
}

function getControlValue(control: AbstractControl, withDisabled: boolean) {
  if (
    withDisabled &&
    (control instanceof FormGroup || control instanceof FormArray)
  ) {
    return control.getRawValue();
  }
  return control.value;
}

export function dirtyCheck<U>(
  control: AbstractControl,
  config: DirtyCheckConfig = {}
): Observable<boolean> {
  const { debounce, withDisabled } = mergeConfig(config);
  const valueChanges$ = 
    control.valueChanges.pipe(
      debounceTime(debounce),
      distinctUntilChanged()
    );
  
  let counter = 0;

  return new Observable((observer) => {
    const isDirty$: Observable<boolean> = combineLatest([
      valueChanges$,
    ]).pipe(
      map( () => { return control.dirty; }),
      startWith(false),
      shareReplay(1)
    );

    observer.add(
      fromEvent(window, 'beforeunload')
        .pipe(withLatestFrom(isDirty$))
        .subscribe(([event, isDirty]) => {
          if (isDirty) {
            event.preventDefault();
            event.returnValue = false;
          }
        })
    );

    return isDirty$.subscribe(observer);
  });
}