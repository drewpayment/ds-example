import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
} from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { ConfirmDialogService } from "@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service";
import { EMPTY, Subject } from "rxjs";
import { switchMap, takeUntil } from "rxjs/operators";
import { ClientAccrualsStoreService } from "../../../../../client-management/services/client-accruals-store.service";
import { ClientAccrualsService } from "../../../../../client-management/services/client-accruals.service";

@Component({
  selector: "ds-client-accruals-time-off-card",
  templateUrl: "./client-accruals-time-off-card.component.html",
  styleUrls: ["./client-accruals-time-off-card.component.scss"],
})
export class ClientAccrualsTimeOffCardComponent implements OnInit, OnDestroy {
  form: FormGroup = this.store.form;

  dropdownLists$ = this._clientAccrualsSvc.dropdownLists$;

  private readonly _destroy$ = new Subject();

  constructor(
    private _cd: ChangeDetectorRef,
    private _clientAccrualsSvc: ClientAccrualsService,
    private store: ClientAccrualsStoreService,
    private _confirmDialog: ConfirmDialogService,
  ) {}

  ngOnInit() {
    this.form.get('accrual.isLeaveManagment').valueChanges
      .subscribe(() => {
        const isOn: boolean = this.form.value.accrual.isLeaveManagment;

        if (!isOn) {
          this.form.get('accrual.hoursInDay').enable();
          this.form.get('accrual.allowAllDays').enable();
          this.form.get('accrual.requestMinimum').enable();
          this.form.get('accrual.requestMaximum').enable();
          this.form.get('accrual.requestIncrement').enable();
          this.form.get('accrual.leaveManagmentAdministrator').enable();
          this.form.get('accrual.isSupEmailRequest').enable();
          this.form.get('accrual.isRealTimeAccruals').enable();
          this.form.get('accrual.projectAmount').enable();
          this.form.get('accrual.projectAmountType').enable();
          this.form.get('accrual.isLeaveManagementUseBalanceOption').enable();
        } else {
          this.form.get('accrual.hoursInDay').disable();
          this.form.get('accrual.allowAllDays').disable();
          this.form.get('accrual.requestMinimum').disable();
          this.form.get('accrual.requestMaximum').disable();
          this.form.get('accrual.requestIncrement').disable();
          this.form.get('accrual.leaveManagmentAdministrator').disable();
          this.form.get('accrual.isSupEmailRequest').disable();
          this.form.get('accrual.isRealTimeAccruals').disable();
          this.form.get('accrual.projectAmount').disable();
          this.form.get('accrual.projectAmountType').disable();
          this.form.get('accrual.isLeaveManagementUseBalanceOption').disable();

          this.form.patchValue({
            accrual: {
              hoursInDay: '',
              allowAllDays: 1,
              requestMinimum: '',
              requestMaximum: '',
              requestIncrement: '',
              leaveManagmentAdministrator: '',
              isSupEmailRequest: '',
              isRealTimeAccruals: false,
              projectAmount: '',
              projectAmountType: 1,
              isLeaveManagementUseBalanceOption: '',
            }
          })
        }
      });
  }

  ngOnDestroy() {
    this._destroy$.next();
  }

  deleteClientAccrualLeaveManagementPendingAwards = () => {
    const options = {
      title: "Are you sure you want to delete pending awards?",
      message: "",
      confirm: "Delete",
    };
    this._confirmDialog.open(options);
    this._confirmDialog.confirmed()
    .pipe(
      switchMap((confirmed) => {
        if (confirmed) {
          return this._clientAccrualsSvc
           .deleteClientAccrualLeaveManagementPendingAwards();
        } else {
          return EMPTY;
        }
      })
    )
    .subscribe(_ => {});

  }
}
