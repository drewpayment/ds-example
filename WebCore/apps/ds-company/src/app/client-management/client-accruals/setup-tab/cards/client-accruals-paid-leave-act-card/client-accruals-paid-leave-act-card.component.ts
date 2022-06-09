import {
  Component,
  OnDestroy,
  OnInit,
} from "@angular/core";
import {
  FormArray,
  FormGroup,
} from "@angular/forms";
import { ClientAccrualConstants } from "apps/ds-company/src/app/models/leave-management/client-accrual-constants";
import { BehaviorSubject, Observable, Subject } from "rxjs";
import { pairwise, startWith, take, takeUntil } from "rxjs/operators";
import { ClientAccrualsStoreService } from "../../../../../client-management/services/client-accruals-store.service";
import { ClientAccrualsService } from "../../../../services/client-accruals.service";

@Component({
  selector: "ds-client-accruals-paid-leave-act-card",
  templateUrl: "./client-accruals-paid-leave-act-card.component.html",
  styleUrls: ["./client-accruals-paid-leave-act-card.component.scss"],
})
export class ClientAccrualsPaidLeaveActCardComponent
  implements OnInit, OnDestroy {

  form: FormGroup = this.store.form;
  dropdownLists$ = this._clientAccrualsSvc.dropdownLists$;

  private _isPMLAEnabled$ = new BehaviorSubject<boolean>(false);
  isPMLAEnabled$ = this._isPMLAEnabled$.asObservable();

  get isPaidLeaveActForm() {
    return this.form.get('accrual.isPaidLeaveAct');
  }

  private readonly _destroy$ = new Subject();

  constructor(
    private store: ClientAccrualsStoreService,
    private _clientAccrualsSvc: ClientAccrualsService,
  ) {}

  ngOnInit() {
    this._clientAccrualsSvc.getIsPMLAEnabled$()
      .pipe(take(1))
      .subscribe(isPMLAEnabledForClient => {
        this._isPMLAEnabled$.next(isPMLAEnabledForClient)

        if (isPMLAEnabledForClient && this.isPaidLeaveActForm.disabled) {
          // Enable form control if not already enabled and isPMLAEnabledForClient is true.
          this.isPaidLeaveActForm.enable();
        }
      });

    (this.isPaidLeaveActForm.valueChanges as Observable<boolean>)
      .pipe(
        startWith(this.isPaidLeaveActForm.value),
        pairwise(),
        takeUntil(this._destroy$)
      )
      .subscribe(([a, b]) => {
        if (this._isPMLAEnabled$.value && a !== b) {
          const accrualSchedules = this.store.clientAccrualSchedulesForm;

          // If going from isPaidLeaveAct off=>on
          if (b || b === null) {

            this.form.patchValue({
              accrual: {
                planType: ClientAccrualConstants.PTO_PLNTYPE,
                isActive: true,
                notes: `${this.form.value.accrual.notes ? this.form.value.accrual.notes + '\n': ''}${ClientAccrualConstants.DO_NOT_EDIT_NOTE}`,
                employeeTypeId: ClientAccrualConstants.PTO_EETYPE,
                employeeStatusId: ClientAccrualConstants.PTO_EESTATUS,
                waitingPeriodReferencePoint: ClientAccrualConstants.PTO_REFTYPE,
                serviceReferencePointId: ClientAccrualConstants.PTO_REFPNT,
                accrualCarryOverOptionId: ClientAccrualConstants.PTO_CRYOVER,
                accrualBalanceOptionId: ClientAccrualConstants.PTO_BALOPT,
                isAccrueWhenPaid: ClientAccrualConstants.PTO_ACCRUEWHENPAID,
                units: ClientAccrualConstants.PTO_UNITS,
                isShowOnStub: ClientAccrualConstants.PTO_SHOWONSTUB,
                hoursPerWeekAct: ClientAccrualConstants.PTO_HOURS,
                allowHoursRollOver: ClientAccrualConstants.PTO_ROLLHOURS,
              }
            });

            // If there are no schedules yet,
            // create a new schedule for the paidLeaveAct,
            // and push onto the empty schedules array.
            if (accrualSchedules.value.length === 0) {
              const pmLaSchedule = this.store.createClientAccrualPaidLeaveActScheduleForm();
              accrualSchedules.push(pmLaSchedule);
            }
          }

          // If going from isPaidLeaveAct on=>off
          if (!b) {
            const selectedAccrualIndex = this.store.getClientAccrualIndex(this.store.selectedClientAccrualId);
            const storedAccrual = this.store.clientAccruals[selectedAccrualIndex];
            this.form.patchValue({
              accrual: {
                planType: storedAccrual.planType, // 0,
                isActive: true,
                notes: storedAccrual.notes,
                employeeTypeId: storedAccrual.employeeTypeId, // 0,
                employeeStatusId: storedAccrual.employeeStatusId, // 0,
                waitingPeriodReferencePoint: 1,
                serviceReferencePointId: storedAccrual.serviceReferencePointId, // 0,
                // accrualCarryOverOptionId: 1, //'', // 0,
                accrualBalanceOptionId: storedAccrual.accrualBalanceOptionId, // 0,
                isAccrueWhenPaid: true,
                units: storedAccrual.units, // 0,
                isShowOnStub: true,
                // hoursPerWeekAct: '',
                allowHoursRollOver: false,
                hoursPerWeekAct: storedAccrual.hoursPerWeekAct
              }
            });

            // If there's only one schedule in the array, and it doesn't yet have an assigned scheduleId,
            // then it was added in this session and is not yet saved, so we can safely remove it.
            if (
              accrualSchedules.value.length === 1
              && accrualSchedules.value[0].clientAccrualScheduleId == ClientAccrualConstants.NEW_ENTITY_ID
            ) {
              accrualSchedules.clear();
            }
          }

          if (b) {
            this.form.get('accrual.hoursPerWeekAct').enable();
            this.form.get('accrual.allowHoursRollOver').enable();
          } else {
            this.form.get('accrual.hoursPerWeekAct').disable();
            this.form.get('accrual.allowHoursRollOver').disable();
          }
        }
    });
  }

  ngOnDestroy() {
    this._destroy$.next();
  }

}
