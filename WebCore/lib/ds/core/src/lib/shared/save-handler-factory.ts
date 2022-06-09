import { SaveHandlerImpl, SaveHandlerImplNoSuccessMsg, AttachSaveHandlerImpl, AutoSaveThrottleStrategy } from './save-handler-strategy';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { Observable, OperatorFunction } from 'rxjs';
import { InjectedAttachSaveHandler } from './save-handler-injection';
import { ElementRef } from '@angular/core';

export const applyMsgSvcToSaveHandler: (svc: DsMsgService, msgToDisplayWhenInFlight?: string)
=> PASaveHandler = (msgSvc: DsMsgService, msgToDisplayWhenInFlight?: string) => {
    // return function as expression so that angular's aot compiler doesn't complain about referencing a function that is not exported
    return (<T, R>(
        SaveFn: (input: T) => Observable<R>,
        operation: string,
        continueStreamOnFailure: boolean
        ) => SaveHandlerImpl(
            msgSvc,
            SaveFn,
            operation,
            continueStreamOnFailure,
            null,
            msgToDisplayWhenInFlight
        ));
};
/**
 * Returns an implementation of `SaveHandler` that displays no success message when the request completes
 * @param msgSvc The message service we need to inject into the save handler
 */
export const applyMsgSvcToSaveHandlerNoSuccessMsg: (svc: DsMsgService, msgToDisplayWhenInFlight?: string)
=> PASaveHandler = (msgSvc: DsMsgService, msgToDisplayWhenInFlight?: string) => {
    // return function as expression so that angular's aot compiler doesn't complain about referencing a function that is not exported
    return (<T, R>(
        SaveFn: (input: T) => Observable<R>,
        operation: string,
        continueStreamOnFailure: boolean) => SaveHandlerImplNoSuccessMsg(
            msgSvc,
            SaveFn,
            operation,
            continueStreamOnFailure,
            null,
            msgToDisplayWhenInFlight
        ));
};

export const attachSaveHandlerFactory: (svc: DsMsgService) => InjectedAttachSaveHandler =
 (msgSvc: DsMsgService) => {
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

export const AutoSaveThrottleStrategyFactory: (svc: DsMsgService, areEqual: (newVal: any, oldVal: any)
=> boolean, msgToDisplayWhenInFlight?: string) => PASaveHandler = 
(msgSvc: DsMsgService, areEqual: (newVal: any, oldVal: any) => boolean, msgToDisplayWhenInFlight?: string) => {
    // return function as expression so that angular's aot compiler doesn't complain about referencing a function that is not exported
    return (<T, R>(
        SaveFn: (input: T) => Observable<R>,
        operation: string,
        continueStreamOnFailure: boolean) => AutoSaveThrottleStrategy(areEqual)(
            msgSvc,
            SaveFn,
            operation,
            continueStreamOnFailure,
            null,
            msgToDisplayWhenInFlight
        ));
};


    /**
     * @description A generic save handler that already has the `DsMsgService` injected.  Success and error 
     * messages are displayed at appropriate times.  To use this, inject `PASaveHandlerToken` into the constructor of your
     * component/service.
     * 
     * @param SaveFn The http call to make to the server that will save the data to the db.
     * @param operation A description of the operation it is trying to perform.  This will be included in the 
     * success and error messages to give the user some helpful feedback. (ex: in the `review-policy.service.ts` the 
     * save handler is tied to a function that copies a `ReviewTemplate`.  The value passed for that operation is 'Copy Review'.  
     * On success a message is displayed saying `Copy Review completed successfully`.  On failure a message is diplayed saying 
     * `Copy Review failed: ... error message details ...`)
     * @param continueStreamOnFailure When true, the stream will continue to react to events even on failure.  By default, the
     * stream stops when an unhandled error is thrown.
     */
    export type PASaveHandler = <T, R>(
        SaveFn: (input: T) => Observable<R>,
        operation: string,
        continueStreamOnFailure: boolean,
        msgToDisplayWhenInFlight?: string
    ) => OperatorFunction<T, R>;
    
