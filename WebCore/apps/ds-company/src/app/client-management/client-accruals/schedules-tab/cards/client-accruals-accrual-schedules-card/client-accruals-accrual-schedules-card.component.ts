import {
  Component,
  OnDestroy,
  OnInit,
} from "@angular/core";
import {
  FormGroup,
  FormArray,
} from "@angular/forms";
import { Subject } from "rxjs";
import { finalize, map, takeUntil, tap } from "rxjs/operators";
import { ClientAccrualsService } from "../../../../../client-management/services/client-accruals.service";
import { ConfirmDialogService } from "@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service";
import { MatTableDataSource } from "@angular/material/table";
import { ClientAccrualsStoreService } from "../../../../../client-management/services/client-accruals-store.service";
import { ClientAccrualSchedule, mapIsEditModeToClientAccrualSchedulesEditIconType, ServiceCarryOverFrequencyType, ServiceCarryOverUntilFrequencyType, ServiceCarryOverWhenFrequencyType, ServiceFrequencyType, ServiceRewardFrequencyType, ServiceStartEndFrequencyType } from '@ds/core/employee-services/models/client-accrual-schedule.model';
import { ServiceStartEndFrequency } from 'apps/ds-company/src/app/models/leave-management/service-start-end-frequency';
import { ServiceFrequency } from 'apps/ds-company/src/app/models/leave-management/service-frequency';
import { ServiceRewardFrequency } from 'apps/ds-company/src/app/models/leave-management/service-reward-frequency';
import { ServiceRenewFrequency } from 'apps/ds-company/src/app/models/leave-management/service-renew-frequency';
import { ServiceCarryOverFrequency } from 'apps/ds-company/src/app/models/leave-management/service-carry-over-frequency';
import { ServiceCarryOverWhenFrequency } from 'apps/ds-company/src/app/models/leave-management/service-carry-over-when-frequency';
import { ServiceCarryOverTillFrequency } from 'apps/ds-company/src/app/models/leave-management/service-carry-over-till-frequency';
import { LeaveManagementApiService } from '../../../../../client-management/services/leave-management-api.service';
import { ClientAccrualConstants } from "apps/ds-company/src/app/models/leave-management/client-accrual-constants";
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";


