import { Injectable } from '@angular/core';
import {
  Observable,
  Subject,
  of,
  BehaviorSubject,
} from 'rxjs';
import {
  catchError,
  map,
  shareReplay,
  publishReplay,
  refCount,
  concatMap,
  tap,
  skipWhile,
  takeUntil,
} from 'rxjs/operators';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { UserInfo } from './shared/user-info.model';
import { IClientAccountFeatureDto } from '@ajs/labor/models/company-rules-dtos.model';
import * as moment from 'moment';
import { UserSettingsInput } from './employee-services/models';
import { SiteConfiguration, UserBetaFeature, IClientFeature } from './shared';
import { ConfigUrl, ConfigUrlType } from './shared/config-url.model';
import { BetaFeatureType } from '@ds/core/shared';
import { UserTermsAndConditionsDto } from './shared/user-terms-and-conditions.model';
import { ITermsAndConditionsVersion } from './shared/terms-and-conditions-version.model';
import { Timeout } from './shared/timeout.model';
import { ISystemFeedbackData } from './users/beta-features/menu-wrapper-toggle/toggle-feedback-dialog/toggle-feedback-dialog-data.model';
import { saveAs } from 'file-saver';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { prepareUserInfo } from 'lib/utilties/prepare-user-info.observable';
import { Store } from '@ngrx/store';
import { UserState } from './users/store/user.reducer';
import { ClearUser, UpdateUser } from './users/store/user.actions';
import { Features } from '@ds/admin/client-statistics/shared/models/featureEnum';
import { NgxMessageService } from './ngx-message/ngx-message.service';

