import { Observable, of, empty, fromEvent, iif, Subscription, throwError, OperatorFunction,
     merge, ObservedValueOf, ObservableInput } from 'rxjs';
import { publishReplay, refCount, map, exhaustMap, expand, withLatestFrom, last, finalize, tap, concatMap,
     catchError, takeUntil, scan, filter } from 'rxjs/operators';
import { InjectionToken, ElementRef, Provider, ChangeDetectorRef } from '@angular/core';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { Maybe } from './Maybe';
import { AbstractControl } from '@angular/forms';

export function createCache<T>(request: () => Observable<T>, buffer?: number) {
    return request().pipe(
        publishReplay(buffer),
        refCount()
    );
}

export function DoLongRunningTask<U, T>(longRunningTask: (input: U) => Observable<T>, inputStream$: Observable<U>,
 areInputsEqual: (newVal: U, oldVal: U) => boolean) {

    return inputStream$.pipe(
        map<U, LongRunningTaskRecursionModel<U, T>>(startingData => ({ prevInputVal: null, currentInputVal: startingData, result: null })),
        exhaustMap(x => of(x).pipe(
            expand((next) => {
                return areInputsEqual(next.currentInputVal, next.prevInputVal) ? empty() :
                 executeLongRunningTask(next.currentInputVal, inputStream$, longRunningTask);
            }),
            last(),
            map(x => x.result)
        ))
    );
}

function executeLongRunningTask<U, T>(currentInputVal: U, inputStream: Observable<U>, longRunningTask:
     (input: U) => Observable<T>): Observable<LongRunningTaskRecursionModel<U, T>> {
    return longRunningTask(currentInputVal).pipe(
        withLatestFrom(inputStream),
        map<[T, U], LongRunningTaskRecursionModel<U, T>>(result => ({ prevInputVal: currentInputVal, result: result[0],
             currentInputVal: result[1] })));
}

interface LongRunningTaskRecursionModel<U, T> {
    result: T;
    prevInputVal: U;
    currentInputVal: U;
}

const AttachSaveHandlerImpl: AttachSaveHandler = <T>(
    element: ElementRef,
    setSubmitted: () => void,
    isValid: () => boolean,
    SaveFn: () => Observable<T>,
    cancel$: Observable<any>,
    msgSvc: DsMsgService,
    operation: string,
    continueStreamOnFailure?: boolean) => {
    const continueOnFailure = !!continueStreamOnFailure;

    return fromEvent(element.nativeElement, 'click').pipe(
        tap(setSubmitted),
        SaveHandlerImpl(
            msgSvc,
            SaveFn,
            operation,
            continueOnFailure,
            isValid),
        takeUntil(cancel$)).subscribe();
};

const attachSaveHandlerFactory: (svc: DsMsgService) => InjectedAttachSaveHandler = (msgSvc: DsMsgService) => {
    return (<T>(element: ElementRef,
        setSubmitted: () => void,
        isValid: () => boolean,
        SaveFn: () => Observable<T>,
        cancel$: Observable<any>,
        operation: string,
        continueStreamOnFailure?: boolean) => AttachSaveHandlerImpl(
            element,
            setSubmitted,
            isValid,
            SaveFn,
            cancel$,
            msgSvc,
            operation,
            continueStreamOnFailure
        ));
};

export const AttachSaveHandlerToken = new 
InjectionToken('Used to inject a partially applied function that can attach a save handler to a page element.');

export const attachSaveHandlerFnProvider: Provider = {
    provide: AttachSaveHandlerToken,
    useFactory: attachSaveHandlerFactory,
    deps: [DsMsgService]
};

export const SaveHandlerImpl: SaveHandler =
    <T, R>(msgSvc: DsMsgService,
        apiCall: (input: T) => Observable<R>,
        operation: string,
        continueStreamOnFailure: boolean,
        isValid?: () => boolean) => (source: Observable<T>) =>
            source.pipe(exhaustMap((input) => {

                const effect = of(input).pipe(
                    tap(() => msgSvc.setMessage('Saving....', MessageTypes.warning)),
                    concatMap((x) => apiCall(x)),
                    tap(() => msgSvc.setTemporarySuccessMessage(operation + ' completed successfully.')));

                const saveFn$ = httpError(effect, msgSvc, operation, continueStreamOnFailure, {} as R);

                return iif(() => null == isValid || isValid(),
                    saveFn$,
                    empty());
            }));

