import { Injectable } from '@angular/core';
import { Subject, ReplaySubject, Observable, of, throwError } from 'rxjs';
import { catchError, defaultIfEmpty, tap, map, concat, switchMap, concatMap, share, finalize, take, publishReplay, refCount } from 'rxjs/operators';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AccountService } from '@ds/core/account.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { UserInfo } from '@ds/core/shared';
import { checkboxComponent } from '@ajs/applicantTracking/application/inputComponents';
import { IEmployeeDirectDepositInfo } from './employee-direct-deposit-info.model';
import { IClientEssOptions } from './client-ess-options.model';

@Injectable({
    providedIn: 'root'
})
export class EmployeeDirectDepositsService {
    private readonly api = 'api';
    user: UserInfo;

    constructor(private http: HttpClient, private account: AccountService, private msg: DsMsgService) {
    }

    isPrenoteClient(clientId: number): Observable<boolean> {
        const url = `${this.api}/clients/${clientId}/is-prenote-client`;
        return this.http.get<boolean>(url)
            .pipe(
            catchError(this.httpError('isPrenoteClient', <boolean>{}))
            );
    }

    getClientEssOptions(clientId: number): Observable<IClientEssOptions> {
        const url = `${this.api}/clients/${clientId}/ess-options`;
        return this.http.get<IClientEssOptions>(url)
            .pipe(
            catchError(this.httpError('getClientEssOptions', <IClientEssOptions>{}))
            );
    }

    getOnboardingAccounts(employeeId: number): Observable<IEmployeeDirectDepositInfo[]> {
        const url = `${this.api}/employees/${employeeId}/direct-deposits/onboarding-accounts`;
        return this.http.get<IEmployeeDirectDepositInfo[]>(url)
            .pipe(
            catchError(this.httpError('getOnboardingAccounts', <IEmployeeDirectDepositInfo[]>[]))
            );
    }

    updateEmployeeDirectDepositAmounts(model: IEmployeeDirectDepositInfo[], employeeId: number): Observable<IEmployeeDirectDepositInfo[]> {
        const url = `${this.api}/employees/${employeeId}/direct-deposit-amounts`;
        return this.http.put<IEmployeeDirectDepositInfo[]>(url, model)
            .pipe(
            catchError(this.httpError('updateEmployeeDirectDeposits', <IEmployeeDirectDepositInfo[]>{}))
            );
    }

    addEmployeeDirectDeposit(model: IEmployeeDirectDepositInfo, employeeId: number): Observable<IEmployeeDirectDepositInfo> {
        const url = `${this.api}/employees/${employeeId}/employee-direct-deposit`;
        return this.http.put<IEmployeeDirectDepositInfo>(url, model)
            .pipe(
            catchError(this.httpError('addEmployeeDirectDeposit', <IEmployeeDirectDepositInfo>{}))
            );
    }

    private httpError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            const errorMsg = error.error.errors != null && error.error.errors.length
                ? error.error.errors[0].msg
                : error.message;

            this.account.log(error, `${operation} failed: ${errorMsg}`);

            // TODO: better job of transforming error for user consumption
            this.msg.setTemporaryMessage(`Sorry, this operation failed: ${errorMsg}`, MessageTypes.error, 6000);

            // let app continue by return empty result
            return of(result as T);
        };
    }
}
