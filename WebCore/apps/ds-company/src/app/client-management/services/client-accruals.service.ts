import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";
import { coerceBooleanProperty } from "@angular/cdk/coercion";
import { Injectable } from "@angular/core";
import { FormGroup, FormArray, AbstractControl } from "@angular/forms";
import { AccountService } from "@ds/core/account.service";
import { IClientAccrual } from "@ds/core/employee-services/models";
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";
import { ConfirmDialogService } from "@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service";
import {
  forkJoin,
  Observable,
  EMPTY,
  of,
  throwError,
} from "rxjs";
import {
  catchError,
  finalize,
  map,
  shareReplay,
  switchMap,
  take,
  tap,
} from "rxjs/operators";
import { ClientAccrualConstants } from "../../models/leave-management/client-accrual-constants";
import { ClientAccrualDropdownsDto, IClientCalendar } from "../../models/leave-management/client-accrual-dropdowns-dto";
import { ClientAccrualSaveMessages } from "../../models/leave-management/client-accrual-save-messages";

import { ClientAccrualsStoreService } from "./client-accruals-store.service";
import { LeaveManagementApiService } from "./leave-management-api.service";

@Injectable({
  providedIn: "root",
})
export class ClientAccrualsService {

  private _dropdownLists$ = this.loadTimeOffPoliciesFormUiDependencies().pipe(
    shareReplay()
  );
  get getTimeOffPoliciesPageViewDependencies$(): Observable<ClientAccrualDropdownsDto> {
    // if the private dropdownlists var is null, call the method to load them from the API
    // if (this._dropdownLists$.getValue() === null) this.loadTimeOffPoliciesFormUiDependencies();
    return this._dropdownLists$;
  }
  get dropdownLists$() {
    return this._dropdownLists$;
  }

  constructor(
    private _accountSvc: AccountService,
    private _apiSvc: LeaveManagementApiService,
    private _ngxMsgSvc: NgxMessageService,
    private store: ClientAccrualsStoreService,
    private confirmDialog: ConfirmDialogService
  ) {}

  /**
   *
   * @returns Observable boolean of whether PMLA is enabled for the currently selected client.
   */
  getIsPMLAEnabled$() {
    return this._accountSvc.PassUserInfoToRequest((userInfo) =>
        this._apiSvc.getClientCalendar(userInfo.selectedClientId())
      ).pipe(
        map(x => {
          let result = coerceBooleanProperty(x.calendarFrequencyWeeklyId || x.calendarFrequencyAltWeekId || x.calendarFrequencyBiWeeklyId);
          return result;
        })
      );
  }

  private loadTimeOffPoliciesFormUiDependencies(): Observable<ClientAccrualDropdownsDto> {
    return forkJoin(
      this._apiSvc.getClientAccrualDropdownDtos(),
      this._apiSvc.getEmployeePayTypeList(),
      this._accountSvc.PassUserInfoToRequest((userInfo) =>
        this._apiSvc.getClientEarningsList(userInfo.selectedClientId(), true)
      ),
      this._accountSvc.PassUserInfoToRequest((userInfo) =>
        this._apiSvc.getCompanyAdminList(userInfo.selectedClientId(), true)
      ),
    ).pipe(
      map(
        ([
          {
            serviceFrequencies,
            serviceRewardFrequencies,
            serviceRenewFrequencies,
            serviceCarryOverFrequencies,
            serviceCarryOverTillFrequencies,
            serviceCarryOverWhenFrequencies,
            serviceReferencePointFrequencies,
            serviceStartEndFrequencies,
            servicePlanTypes,
            serviceTypes,
            serviceUnits,
            accrualBalanceOptions,
            clientAccrualEmployeeStatuses,
            clientAccrualCarryOverOptions,
            clientAccrualClearOptions,
            serviceBeforeAfters,
            autoApplyAccrualPolicyOptions,
          },
          employeePayTypes,
          clientEarnings,
          companyAdmins,
        ]) => ({
          serviceFrequencies,
          serviceRewardFrequencies,
          serviceRenewFrequencies,
          serviceCarryOverFrequencies,
          serviceCarryOverTillFrequencies,
          serviceCarryOverWhenFrequencies,
          serviceReferencePointFrequencies,
          serviceStartEndFrequencies,
          servicePlanTypes,
          serviceTypes,
          serviceUnits,
          accrualBalanceOptions,
          clientAccrualEmployeeStatuses,
          clientAccrualCarryOverOptions,
          clientAccrualClearOptions,
          serviceBeforeAfters,
          autoApplyAccrualPolicyOptions,
          employeePayTypes,
          clientEarnings,
          companyAdmins,
        })
      )
    );
  }

