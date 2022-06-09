import { Injectable } from "@angular/core";
import { Subject, ReplaySubject, Observable, of, throwError } from "rxjs";
import { catchError, defaultIfEmpty, tap, map, concat, switchMap, concatMap, share, finalize, take, publishReplay, refCount } from 'rxjs/operators';
import { HttpClient, HttpParams } from "@angular/common/http";
import { AccountService } from "@ds/core/account.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";
import { UserInfo } from "@ds/core/shared";
import { ICountry } from './country.model';
import { IState } from './state.model';
import { ICounty } from './county.model';

@Injectable({
    providedIn: 'root'
})
export class EmployeeCommonService {
    private readonly api = 'api';
    user: UserInfo;

    constructor(private http: HttpClient, private account: AccountService, private msg: DsMsgService) {
    }

    //Location related services
    getCountries(): Observable<ICountry[]> {
        const url = `${this.api}/location/countries`;
        return this.http.get<ICountry[]>(url)
            .pipe(
            catchError(this.httpError('getCountries', <ICountry[]>[]))
            );
    }

    getStatesByCountryId(countryId: number): Observable<IState[]> {
        const url = `${this.api}/location/countries/${countryId}/states`;
        return this.http.get<IState[]>(url)
            .pipe(
            catchError(this.httpError('getStatesByCountryId', <IState[]>[]))
            );
    }

    getCountiesByStateId(stateId: number): Observable<ICounty[]> {
        const url = `${this.api}/location/states/${stateId}/counties`;
        return this.http.get<ICounty[]>(url)
            .pipe(
            catchError(this.httpError('getCountiesByStateId', <ICounty[]>[]))
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