export const baseSaveHandlerImpl: BaseSaveHandler = <T, R>(
    throttleStrategy: <O extends ObservableInput<R>>(project: (value: T, index: number) => O) => OperatorFunction<T, ObservedValueOf<O>>,
    msgSvc: DsMsgService,
    beforeCall: (input: T) => void,
        apiCall: (input: T) => Observable<R>,
        operation: string,
        onSuccess: (input: R) => void,
        continueStreamOnFailure: boolean,
        isValid?: () => boolean) => (source: Observable<T>) => source.pipe(throttleStrategy((input) => {

    const effect = of(input).pipe(
        tap(x => beforeCall(x)),
        concatMap((x) => apiCall(x)),
        tap(x => onSuccess(x)));

    const saveFn$ = httpError(effect, msgSvc, operation, continueStreamOnFailure, {} as R);

    return iif(() => null == isValid || isValid(),
        saveFn$,
        empty());

}));

export function httpError<T>(input: Observable<T>, msgSvc: DsMsgService, operation = 'operation', continueOnFailure: boolean, result?: T) {
    return input.pipe(catchError(error => {
        const errorList = new Maybe(error).map(x => x.error).map(x => x.errors).map( x => x.length ? x[0].msg : null);
        const message = new Maybe(error).map(x => x.message);

        let errorMsg = errorList.valueOr(message.value());

    log(error, `Sorry, this operation failed: ${operation.toLowerCase()}. ${errorMsg}`);

    // TODO: better job of transforming error for user consumption
    msgSvc.setTemporaryMessage(`${operation} failed: ${errorMsg}`, MessageTypes.error, 6000);

    // let app continue by return empty result
    return continueOnFailure ? of(result as T) : throwError(error);
    }));
    
}

export const applyMsgSvcToErrorHandler: (svc: DsMsgService) => AttachErrorHandler = (msgSvc: DsMsgService) => {
    return (<T>(
        apiCall: Observable<T>,
        operation: string,
        continueStreamOnFailure: boolean) => httpError(apiCall, msgSvc, operation, continueStreamOnFailure, {} as T));
};

export const applyMsgSvcToSaveHandler: (svc: DsMsgService) => PASaveHandler = (msgSvc: DsMsgService) => {
    // return function as expression so that angular's aot compiler doesn't complain about referencing a function that is not exported
    return (<T, R>(
        SaveFn: (input: T) => Observable<R>,
        operation: string,
        continueStreamOnFailure: boolean) => SaveHandlerImpl(
            msgSvc,
            SaveFn,
            operation,
            continueStreamOnFailure
        ));
};

/**
 * When passed into the @Inject decorator, an implementation of `PASaveHandler` is injected
 */
export const PASaveHandlerToken = new InjectionToken('Used to inject a partially applied save handler function.');

export const createSaveHandlerFnProvider: Provider = {
    provide: PASaveHandlerToken,
    useFactory: applyMsgSvcToSaveHandler,
    deps: [DsMsgService]
};

/**
 * When passed into the @Inject decorator, an implementation of `AttachErrorHandler` is injected
 */
export const AttachErrorHandlerFn = new InjectionToken('Used to inject a partially applied Error Handler function.');

export const AttachErrorHandlerFnProvider: Provider = {
    provide: AttachErrorHandlerFn,
    useFactory: applyMsgSvcToErrorHandler,
    deps: [DsMsgService]
};

function log(error: any, message: string): any {
    // this is where we would wire up any logging we will do when we throw JS errors.
    if (error) console.dir(error);

    console.log(message); // for now, we are simply printing errors to the console.
}

/**
 * @description Attaches a `PASaveHandler` to the provided element reference. @see PASaveHandler
 * @param element The element on the page we want to react to.
 * @param setSubmitted A callback function to set a `submitted` property on the containing 
 * component/service.  This callback function is called whenever the provided `element` emits.
 * @param cancel$ Stop reacting to events when this emits.
 */
type AttachSaveHandler = <T>(
    element: ElementRef,
    setSubmitted: () => void,
    isValid: () => boolean,
    SaveFn: () => Observable<T>,
    cancel$: Observable<any>,
    msgSvc: DsMsgService,
    operation: string,
    continueStreamOnFailure?: boolean) => Subscription;

