import { Injectable } from '@angular/core';
import { DsPopupService } from '@ajs/ui/popup/ds-popup.service';
import { StoreBuilder, StoreAction } from '@ds/core/shared/store-builder';
import { applyMsgSvcToErrorHandler } from '@ds/core/shared/shared-api-fn';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { of, Subject, Observable, from, race } from 'rxjs';
import { filter, map, tap } from 'rxjs/operators';
import { applyMsgSvcToSaveHandlerNoSuccessMsg } from '@ds/core/shared/save-handler-factory';
import { PopupService } from '@ds/core/popup/popup.service';

@Injectable({
  providedIn: 'root',
})
export class PayrollRequestReportStoreService {
  private openPopupDispatcher: Subject<null> = new Subject();
  private closePopupDispatcher: Subject<null> = new Subject();

  isPayrollRequestReportOpen$: Observable<boolean>;

  constructor(
    popup: PopupService,
    popupSvc: DsPopupService,
    msgSvc: DsMsgService
  ) {
    const builder = new StoreBuilder<boolean>();

    const attachErrorHandler = applyMsgSvcToErrorHandler(msgSvc);
    builder.setDataSource(
      of(false),
      attachErrorHandler,
      'Open Payroll Request Report'
    );

    const openPopup = this.createOpenPopupAction(
      builder,
      this.openPopupDispatcher,
      this.closePopupDispatcher,
      popup,
      msgSvc
    );
    const closePopup = this.createClosePopupAction(
      builder,
      this.closePopupDispatcher,
      msgSvc
    );

    this.isPayrollRequestReportOpen$ = builder
      .addAction(openPopup)
      .addAction(closePopup)
      .Build();
  }

  private createClosePopupAction(
    builder: StoreBuilder<boolean>,
    dispatcher: Subject<null>,
    msgSvc: DsMsgService
  ): StoreAction<null, boolean, null> {
    const action = builder.scaffoldAction<null, null>();
    action.dispatcher$ = dispatcher;
    action.effect = (a, b) => of(null);
    action.operation = 'Close Payroll Request Report';
    action.setupFn = builder.nullSetupFn();
    action.updateState = (result, oldVal) => false;
    action.normalizeSaveHandler = applyMsgSvcToSaveHandlerNoSuccessMsg(msgSvc);
    return action;
  }

  private createOpenPopupAction(
    builder: StoreBuilder<boolean>,
    openPopupDispatcher: Subject<null>,
    closePopupDispatcher: Subject<null>,
    popupSvc: PopupService,
    msgSvc: DsMsgService
  ): StoreAction<boolean, boolean, null> {
    const action = builder.scaffoldAction<boolean, null>();

    action.dispatcher$ = openPopupDispatcher;
    action.effect = (a, b) => {
      const p = popupSvc.open(
        'Popup.aspx#/performance/print-total-payout',
        'Payroll Request Report',
        { height: 640, width: 960 }
      );

      return race(
        from(p.loaded().pipe(map((val) => !!val))),
        from(
          p.closed().pipe(
            tap(() => closePopupDispatcher.next(null)),
            map(() => true)
          )
        )
      ).pipe(filter((v) => !!v));
    };
    action.normalizeSaveHandler = applyMsgSvcToSaveHandlerNoSuccessMsg(msgSvc);
    action.updateState = () => true;
    action.operation = 'Open Payroll Request Report';
    action.setupFn = builder.nullSetupFn();

    return action;
  }

  open(): void {
    this.openPopupDispatcher.next();
  }
}