  // Wipe out the form changes, revert back to selected accrual state.
  onCancel = () => {
    this.store.unsetIsResetFormViaPolicyChange();
    this.store.resetFormChanges();
  }

  onDeleteClientAccrual$ = () => {
    if (this.store.selectedClientAccrualId > ClientAccrualConstants.NEW_ENTITY_ID) {
      return this.deleteClientAccrual$(this.store.selectedClientAccrualId);
    } else {
      return EMPTY;
    }
  }

  private _subscribeOrReturnSourceObservable<T>(
    sourceObservable$: Observable<T>,
    doSubscribe: boolean
  ) {
    if (doSubscribe) {
      sourceObservable$.subscribe((_) => {});
      return EMPTY;
    } else {
      return sourceObservable$;
    }
  }

  // Submit changes if any pending.
  submitIfHasPendingChanges = (
    keepOriginalSchedules: boolean,
    keepOriginalProratedSchedules: boolean,
    doSubscribe: boolean = true
  ): Observable<IClientAccrual> => {

    this.store.updateIsKeepOriginalSchedules(keepOriginalSchedules);
    this.store.updateIsKeepOriginalProratedSchedules(keepOriginalProratedSchedules);

    let saveClientAccrual$: Observable<IClientAccrual>;

    // Determine whether or not the form needs to be submitted.
    if (this.store.formHasUnsavedChanges || this.store.isNewAccrual) {
      saveClientAccrual$ = this.onSubmit(
        keepOriginalSchedules,
        keepOriginalProratedSchedules,
        false
      );
    } else {
      saveClientAccrual$ = EMPTY;
    }

    saveClientAccrual$ = saveClientAccrual$.pipe(finalize(() => {
      // Re-enable the either of the schedules forms, if necessary.
      if (this.store.clientAccrualSchedulesForm.disabled) {
        this.store.clientAccrualSchedulesForm.enable({onlySelf: true});
      }
      if (this.store.clientAccrualProratedSchedulesForm.disabled) {
        this.store.clientAccrualProratedSchedulesForm.enable({onlySelf: true});
      }
    }));

    return this._subscribeOrReturnSourceObservable(
      saveClientAccrual$,
      doSubscribe
    );
  }

  // Arrow func syntax to bind `this` to ClientAccrualsService,
  // regardless of whether the func is "proxied" through another component.
  onSubmit = (
    keepOriginalSchedules: boolean,
    keepOriginalProratedSchedules: boolean,
    doSubscribe: boolean = true
  ): Observable<IClientAccrual> => {
    const messages = {
      invalidFormErrorMsg: "Form is invalid, fix before submitting.",
      savingMsg: `Saving...`,
      successMsg: "Policy was successfully saved.",
      errorMsg: "There was an error saving the policy!",
    } as ClientAccrualSaveMessages;

    const accrual = this.store.prepareClientAccrualModel(this.store.form);

    if (keepOriginalSchedules || keepOriginalProratedSchedules) {
      const index = this.store.getClientAccrualIndex(accrual.clientAccrualId);
      const selectedAccrual = this.store.clientAccruals[index];
      if (keepOriginalSchedules) {
        // Disable to temporarily exclude from validation...
        this.store.clientAccrualSchedulesForm.disable({onlySelf: true});
        accrual.clientAccrualSchedules = selectedAccrual.clientAccrualSchedules;
      }
      if (keepOriginalProratedSchedules) {
        // Disable to temporarily exclude from validation...
        this.store.clientAccrualProratedSchedulesForm.disable({onlySelf: true});
        accrual.clientAccrualProratedSchedules = selectedAccrual.clientAccrualProratedSchedules;
      }
    }

    const saveClientAccrual$ = this._saveClientAccrual(
      accrual,
      messages,
      keepOriginalSchedules,
      keepOriginalProratedSchedules,
    ).pipe(
      take(1)
    );

    return this._subscribeOrReturnSourceObservable(
      saveClientAccrual$,
      doSubscribe
    );
  }