/**
 * @description Attaches a `PASaveHandler` to the provided element reference.  Already has the `DsMsgService` 
 * injected. To use this, inject `AttachSaveHandlerImpl` into the constructor of your component. @see PASaveHandler @see AttachSaveHandler
 */
export type InjectedAttachSaveHandler = <T>(element: ElementRef,
    setSubmitted: () => void,
    isValid: () => boolean,
    SaveFn: () => Observable<T>,
    cancel$: Observable<any>,
    operation: string,
    continueStreamOnFailure?: boolean) => Subscription;

    /**
     * @description A generic save handler that already has the `DsMsgService` injected.  Success and error 
     * messages are displayed at appropriate times.  To use this, inject `PASaveHandlerToken` into the 
     * constructor of your component/service.
     * 
     * @param SaveFn The http call to make to the server that will save the data to the db.
     * @param operation A description of the operation it is trying to perform.  This will be included in the 
     * success and error messages to give the user some helpful feedback. (ex: in the `review-policy.service.ts` the 
     * save handler is tied to a function that copies a `ReviewTemplate`.  The value passed for that operation is 'Copy Review'.  
     * On success a message is displayed saying `Copy Review completed successfully`.  On failure a message is diplayed saying 
     * `Copy Review failed: ... error message details ...`)
     * @param continueStreamOnFailure When true, the stream will continue to react to events even on failure.  
     * By default, the stream stops when an unhandled error is thrown.
     */
export type PASaveHandler = <T, R>(
    SaveFn: (input: T) => Observable<R>,
    operation: string,
    continueStreamOnFailure: boolean
) => OperatorFunction<T, R>;

/**
 * @description Wraps an api call with a default error handler.
 * @param apiCall The api call to wrap.
 * @param operation A description of the operation it is trying to perform.  This will be included in the
 * success and error messages to give the user some helpful feedback. (ex: in the `review-policy.service.ts` the
 * api call is tied to a function that copies a `ReviewTemplate`.  The value passed for that operation is 'Copy Review'.
 * On success a message is displayed saying `Copy Review completed successfully`.  On failure a message is diplayed saying
 * `Copy Review failed: ... error message details ...`)
 * @param continueStreamOnFailure When true, the stream will continue to react to events even on failure.
 *   By default, the stream stops when an unhandled error is thrown.
 */
export type AttachErrorHandler = <T>(
    apiCall: Observable<T>,
    operation: string,
    continueStreamOnFailure: boolean) => Observable<T>;

type BaseSaveHandler = <T, R>(
    throttleStrategy: <O extends ObservableInput<R>>(project: (value: T, index: number) => O) => OperatorFunction<T, ObservedValueOf<O>>,
    msgSvc: DsMsgService,
    beforeCall: (input: T) => void,
    apiCall: (input: T) => Observable<R>,
    operation: string,
    onSuccess: (input: R) => void,
    continueStreamOnFailure: boolean,
    isValid?: () => boolean) => OperatorFunction<T, R>;

type SaveHandler = <T, R>(
    msgSvc: DsMsgService,
    apiCall: (input: T) => Observable<R>,
    operation: string,
    continueStreamOnFailure: boolean,
    isValid?: () => boolean) => OperatorFunction<T, R>;

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
    controlAndDependency: { invalidControl: AbstractControl, dependency: AbstractControl }[],
    ref: ChangeDetectorRef,
    cancel$: Observable<any>
): Subscription {
    const updatedControls: Observable<any>[] = [];
    controls.forEach(control => {
        updatedControls.push(
            control.statusChanges.pipe(
                scan<string, { status: string, changed: boolean }>((acc, curr) => {
                    acc.changed = (curr || '').localeCompare(acc.status) !== 0;
                    acc.status = curr;
                    return acc;

                }, { status: 'VALID', changed: false }),
                map(x => x.changed),
                filter(x => x === true)
            ));
    });

    controlAndDependency.forEach(control => {
        updatedControls.push(
            control.dependency.valueChanges.pipe(
                withLatestFrom(of(control.invalidControl)),
                filter(x => x[1].invalid),
            )
        );
    });  

    return merge(...updatedControls).pipe(
        tap(() => ref.markForCheck()),
        takeUntil(cancel$))
        .subscribe();
}
