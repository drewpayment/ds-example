import { Injectable } from "@angular/core";
import { Subject, ReplaySubject, Observable, of, throwError } from "rxjs";
import { catchError, defaultIfEmpty, tap, map, concat, switchMap, concatMap, share, finalize, take, publishReplay, refCount } from 'rxjs/operators';
import { HttpClient, HttpParams } from "@angular/common/http";
import { AccountService } from "@ds/core/account.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";
import { UserInfo } from "@ds/core/shared";
import { ICompanyResourceFolder } from './company-resource-folder.model';

@Injectable({
    providedIn: 'root'
})
export class CompanyResourcesService {
    private readonly api = 'api';
    user: UserInfo;

    constructor(private http: HttpClient, private account: AccountService, private msg: DsMsgService) {
    }

    getCompanyResourcesByEmployee(employeeId: number): Observable<ICompanyResourceFolder[]> {
        const url = `${this.api}/employees/${employeeId}/resources`;
        return this.http.get<ICompanyResourceFolder[]>(url)
            .pipe(
                catchError(this.httpError('getCompanyResourcesByEmployee', <ICompanyResourceFolder[]>[]))
            );
    }

    private httpError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            let errorMsg = error.error.errors != null && error.error.errors.length
                ? error.error.errors[0].msg
                : error.message;

            this.account.log(error, `${operation} failed: ${errorMsg}`);

            // TODO: better job of transforming error for user consumption
            this.msg.setTemporaryMessage(`Sorry, this operation failed: ${errorMsg}`, MessageTypes.error, 6000);

            // let app continue by return empty result
            return of(result as T);
        }
    }
}
