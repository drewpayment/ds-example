import { InjectionToken, Provider, ElementRef } from '@angular/core';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { applyMsgSvcToSaveHandler, attachSaveHandlerFactory } from './save-handler-factory';
import { Observable, Subscription } from 'rxjs';
import { applyMsgSvcToErrorHandler } from './shared-api-fn';


/**
 * When passed into the @Inject decorator, an implementation of `PASaveHandler` is injected
 */
export const PASaveHandlerToken = new InjectionToken('Used to inject a partially applied save handler function.');

export const createSaveHandlerFnProvider: Provider = {
    provide: PASaveHandlerToken,
    useFactory: applyMsgSvcToSaveHandler,
    deps: [DsMsgService]
};

export const AttachSaveHandlerToken =
new InjectionToken('Used to inject a partially applied function that can attach a save handler to a page element.');

export const attachSaveHandlerFnProvider: Provider = {
    provide: AttachSaveHandlerToken,
    useFactory: attachSaveHandlerFactory,
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
