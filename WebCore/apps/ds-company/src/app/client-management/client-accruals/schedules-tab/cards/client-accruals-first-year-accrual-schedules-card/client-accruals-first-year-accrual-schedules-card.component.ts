import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormArray, FormControl, ValidationErrors } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ClientAccrualFirstYearSchedule } from '@ds/core/employee-services/models/client-accrual-first-year-schedule.model';
import { mapIsEditModeToClientAccrualSchedulesEditIconType } from '@ds/core/employee-services/models/client-accrual-schedule.model';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { ClientAccrualConstants } from 'apps/ds-company/src/app/models/leave-management/client-accrual-constants';
import { ServiceReferencePointFrequency } from 'apps/ds-company/src/app/models/leave-management/service-reference-point-frequency';
import { ServiceStartEndFrequency } from 'apps/ds-company/src/app/models/leave-management/service-start-end-frequency';
import { Observable, Subject, } from 'rxjs';
import { debounceTime, finalize, tap } from 'rxjs/operators';
import { map, takeUntil } from 'rxjs/operators';
import { ClientAccrualsStoreService } from '../../../../../client-management/services/client-accruals-store.service';
import { ClientAccrualsService } from '../../../../../client-management/services/client-accruals.service';

@Component({
  selector: 'ds-client-accruals-first-year-accrual-schedules-card',
  templateUrl: './client-accruals-first-year-accrual-schedules-card.component.html',
  styleUrls: ['./client-accruals-first-year-accrual-schedules-card.component.scss'],
})
export class ClientAccrualsFirstYearAccrualSchedulesCardComponent
  implements OnInit, OnDestroy {

  form: FormGroup = this.store.form;
  get schedulesForm(): FormArray {
    return this.form.get('accrual.clientAccrualProratedSchedules') as FormArray;
  }
  get accrualForm(): FormGroup {
    return this.form.get('accrual') as FormGroup;
  }
  get proratedServiceReferencePointOverrideIdForm(): FormControl {
    return this.form.get('accrual.proratedServiceReferencePointOverrideId') as FormControl;
  }
  get proratedWhenToAwardTypeIdForm(): FormControl {
    return this.form.get('accrual.proratedWhenToAwardTypeId') as FormControl;
  }
  get proratedWhenToAwardTypeIdValue() {
    const form = this.proratedWhenToAwardTypeIdForm;
    return (form == null)
      ? ClientAccrualConstants.PRORATEDWHENTOAWARD
      : form.value;
  }
  get isAwardByDay() {
    return this.proratedWhenToAwardTypeIdValue == 1;
  }
  get isAwardByDate() {
    return !this.isAwardByDay;
  }

  private lastSchedulesSnapshot: ClientAccrualFirstYearSchedule[] = [];
  status: string;
  isLoading = true;
  submitted = false;
  dataSource = new MatTableDataSource<ClientAccrualFirstYearSchedule>(this.schedulesForm.value);
  serviceStartEndFrequencies: ServiceStartEndFrequency[] = [];
  serviceReferencePointFrequencies: ServiceReferencePointFrequency[];

  readonly displayedColumns: string[] = ['days', 'date', 'from', 'to', 'reward', 'action'];

  private _isEditMode = false;
  get isEditMode() { return this._isEditMode; }
  editIcon$ = this.store.isEditingFirstYearAccrualSchedules$.pipe(
    map(mapIsEditModeToClientAccrualSchedulesEditIconType)
  );

  private destroy$ = new Subject();

  private _opts = Object.freeze({ emitEvent: false });

  proratedTierDatesError$: Observable<boolean>;

  constructor(
    private _clientAccrualsSvc: ClientAccrualsService,
    private store: ClientAccrualsStoreService,
    private _confirmDialog: ConfirmDialogService,
  ) {}

  // Returns true if has error key for either of:
  // - proratedDoesNotCoverFullYear
  // - gapInCoverage
  // and key's value is truthy.
  // Otherwise, returns false.
  private _hasProratedTierGapError(errors: ValidationErrors): boolean {
    if (errors == null) {
      return false;
    }
    const eKeys = Object.keys(errors);
    const result = eKeys.some(k => {
      const proratedDoesNotCoverFullYear = (k === 'proratedDoesNotCoverFullYear' && errors[k]);
      const gapInCoverage = (k === 'gapInCoverage' && errors[k]);
      return (proratedDoesNotCoverFullYear || gapInCoverage);
    });
    return result;
  }

  ngOnInit() {
    this.status = '';

    // Only subscribed to via async pipe in template.
    this.proratedTierDatesError$ = this.schedulesForm.statusChanges.pipe(
      debounceTime(100),
      map(_ => {
        let result = false;

        const rowsForms = this.schedulesForm.controls as FormGroup[];
        if (rowsForms == null || rowsForms.length === 0) {
          return false;
        }

        // Search rows for schedule from/to tier gap errors.
        // Currently, we only validate and apply these errors when manually
        // triggering proratedNoGapsBetweenTiers_proratedRowsCompleteFullAnnualCycle
        // whenever a save is triggered.
        rowsForms.forEach(row => {
            const fromToErrors = {
              from: row.get('scheduleFrom').errors,
              to: row.get('scheduleTo').errors,
            };
            result = (result
              || this._hasProratedTierGapError(fromToErrors.from)
              || this._hasProratedTierGapError(fromToErrors.to)
            );
            if (result) return;
        });

        return result;
      })
    );

    this.store.isEditingFirstYearAccrualSchedules$.pipe(
      takeUntil(this.destroy$),
    ).subscribe(isEditMode => {
      if (isEditMode) {
        this.proratedServiceReferencePointOverrideIdForm.enable(this._opts);
        this.proratedWhenToAwardTypeIdForm.enable(this._opts);
      } else {
        this.proratedServiceReferencePointOverrideIdForm.disable(this._opts);
        this.proratedWhenToAwardTypeIdForm.disable(this._opts);
      }
    });

    this.store.isEditingFirstYearAccrualSchedules$
      .pipe(
        tap(),
        takeUntil(this.destroy$)
      )
      .subscribe(isEditMode => this._isEditMode = isEditMode);

    this._clientAccrualsSvc.dropdownLists$
      .pipe(
        takeUntil(this.destroy$),
        map(data => {
          this.serviceReferencePointFrequencies = data.serviceReferencePointFrequencies;
          this.isLoading = false;
          return this.serviceStartEndFrequencies;
        }),
      ).subscribe();

    this.schedulesForm.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe((values: ClientAccrualFirstYearSchedule[]) => {
        if (values && values.length) {
          const newCount = values.length;
          const oldCount = this.lastSchedulesSnapshot.length;
          const newIdsKey = values.map(r => r.clientAccrualProratedScheduleId).sort().join() + `${newCount}`;
          const oldIdsKey = this.lastSchedulesSnapshot.map(r => r.clientAccrualProratedScheduleId).sort().join() + `${oldCount}`;

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
    this.destroy$.next();
  }

  addScheduleRow() {
    // const newRow = this.store.createClientAccrualProratedScheduleForm();
    const newRow = this.store.createNextClientAccrualProratedScheduleForm();
    this.schedulesForm.markAsTouched();
    this.schedulesForm.markAsDirty();
    this.schedulesForm.push(newRow);

    // if (!this.isEditMode) this.toggleEdit(this.isEditMode);
    this.store.updateFirstYearAccrualSchedulesEditStatus(true);
  }

  copyScheduleRow(source: ClientAccrualFirstYearSchedule, copyIndex: number) {
    // Apparently we can't actually trust that source param is up to date with the actual form state...
    // Probably caused by something to do with change detection?
    source = this.schedulesForm.value[copyIndex] as ClientAccrualFirstYearSchedule;
    const index = copyIndex + 1;
    const newRow = this.store.createClientAccrualProratedScheduleForm();
    const newRowFromToDates = this.store.getShiftedTimespan(
      source.scheduleFrom,
      source.scheduleTo
    );
    newRow.setValue({
      ...source,
      scheduleFrom: newRowFromToDates.scheduleFrom,
      scheduleTo: newRowFromToDates.scheduleTo,
      clientAccrualProratedScheduleId: ClientAccrualConstants.NEW_ENTITY_ID
    });
    this.schedulesForm.markAsTouched();
    this.schedulesForm.markAsDirty();
    this.schedulesForm.insert(index, newRow);
    this.store.updateFirstYearAccrualSchedulesEditStatus(true);
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

        if (this.schedulesForm.length === 0) {
          // Reset this back to null
          this.form.patchValue({
            accrual: {
              proratedServiceReferencePointOverrideId: ''
            }
          });
        }
      } else {
        this.status = "Delete cancelled";
        return false;
      }
    });
  }

  // Toggling to readonly will cause whole clientAccrual to save via api,
  // and set this schedules grid to readonly on successful save.
  toggleEdit(wasEditMode: boolean) {
    if (wasEditMode) {
      // Is currently in edit mode, toggling to readonly.
      this._clientAccrualsSvc.submitIfHasPendingChanges(true, false, false)
        .pipe(
          // Handle EMPTY when no save occurred/was-needed.
          finalize(() => {
            if (this.store.form.valid) {
              this.store.updateFirstYearAccrualSchedulesEditStatus(false);
            }
          })
        )
        .subscribe(accrual => {
          if (accrual != null) {
            this.store.updateFirstYearAccrualSchedulesEditStatus(false);
          }
        });
    } else {
      this.store.updateFirstYearAccrualSchedulesEditStatus(true);
    }
  }

}
