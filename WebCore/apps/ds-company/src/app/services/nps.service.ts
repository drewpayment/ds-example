import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, Subject } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { AccountService } from '@ds/core/account.service';
import { MessageService } from './message.service';
import { Store } from '@ngrx/store';
import { UserState } from '@ds/core/users/store/user.reducer';
import { NpsResponse, NpsScore, NpsResponseFilter } from '@models';

@Injectable()
export class NpsService {

    private api = `api/nps`;

    npsResponses$ = new Subject<NpsResponse[]>();
    npsScores$ = new Subject<NpsScore[]>();
    clientOrganizations$ = new Subject<any[]>();
    clients$ = new Subject<any[]>();

    constructor(
        private http: HttpClient,
        private accountService: AccountService,
        private ngxMsgSvc: MessageService,
        private store: Store<UserState>,
    ) {
        //this.fetchFakeBillingResolver();
    }

    getNPSResponses(dto: NpsResponseFilter): Observable<NpsResponse[]> {
        const url = `${this.api}/nps-responses`;

        //let params = new HttpParams().set("dto", JSON.stringify(dto));
        return this.http.post<NpsResponse[]>(url, dto)
    }

    saveNpsResponse(responseItem: NpsResponse) {
        let url = `${this.api}/save/nps-response`;

        return this.http.post<NpsResponse>(url, responseItem);
    }

    getNPSScores(dto: NpsResponseFilter): Observable<NpsScore[]> {
        const url = `${this.api}/nps-responses/scores`;

        //let params = new HttpParams().set("dto", JSON.stringify(dto));
        return this.http.post<NpsScore[]>(url, dto)
    }

    getOrganizationsList(): Observable<any[]> {
        const url = `api/client/organizations`;
        return this.http.get<any[]>(url);
    }

    getClientsList(): Observable<any[]> {
        const url = `api/client/clients`;
        return this.http.get<any[]>(url);
    }

    fetchNpsResponses(dto) {
        const apiCall = this.getNPSResponses(dto).pipe(tap(x => this.npsResponses$.next(x)));
        this.handleError(apiCall).subscribe();
    }

    fetchNpsScores(dto) {
        const apiCall = this.getNPSScores(dto).pipe(tap(x => this.npsScores$.next(x)));
        this.handleError(apiCall).subscribe();
    }

    fetchOrganizationsList() : Observable<void> {
        const apiCall = this.getOrganizationsList().pipe(tap(x => this.clientOrganizations$.next(x)));
        return this.handleError(apiCall);
    }

    fetchClientsList() : Observable<void> {
        const apiCall = this.getClientsList().pipe(tap(x => this.clients$.next(x)));
        return this.handleError(apiCall);
    }

    handleError<T>(apiCall: Observable<T>): Observable<any> {
        return apiCall.pipe(catchError(error => {
            this.ngxMsgSvc.setErrorMessage(error.message);
            return of(null);
        }));
    }

}
