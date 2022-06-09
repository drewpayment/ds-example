import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { ClientOption, ClientService } from '@ds/core/clients/shared';
import { SiteConfiguration, UserInfo, UserType } from '@ds/core/shared';
import {
  EmployeeBasic,
  UserSupervisorAccessInfo,
  UpdateUserProfileAccountDisableRequest,
  UserProfile,
  NewUserRequest,
  UserPin,
} from '@models';
import { UserClientAccess } from '@models/user-client-access.model';
import { UserProfileSearchFilters } from '@models/user-profile-search-filters.model';
import {
  BehaviorSubject,
  combineLatest,
  forkJoin,
  iif,
  Observable,
  of,
  Subject,
} from 'rxjs';
import { map, switchMap, takeUntil, tap } from 'rxjs/operators';

@Injectable()
export class UserProfileService {
  private api = `api/employees`;
  private destroy$ = new Subject();
  private _user = new BehaviorSubject<UserInfo>(null);
  private _userSupervisorAccess = new BehaviorSubject<UserSupervisorAccessInfo>(
    null
  );
  private _assignableEmployees = new BehaviorSubject<EmployeeBasic[]>([]);
  private _assignableUsers = new BehaviorSubject<UserProfile[]>([]);

  private _includeTerminatedSearch = new BehaviorSubject<boolean>(false);
  private _isLoading$ = new BehaviorSubject<boolean>(true);
  private _isAddUserView$ = new BehaviorSubject<boolean>(false);
  private _cancelForm$ = new Subject<void>();
  private _userSupCanEnableEes$ = new BehaviorSubject<boolean>(false);

  get isAddUserview$(): Observable<boolean> {
    return this._isAddUserView$.asObservable();
  }

  get cancelForm$(): Observable<void> {
    return this._cancelForm$.asObservable();
  }

  get isLoading$(): Observable<boolean> {
    return this._isLoading$.asObservable();
  }

  get user$(): Observable<UserInfo> {
    return this._user.asObservable();
  }

  get userSupervisorAccess$(): Observable<UserSupervisorAccessInfo> {
    return this._userSupervisorAccess.asObservable();
  }

  // this checks to make sure the currently logged in user viewing the user profile
  // page is a supervisor and that they are allowed to enable employees based on
  // the client options page option "Supervisors can enable employees"
  userSupCanEnableEmps$: Observable<boolean> = combineLatest([
    this._user,
    this.userSupervisorAccess$,
    this._userSupCanEnableEes$.asObservable(),
  ]).pipe(
    map(
      ([user, access, canEnable]) =>
        (!!user && user.isRole(UserType.systemAdmin, UserType.companyAdmin)) ||
        (!!user && user.isRole(UserType.supervisor) && access && canEnable)
    )
  );

  get assignableUsers$(): Observable<UserProfile[]> {
    return this._assignableUsers.asObservable();
  }

  get assignableEmployees$(): Observable<EmployeeBasic[]> {
    return this._assignableEmployees.asObservable();
  }

  get includeTerminatedSearch$(): Observable<boolean> {
    return this._includeTerminatedSearch.asObservable();
  }

  private _includeTerminated = false;
  get includeTerminated(): boolean {
    return this._includeTerminated;
  }

  lastUserProfile$ = new BehaviorSubject<UserProfile>(null);

  constructor(
    private account: AccountService,
    private http: HttpClient,
    private clientService: ClientService
  ) {
    this.account
      .getUserInfo()
      .pipe(
        takeUntil(this.destroy$),
        tap((user) => this._user.next(user))
      )
      .subscribe();
  }

  setFilters(filters: UserProfileSearchFilters) {}

  setLoadingState(isLoading: boolean) {
    this._isLoading$.next(!!isLoading);
  }

  setAddUserViewState(showAddUserView: boolean) {
    this._isAddUserView$.next(!!showAddUserView);
  }

  cancelForm() {
    this._cancelForm$.next();
  }

  setIncludeTerminated(value: boolean) {
    this._includeTerminatedSearch.next(!!value);
  }

  getUserProfileSecurityInformation(userId: number): Observable<UserProfile> {
    this.setLoadingState(true);
    return this.http
      .get<UserProfile>(`api/account/users/${userId}/profile-info`)
      .pipe(tap(() => this.setLoadingState(false)));
  }