@Component({
  selector: "ds-client-accruals-accrual-schedules-card",
  templateUrl: "./client-accruals-accrual-schedules-card.component.html",
  styleUrls: ["./client-accruals-accrual-schedules-card.component.scss"],
})
export class ClientAccrualsAccrualSchedulesCardComponent
implements OnInit, OnDestroy {

  form: FormGroup = this.store.form;
  private lastSchedulesSnapshot: ClientAccrualSchedule[] = [];
  get schedulesForm(): FormArray {
    return this.form.get('accrual.clientAccrualSchedules') as FormArray;
  }
  status: string;
  isLoading = true;
  submitted = false;
  dropdownLists$ = this._clientAccrualsSvc.dropdownLists$;
  dataSource = new MatTableDataSource<ClientAccrualSchedule>(this.schedulesForm.value);
  serviceStartEndFrequencies: ServiceStartEndFrequency[] = [];
  serviceFrequencies: ServiceFrequency[];
  serviceRewardFrequencies: ServiceRewardFrequency[];
  serviceRenewFrequencies: ServiceRenewFrequency[];
  serviceCarryOverFrequencies: ServiceCarryOverFrequency[];
  serviceCarryOverWhenFrequencies: ServiceCarryOverWhenFrequency[];
  serviceCarryOverUntilFrequencies: ServiceCarryOverTillFrequency[];

  readonly displayedColumns: string[] = [
    "startService",
    "endService",
    "type",
    "reward",
    "maxHours",
    "renewType",
    "accrualLimit",
    "balanceLimit",
    "carryOver",
    "rateCarryOverMax",
    "carryOverOn",
    "until",
    "action",
  ];

  private _isEditMode = false;
  get isEditMode() { return this._isEditMode; }
  editIcon$ = this.store.isEditingAccrualSchedules$.pipe(
    map(mapIsEditModeToClientAccrualSchedulesEditIconType)
  );

  private _destroy$ = new Subject();

  constructor(
    private _clientAccrualsSvc: ClientAccrualsService,
    private _confirmDialog: ConfirmDialogService,
    private store: ClientAccrualsStoreService,
    private leaveManagementService: LeaveManagementApiService,
    private msg: NgxMessageService,
  ) {}

  ngOnInit() {
    this.status = "";

    this.store.isEditingAccrualSchedules$
      .pipe(
        tap(),
        takeUntil(this._destroy$)
      )
      .subscribe(isEditMode => this._isEditMode = isEditMode);

    this._clientAccrualsSvc.dropdownLists$
      .pipe(
        takeUntil(this._destroy$),
        map(data => {
          this.serviceStartEndFrequencies = data.serviceStartEndFrequencies;
          this.serviceFrequencies = data.serviceFrequencies;
          this.serviceRewardFrequencies = data.serviceRewardFrequencies;
          this.serviceRenewFrequencies = data.serviceRenewFrequencies;
          this.serviceCarryOverFrequencies = data.serviceCarryOverFrequencies;
          this.serviceCarryOverWhenFrequencies = data.serviceCarryOverWhenFrequencies;
          this.serviceCarryOverUntilFrequencies = data.serviceCarryOverTillFrequencies;
          this.isLoading = false;
          return this.serviceStartEndFrequencies;
        }),
      ).subscribe();

    this.schedulesForm.valueChanges
      .pipe(takeUntil(this._destroy$))
      .subscribe((values: ClientAccrualSchedule[]) => {
        if (values && values.length) {
          const newCount = values.length;
          const oldCount = this.lastSchedulesSnapshot.length;
          const newIdsKey = values.map(r => r.clientAccrualScheduleId).sort().join() + `${newCount}`;
          const oldIdsKey = this.lastSchedulesSnapshot.map(r => r.clientAccrualScheduleId).sort().join() + `${oldCount}`;

          if (newCount !== oldCount && newIdsKey !== oldIdsKey) {
            this.lastSchedulesSnapshot = values;
            this.dataSource.data = this.lastSchedulesSnapshot;
          }
        } else {
          this.lastSchedulesSnapshot = [];
          this.dataSource.data = this.lastSchedulesSnapshot;
        }
      });

  }

  ngOnDestroy() {
    this._destroy$.next();
  }

  addScheduleRow() {
    const newRow = this.store.createClientAccrualScheduleForm();
    this.schedulesForm.markAsTouched();
    this.schedulesForm.markAsDirty();
    this.schedulesForm.push(newRow);

    // if (!this.isEditMode) this.toggleEdit(this.isEditMode);
    this.store.updateAccrualSchedulesEditStatus(true);
  }

  copyScheduleRow(source: ClientAccrualSchedule, copyIndex: number) {
    // Apparently we can't actually trust that source param is up to date with the actual form state...
    // Probably caused by something to do with change detection?
    source = this.schedulesForm.value[copyIndex] as ClientAccrualSchedule;
    const index = copyIndex + 1;
    const newRow = this.store.createClientAccrualScheduleForm({
      ...source,
      clientAccrualScheduleId: ClientAccrualConstants.NEW_ENTITY_ID
    });
    this.schedulesForm.markAsTouched();
    this.schedulesForm.markAsDirty();
    this.schedulesForm.insert(index, newRow);
    this.store.updateAccrualSchedulesEditStatus(true);
  }

  deleteScheduleRow(index: number) {
    const options = {
      title: "Are you sure you want to delete this schedule?",
      message: "",
      confirm: "Delete",
    };
    this._confirmDialog.open(options);
    this._confirmDialog.confirmed().subscribe((confirmed) => {
      if (confirmed) {
        this.status = "Deleted";

        this.schedulesForm.markAsTouched();
        this.schedulesForm.markAsDirty();
        this.schedulesForm.removeAt(index);

        if (this.schedulesForm.value.length === 0) {
          // this.schedulesForm.clear();
          // this.toggleEdit(this.isEditMode);
          // this.store.updateAccrualSchedulesEditStatus(false, false);

          // FIXME: Not sure how to handle setting edit mode here,
          // with respect to the other schedules table edit mode...
        }
        // this.store.updateAccrualSchedulesEditStatus(true);
      } else {
        this.status = "Delete cancelled";
        return false;
      }
    });
  }

  // Toggling to readonly will cause whole clientAccrual to save via api,
  // and set this schedules grid to readonly on successful save.
  toggleEdit(wasEditMode: boolean) {
    // this._clientAccrualsSvc.toggleSchedulesEditMode(this.isEditMode, this.store.updateAccrualSchedulesEditStatus);

    // if (this.isEditMode) {
    if (wasEditMode) {
      // Is currently in edit mode, toggling to readonly.
      this._clientAccrualsSvc.submitIfHasPendingChanges(false, true, false)
        .pipe(
          // Handle EMPTY when no save occurred/was-needed.
          finalize(() => {
            if (this.store.form.valid) {
              this.store.updateAccrualSchedulesEditStatus(false);
            }
          })
        )
        .subscribe(accrual => {
          if (accrual != null) {
            this.store.updateAccrualSchedulesEditStatus(false);
          }
        });
    } else {
      this.store.updateAccrualSchedulesEditStatus(true);
    }
  }

  buildStartEndFrequencyLabel(amount: number, startEndFrequencyType: ServiceStartEndFrequencyType): string {
    if ((!amount || amount < 1) && !startEndFrequencyType) return '';

    let typeLabel = ServiceStartEndFrequencyType[startEndFrequencyType] || '';

    if (amount == 1) {
      typeLabel = typeLabel.slice(0, typeLabel.length - 1);
      return `${amount} ${typeLabel}`;
    }

    return `${amount || '0'} ${typeLabel}`;
  }

  buildRewardLabel(element: ClientAccrualSchedule): string {
    const label = ServiceRewardFrequencyType[element.serviceRewardFrequencyId] || '';
    return `${element.reward || ''} ${label}`;
  }

  buildRenewLabel(id: number): string {
    const freq = this.serviceRenewFrequencies.find(r => r.id == id);
    const label = freq != null ? freq.description : '';
    return label;
  }

  buildCarryOverLabel(element: ClientAccrualSchedule): string {
    const label = ServiceCarryOverFrequencyType[element.serviceCarryOverFrequencyId] || '';
    return `${element.carryOver || ''} ${label}`;
  }

  buildCarryOverWhenLabel(element: ClientAccrualSchedule): string {
    return ServiceCarryOverWhenFrequencyType[element.serviceCarryOverWhenFrequencyId] || '';
  }

  buildCarryOverUntilLabel(element: ClientAccrualSchedule): string {
    const label = ServiceCarryOverUntilFrequencyType[element.serviceCarryOverTillFrequencyId] || '';
    return `${element.serviceCarryOverTill || ''} ${label}`;
  }

  buildServiceFrequencyLabel(element: ClientAccrualSchedule): string {
    const label = ServiceFrequencyType[element.serviceFrequencyId] || '';
    return label;
  }

}
