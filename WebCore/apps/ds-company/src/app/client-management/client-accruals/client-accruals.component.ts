import { coerceNumberProperty } from "@angular/cdk/coercion";
import {
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
} from "@angular/core";
import { FormArray, FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { AccountService } from "@ds/core/account.service";
import { IClientAccrual } from "@ds/core/employee-services/models";
import { UserInfo } from "@ds/core/shared";
import { ConfirmDialogService } from "@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service";
import {
  BehaviorSubject,
  combineLatest,
  EMPTY,
  forkJoin,
  Observable,
  Subject,
} from "rxjs";
import {
  catchError,
  distinctUntilChanged,
  map,
  pairwise,
  skip,
  skipUntil,
  startWith,
  switchMap,
  take,
  takeUntil,
  tap,
  withLatestFrom,
} from "rxjs/operators";
import { ClientAccrualConstants } from "../../models/leave-management/client-accrual-constants";
import { ClientAccrualsStoreService } from "../../client-management/services/client-accruals-store.service";
import { ClientAccrualsService } from "../../client-management/services/client-accruals.service";
import { LeaveManagementApiService } from "../../client-management/services/leave-management-api.service";
import { ClientAccrualDropdownsDto } from "../../models/leave-management/client-accrual-dropdowns-dto";
import { ClientAccrualSchedule } from "@ds/core/employee-services/models/client-accrual-schedule.model";
import { ClientAccrualFirstYearSchedule } from "@ds/core/employee-services/models/client-accrual-first-year-schedule.model";
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
  selector: "ds-client-accruals",
  templateUrl: "./client-accruals.component.html",
  styleUrls: ["./client-accruals.component.scss"],
})
export class ClientAccrualsComponent implements OnInit, OnDestroy {
  // Manually handling routerLinkActive, since we are no longer using routerLink for nav.
  get isSetupTab() { return this.store.isSetupTab; }
  get isSchedulesTab() { return this.store.isSchedulesTab; }

  isLoading = true;
  user: UserInfo;
  private destroy$ = new Subject();
  clientAccruals$ = this.store.clientAccruals$;
  f: FormGroup = this.store.form;
  private _wasSelectedAccrualChangeCanceled$ = new BehaviorSubject(false);
  private get accrualSchedulesForm(): FormArray {
    return this.f.get('accrual.clientAccrualSchedules') as FormArray;
  }
  private get proratedSchedulesForm(): FormArray {
    return this.f.get('accrual.clientAccrualProratedSchedules') as FormArray;
  }
  private get clientAccrualIdForm(): FormControl {
    return this.f.get('accrual.clientAccrualId') as FormControl;
  }
  private get clientAccrualIdUIForm(): FormControl {
    return this.f.get('accrual.clientAccrualIdUI') as FormControl;
  }
  private _selectedClientAccrualId = ClientAccrualConstants.NEW_ENTITY_ID;

  // For setting [attr.disabled] on clientAccrualIdUI
  attrDisabledSelectedAccrualDropdown$ = this.store.canChangePolicy$.pipe(
    map(canChangePolicy => canChangePolicy ? null : 'true' )
  );

  copySelectedAccrual = this.clientAccrualsService.copySelectedAccrual;

  // private readonly _options = Object.freeze({
  //   title: 'There are unsaved changes. Do you wish to continue?',
  //   message: '',
  //   confirm: "Delete"
  // });