export interface IActionNotAllowedRejection {
  reason: string;
  actionContext: any;
  actionsNotAllowed: string[];
}

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  static readonly NGX_SERVICE_NAME = 'ngxAccountService';

  private readonly api: string = 'api/account';
  private readonly anonapi: string = 'api/anonaccount';

  permissions: string[];
  permissions$: Subject<string[]> = new Subject<string[]>();
  cachedPermissions$: Observable<string[]>;
  private _serverTime$ = new BehaviorSubject<string>(null);
  serverTimeIntervalTask: NodeJS.Timer;

  tncItems$ = new BehaviorSubject<ITermsAndConditionsVersion[]>(null);

  private _betaFeatures: UserBetaFeature[] = [
    {
      betaFeatureId: BetaFeatureType.MenuWrapper,
      isBetaActive: false,
    } as UserBetaFeature,
  ];
  private betaFeatures$ = new BehaviorSubject<UserBetaFeature[]>(
    this._betaFeatures
  );
  get betaFeatures(): Observable<UserBetaFeature[]> {
    return this.betaFeatures$.asObservable();
  }

  _siteUrls$ = this.http
    .get<ConfigUrl[]>(this.anonapi + '/site-urls')
    .pipe(
      shareReplay(),
      map(sites => {
        sites.forEach(site => {
          site.url = site.url.replace(/\/?$/, '/');
        });
        return sites;
      }),
    );

  isUserInfoInflight = false;

  private _hideMenuWrapperToolip = false;
  private get _hideMenuTooltipStorage() {
    let result = this._hideMenuWrapperToolip;
    if (localStorage) {
      const item = localStorage.getItem('_smwt');

      if (item) result = coerceBooleanProperty(item);
    }
    return result;
  }

  private set _hideMenuTooltipStorage(value: boolean) {
    this._hideMenuWrapperToolip = coerceBooleanProperty(value);
    this._hideMenuWrapperTooltip$.next(this._hideMenuWrapperToolip);
    if (localStorage)
      localStorage.setItem('_smwt', `${this._hideMenuWrapperToolip}`);
  }

  private _hideMenuWrapperTooltip$ = new BehaviorSubject<boolean>(
    this._hideMenuTooltipStorage
  );

  get hideMenuWrapperTooltip$() {
    return this._hideMenuWrapperTooltip$.asObservable();
  }

  constructor(private http: HttpClient, private store: Store<UserState>, private msg: NgxMessageService) {}

  setHideMenuWrapperTooltip(value: boolean) {
    this._hideMenuTooltipStorage = value;
  }

  updateMenuWrapperTooltipVisibility(value: 'show' | 'hide') {
    const isHide = coerceBooleanProperty(value === 'hide');
    this._hideMenuWrapperTooltip$.next(isHide);
  }

  private userInfo$: Observable<UserInfo>;
  private reloadUserInfo$ = new Subject();

  private updateBetaFeatures = () =>
    this.betaFeatures$.next(this._betaFeatures);

  checkBetaFeatureIsActive(featureType: BetaFeatureType): boolean {
    const feature = this._betaFeatures.find(
      (bf) => bf.betaFeatureId === featureType
    );
    if (feature != null) {
      return feature.isBetaActive;
    }
    return false;
  }

  /**
   * Publicly exposed function to get the authenticated user. Can pass option to manually reload user,
   * but will also check if we have already received a user and use the cached user if we don't explicitly
   * ask for a new user to be loaded.
   */
  readonly getUserInfo: (
    reloadUser?: boolean,
    disableEmulation?: boolean
  ) => Observable<UserInfo> = (reloadUser?: boolean) => {
    if (reloadUser) {
      this.reloadUserInfo$.next();
      this.store.dispatch(new ClearUser());
      this.userInfo$ = null;
    }

    if (!this.userInfo$) {
      this.userInfo$ = this.requestUserInfo().pipe(
        prepareUserInfo(),
        takeUntil(this.reloadUserInfo$),
        shareReplay()
      );
    }

    return this.userInfo$;
  };

  private requestUserInfo(): Observable<UserInfo> {
    return this.http.get<UserInfo>(`${this.api}/userinfo`).pipe(
      tap((user) => {
        user.betaFeatures.forEach((ubf) => {
          const index = this._betaFeatures.findIndex(
            (bf) => bf.betaFeatureId === ubf.betaFeatureId
          );

          if (index > -1) {
            this._betaFeatures[index] = ubf;
          } else {
            this._betaFeatures.push(ubf);
          }
          this.updateBetaFeatures();
        });
        this.store.dispatch(new UpdateUser(user));
      })
    );
  }

  saveUserBetaFeature(dto: UserBetaFeature): Observable<UserBetaFeature> {
    const url = `api/account/userinfo/${dto.userId}/features`;
    return this.http.post<UserBetaFeature>(url, dto);
  }

  hasMenuWrapperFeature(): Observable<boolean> {
    return this.getUserInfo().pipe(
      map((user) => {
        const feature = user.betaFeatures.find(
          (b) => b.betaFeatureId === BetaFeatureType.MenuWrapper
        );
        return feature && feature.isBetaActive;
      })
    );
  }

  getServerTime(): Observable<string> {
    const url = `api/what-time-is-it`;

    this.http.get<string>(url).subscribe((time) => {
      this._serverTime$.next(time);
      if (time) this._startServerTimeUpdateTask();
    });

    return this._serverTime$.pipe(skipWhile((x) => x == null || x === ''));
  }

  private _startServerTimeUpdateTask() {
    this.serverTimeIntervalTask = setInterval(() => {
      const currentTime = this._serverTime$.getValue();
      if (!currentTime) return;
      const currentTimeMoment = moment(currentTime).add(1, 's');
      this._serverTime$.next(currentTimeMoment.format());
    }, 1000);
  }

  getUserProfileSettingsInfo(
    clientId: number,
    userId: number
  ): Observable<UserInfo> {
    const params = new HttpParams()
      .append('clientId', `${clientId}`)
      .append('userId', `${userId}`);

    return this.http.get<UserInfo>(this.api + '/user-profile', {
      params: params,
    });
  }

  saveUserProfileSettingsInfo(
    clientId: number,
    userId: number,
    input: UserSettingsInput
  ): Observable<UserSettingsInput> {
    const params = new HttpParams()
      .append('clientId', `${clientId}`)
      .append('userId', `${userId}`);
    return this.http.post<UserSettingsInput>(
      this.api + '/user-profile',
      input,
      { params: params }
    );
  }

  PassUserInfoToRequest<U>(
    fn: (userInfo: UserInfo) => Observable<U>
  ): Observable<U> {
    return this.getUserInfo().pipe(concatMap((x) => fn(x)));
  }

  /**
   * Returns the current user's allowed actions / permissions.
   */
  getAllowedActions(reloadPermissions?: boolean): Observable<string[]> {
    if (!this.cachedPermissions$ || reloadPermissions) {
      this.cachedPermissions$ = this.http
        .get<string[]>(this.api + '/permissions')
        .pipe(publishReplay(), refCount());
    }
    return this.cachedPermissions$;
  }

  /**
   * Checks if the user has access to perform the specfied actions / permissions.
   */
  canPerformActions(
    actions: string | string[],
    actionContextCaption?: any
  ): Observable<boolean | IActionNotAllowedRejection> {
    let result: boolean | IActionNotAllowedRejection;
    if (!(actions instanceof Array)) actions = <string[]>[actions];

    return this.getAllowedActions().pipe(
      map((allowedActions) => {
        const notAllowed = [];
        (<string[]>actions).forEach((action) => {
          if (!this.actionFound(allowedActions, action))
            notAllowed.push(action);
        });
        if (notAllowed.length) {
          result = {
            reason: 'Not Authorized',
            actionContext: actionContextCaption,
            actionsNotAllowed: notAllowed,
          };
        } else {
          result = true;
        }

        return result;
      })
    );
  }

  canPerformAction(action: string): Observable<boolean> {
    return this.canPerformActions(action)
      .pipe(
        map(result => {
          if (result !== null && typeof result === 'object') {
            return false;
          }
          return true;
        }),
      );
  }

  /**
   * Returns the legacy (DominionSource) header links the current user has access to.
   */
  getAccessibleLegacyLinks(): Observable<any> {
    return this.http.get(this.api + '/legacy-links');
  }

  getLegacyRootUrl(): Observable<string> {
    return this.http.get<string>(this.api + '/legacy-root');
  }

  getSiteUrls(): Observable<ConfigUrl[]> {
    return this._siteUrls$;
  }

  getSiteConfig(type: ConfigUrlType): Observable<ConfigUrl> {
    return this._siteUrls$.pipe(map((x) => x.find((y) => y.siteType === type)));
  }

  /**
   * Sends a feedback email message to dominion.
   */
  sendFeedback(subject: string, feedback: string): Observable<any> {
    return this.getUserInfo().pipe(
      concatMap((userInfo) => {
        let data = {
          userInfo: userInfo,
          subject: subject,
          feedback: feedback,
        };
        return this.http.post(this.api + '/send-feedback', data);
      })
    );
  }

  /**
   * Returns the current system-wide list of secret questions.
   */
  getSecretQuestions(): Observable<any> {
    return this.http.get(this.api + '/secret-questions');
  }

  /**
   * GETs the account settings for the current user. (Includes first/last name secret question info, etc)
   */
  getAccountSettings(): Observable<any> {
    return this.http
      .get(this.api + '/settings')
      .pipe(catchError(this.handleError('getAccountSettings', null)));
  }

  /**
   * Save changes to the current user account's settings (i.e. first/last name, secrect question info, etc)
   */
  saveAccountSettings(settings): Observable<any> {
    return this.http.put(this.api + '/settings', settings);
  }

  /**
   * Verifies the current user's password.
   */
  verifyPassword(
    userId: number,
    authUserId: number,
    pwd: string
  ): Observable<any> {
    let dto = {
      userId: userId,
      authUserId: authUserId,
      password: <any>window.btoa(pwd),
    };
    return this.http.post(this.api + '/password', dto);
  }

  /**
   * Verifies a new password for the user, based on the user's security.
   */
  verifyNewPassword(
    userId: number,
    authUserId: number,
    p1: string,
    p2: string
  ): Observable<any> {
    let dto = {
      userId: userId,
      authUserId: authUserId,
      password: <any>window.btoa(p1),
      password2: <any>window.btoa(p2),
    };
    return this.http.post(this.api + '/password', dto);
  }

  /**
   * Checks if the given username is available.
   */
  isUsernameAvailable(
    userId: number,
    requestedUsername: string
  ): Observable<any> {
    const dto = {
      userId: userId,
      requestedUsername: requestedUsername,
    };
    return this.http.post(this.api + '/username/availability', dto);
  }

  /**
   * Attempts to change the specified user's password.
   */
  changePassword(
    userId: number,
    authUserId: number,
    requestedPassword: string,
    currentPassword: string
  ): Observable<any> {
    const dto = {
      userId: userId,
      authUserId: authUserId,
      requestedPassword: <any>window.btoa(requestedPassword),
      currentPassword: <any>window.btoa(currentPassword),
    };
    return this.http.post(this.api + '/password', dto);
  }

  /**
   * GETs the password rules for the specified user.
   */
  getPasswordRules(userId: number): Observable<any> {
    return this.http.get(this.api + '/rules/' + userId);
  }

  /**
   * Validates a password against the user's password requirements.
   */
  doesMeetPasswordRules(userId: number, password: string): Observable<any> {
    const dto = {
      userId: userId,
      password: <any>window.btoa(password),
    };
    return this.http.post(this.api + '/password/rules', dto);
  }

  /**
   * GETs a site configuration.
   */
  getSiteConfigurations(): Observable<SiteConfiguration> {
    return this.http.get<SiteConfiguration>(this.api + '/site-config');
  }

  getMfaRequirements(authUserId: number): Observable<any> {
    return this.http
      .get(this.api + '/mfa-requirements/' + authUserId)
      .pipe(catchError(this.handleError('getMfaRequirements', null)));
  }

  appAuthEmailCheck(emailDto): Observable<any> {
    return this.http.put(this.api + '/auth-app-email', emailDto);
  }

  getClientAccountFeature(
    clientId: number,
    feature: number
  ): Observable<IClientAccountFeatureDto> {
    const url = `api/clients/${clientId}/features/${feature}`;
    return this.http
      .get<IClientAccountFeatureDto>(url)
      .pipe(catchError(this.handleError('getClientAccountFeatures', null)));
  }

  getUserTermsAndConditions(
    userId: number
  ): Observable<UserTermsAndConditionsDto> {
    return this.http
      .get<UserTermsAndConditionsDto>(
        `${this.api}/terms-and-conditions/user/${userId}`
      )
      .pipe(catchError(this.handleError('getUserTermsAndConditions', null)));
  }

  getLatestTermsAndConditionsVersion(
    msg: NgxMessageService
  ): Observable<ITermsAndConditionsVersion> {
    return this.http
      .get(`${this.api}/latest-terms-and-conditions`, { responseType: 'blob' })
      .pipe(
        tap(() => msg.loading(false)),
        catchError(this.handleError('getLatestTermsAndConditionVersion', null))
      );
  }

  getTermsAndConditionsVersion(): Observable<ITermsAndConditionsVersion[]> {
    return this.http.get<ITermsAndConditionsVersion[]>(
      `${this.api}/terms-and-conditions`
    );
  }

  fetchTermsAndConditionsVersions() {
    this.getTermsAndConditionsVersion().subscribe((x) =>
      this.tncItems$.next(x)
    );
  }

  getTermsAndConditionsVersionDownload(filepath: string, filename: string) {
    const url = `${this.api}/terms-and-conditions/download`;

    let params = new HttpParams();
    params = params.append('filename', filename);
    if (filepath.includes('OldServiceAgreements'))
      params = params.append('isOldAgreement', 'true');

    return this.http
      .get<any>(url, {
        responseType: 'blob' as any,
        observe: 'response',
        params: params,
      })
      .pipe(
        map((response: HttpResponse<Blob>) => {
          saveAs(response.body, filename);
          return response.body;
        })
      );
  }

  uploadTermsAndConditionVersion(fileToUpload: File, itemCount: number) {
    const url = `${this.api}/terms-and-conditions/upload`;
    let params = new HttpParams();
    params = params.append('itemCount', itemCount.toString());

    const formData: FormData = new FormData();
    formData.append('fileKey', fileToUpload, fileToUpload.name);

    return this.http.post(url, formData, { params: params });
  }

  userProcessedTermsAndConditions(
    isAccepted: boolean,
    msg: NgxMessageService
  ): Observable<UserTermsAndConditionsDto> {
    return this.PassUserInfoToRequest((user) => {
      return this.http
        .post<UserTermsAndConditionsDto>(
          `${this.api}/process-terms-and-conditions/user/${
            user.userId
          }/isAccepted/${(!!isAccepted).toString()}`,
          null
        )
        .pipe(
          tap(() => msg.loading(false)),
          catchError(this.handleError('process-terms-and-conditions', null))
        );
    });
  }

  getTimeoutDuration() {
    return this.http.get<Timeout>(`${this.api}/timeout`);
  }

  getSystemFeedback(
    userId: number,
    feedbackType: number
  ): Observable<ISystemFeedbackData> {
    const url = `api/account/user/${userId}/system-feedback/${feedbackType}`;
    return this.http
      .get<ISystemFeedbackData>(url)
      .pipe(catchError(this.handleError('getSystemFeedback', null)));
  }

  saveSystemFeedback(
    userId: number,
    dto: ISystemFeedbackData
  ): Observable<ISystemFeedbackData> {
    const url = `api/account/user/${userId}/system-feedback`;
    return this.http.post<ISystemFeedbackData>(url, dto);
  }

  hasFeature(clientId, featureType: Features): Observable<IClientFeature> {
    const url = `api/clients/${clientId}/features/${featureType}`;
    return this.http
      .get<IClientFeature>(url)
      .pipe(catchError(this.handleError('hasFeature', null)));
  }

  checkIfEmployeeIsActiveInOnboarding(employeeId): Observable<boolean> {
    const url = `api/onboarding/employee/checker/${employeeId}`;
    let params = new HttpParams();
    params = params.append('employeeId', employeeId.toString());

    return this.http.get<boolean>(url, { params: params });
  }

  SyncUserRecoveryEmail() {
    const url = `api/account/userinfo/sync/recovery/email`;

    return this.http.get(url);
  }

  /***********************
   * PRIVATE FUNCTIONS
   */

  handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send error to remote logging
      console.error(error); // log to console for now

      const errorMsg =
        error.error.errors != null && error.error.errors.length
          ? error.error.errors[0].msg
          : error.message;

      // TODO: better job of transforming error for user consumption
      this.log(error, `${operation} failed: ${errorMsg}`);

      // let app continue by return empty result
      return of(result as T);
    };
  }

  log(error: any, message: string): any {
    // this is where we would wire up any logging we will do when we throw JS errors.
    if (error) console.dir(error);

    console.log(message); // for now, we are simply printing errors to the console.
  }

  /**
   * Checks if a single action is in the list of allowed actions.
   */
  private actionFound(allowedActions: string[], action: string): boolean {
    return allowedActions.some(x => x === action);
  }
}

export const ACTION_TYPES = {
  CLIENT_VIEWRATES: 'Client.ReadClientRate',
  CLIENTRATE_VIEWHOURLYRATES: 'ClientRate.ViewHourlyRates',
  CLIENTRATE_VIEWSALARYRATES: 'ClientRate.ViewSalaryRates',
  SUPERVISOR: {
    ASSIGN_SUPERVISOR_ACCESS: 'Supervisor.AssignSupervisorAccess',
    SEND_PASSWORD_RESET: 'Supervisor.SendPasswordReset',
  }
};
