import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { ClientAccrualConstants } from 'apps/ds-company/src/app/models/leave-management/client-accrual-constants';
import { BehaviorSubject, combineLatest, EMPTY, Subject } from 'rxjs';
import { distinctUntilChanged, map, startWith, switchMap, take, takeUntil } from 'rxjs/operators';
import { ClientAccrualsService } from '../../../../../client-management/services/client-accruals.service';
import { ClientAccrualsStoreService } from '../../../../services/client-accruals-store.service';

@Component({
    selector: 'ds-client-accruals-footer-card',
    templateUrl: './client-accruals-footer-card.component.html',
    styleUrls: ['./client-accruals-footer-card.component.scss'],
})
export class ClientAccrualsFooterCardComponent implements OnInit, OnDestroy {

  private destroy$ = new Subject();

  private _cancelOrDelete$ = new BehaviorSubject<string>('Cancel');
  cancelOrDelete$ = this._cancelOrDelete$.asObservable();

  isDisableCancelOrDeleteButton$ = combineLatest(
      this.cancelOrDelete$,
      this.store.selectedClientAccrualIdForm.valueChanges
        .pipe(startWith(ClientAccrualConstants.NEW_ENTITY_ID))
    ).pipe(
      map(([cancelOrDelete, selectedClientAccrualId]: [string, string|number]) => {
        const isDelete = (cancelOrDelete.toLowerCase() === 'delete');
        const isNewAccrual = (
          selectedClientAccrualId <= ClientAccrualConstants.NEW_ENTITY_ID
        );
        return (isDelete && isNewAccrual);
      })
    );

  onCancel = this.clientAccrualsService.onCancel;
  onDeleteClientAccrual$ = this.clientAccrualsService.onDeleteClientAccrual$;
  onSubmit = () => this.clientAccrualsService.onSubmit(false, false, true);

  onCancelOrDeleteFn = () => {
    // Trigger different actions based on whether this is the delete or cancel button mode.
    if (this._cancelOrDelete$.value.toLowerCase() === 'delete') {
      const options = {
        title: "Are you sure you want to delete this Accrual Policy?",
        message: "",
        confirm: "Delete",
      };
      this.confirmDialog.open(options);
      this.confirmDialog.confirmed()
        .pipe(
          switchMap((confirmed) => {
            if (confirmed) {
              // Exec delete function.
              return this.onDeleteClientAccrual$();
            } else {
              return EMPTY;
            }
          }),
        )
        .subscribe(_ => {});
    } else {
      // Exec cancel function.
      this.onCancel();
    }
  }

  constructor(
      private clientAccrualsService: ClientAccrualsService,
      private store: ClientAccrualsStoreService,
      private confirmDialog: ConfirmDialogService,
  ) { }

  ngOnInit() {
    this.store.currentTabName$.pipe(
      takeUntil(this.destroy$),
    ).subscribe(tabName => {
      switch (tabName) {
        case 'setup':
          this._cancelOrDelete$.next('Delete');
          break;
        case 'schedules':
          this._cancelOrDelete$.next('Cancel');
          break;
        default:
          this._cancelOrDelete$.next('Cancel');
      }
    });
  }

  ngOnDestroy() {
    this.destroy$.next();
  }

}