  // Copies the currently selected accrual, and immediately saves to api.
  copySelectedAccrual = () => {
    const options = {
      title: 'Are you sure you want to copy this policy?',
      message: '',
      confirm: "Copy Policy"
    };
    this.confirmDialog.open(options);
    this.confirmDialog.confirmed().subscribe(confirmed => {
      if ( confirmed ) {
        const copy = this.store.prepareClientAccrualModel(this.store.form);

        copy.clientAccrualId = ClientAccrualConstants.NEW_ENTITY_ID;
        copy.clientAccrualEarnings.forEach(
          (x) => (x.clientAccrualId = ClientAccrualConstants.NEW_ENTITY_ID)
        );
        copy.clientAccrualSchedules.forEach(
          (x) => (x.clientAccrualId = ClientAccrualConstants.NEW_ENTITY_ID)
        );
        copy.clientAccrualProratedSchedules.forEach(
          (x) => (x.clientAccrualId = ClientAccrualConstants.NEW_ENTITY_ID)
        );
        const copyOfPrefixStr = "Copy of: ";
        copy.description = `${copyOfPrefixStr}${copy.description}`;

        if (copy.description.length > 35) {
          // Truncate to 35 chars.
          copy.description = copy.description.substring(0, 35);
        }

        const messages = {
          invalidFormErrorMsg: "Form is invalid, fix before copying policy.",
          savingMsg: `Saving Policy: '${copy.description}'`,
          successMsg: "Policy was successfully copied and saved.",
          errorMsg: "There was an error saving the policy!",
        } as ClientAccrualSaveMessages;

        this._saveClientAccrual(copy, messages, false, false)
          .pipe(take(1))
          .subscribe(_ => {});
      } else {
          return;
      }
    });
  }

