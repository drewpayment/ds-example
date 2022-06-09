import { Observable, of, throwError, Subject, combineLatest, ReplaySubject } from 'rxjs';
import { PayrollRequestReportArgs, PAYROLL_REQUEST_REPORT_KEY, ClientSideFilters } from './payroll-request-report-args.model';
import { DsStorageService } from '@ds/core/storage/storage.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { PayrollRequestService } from '../payroll-request.service';
import { StoreBuilder, StoreAction } from '@ds/core/shared/store-builder';
import { applyMsgSvcToErrorHandler } from '@ds/core/shared/shared-api-fn';
import { applyMsgSvcToSaveHandlerNoSuccessMsg } from '@ds/core/shared/save-handler-factory';
import { Injectable } from '@angular/core';
import { PerformanceManagerService } from '../../performance-manager.service';
import { IReviewStatusSearchOptions, IReviewSearchOptions } from '../..';
import { filter, map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
  })
  export class PayrollRequestReportArgsStore {
  
    private getParams = new Subject();
  
    reportParams$: Observable<PayrollRequestReportArgs>;

    private showMeritClicked$: Subject<boolean> = new ReplaySubject(1);
    private showOneTimeClicked$: Subject<boolean> = new ReplaySubject(1);
    private showTable$: Subject<boolean> = new ReplaySubject(1);
    private selectedApprovalStatus$: Subject<number[]> = new ReplaySubject(1);
  
    constructor(
      storageSvc:DsStorageService,
      msgSvc: DsMsgService,
      managerSvc: PerformanceManagerService) {
        const builder = new StoreBuilder<PayrollRequestReportArgs>();
  
        const attachErrorHandler = applyMsgSvcToErrorHandler(msgSvc);
  
        builder.setDataSource(of(storageSvc.get(PAYROLL_REQUEST_REPORT_KEY).data), attachErrorHandler, 'Get payroll request report data');
  
        const options$ = this.createSearchOptionsStream(managerSvc.activeReviewSearchOptions$)
        const clientSideFilters$ = this.createClientSideFiltersStream(this.showMeritClicked$, this.showOneTimeClicked$, this.showTable$, this.selectedApprovalStatus$)
  
        this.reportParams$ = builder
        .addAction(this.createSetReportParamsAction(builder, storageSvc, msgSvc, options$, clientSideFilters$))
        .Build();
  
  
       }
       
    private createSetReportParamsAction(
      builder: StoreBuilder<PayrollRequestReportArgs>,
      storageSvc: DsStorageService,
      msgSvc: DsMsgService,
      options$: Observable<IReviewStatusSearchOptions>,
      clientSideFilters$: Observable<ClientSideFilters>): StoreAction<PayrollRequestReportArgs, PayrollRequestReportArgs, PayrollRequestReportArgs> {
  
      const action = builder.scaffoldAction<PayrollRequestReportArgs, PayrollRequestReportArgs>();
      action.setupFn = builder.nullSetupFn();
      action.effect = (x, y) => {
        const data = y as PayrollRequestReportArgs;
        var result = storageSvc.set(PAYROLL_REQUEST_REPORT_KEY, data);
        if (result.hasError) {
          return throwError('Failed to set report parameters on localStorage');
        }
        return of(data);
      };
      action.operation = 'Set report parameters';
      action.updateState = (result, oldState) => result;
      action.normalizeSaveHandler = applyMsgSvcToSaveHandlerNoSuccessMsg(msgSvc);

      action.dispatcher$ = combineLatest(options$, clientSideFilters$).pipe(map(x => {
        const result: PayrollRequestReportArgs = {
          searchOptions: x[0],
          clientSideFilters: x[1]
        };

        return result;
      }));

      return action;
    }

    private createSearchOptionsStream(options$: Observable<IReviewSearchOptions>): Observable<IReviewStatusSearchOptions>{
      return options$.pipe(
        filter(val => val != null),// the storage service throws an error if the value for an item in localStorage is set to null
        map(input => {
          (input as IReviewStatusSearchOptions).includeScores = true;
          return input;
        }))
    }

    private createClientSideFiltersStream(showMeritClicked$: Observable<boolean>,
      showOneTimeClicked$: Observable<boolean>,
      showTable$: Observable<boolean>,
      selectedApprovalStatus$: Observable<number[]>): Observable<ClientSideFilters> {
        return combineLatest(showMeritClicked$, showOneTimeClicked$, showTable$, selectedApprovalStatus$).pipe(map(items => {
          const filters: ClientSideFilters = {
            showMerit: items[0],
            showOneTime: items[1],
            showTable: items[2],
            selectedApprovalStatus: items[3]
          };

          return filters;
        }))
      }

  showMeritClicked(enabled: boolean): void {
    this.showMeritClicked$.next(enabled);
  }

  showOneTimeClicked(enabled: boolean): void {
    this.showOneTimeClicked$.next(enabled);
  }

  showTable(isTableVisible: boolean): void {
    this.showTable$.next(isTableVisible);
  }

  setSelectedApprovalStatus(selectedStatuses: number[]): void {
    this.selectedApprovalStatus$.next(selectedStatuses);
  }

    public emitGetParams(){
      this.getParams.next();
    }
  }