import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AccountService } from '@ds/core/account.service';
import { ClientService } from '@ds/core/clients/shared';
import { AutocompleteItem } from '@ds/core/groups/shared/autocomplete-item.model';
import { UserInfo, UserType } from '@ds/core/shared';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import {
  EmployeeBasic,
  UserSupervisorAccessInfo,
  UpdateUserProfileAccountDisableRequest,
  UserProfile,
  NewUserRequest,
  UserPin,
} from '@models';
import { IsObject } from '../../../../../../../lib/utilties';
import {
  BehaviorSubject,
  iif,
  Observable,
  of,
  OperatorFunction,
  Subject,
  Subscription,
  timer,
} from 'rxjs';
import {
  filter,
  map,
  mergeMap,
  startWith,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs/operators';
import { MessageService } from '../../../services/message.service';
import { CompanyAccessDialogComponent } from '../company-access-dialog/company-access-dialog.component';
import { UserProfileService } from '../user-profile.service';
import { PasswordValidator } from './password-validator';
import { FormValidatorService } from './unique-username.validator';
import * as bcrypt from 'bcryptjs';
import { environment } from '../../../../environments/environment';
import { alertAndRethrow, catchHttpError } from '../../../shared/utils/operator-utils';

const SALT = bcrypt.genSaltSync(10);

@Component({
  selector: 'ds-user-profile-form',
  templateUrl: './user-profile-form.component.html',
  styleUrls: ['./user-profile-form.component.scss'],
})
export class UserProfileFormComponent implements OnInit, OnDestroy {
  me: UserInfo;
  meSupAccess: UserSupervisorAccessInfo;
  private allUsers: UserProfile[];
  private _availableUsers$ = new BehaviorSubject<UserProfile[]>([]);
  availableUsers$: Observable<UserProfile[]> =
    this._availableUsers$.asObservable();
  private allEmployees: EmployeeBasic[];
  private _availableEmployees$ = new BehaviorSubject<EmployeeBasic[]>([]);
  availableEmployees$: Observable<EmployeeBasic[]> =
    this._availableEmployees$.asObservable();
  form = this.createForm();
  frmSubmitted = false;

  //#region Form Controls
  get inputsForm(): FormGroup {
    return this.form.get('inputs') as FormGroup;
  }
  get uiForm(): FormGroup {
    return this.form.get('ui') as FormGroup;
  }
  /**
   * This is only to be used for the user autocomplete dropdown and should not be used
   * for determining the rest of the form's state and values. The user field on the form
   * gets changed very often based on focus/unfocus on the form field.
   */
  get userControl(): FormControl {
    return this.form.get('ui.user') as FormControl;
  }
  get usernameControl(): FormControl {
    return this.form.get('inputs.username') as FormControl;
  }
  get userTypeControl(): FormControl {
    return this.form.get('inputs.userType') as FormControl;
  }
  get userDisabledControl(): FormControl {
    return this.form.get('inputs.isUserDisabled') as FormControl;
  }
  get employeeControl(): FormControl {
    return this.form.get('ui.employee') as FormControl;
  }

  get isUserDisabled(): boolean {
    return (
      this.userDisabledControl && !!this.userDisabledControl.value
    );
  }
  get isAccountEnabled(): FormControl {
    return this.form.get('inputs.isAccountEnabled') as FormControl;
  }

  get passwords(): FormGroup {
    return this.form.get('inputs.passwords') as FormGroup;
  }

  get passwordControl(): FormControl {
    return this.form.get('inputs.passwords.password') as FormControl;
  }

  get confirmPasswordControl(): FormControl {
    return this.form.get('inputs.passwords.verifyPassword') as FormControl;
  }

  get tempAccess(): FormGroup {
    return this.form.get('inputs.tempAccess') as FormGroup;
  }
  //#endregion

  //#region UI view controls
  private destroy$ = new Subject();
  isLoadingUser = false;
  isCheckingUsername = false;
  showAppTracking = false;
  showTimeAtt = false;
  // enabled by default until we find out the user is a supervisor and
  // that we might need to restrict this. the business logic is in the
  // user-profile service.
  userSupCanEnableEmps = true;
  //#endregion

  lastUserProfile: UserProfile;
  get isLoading(): boolean {
    return ((<unknown>this.form) as any).isLoading;
  }
  set isLoading(value: boolean) {
    ((<unknown>this.form) as any).isLoading = !!value;
  }
  isAddUserView = false;

  //#region CONSTANTS
  readonly timeoutOptions = [
    { key: 'Default', value: null, units: '' },
    { key: 1, value: 1, units: 'minutes' },
    { key: 2, value: 2, units: 'minutes' },
    { key: 3, value: 3, units: 'minutes' },
    { key: 4, value: 4, units: 'minutes' },
    { key: 5, value: 5, units: 'minutes' },
    { key: 10, value: 10, units: 'minutes' },
    { key: 15, value: 15, units: 'minutes' },
    { key: 20, value: 20, units: 'minutes' },
    { key: 25, value: 25, units: 'minutes' },
    { key: 30, value: 30, units: 'minutes' },
    { key: 'No Limit', value: 0, units: '' },
  ];

  userTypeOptions = [
    { key: 'System Administrator', value: 1, hidden: false },
    { key: 'Company Administrator', value: 2, hidden: false },
    { key: 'Supervisor', value: 4, hidden: false },
    { key: 'Employee', value: 3, hidden: false },
  ];

  readonly ut = {
    sa: 1, // system admin
    ca: 2, // company admin
    ee: 3, // employee
    sup: 4, // supervisor
  };

  readonly validationMessages = {
    password: [
      { type: 'required', message: 'Please enter a password' },
      {
        type: 'minlength',
        message: 'Password must be at least 8 characters long',
      },
      {
        type: 'maxlength',
        message: 'Password must be less than 50 characters long',
      },
      {
        type: 'pattern',
        message:
          'Your password must contain at least one uppercase, one lowercase, and one number',
      },
    ],
    verifyPassword: [
      { type: 'required', message: 'Please confirm your password' },
      { type: 'areEqual', message: 'Please make sure your passwords match' },
    ],
    username: [
      {
        type: 'minlength',
        message: 'Please enter a username that is 4-15 characters',
      },
      {
        type: 'maxlength',
        message: 'Please enter a username that is 4-15 characters',
      },
      {
        type: 'unique',
        message: 'Username already exists. Please enter a different username',
      },
      { type: 'required', message: 'Please enter a username' },
    ],
    firstName: [{ type: 'required', message: 'Please enter a first name' }],
    lastName: [{ type: 'required', message: 'Please enter a last name' }],
    employee: [],
  };

  readonly pwValidators = {
    password: Validators.compose([
      Validators.minLength(8),
      Validators.maxLength(50),
      Validators.required,
      Validators.pattern(
        '^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[a-zA-Z0-9]+$'
      ),
    ]),
    verifyPassword: [Validators.required],
    passwords: (fg: FormGroup) => {
      return PasswordValidator.areEqual(fg);
    },
  }
  //#endregion

  mapper = (profile: UserProfile): AutocompleteItem => {
    return {
      display: profile.displayName,
      value: profile,
    };
  };

  formValueCache: { [key: string]: any } = {};
  userDetail$: Subscription;
  supAccess$: Subscription;

  siteConfiguration$ = this.service
    .getSiteConfigurations()
    .pipe(map((x) => (!!x && !!x.length ? x[0] : null)));

  constructor(
    private account: AccountService,
    private fb: FormBuilder,
    private service: UserProfileService,
    private dialog: MatDialog,
    private msg: MessageService,
    private bo: BreakpointObserver,
    private confirm: ConfirmDialogService,
    private uu: FormValidatorService,
  ) {}

  ngOnInit(): void {
    this.inputsForm.valueChanges.subscribe(() => console.dir(this.inputsForm));
    this.populateUserInformationDropdowns();

    this.service.lastUserProfile$
      .pipe(takeUntil(this.destroy$))
      .subscribe((prof) => {
        this.lastUserProfile = prof;
        this.setPwValidatorsOnUserSelection(!!this.lastUserProfile);
      });

    this.service.isLoading$
      .pipe(takeUntil(this.destroy$))
      .subscribe((isLoading) => {
        if (isLoading) {
          this.isLoading = true;
        } else {
          this.isLoading = false;
        }
      });

    this.service.isAddUserview$
      .pipe(takeUntil(this.destroy$))
      .subscribe((isAddUserView) => {
        if (!!isAddUserView) {
          this.resetForm();
        }

        this.userTypeControl.setValue(UserType.employee);
        this.isAddUserView = isAddUserView;
      });

    this.service.cancelForm$
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => this.cancel());

    const getSupervisorAccess = () => iif(
      () => this.me.isRole(UserType.supervisor),
        this.service.getSupervisorAccess(this.me.userId).pipe(
          tap((access) => {
            this.meSupAccess = access;
          })
        ),
        of(null)
    );

    this.account
      .getUserInfo()
      .pipe(
        takeUntil(this.destroy$),
        tap((user) => (this.me = user)),
        switchMap(getSupervisorAccess),
        tap(() => {
          this.resetUserTypeOptions(true);
          this.service.userSupCanEnableEmps$
            .pipe(takeUntil(this.destroy$))
            .subscribe((canEnable) => (this.userSupCanEnableEmps = canEnable));
        }),
        switchMap(() => this.service.includeTerminatedSearch$),
        switchMap((includeTerminated) =>
          this.service.loadUsersAndEmployees(
            this.me.selectedClientId(),
            includeTerminated
          )
        )
      )
      .subscribe();

    this.listenForUserInformationChanges();
    this.listenForTempAccessChanges();
  }

  //#region UI functions

  selectedUserDisplay(option: UserProfile) {
    return option && option.displayName;
  }

  selectedEmployeeDisplay(employee: EmployeeBasic): string {
    return employee && `${employee.firstName} ${employee.lastName}`;
  }

  selectedUserUserTypeIsNot(...userTypes: number[]): boolean {
    let result = true;
    if (!this.userTypeControl) return false;

    userTypes.forEach((userType) => {
      if (!result) return;
      result = this.userTypeControl.value != userType;
    });
    return result;
  }

  selectedUserUserTypeIs(...userTypes: number[]): boolean {
    if (!this.userTypeControl || !userTypes) return false;

    const formValue = +this.userTypeControl.value;

    if (!formValue && this.isAddUserView) return true;

    return userTypes.includes(formValue);
  }

  /**
   * When the selected user's type matches the `selectedUserType` then it will
   * evaluate whether the selected user's user type matches the usertypes passed
   * from the template.
   *
   * You can pass a third parameter boolean that determines whether the
   *
   * @param selectedUserType
   * @param userTypes
   * @param exclusive @default false
   * @returns boolean
   */
  viewerUserTypes(
    selectedUserType: number,
    userTypes: number[],
    exclusive = false
  ): boolean {
    if (
      !this.lastUserProfile ||
      this.lastUserProfile.userType != selectedUserType
    ) {
      return !exclusive;
    }

    return (
      userTypes &&
      this.me &&
      userTypes.includes(this.me.userTypeId) &&
      this.lastUserProfile.userType == selectedUserType
    );
  }

  resetPassword() {
    this.confirm
      .open({
        title: 'Reset Password',
        message:
          "Please confirm you would like to reset the user's password. This process cannot be undone.",
        confirm: 'OK',
      })
      .afterClosed()
      .pipe(
        filter((x) => !!x),
        switchMap(() =>
          this.service.sendPasswordResetEmail(this.userControl.value.userId)
        )
      )
      .subscribe(
        (res) => {
          if (res) {
            this.msg.setSuccessMessage(
              'Reset password email sent successfully!'
            );
          }
        },
        (err) => this.msg.setErrorResponse(err)
      );
  }

  disableUser() {
    this.setFormUserDisabledState(!this.isUserDisabled);

    const user = this.prepareModel();
    const dto: UpdateUserProfileAccountDisableRequest = {
      userId: user.userId,
      isDisabled: user.isUserDisabled,
    };

    this.service
      .setDisabledState(this.me.selectedClientId(), dto.userId, dto)
      .subscribe(
        () => {
          this.msg.setSuccessMessage('User access has been updated!');
        },
        (err) => {
          this.msg.setErrorResponse(err);
        }
      );
  }

  private setFormUserDisabledState(isDisabled: boolean = true) {
    this.userDisabledControl.setValue(isDisabled, { emitEvent: false });

    if (isDisabled) {
      this.form.disable({ emitEvent: false });
    } else {
      this.form.enable({ emitEvent: false });
    }
    this.userControl.enable();
  }

  unlockAccount() {
    this.isAccountEnabled.setValue(true);

    // send it
    const userDto = this.prepareModel();
    const dto: UpdateUserProfileAccountDisableRequest = {
      userId: userDto.userId,
      isAccountEnabled: userDto.isAccountEnabled,
    };

    this.service
      .setAccountStatus(this.me.selectedClientId(), dto.userId, dto)
      .subscribe(
        () => this.msg.setSuccessMessage('User account unlocked successfully!'),
        (err) => this.msg.setErrorResponse(err)
      );
  }

  openCompanyAccessDialog() {
    if (this.me.userTypeId !== this.ut.sa) return;

    const isDesktop = this.bo.isMatched(Breakpoints.Web);
    const width = isDesktop ? '50vw' : '100vw';

    // make api call and get available companies
    this.service
      .getCompanyAdminAccess(this.userControl.value.userId)
      .pipe(
        switchMap((access) =>
          this.dialog
            .open(CompanyAccessDialogComponent, {
              data: { access },
              width,
            })
            .afterClosed()
        )
      )
      .subscribe((result) => {
        if (!result) return;
        this.msg.setSuccessMessage('Saved company access successfully!');
      });
  }

  openLoginHelpDialog() {
    this.dialog.open(UserProfileLoginHelpDialog, {
      maxWidth: '80vw',
    });
  }

  focusOnAutocomplete() {
    this.formValueCache = this.form.getRawValue();
    this.userControl.setValue(null);
  }

  preventEnterKey(event) {
    event.preventDefault();
    event.stopPropagation();
  }

  focusOffAutocomplete() {
    const formControl = this.form.get('ui.user');

    if (
      !this._isObject(this.userControl.value) &&
      this._isObject(this.formValueCache['ui']['user'])
    ) {
      formControl.setValue(this.formValueCache['ui']['user']);
    }
  }

  focusOnEmpAc() {
    this.formValueCache = this.form.getRawValue();
    this.employeeControl.setValue(null);
  }

  focusOffEmpAc() {
    if (
      !IsObject(this.employeeControl.value) &&
      IsObject(this.formValueCache['ui']['employee'])
    ) {
      this.employeeControl.setValue(this.formValueCache['ui']['employee']);
    }
  }

  notSelfView(): boolean {
    return !(
      this.lastUserProfile &&
      this.me &&
      this.me.userId &&
      this.lastUserProfile.userId === this.me.userId
    );
  }

  /**
   * Check to make sure that the user isn't a Supervisor who is viewing themselves.
   */
  notSupSelfView(): boolean {
    return !(
      this.lastUserProfile &&
      this.me &&
      this.me.userId &&
      this.me.userTypeId === UserType.supervisor &&
      this.lastUserProfile.userId === this.me.userId
    );
  }

  isUserTypeViewingSelf(type: UserType): boolean {
    const result = this.isAddUserView || (
      this.lastUserProfile &&
      this.me &&
      this.me.userId &&
      this.me.userTypeId === type &&
      this.lastUserProfile.userId === this.me.userId
    );
    return result;
  }

  notCASelfView(): boolean {
    return !(
      this.lastUserProfile &&
      this.me &&
      this.me.userId &&
      this.me.userTypeId === UserType.companyAdmin &&
      this.lastUserProfile.userId === this.me.userId
    );
  }

  notSASelfView(): boolean {
    return !(
      this.lastUserProfile &&
      this.me &&
      this.me.userId &&
      this.me.userTypeId === UserType.systemAdmin &&
      this.lastUserProfile.userId === this.me.userId
    );
  }

  meResetPws(): boolean {
    return (
      (this.me &&
        this.me.isRole(UserType.systemAdmin, UserType.companyAdmin)) ||
      (this.me &&
        this.me.userTypeId === UserType.supervisor &&
        this.meSupAccess &&
        this.meSupAccess.canSendPasswords)
    );
  }

  //#endregion

  /**
   * Check our user's permission to make sure they're allowed to create System Admins
   */
  private resetUserTypeOptions(emitEvent: boolean = false) {
    this.userTypeOptions.forEach((opt) => {
      if (opt.value === 1) {
        // we leave the System Administrator option, but disable the field so the user cannot change it
        if (this.lastUserProfile && this.lastUserProfile.userId == this.me.userId) {
          opt.hidden = false;
          this.inputsForm.get('userType').disable({ emitEvent });
        } else {
          opt.hidden = !(this.me.userTypeId == UserType.systemAdmin && this.me.isAllowedToAddSystemAdmin);

          this.inputsForm.get('userType').disabled && this.inputsForm.get('userType').enable({ emitEvent });
        }
      } else if (this.lastUserProfile && this.lastUserProfile.userId == this.me.userId && opt.value == this.me.userTypeId) {
        opt.hidden = false;
        this.inputsForm.get('userType').disable({ emitEvent });
      }
    });
  }

  private _isObject(target): boolean {
    return typeof target === 'object' && target != null;
  }

  private resetFormGroup(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach((control: any) => {
      control.reset();
      if (control.controls) {
        this.resetFormGroup(control);
      }
    });
  }

  private loadUserProfileDetail(userId: number) {
    if (!this.lastUserProfile || this.lastUserProfile.userId != userId) {
      this.lastUserProfile = null;
      this.isLoadingUser = true;
      this.employeeControl.reset();
      this.resetFormGroup(this.inputsForm);

      // user is a supervisor viewing themselves
      if (!this.notSupSelfView()) {
        this.service.setAddUserViewState(false);
      }

      this.userDetail$ && this.userDetail$.unsubscribe();
      this.userDetail$ = this.service
        .getUserProfileSecurityInformation(userId)
        .subscribe((user) => {
          this.userDetail$.unsubscribe();
          user = {
            ...user,
            displayName: this.userControl.value.displayName,
            viewEmployeesType: `${user.viewEmployeesType}`,
            viewRatesType: `${user.viewRatesType}`,
          };

          this.service.lastUserProfile$.next(user);
          this.patchUserForm(user, false);

          if (user.employee)
            this.employeeControl.setValue(user.employee, { emitEvent: false });

          if (user.userType == UserType.systemAdmin) {
            this.form
              .get('inputs.isApplicantTrackingAdmin')
              .setValue(true, { emitEvent: false });
          }

          user.isAccountEnabled
            ? this.form
                .get('inputs.isAccountEnabled')
                ?.disable({ emitEvent: false })
            : this.form
                .get('inputs.isAccountEnabled')
                ?.enable({ emitEvent: false });

          user.userId == this.me.userId
            ? this.form.get('inputs.userType').disable({ emitEvent: false })
            : this.form.get('inputs.userType').enable({ emitEvent: false });

          user.isUserDisabled
            ? this.inputsForm.disable({ emitEvent: false })
            : this.inputsForm.enable({ emitEvent: false });

          this.resetUserTypeOptions();

          this.form.updateValueAndValidity();
          this.isLoadingUser = false;
        });
    }
  }

  selectedUserIsMe(): boolean {
    return (
      this.me &&
      this.userControl &&
      this.userControl.value &&
      this.userControl.value.userId === this.me.userId
    );
  }

  private searchUsers(searchText: string) {
    searchText =
      searchText === undefined || searchText === null ? '' : searchText;

    // WHAT TO DO WHEN THE USER STARTS TYPING INTO THE AUTOCOMPLETE
    const cleanSearch = searchText.replace(' ', '').toLowerCase();
    const currentUser = this.userControl.value;

    // search by first name, last name and username
    const filteredUsers = this.allUsers.filter(
      (x) =>
        (currentUser && x.userId == currentUser.userId) ||
        x.firstName.replace(' ', '').toLowerCase().includes(cleanSearch) ||
        x.lastName.replace(' ', '').toLowerCase().includes(cleanSearch) ||
        x.username.replace(' ', '').toLowerCase().includes(cleanSearch)
    );

    this._availableUsers$.next(filteredUsers);
  }

  private searchEmployees(searchText: string) {
    searchText =
      searchText === undefined || searchText === null ? '' : searchText;

    const cleanSearch = searchText.replace(' ', '').toLowerCase();
    const currentEmp = this.employeeControl.value as EmployeeBasic;

    const filteredEmps = this.allEmployees.filter(
      (x) =>
        (currentEmp && x.employeeId == currentEmp.employeeId) ||
        x.firstName.replace(' ', '').toLowerCase().includes(cleanSearch) ||
        x.lastName.replace(' ', '').toLowerCase().includes(cleanSearch)
    );

    this._availableEmployees$.next(filteredEmps);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private saveNewUserForm() {
    const userInput = this.inputsForm.getRawValue();
    userInput.employeeId = this.form.value.ui.employee
      ? this.form.value.ui.employee.employeeId
      : null;
    const user = new NewUserRequest(userInput);

    this.account
      .getUserInfo()
      .pipe(
        switchMap((loggedInUser) => {
          user.clientId = user.clientId || loggedInUser.selectedClientId();
          user.dsModifiedBy = loggedInUser.userId;

          return this.service
            .saveNewUser(user)
            .pipe(
              catchHttpError(),
              tap((id: number) => user.dsUserId = id),
              this.checkForUserPin(user, userInput.userPin),
            );
        }),
        alertAndRethrow(),
        map(userId => {
          if (userId > 0) {
            const displayUser = {
              displayName: user.lastName + ', ' + user.firstName,
              isUserDisabled: false,
              userId,
              username: user.username,
            } as UserProfile;

            switch (user.dsUserType) {
              case UserType.systemAdmin:
                displayUser.displayName = `A - ${displayUser.displayName}`;
                break;
              case UserType.companyAdmin:
                displayUser.displayName = `C - ${displayUser.displayName}`;
                break;
              case UserType.supervisor:
                displayUser.displayName = `S - ${displayUser.displayName}`;
                break;
              case UserType.employee:
                displayUser.displayName = `E - ${displayUser.displayName}`;
                break;
              default:
                break;
            }

            this.allUsers.push(displayUser);
            this.allUsers = this.allUsers.sort((a, b) => a.displayName.localeCompare(b.displayName));
            this._availableUsers$.next(this.allUsers);

            return displayUser;
          }

          return null;
        })
      )
      .subscribe((user) => {
        if (!!user) {
          this.msg.setSuccessMessage('The user has been saved successfully.');
          // clears the form
          this.cancel();

          this.userControl.setValue(user);
          this.loadUserProfileDetail(this.userControl.value.userId);
        }
      });
  }

  private checkForUserPin(user: NewUserRequest, userPin: string) {
    return tap(() => {
      if (!!user.dsUserId && user.dsUserType == UserType.companyAdmin && !!userPin) {
        const dto = {
          userId: user.dsUserId,
          clientId: user.clientId,
          pin: userPin,
        } as UserPin;

        this.service.saveCAUserPin(dto).subscribe(
          () => {},
          (err) => console.error(err, 'error')
        );
      }
    });
  }

  private updateExistingUserForm() {
    const user = new NewUserRequest(this.inputsForm.value);
    this.msg.loading(true, 'Saving...');

    this.account.getUserInfo().pipe(
      tap(u => user.clientId = u.selectedClientId()),
      switchMap(() => this.service.updateUser(user)),
      alertAndRethrow(),
      this.checkForUserPin(user, this.inputsForm.value.userPin),
    ).subscribe(() => {
      this.msg.setSuccessMessage('Successfully updated your user!');

      let selectedDisplayUser = null;
      this.allUsers = this.allUsers.map(u => {
        if (u.userId === user.dsUserId) {
          let displayUser = {...u};
          selectedDisplayUser = displayUser;
          return displayUser;
        }
        return u;
      });

      this._availableUsers$.next(this.allUsers);
      this.cancel();
      this.userControl.setValue(selectedDisplayUser);
      this.loadUserProfileDetail(this.userControl.value.userId);
    });
  }

  saveForm() {
    this.frmSubmitted = true;
    this.form.get('inputs').markAllAsTouched();

    if (this.form.invalid) {
      console.log('Form is inavlid');
      return;
    }

    if (!this.form.value.ui.user) {
      this.saveNewUserForm();
    } else {
      this.updateExistingUserForm();
    }
  }

  /**
   * This resets the form completely and removes the cached user from the history.
   * We use this when clicking the cancel button on the form.
   */
  resetForm() {
    this.inputsForm.enable({ emitEvent: false });
    this.service.lastUserProfile$.next(null);
    this.form.reset();
  }

  cancel() {
    this.resetForm();
    this.service.setAddUserViewState(false);
  }

  isObject(value: any): boolean {
    return IsObject(value);
  }

  inputForm(key: string): FormControl {
    if (!key) return null;
    return this.form.get(`inputs.${key}`) as FormControl;
  }

  private prepareModel(): UserProfile {
    const fv = this.form.getRawValue().inputs;
    return {
      userId: fv.userId || 0,
      userType: fv.userType,
      firstName: fv.firstName,
      lastName: fv.lastName,
      username: fv.username,
      employeeId: fv.employeeId,
      employeeStatusType: fv.employeeStatusType,
      password: fv.passwords.password,
      verifyPassword: fv.passwords.verifyPassword,
      forceUserPasswordReset: fv.forceUserPasswordReset,
      email: fv.email,
      userPin: fv.userPin,
      isAccountEnabled: fv.isAccountEnabled,
      isUserDisabled: fv.isUserDisabled,
      hasEmployeeAccess: fv.hasEmployeeAccess,
      sessionTimeout: fv.sessionTimeout,
      isEssViewOnly: fv.isEssViewOnly,
      isApplicantTrackingAdmin: fv.isApplicantTrackingAdmin,
      viewEmployeesType: fv.viewEmployeesType,
      viewRatesType: fv.viewRatesType,
      hasEssSelfService: fv.hasEssSelfService,
      isReportingAccessOnly: fv.isReportingAccessOnly,
      blockPayrollAccess: fv.blockPayrollAccess,
      isEmployeeNavigatorAdmin: fv.isEmployeeNavigatorAdmin,
      blockHr: fv.blockHr,
      hasGLAccess: fv.hasGLAccess,
      hasTaxPacketsAccess: fv.hasTaxPacketsAccess,
      hasTimeAndAttAccess: fv.hasTimeAndAttAccess,
      isTimeclockAppOnly: fv.isTimeclockAppOnly,
    };
  }

  /**
   * This method takes a boolean verifying if the form has a valid existing user selected
   * and then appropriately sets / unsets the form's validators on the password fields.
   *
   * (only intended to be used reactively after user selection)
   *
   * @param clearValidators
   */
  private setPwValidatorsOnUserSelection(clearValidators: boolean) {
    if (clearValidators) {
      this.passwords.get('password').clearValidators();
      this.passwords.get('verifyPassword').clearValidators();
      this.passwords.clearValidators();
    } else {
      this.passwords.get('password').setValidators(this.pwValidators.password);
      this.passwords.get('verifyPassword').setValidators(this.pwValidators.verifyPassword);
      this.passwords.setValidators(this.pwValidators.passwords);
    }
  }

  private onPasswordChange(event, key: string) {
    if (!this.lastUserProfile) return;
    const oppKey = key === 'password' ? 'verifyPassword' : 'password';
    const value = event.target.value;
    const oppValue = this.passwords.get(oppKey)?.value;

    if (value && value.length || oppValue && oppValue.length) {
      this.passwords.get(key).setValidators(this.pwValidators[key]);
      this.passwords.get(oppKey).setValidators(this.pwValidators[oppKey]);
      this.passwords.setValidators(this.pwValidators.passwords);
    } else {
      this.passwords.get(key).clearValidators();
      this.passwords.get(oppKey).clearValidators();
      this.passwords.clearValidators();
    }

    this.passwords.get(key).updateValueAndValidity({emitEvent: false});
    this.passwords.get(oppKey).updateValueAndValidity({emitEvent: false});
    this.passwords.updateValueAndValidity();
  }

  private listenForTempAccessChanges() {
    this.inputForm('hasTempAccess').valueChanges.pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        const hasTempAccess = this.inputForm('hasTempAccess').value;

        if (hasTempAccess) {
          this.tempAccess.get('fromDate').setValidators([Validators.required]);
          this.tempAccess.get('toDate').setValidators([Validators.required]);
          this.tempAccess.get('fromDate').updateValueAndValidity({emitEvent: false});
          this.tempAccess.get('toDate').updateValueAndValidity({emitEvent: false});
        } else {
          this.tempAccess.setValue({
            fromDate: null,
            toDate: null,
          }, {emitEvent: false});
          this.tempAccess.get('fromDate').clearValidators();
          this.tempAccess.get('toDate').clearValidators();
          this.tempAccess.get('fromDate').updateValueAndValidity({emitEvent: false});
          this.tempAccess.get('toDate').updateValueAndValidity({emitEvent: false});
        }
      });
  }

  private listenForUserInformationChanges() {
    this.userControl.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        // user selected someone from the list
        if (IsObject(this.userControl.value)) {
          this.loadUserProfileDetail(this.userControl.value.userId);
        } else {
          this.searchUsers(this.userControl.value);
        }
      });

    this.employeeControl.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        const employee = this.employeeControl.value;

        if (!!employee && IsObject(employee)) {
          this.form.patchValue({
            inputs: { employeeStatusType: employee.employeeStatusType },
          });
        } else {
          this.searchEmployees(this.employeeControl.value);
        }
      });

    // changes that need to happen based on user type changes
    this.inputForm('userType')
      .valueChanges.pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        const userType = this.inputForm('userType').value;

        if (userType == UserType.supervisor || userType == UserType.employee) {
          this.validationMessages.employee.push({
            type: 'required',
            message: 'Employee assignment is required',
          });
          this.employeeControl.setValidators([Validators.required]);
          this.employeeControl.updateValueAndValidity();

          if (userType == UserType.employee) {
            this.inputsForm.patchValue({
              viewEmployeesType: 4,
              viewRatesType: 4,
            });
          }
        } else {
          this.validationMessages.employee = [];
          this.employeeControl.clearValidators();
          this.employeeControl.updateValueAndValidity();
        }

        if (userType == UserType.systemAdmin) {
          this.inputForm('isApplicantTrackingAdmin').setValue(true);
        }
      });

    this.inputForm('username')
      .statusChanges.pipe(
        takeUntil(this.destroy$),
        startWith(this.inputForm('username').status)
      )
      .subscribe((status) => (this.isCheckingUsername = status === 'PENDING'));
  }

  private populateUserInformationDropdowns() {
    this.service.assignableUsers$
      .pipe(
        takeUntil(this.destroy$),
        tap((users) => {
          this.allUsers = users;
          this._availableUsers$.next(users);
        })
      )
      .subscribe();

    this.service.assignableEmployees$
      .pipe(
        takeUntil(this.destroy$),
        tap((emps) => {
          this.allEmployees = emps;
          this._availableEmployees$.next(emps);
        })
      )
      .subscribe();
  }

  private patchUserForm(user: UserProfile, emitEvent: boolean = true) {
    this.inputsForm.patchValue({
      userId: user.userId,
      userType: user.userType,
      firstName: user.firstName,
      lastName: user.lastName,
      username: user.username,
      employeeId: user.employeeId,
      employeeStatusType: user.employeeStatusType,
      passwords: {
        password: user.password,
        verifyPassword: user.verifyPassword,
      },
      forceUserPasswordReset: user.forceUserPasswordReset,
      email: user.email,
      userPin: user.userPin,
      isAccountEnabled: user.isAccountEnabled,
      isUserDisabled: user.isUserDisabled,
      hasTempAccess: user.hasTempAccess,
      tempAccess: {
        fromDate: user.fromDate,
        toDate: user.toDate,
      },
      hasTimeAndAttAccess: user.hasTimeAndAttAccess,
      hasEmployeeAccess: user.hasEmployeeAccess,
      hasGLAccess: user.hasGLAccess,
      hasTaxPacketsAccess: user.hasTaxPacketsAccess,
      isApplicantTrackingAdmin: user.isApplicantTrackingAdmin,
      isEmployeeNavigatorAdmin: user.isEmployeeNavigatorAdmin,
      isEssViewOnly: user.isEssViewOnly,
      isReportingAccessOnly: user.isReportingAccessOnly,
      isTimeclockAppOnly: user.isTimeclockAppOnly,
      sessionTimeout: user.sessionTimeout,
      viewEmployeesType: user.viewEmployeesType,
      viewRatesType: user.viewRatesType,
      blockPayrollAccess: user.blockPayrollAccess,
      blockHr: user.blockHr,
    }, {emitEvent});
  }

  private createForm(): FormGroup {
    const validatePassRegex = /^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,50}$/;
    return this.fb.group({
      // should only be used in the UI for the autocomplete
      ui: this.fb.group({
        user: this.fb.control(''), // should only be used in the UI for the autocomplete
        employee: this.fb.control(''),
      }),
      inputs: this.fb.group({
        userId: this.fb.control(''),
        userType: this.fb.control('', [Validators.required]),
        firstName: this.fb.control('', [Validators.required]),
        lastName: this.fb.control('', [Validators.required]),
        username: this.fb.control('', {
          updateOn: 'blur',
          validators: [
            Validators.required,
            Validators.minLength(4),
            Validators.maxLength(15),
          ],
          asyncValidators: this.uu.validate(true),
        }),
        employeeId: this.fb.control(''),
        employeeStatusType: this.fb.control({ value: '', disabled: true }),
        passwords: this.fb.group(
          {
            password: this.fb.control('', {
              validators: Validators.compose([
                Validators.minLength(8),
                Validators.maxLength(50),
                Validators.required,
                Validators.pattern(
                  '^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[a-zA-Z0-9]+$'
                ),
              ]),
              updateOn: 'blur',
            }),
            verifyPassword: this.fb.control('', {
              validators: [Validators.required],
              updateOn: 'blur',
            }),
          },
          {
            validators: (fg: FormGroup) => {
              return PasswordValidator.areEqual(fg);
            },
          }
        ),
        forceUserPasswordReset: this.fb.control(false),
        email: this.fb.control(''),
        userPin: this.fb.control(''),
        isAccountEnabled: this.fb.control(false),
        isUserDisabled: this.fb.control(false),
        hasTempAccess: this.fb.control(false), // only available if account is enabled
        tempAccess: this.fb.group({
          fromDate: this.fb.control(''),
          toDate: this.fb.control(''),
        }),
        sessionTimeout: this.fb.control(''),
        isEssViewOnly: this.fb.control(false),
        isApplicantTrackingAdmin: this.fb.control(false),
        viewEmployeesType: this.fb.control('4'),
        viewRatesType: this.fb.control('4'),
        hasEssSelfService: this.fb.control(false),
        isReportingAccessOnly: this.fb.control(false),
        blockPayrollAccess: this.fb.control(false),
        isEmployeeNavigatorAdmin: this.fb.control(false),
        blockHr: this.fb.control(false),
        hasEmployeeAccess: this.fb.control(false),
        hasGLAccess: this.fb.control(false),
        hasTaxPacketsAccess: this.fb.control(false),
        hasTimeAndAttAccess: this.fb.control(false),
        isTimeclockAppOnly: this.fb.control(false),
      }),
    });
  }
}

@Component({
  selector: 'ds-login-help-dialog',
  template: `
    <div mat-dialog-title>User Login Access vs Disabled Users</div>
    <div mat-dialog-content>
      <p>
        <span class="bold">User Login Access: </span>
        This control gives us visibility into the authentication process and
        tells us the current status of the user's ability to login to Dominion
        products & services. This access cannot be manually revoked, but is
        controlled by the authentication system. If a user is locked out and
        cannot reset their password an admin can assist them by unlocking their
        account by re-enabling this setting from this page.
      </p>

      <p>
        <span class="bold">Disabling Users: </span>
        The "Disable User" button does exactly as it says. It disables and locks
        the user's account and stops them from being able to login. This also
        locks their user profile and the user record cannot be edited until they
        are re-enabled. This is a security feature provided to admins to assist
        enabling/disabling manually but does not interact with the
        authentication system directly.
      </p>
    </div>
    <div mat-dialog-actions>
      <button mat-button mat-dialog-close>Close</button>
    </div>
  `,
})
export class UserProfileLoginHelpDialog {}

enum UserTypeChangeType {
  NoChange,
  ToSupervisor,
  FromSupervisor,
}
