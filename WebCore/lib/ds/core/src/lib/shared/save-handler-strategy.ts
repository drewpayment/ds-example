import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { Observable, OperatorFunction, fromEvent, Subscription, of, empty, ObservedValueOf, ObservableInput, from, Subject, Observer} from 'rxjs';
import { takeUntil, tap, exhaustMap, switchMap, map, expand, last, withLatestFrom, startWith } from 'rxjs/operators';
import { baseSaveHandlerImpl } from './shared-api-fn';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { ElementRef } from '@angular/core';
import { SetupFnInput, IsSetupFnInput } from './store-builder';

export const SaveHandlerImpl: SaveHandler =
    <T, R>(msgSvc: DsMsgService,
        apiCall: (input: T) => Observable<R>,
        operation: string,
        continueStreamOnFailure: boolean,
        isValid?: () => boolean,
        msgToDisplayWhenInFlight?: string) =>
        baseSaveHandlerImpl<T, R>(
            exhaustMap,
            msgSvc,
            () => msgSvc.setMessage(msgToDisplayWhenInFlight ? msgToDisplayWhenInFlight : 'Saving....', MessageTypes.warning),
            apiCall,
            operation,
            () => msgSvc.setTemporarySuccessMessage(operation + ' completed successfully.'),
            continueStreamOnFailure,
            isValid
        );

export const SaveHandlerImplNoSuccessMsg: SaveHandler =
    <T, R>(msgSvc: DsMsgService,
        apiCall: (input: T) => Observable<R>,
        operation: string,
        continueStreamOnFailure: boolean,
        isValid?: () => boolean,
        msgToDisplayWhenInFlight?: string) =>
        baseSaveHandlerImpl<T, R>(
            switchMap,
            msgSvc,
            () => msgSvc.setMessage(msgToDisplayWhenInFlight 
                ? msgToDisplayWhenInFlight 
                : 'Loading...', 
                MessageTypes.warning
                ),
            apiCall,
            operation,
            () => msgSvc.loading(false),
            continueStreamOnFailure,
            isValid
        );



export const AttachSaveHandlerImpl: AttachSaveHandler = <T>(
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
        SaveHandlerImpl<any, T>(
            msgSvc,
            SaveFn,
            operation,
            continueOnFailure,
            isValid),
        takeUntil(cancel$)).subscribe();
};

/**
 * Sends requests one at a time.  When the current request finishes and the input has changed, call the request again with the new input.
 * 
 * With this strategy, the user cannot queue up a bunch of requests when the server is slow.  We also don't have to worry about handling 
 * cancelled requests.  It also ensures that the latest set of inputs is always saved to the server (unless the user leaves the 
 * page before the latest changes can be sent or an exception is thrown in the stream).
 * 
 * @param isLoadingHook An observer that will react when the form has started and finished saving a dirty form.
 */
export const AutoSaveThrottleStrategy: (areInputsEqual: (newVal: any, oldVal: any) => boolean, isLoadingHook?: Observer<LoadingResult>) => SaveHandler = 
(areEqual, isLoadingHook) => {

    var oldAreEqual = areEqual;
    areEqual = (itemA: any, itemB: any) => {
        if(itemA == null && itemB != null){
            return false;
        }

        if(itemB == null && itemA != null){
            return false;
        }
        return oldAreEqual(itemA, itemB);
    }

    var beforeCall: (msgSvc: DsMsgService, msgToDisplayWhenInFlight: string) => <T>(input: T) => void;
    var afterCall: (msgSvc: DsMsgService) => <R>(input: R) => void;
    if(isLoadingHook) {
        beforeCall = (msgSvc: DsMsgService, msgToDisplayWhenInFlight: string) => (input) => isLoadingHook.next({ isLoading: true, input: input });
        afterCall = (msgSvc: DsMsgService) => (input) => isLoadingHook.next({ isLoading: false, input: input });
    } else {
        beforeCall = (msgSvc: DsMsgService, msgToDisplayWhenInFlight: string) => () => msgSvc.setMessage(msgToDisplayWhenInFlight ? msgToDisplayWhenInFlight : 'Loading....', MessageTypes.warning);
        afterCall = (msgSvc: DsMsgService) => (input) => msgSvc.loading(false);
    }

    return <T, R>(msgSvc: DsMsgService,
    apiCall: (input: T) => Observable<R>,
    operation: string,
    continueStreamOnFailure: boolean,
    isValid?: () => boolean,
    msgToDisplayWhenInFlight?: string) =>
    baseSaveHandlerImpl<T, R>(
        DoLongRunningTask(areEqual),
        msgSvc,
        beforeCall(msgSvc, msgToDisplayWhenInFlight),
        apiCall,
        operation,
        afterCall(msgSvc),
        continueStreamOnFailure,
        isValid
    );
}