  updateUserProfile(
    clientId: number,
    userId: number,
    dto: UserProfile
  ): Observable<UserProfile> {
    return this.http.put<UserProfile>(
      `api/account/clients/${clientId}/user-profiles/${userId}`,
      dto
    );
  }

  setAccountStatus(
    clientId: number,
    userId: number,
    dto: UpdateUserProfileAccountDisableRequest
  ): Observable<void> {
    return this.http.put<void>(
      `api/users-profiles/clients/${clientId}/users/${userId}/account-status`,
      dto
    );
  }

  setDisabledState(
    clientId: number,
    userId: number,
    dto: UpdateUserProfileAccountDisableRequest
  ): Observable<void> {
    return this.http.put<void>(
      `api/user-profiles/clients/${clientId}/users/${userId}/disabled`,
      dto
    );
  }

  loadUsersAndEmployees(
    clientId: number,
    includeTerminated: boolean = false
  ): Observable<void> {
    return combineLatest(
      this.getEmployeesByClient(clientId, !!includeTerminated),
      this.getUserProfilesByClient(clientId, !!includeTerminated)
    ).pipe(
      map(([employees, profiles]) => {
        return [
          employees,
          profiles.map((prof) => {
            const [lastName, firstName] = prof.displayName
              .split(',')
              .map((val) => val.trim());
            return { ...prof, firstName, lastName };
          }),
        ];
      }),
      map(([employees, profiles]: [EmployeeBasic[], UserProfile[]]) => {
        this._assignableEmployees.next(employees);
        this._assignableUsers.next(profiles);
        return;
      })
    );
  }

  getCompanyAdminAccess(userId: number): Observable<UserClientAccess[]> {
    return this.http.get<UserClientAccess[]>(
      `api/user-profiles/${userId}/company-access`
    );
  }

  saveCompanyAdminAccess(
    userId: number,
    access: UserClientAccess[]
  ): Observable<UserClientAccess[]> {
    return this.http.post<UserClientAccess[]>(
      `api/user-profiles/${userId}/company-access`,
      access
    );
  }

  sendPasswordResetEmail(userId: number): Observable<boolean> {
    return this.http.get<boolean>(`api/user-profiles/${userId}/reset`);
  }

  getSiteConfigurations(): Observable<SiteConfiguration[]> {
    return this.http.get<SiteConfiguration[]>(
      `api/user-profiles/auth-configuration`
    );
  }

  getSupervisorAccess(userId: number): Observable<UserSupervisorAccessInfo> {
    return this.http
      .get<UserSupervisorAccessInfo>(
        `api/access-rules/users/${userId}/supervisor-security`
      )
      .pipe(
        tap(() => {
          this.clientService
            .getClientAccountOption(
              ClientOption.Payroll_SupervisorsCanEnableEmployees
            )
            .subscribe((res) =>
              this._userSupCanEnableEes$.next(res && !!Math.abs(res.value))
            );
        })
      );
  }

  saveNewUser(user: NewUserRequest): Observable<number> {
    return this.http.post<number>(`api/user-profiles`, user);
  }

  updateUser(user: NewUserRequest): Observable<void> {
    return this.http.put<void>(`api/user-profiles/${user.dsUserId}`, user);
  }

  saveCAUserPin(pin: UserPin): Observable<UserPin> {
    return this.http.post<UserPin>(`api/user-profiles/${pin.userId}/user-pin`, pin);
  }

  destroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private getEmployeesByClient(
    clientId: number,
    includeTerminated: boolean
  ): Observable<EmployeeBasic[]> {
    return this.http.get<EmployeeBasic[]>(`${this.api}/clients/${clientId}`, {
      params: { includeTerminated: `${includeTerminated}` },
    });
  }

  // Only returns a subset of UserProfile with userId, displayName and username.
  private getUserProfilesByClient(
    clientId: number,
    includeTerminated: boolean = null
  ): Observable<UserProfile[]> {
    return this.http.get<UserProfile[]>(
      `api/account/clients/${clientId}/user-profiles`,
      {
        params: {
          includeTerminated:
            includeTerminated !== null ? `${includeTerminated}` : null,
        },
      }
    );
  }
}