  constructor(
    private clientAccrualsService: ClientAccrualsService,
    private accountService: AccountService,
    private store: ClientAccrualsStoreService,
    private leaveManagementService: LeaveManagementApiService,
    // private confirmDialog: ConfirmDialogService,
    private router: Router,
    private route: ActivatedRoute,
    private ngxMsgSvc: NgxMessageService,
    private cd: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    // Watch for programmatic changes to the selectedAccrual from service.
    // Update UI form if necessary.
    this.clientAccrualIdForm.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe((x: number|string) => {
        const uiIsAString = (typeof this.clientAccrualIdUIForm.value === 'string');
        const uiEqualsX = (this.clientAccrualIdUIForm.value == x);

        // This check prevents infinite loop with formPairwiseSelectedAccrualId$ subscription.
        if (!uiIsAString || !uiEqualsX) {
          this.clientAccrualIdUIForm.setValue(x.toString());
        }
      });

    // Will give us access to the two most recent values of selected ClientAccrualId from UI.
    const formPairwiseSelectedAccrualId$ = this.clientAccrualIdUIForm.valueChanges
      .pipe(
        startWith(this.clientAccrualIdUIForm.value),
        pairwise()
      ) as Observable<[number|string, number|string]>;

    // Logic for when to ask for confirmation before switching policies,
    // and resetting selected policy change when user declines to loose changes to change selected policy.
    formPairwiseSelectedAccrualId$.pipe(
      withLatestFrom(this._wasSelectedAccrualChangeCanceled$),
      takeUntil(this.destroy$),
      switchMap(([[a, b], wasSelectedAccrualChangeCanceled]) => {

        this.clientAccrualIdUIForm.markAsPristine();

        // Doesn't do anything unless we actually subscribe to it.
        // Used in two of the four outcomes of the decision tree.
        const fetchBAndChangeToB$ = this.clientAccrualsService
          .getClientAccrualAndReplaceInList$(coerceNumberProperty(b))
          .pipe(
            catchError((err, caught) => {
              const errorMsg = (err && typeof err.message === 'string')
                ? err.message
                : 'Unable to load ClientAccrual data.';
              this.ngxMsgSvc.clearMessage();
              this.ngxMsgSvc.setErrorMessage(errorMsg);
              // Set UI back to previously selected clientAccrualId
              this._wasSelectedAccrualChangeCanceled$.next(true);
              this.clientAccrualIdUIForm.setValue(a.toString());
              return EMPTY;
            }),
            tap(clientAccrualId => {
              // Set next on "actual" _form for service to react to.
              this._wasSelectedAccrualChangeCanceled$.next(false);
              this.store.setIsResetForm();
              this.store.setIsResetFormViaPolicyChange();
              this.clientAccrualIdForm.setValue(clientAccrualId.toString());
              this.ngxMsgSvc.clearMessage();
            })
          );

        if (this.store.formHasUnsavedChanges) {
          if (!wasSelectedAccrualChangeCanceled) {
            // Yuck. Message doesn't even make sense in this context... ¯\_(ツ)_/¯
            const confirmed = confirm('Leave page with unsaved changes?');
            // this.confirmDialog.open(this._options);
            // return this.confirmDialog.confirmed().pipe(
            //   tap(confirmed => {
                if (confirmed) {
                  this.ngxMsgSvc.clearMessage();
                  this.ngxMsgSvc.setWarningMessage('Loading...');
                  return fetchBAndChangeToB$;
                } else {
                  // Set UI back to previously selected clientAccrualId
                  this._wasSelectedAccrualChangeCanceled$.next(true);
                  this.clientAccrualIdUIForm.setValue(a.toString());
                  return EMPTY;
                }
            // }));
          } else {
            // Prevents infinite loop of confirm dialogs from previous iteration where we:
            // "Set UI back to previously selected clientAccrualId"
            this._wasSelectedAccrualChangeCanceled$.next(false);
            return EMPTY;
          }
        } else {
          this.ngxMsgSvc.clearMessage();
          this.ngxMsgSvc.setWarningMessage('Loading...');
          return fetchBAndChangeToB$;
        }
      }),
      takeUntil(this.destroy$),
    ).subscribe(_ => {});

    // Setup subscription to wait for things to load from api, and show on page once ready.
    forkJoin(
      // Ignore the initial value of the backing _clientAccruals$ BehaviorSubject.
      this.clientAccruals$.pipe(skip(1), take(1)),
      this.clientAccrualsService.dropdownLists$.pipe(take(1))
    ).pipe(
      take(1)
    ).subscribe(_ => {
      this.isLoading = false;
      this.cd.detectChanges();
    });

    // Fetch data from api.
    this.store.loadClientAccruals(
      this.accountService,
      this.leaveManagementService
    );

    ///////////////////////////////////////////////////////////////////////////
    // "Main loop"
    // Handle when user changes the "Policy" dropdown,
    // changing the form and populating with client accrual data.
    ///////////////////////////////////////////////////////////////////////////
    this.f.valueChanges.pipe(
      withLatestFrom(
        combineLatest(
          this.store.isResetForm$,
          this.store.isResetFormViaPolicyChange$,
          this.clientAccruals$,
          this.store.selectedClientId$,
          this.store.isKeepOriginalSchedules$,
          this.store.isKeepOriginalProratedSchedules$,
          this.clientAccrualsService.dropdownLists$
        )
      ),
      takeUntil(this.destroy$)
    ).subscribe(([
      _,
      [
        isResetForm,
        isResetFormViaPolicyChange,
        clientAccruals,
        selectedClientId,
        isKeepOriginalSchedules,
        isKeepOriginalProratedSchedules,
        dropdownLists
      ]
    ]: [
      any,
      [
        boolean,
        boolean,
        IClientAccrual[],
        number,
        boolean,
        boolean,
        ClientAccrualDropdownsDto,
      ]
    ]) => {
      const accrualId = coerceNumberProperty(this.f.value.accrual.clientAccrualId);

      // Check to see if the accrual id from the form is the same as what is currently selected
      // according to our local component. If they match, that means the event is not a valid
      // policy change and some child event in the form bubbled up.
      if (accrualId == this._selectedClientAccrualId && !isResetForm) {
        return;
      }
      this._selectedClientAccrualId = accrualId;
      if (isResetForm) {
        this.store.unsetIsResetForm();
      }

      const selected = clientAccruals.find(
        (ca) => ca.clientAccrualId == accrualId
      );

      const isSelectedAccrualNew = (
        selected.clientAccrualId <= ClientAccrualConstants.NEW_ENTITY_ID
      );

      if (selected && !isSelectedAccrualNew) {

        if (selected.isPaidLeaveAct) {
          this.f.get('accrual.hoursPerWeekAct').enable();
          this.f.get('accrual.allowHoursRollOver').enable();
        }
        if (!selected.isPaidLeaveAct) {
          this.f.get('accrual.hoursPerWeekAct').disable();
          this.f.get('accrual.allowHoursRollOver').disable();
        }
        if (selected.isLeaveManagment) {
          this.f.get('accrual.hoursInDay').enable();
          this.f.get('accrual.allowAllDays').enable();
          this.f.get('accrual.requestMinimum').enable();
          this.f.get('accrual.requestMaximum').enable();
          this.f.get('accrual.requestIncrement').enable();
          this.f.get('accrual.leaveManagmentAdministrator').enable();
          this.f.get('accrual.isSupEmailRequest').enable();
          this.f.get('accrual.isRealTimeAccruals').enable();
          this.f.get('accrual.projectAmount').enable();
          this.f.get('accrual.projectAmountType').enable();
          this.f.get('accrual.isLeaveManagementUseBalanceOption').enable();
        }
        if (!selected.isLeaveManagment) {
          this.f.get('accrual.hoursInDay').disable();
          this.f.get('accrual.allowAllDays').disable();
          this.f.get('accrual.requestMinimum').disable();
          this.f.get('accrual.requestMaximum').disable();
          this.f.get('accrual.requestIncrement').disable();
          this.f.get('accrual.leaveManagmentAdministrator').disable();
          this.f.get('accrual.isSupEmailRequest').disable();
          this.f.get('accrual.isRealTimeAccruals').disable();
          this.f.get('accrual.projectAmount').disable();
          this.f.get('accrual.projectAmountType').disable();
          this.f.get('accrual.isLeaveManagementUseBalanceOption').disable();

          this.f.patchValue({
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
            // this.f.controls['accrual.carryOverToId'].setValue(selected.carryOverToId)
        }

        // Mapping to accommodate id prop name difference,
        // and to include the earning description for the form.
        const mappedEarnings = (selected.clientAccrualEarnings || []).map(x => {
          const earning = dropdownLists.clientEarnings.find(
            (e) => e.clientEarningId == x.clientAccrualEarningId
          );
          return earning || null;
        }).filter(earning => !!earning);

        this.f.patchValue(
          {
            accrual: {
              ...selected,
              description: (selected.clientAccrualId == ClientAccrualConstants.NEW_ENTITY_ID)
                ? '' // Blank out the name if new accrual
                : selected.description,
              clientAccrualId: selected.clientAccrualId.toString(),
              clientId: selected.clientId || selectedClientId,
              proratedWhenToAwardTypeId: selected.proratedWhenToAwardTypeId || 1,
              clientEarnings: mappedEarnings,
              // isActive: selected.isActive || true,

              // Conditionally keeps pending schedules changes.
              clientAccrualSchedules: (isKeepOriginalSchedules)
                ? this.store.clientAccrualSchedulesForm.getRawValue()
                : selected.clientAccrualSchedules,
              clientAccrualProratedSchedules: (isKeepOriginalProratedSchedules)
                ? this.store.clientAccrualProratedSchedulesForm.getRawValue()
                : selected.clientAccrualProratedSchedules,
            },
          },
          { emitEvent: false }
        );

        // this.f.get("accrual.clientEarnings").setValue(mappedEarnings);

        // update schedules
        if (!isKeepOriginalSchedules) {
          // this.store.updateAccrualSchedulesEditStatus(false);
          this.updateClientAccrualSchedulesFormArray(selected);
        } else {
          const schedulesForm = this.f.get("accrual.clientAccrualSchedules");
          schedulesForm.markAllAsTouched();
          schedulesForm.markAsDirty();
        }

        if (!isKeepOriginalProratedSchedules) {
          // this.store.updateFirstYearAccrualSchedulesEditStatus(false);
          this.updateClientAccrualProratedSchedulesFormArray(selected);
        } else {
          const proratedForm = this.f.get("accrual.clientAccrualProratedSchedules");
          proratedForm.markAllAsTouched();
          proratedForm.markAsDirty();
        }
      } else if (selected) {
        // Calling this.f.reset() was causing issues with
        // resetting some controls to the correct value...
        this.f.patchValue(
          {
            accrual: {
              // ...selected,
              ...this.store.newClientAccrualKernel,
              description: ''
            }
          },
          { emitEvent: false }
        );
        this.ngxMsgSvc.clearMessage();
      }

      // Trigger valueChange emission for first-year-schedules-card to update view accordingly.
      this.store.form.get('accrual.proratedWhenToAwardTypeId')
        .updateValueAndValidity({onlySelf: true});

      if (isResetForm) {
        // We want to emit an event for clientEarnings valueChanges.
        this.f.get("accrual.clientEarnings").updateValueAndValidity();
        // this.f.get("accrual.clientEarnings").setValue(mappedEarnings);

        this.store.unsetIsResetForm();

        this.store.form.markAsUntouched();
        this.store.form.markAsPristine();
        this.ngxMsgSvc.clearMessage();

        if (!isKeepOriginalSchedules) {
          const schedulesForm = this.f.get("accrual.clientAccrualSchedules");
          this.updateClientAccrualSchedulesFormArray(selected);

          // Check if we've reset the form via policyChange (and not via cancel button),
          // but the ClientAccrualSchedule data that we're resetting to is already invalid.
          // This can happen when loading a legacy accrual by changing selected accrual via the dropdown.
          //
          // (IE: one that was saved before we had more rigorous/stringent ClientAccrualSchedule validation.
          // Some older policies were created before recent client-side validation was created;
          // mostly in the End Service.)
          if (!isSelectedAccrualNew && !schedulesForm.valid && isResetFormViaPolicyChange) {
            const setSchedulesTableIntoEditModeAndShowErrors = () => {
              this.store.unsetIsResetFormViaPolicyChange();
              schedulesForm.markAllAsTouched();
              schedulesForm.markAsDirty();
              this.store.updateAccrualSchedulesEditStatus(true);
              this.cd.detectChanges();
            };
            const urlFragments = ['schedules'];
            if (!this.getIsAlreadyOnRequestedTab(urlFragments)) {
              // Navigate directly to the schedules tab.
              this.router.navigate(urlFragments, { relativeTo: this.route })
                .then(x => setSchedulesTableIntoEditModeAndShowErrors());
            } else {
              setSchedulesTableIntoEditModeAndShowErrors();
            }
          }
          // this.store.updateAccrualSchedulesEditStatus(!schedulesForm.valid);
        } else {
          const schedulesForm = this.f.get("accrual.clientAccrualSchedules");
          schedulesForm.markAllAsTouched();
          schedulesForm.markAsDirty();
        }

        if (!isKeepOriginalProratedSchedules) {
          // this.store.updateFirstYearAccrualSchedulesEditStatus(false);
          this.updateClientAccrualProratedSchedulesFormArray(selected);
        } else {
          const proratedForm = this.f.get("accrual.clientAccrualProratedSchedules");
          proratedForm.markAllAsTouched();
          proratedForm.markAsDirty();
        }

        // If this isResetForm, but not also isResetFormViaPolicyChange,
        // ensure that we reset the schedules tables back to readonly mode.
        if (!isResetFormViaPolicyChange) {
          this.store.updateAccrualSchedulesEditStatus(false);
          this.store.updateFirstYearAccrualSchedulesEditStatus(false);
        }
      }

      // Catchall reset
      this.store.updateIsKeepOriginalSchedules(false);
      this.store.updateIsKeepOriginalProratedSchedules(false);
    });
    ///////////////////////////////////////////////////////////////////////////
  }

  private updateClientAccrualSchedulesFormArray(selected: IClientAccrual) {
    this.accrualSchedulesForm.clear();

    if (selected.clientAccrualSchedules && selected.clientAccrualSchedules.length) {
      selected.clientAccrualSchedules.forEach((s: ClientAccrualSchedule) => {
        const newRow = this.store.createClientAccrualScheduleForm(s);
        this.accrualSchedulesForm.push(newRow);
      });
    }
  }

  private updateClientAccrualProratedSchedulesFormArray(selected: IClientAccrual) {
    this.proratedSchedulesForm.clear();

    if (selected.clientAccrualProratedSchedules && selected.clientAccrualProratedSchedules.length) {
      selected.clientAccrualProratedSchedules.forEach((s: ClientAccrualFirstYearSchedule) => {
        const newRow = this.store.createClientAccrualProratedScheduleForm(s);
        this.proratedSchedulesForm.push(newRow);
      });
    }
  }

  private getIsAlreadyOnRequestedTab(urlFragments: string[] = []) {
    const urlPrefix = '/sys/leave-management/client-accruals/';
    const url = `${urlPrefix}${urlFragments.join('/')}`;
    return (this.router.url === url);
  }

  submitIfHasPendingChangesAndNavigate(
    urlFragments: string[] = []
  ) {
    if (this.getIsAlreadyOnRequestedTab(urlFragments)) {
      return; // Already on that tab.
    }

    if (this.store.formHasUnsavedChanges || this.store.isNewAccrual) {
      // Try to save changes if dirty or isNewAccrual
      this.clientAccrualsService.submitIfHasPendingChanges(false, false, false)
      .subscribe((accrual) => {
        if (urlFragments.length > 0) {
          // Once saved, try to navigate where we tried to before saving...
          this.router.navigate(urlFragments, { relativeTo: this.route });
        }
      });
    } else {
      // Navigate if no pending changes to save.
      this.router.navigate(urlFragments, { relativeTo: this.route });
    }
  }

  ngOnDestroy() {
    this.destroy$.next();
  }
}
