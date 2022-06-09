import { Injectable, Inject } from '@angular/core';
import { Observable, of } from 'rxjs';
import { IClientData } from '@ajs/onboarding/shared/models';
import { Moment } from 'moment';
import * as moment from 'moment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AccountService } from '@ds/core/account.service';
import { switchMap, tap, map } from 'rxjs/operators';
import { DOCUMENT } from '@angular/common';
import { UserInfo } from '@ds/core/shared';
import { Store } from '@ngrx/store';
import { getUser, UserState } from '@ds/core/users/store/user.reducer';

interface HttpCache<T> {
    data: T;
    updatedAt: Moment;
    isActiveFilter?: boolean;
}

@Injectable()
export class ClientSelectorService {

    private _clients: HttpCache<IClientData[]>;
    private _client: HttpCache<IClientData>;

    constructor(
        private http: HttpClient,
        @Inject(DOCUMENT) private document: Document,
        private store: Store<UserState>,
    ) {}

    getClients(isActive = true): Observable<IClientData[]> {
        if (this._clients && this.isCacheValid(this._clients, isActive)) {
            return of(this._clients.data);
        }

        return this.http.get<IClientData[]>(`api/clients`, {
            params: new HttpParams({ fromString: `isActive=${isActive}` }),
        })
        .pipe(
            map(clients => clients.sort((a, b) => {
                const aName = a.clientName.toLowerCase();
                const bName = b.clientName.toLowerCase();
                if (aName < bName)
                    return -1;
                if (aName > bName)
                    return 1;
                return 0;
            })),
            tap(clients => this._clients = {
                data: clients,
                updatedAt: moment(),
                isActiveFilter: isActive
            })
        );
    }

    getCurrentClient() {
        if (this._client && this.isCacheValid(this._client)) {
            return of(this._client.data);
        }

        return this.store.select(getUser)
            .pipe(
                switchMap(user =>
                    this.http.get<IClientData>(`api/clients/${user.lastClientId}`)),
                tap(client => this._client = {
                    data: client,
                    updatedAt: moment()
                })
            );
    }

    selectClient(clientId: number) {
        this.http.post(`api/account/last-client`, { clientId })
            .subscribe(_ => {
                const path = this.document.location.pathname;
                const reloadPath = path + '?' + this.document.location.search.substring(1);
                this.document.location.href = reloadPath;
            });
    }

    changeClient(clientId: number): Observable<UserInfo> {
        return this.http.post<UserInfo>(`api/account/change-client`, { clientId });
    }

    private isCacheValid<T>(cache: HttpCache<T>, isActiveFilter = true): boolean {
        const now = moment();
        if (!moment(cache.updatedAt).isValid) return false;
        const props = Object.keys(cache);
        if (props.includes('isActiveFilter')) {
            return cache.isActiveFilter === isActiveFilter;
        }
        return cache.updatedAt.diff(now, 'm') < 1;
    }

}