  // Shared save logic.
  private _saveClientAccrual = (
    accrual: IClientAccrual,
    messages: ClientAccrualSaveMessages,
    keepOriginalSchedules: boolean,
    keepOriginalProratedSchedules: boolean,
  ): Observable<IClientAccrual> => {
    let saved$: Observable<IClientAccrual>;

    // TODO: Shouldn't need this if the rest is working as intended...
    // this.store.form.updateValueAndValidity();
    // this._markInvalidControlsAsTouchedThenUpdateValueAndValidity(this.store.form, true);

    const proratedValidationErrors = this.store
      .proratedNoGapsBetweenTiers_proratedRowsCompleteFullAnnualCycle(
        this.store.clientAccrualProratedSchedulesForm
      );

    const noProratedValidationErrors = (
      proratedValidationErrors == null || Object.keys(proratedValidationErrors).length === 0
    );

    if (noProratedValidationErrors && this.store.form.valid) {
      this._setMessageWithNoTimeout(messages.savingMsg);

      const selectedAccrualIndex = this.store.getClientAccrualIndex(accrual.clientAccrualId);

      saved$ = this._apiSvc
        .postClientAccrual(accrual)
        .pipe(
          catchError((err, caught) => {
            this._ngxMsgSvc.clearMessage();
            this._ngxMsgSvc.setErrorMessage(messages.errorMsg);
            return EMPTY;
          }),
          tap((savedAccrual) => {
            if (savedAccrual) {
              // Replace existing accrual with updated version from server.
              // If completely new (or copied) accrual policy, assert(selectedAccrualIndex === -1).
              if (selectedAccrualIndex > 0) {
                // Replace value if previous version existed in list.
                this.store.replaceClientAccrualAtIndex(selectedAccrualIndex, savedAccrual);
              } else {
                // Push value onto list if new.
                this.store.pushClientAccrual(savedAccrual);
              }

              // Set flags for accrual.clientAccrualId.valueChanges loop to properly update the form,
              // when updating an existing accrual.
              this.store.setIsResetForm();
              this.store.updateIsKeepOriginalSchedules(keepOriginalSchedules);
              this.store.updateIsKeepOriginalProratedSchedules(keepOriginalProratedSchedules);

              // Reset the autoApplyAccrualPolicyOptionId when saved successfully.
              this.store.form.get('accrual.autoApplyAccrualPolicyOptionId').setValue(0);

              // Update the UI.
              this.store.form
                .get("accrual.clientAccrualId")
                .setValue(savedAccrual.clientAccrualId.toString());

              this._ngxMsgSvc.clearMessage();
              this._ngxMsgSvc.setSuccessMessage(messages.successMsg);
            } else {
              this._ngxMsgSvc.clearMessage();
              this._ngxMsgSvc.setErrorMessage(messages.errorMsg);
            }
          })
        );
    } else {

      this.store.form.markAllAsTouched();

      this._ngxMsgSvc.clearMessage();
      this._ngxMsgSvc.setErrorMessage(messages.invalidFormErrorMsg);
      saved$ = EMPTY;
    }

    return saved$;
  }

  // You'll want to manually call this._ngxMsgSvc.clearMessage() once ready.
  private _setMessageWithNoTimeout = (
    msg: string,
    msgType: MessageTypes = MessageTypes.warning
  ) => {
    this._ngxMsgSvc.clearMessage();
    this._ngxMsgSvc.message.text = msg;
    this._ngxMsgSvc.message.type = msgType;
    this._ngxMsgSvc.message$.next(this._ngxMsgSvc.message);
  }

  getClientAccrualAndReplaceInList$ = (
    clientAccrualId: number
  ): Observable<number> => {
    // Don't need to fetch anything if this is a new accrual.
    if (clientAccrualId == ClientAccrualConstants.NEW_ENTITY_ID) {
      return of(clientAccrualId);
    }

    // Otherwise, fetch latest state of the accrual, and replace in the list.
    const selectedAccrualIndex = this.store.getClientAccrualIndex(clientAccrualId);

    return this._apiSvc.getClientAccrual(clientAccrualId, true)
      .pipe(
        switchMap(accrual => {
          if (accrual != null) {
            return of(accrual);
          } else {
            const error = new Error('Unable to load ClientAccrual data.');
            return throwError(error);
          }
        }),
        tap(accrual => {
          // Replace existing accrual with updated version from server.
          // If completely new (or copied) accrual policy, assert(selectedAccrualIndex === -1).
          if (accrual != null && selectedAccrualIndex > 0) {
            // Replace value if previous version existed in list.
            this.store.replaceClientAccrualAtIndex(selectedAccrualIndex, accrual);
          } else {
            // Something went wrong...
          }
        }),
        map(accrual => accrual.clientAccrualId),
      );
  }