/**
 * Make sure we include getters during serialization.  
 * A lot of objects in typescript use getters which 
 * are methods.  JSON.stringify skips methods.
 */
function convertGettersToProperties() {
    const proto = Object.getPrototypeOf(this);
    const jsonObj: any = Object.assign({}, this);
  
    Object.entries(Object.getOwnPropertyDescriptors(proto))
      .filter(([key, descriptor]) => typeof descriptor.get === 'function')
      .map(([key, descriptor]) => {
        if (descriptor && key[0] !== '_') {
          try {
            const val = (this as any)[key];
            jsonObj[key] = val;
          } catch (error) {
            console.error(`Error calling getter ${key}`, error);
          }
        }
      });
  
    return jsonObj;
  }

  interface CopyService {
      getItemToCopy(): any;
      getResult(copiedItem: any): any;
  }

  class StoreStreamItemStrategy<M,U> implements CopyService {

    constructor(private item: SetupFnInput<M,U>) {}

      getItemToCopy() {
          return this.item.idOrNewValue
      }
      getResult(copiedValue: M) {
          this.item.idOrNewValue = copiedValue;
          return this.item;
      }

  }

  class UnknownItemStrategy implements CopyService {

    constructor(private item: any) { }

      getItemToCopy() {
          return this.item;
      }
      getResult(copiedItem: any) {
          return copiedItem;
      }

  }

  function getCopyService<U>(input: U): CopyService {
    if(IsSetupFnInput(input)) {
        return new StoreStreamItemStrategy<U, U>(input);
    } else {
        return new UnknownItemStrategy(input);
    }
  }

function DoLongRunningTask<U, O extends ObservableInput<any>>(areInputsEqual: (newVal: U, oldVal: U) => boolean): 
(project: (value: U, index: number) => O) => OperatorFunction<U, ObservedValueOf<O>> {
return (project) => {
    return <OperatorFunction<U, ObservedValueOf<O>>>((source: Observable<U>) => {
        var oldSource = source;
        source = oldSource.pipe(map(x => {
            const isObject = typeof x === 'object' && x != null;
            if(isObject){
                var itemToCopy = null;

                const service = getCopyService(x);

                itemToCopy = service.getItemToCopy();

                itemToCopy.convertGettersToProperties = convertGettersToProperties;
                const copiedValue = JSON.parse(JSON.stringify(itemToCopy.convertGettersToProperties()));
                itemToCopy.convertGettersToProperties = undefined;

                const result = service.getResult(copiedValue);

                return result;
            }
            return x;
        }))
        return source.pipe(
        exhaustMap(x => of(x).pipe(
            map(startingData => ({ prevInputVal: null, currentInputVal: startingData, result: null, index: -1 })),
            expand((next) => {
                return areInputsEqual(next.currentInputVal, next.prevInputVal) ?
                empty() : executeLongRunningTask<U, O>(next.currentInputVal, source.pipe(startWith(next.currentInputVal)),
                project, next.index++);
            }),
            last(),
            map(x => x.result)
        )))
    }
    );
};
    
}


function executeLongRunningTask<U, O extends ObservableInput<any>>(currentInputVal: U, inputStream: Observable<U>,
    longRunningTask: (value: U, index: number) => O, index: number): Observable<LongRunningTaskRecursionModel<U, O>> {
    return from(longRunningTask(currentInputVal, index)).pipe(
        withLatestFrom(inputStream),
        map<[O, U], LongRunningTaskRecursionModel<U, O>>(result => ({ prevInputVal: currentInputVal,
            result: result[0], currentInputVal: result[1], index: index })));
}


interface LongRunningTaskRecursionModel<U, T> {
    result: T;
    prevInputVal: U;
    currentInputVal: U;
    index: number;
}

export interface LoadingResult {
    isLoading: boolean;
    input: any
}

export interface TypedLoadingResult<T> extends LoadingResult {
    input: T
}




export type SaveHandler = <T, R>(
    msgSvc: DsMsgService,
    apiCall: (input: T) => Observable<R>,
    operation: string,
    continueStreamOnFailure: boolean,
    isValid?: () => boolean,
    msgToDisplayWhenInFlight?: string) => OperatorFunction<T, R>;

type PredefinedSaveHandler<T, R> = (
    msgSvc: DsMsgService,
    apiCall: (input: T) => Observable<R>,
    operation: string,
    continueStreamOnFailure: boolean,
    isValid?: () => boolean) => OperatorFunction<T, R>;


    
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