  deleteClientAccrual$ = (clientAccrualId: number) => {
    const selectedAccrualIndex = this.store.getClientAccrualIndex(clientAccrualId);

    this._setMessageWithNoTimeout('Deleting...');

    const deleted$ = this._apiSvc.deleteClientAccrual(clientAccrualId)
    .pipe(
      catchError((err, caught) => {
        const defaultErrorMsg = 'Unable to delete ClientAccrual.';
        let errMsg: string = null;
        const hasErrors = (
          err.error != null
          && Array.isArray(err.error.errors)
          && err.error.errors.length > 0
        );
        if (hasErrors) {
          const errErrors = err.error.errors as {msg: string}[];
          // Concat all of the error messages into a single string.
          errMsg = errErrors.reduce((acc, curr) => {
            acc += (curr.msg != null && typeof curr.msg === 'string')
              ? curr.msg + ' '
              : ' ';
            return acc;
          }, '').trim();
        }
        const errorMessage = (errMsg === '') ? defaultErrorMsg : errMsg;
        this._setMessageWithNoTimeout(errorMessage, MessageTypes.error);
        // this._ngxMsgSvc.setErrorResponse(err);
        return EMPTY;
      }),
      tap((statusMessage) => {
        statusMessage = (typeof statusMessage === 'string') ? statusMessage : '';
        const wasDeleted = (statusMessage.toLowerCase() === 'success.');
        if (wasDeleted) {
          this._ngxMsgSvc.setSuccessMessage('Policy was successfully deleted.');
          this.store.removeClientAccrualAtIndex(selectedAccrualIndex, clientAccrualId);
        } else {
          // Display the error message sent back from the server.
          this._setMessageWithNoTimeout(statusMessage, MessageTypes.error);
        }
      })
    );

    return deleted$;
  }

  deleteClientAccrualLeaveManagementPendingAwards = () => {
    this._setMessageWithNoTimeout('Deleting pending awards...');
    const result$ = this._apiSvc.deleteClientAccrualLeaveManagementPendingAwards(
      this.store.selectedClientAccrualId
    ).pipe(
      catchError((err, caught) => {
        this._ngxMsgSvc.setErrorResponse(err);
        return EMPTY;
      }),
      tap(statusMessage => {
        statusMessage = (typeof statusMessage === 'string') ? statusMessage : '';
        const wasDeleted = (statusMessage.toLowerCase() === 'success.');
        if (wasDeleted) {
          this._ngxMsgSvc.setSuccessMessage('Pending awards were successfully deleted.');
        } else {
          // Display the error message sent back from the server.
          this._setMessageWithNoTimeout(statusMessage, MessageTypes.error);
        }
      }),
    );
    return result$;
  }

  ////////////////////////
  // #region DEBUG ONLY
  // This will trigger the view to show validation messages/styles on invalid form controls.
  private _markInvalidControlsAsTouchedThenUpdateValueAndValidity(
    formToInvestigate: FormGroup | FormArray,
    doLog: boolean = false
  ) {
    const controls = this._findInvalidControlsRecursive(formToInvestigate);

    if (doLog) {
      console.log("logInvalidControlsRecursive");
    }

    controls.forEach((control) => {
      control.control.markAsTouched();
      control.control.updateValueAndValidity();
      if (doLog) {
        console.log(`${control.key}: ${JSON.stringify(control.control.errors)}`);
      }
    });

    return controls.length > 0;
  }

  // Returns an array of invalid controls/groups, or a zero-length array if
  // no invalid controls/groups where found
  // See: https://stackoverflow.com/a/52312518/13188284
  private _findInvalidControlsRecursive(
    formToInvestigate: FormGroup | FormArray
  ): { key: string; control: AbstractControl }[] {
    const invalidControls: { key: string; control: AbstractControl }[] = [];
    const recursiveFunc = (form: FormGroup | FormArray) => {
      Object.keys(form.controls).forEach((field) => {
        const control = form.get(field);
        if (control.invalid) {
          invalidControls.push({ key: field, control: control });
        }
        if (control instanceof FormGroup) {
          recursiveFunc(control);
        } else if (control instanceof FormArray) {
          recursiveFunc(control);
        }
      });
    };
    recursiveFunc(formToInvestigate);
    return invalidControls;
  }
  // #endregion DEBUG ONLY
  ////////////////////////
}
