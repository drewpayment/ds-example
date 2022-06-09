import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IUserActionTypeClaimType, IOtherAccessClaimType, IClientAccessInfo, IUserAccessInfo } from './';
import { IClientInfo } from './client.model';
import { IContact } from '@ds/core/contacts';
import { IAccountFeatureClaimType } from './account-feature-claim-type.model';

@Injectable({
    providedIn: 'root'
})
export class AccessRuleApiService {

    API_BASE = 'api/access-rules';

    constructor(
        private http: HttpClient
    ) { }

    getClaimTypes() {
        return this.http.get<(IUserActionTypeClaimType | IUserActionTypeClaimType | IAccountFeatureClaimType | IOtherAccessClaimType)[]>(`${this.API_BASE}/claim-types`);
    }

    /**
     * TODO: This needs to be moved to the proper service and module (i.e. 'ClientsModule')
     * Gets the list of clients that the current user has access to based on the given status filter.
     * @param Boolean isActive Whether the returned clients should only be active or not. Passing in null means no filtering is applied and all clients are returned for the user.
     */
    getClients(isActive = null) {
        var params = new HttpParams();
        if (isActive !== null)
            params = params.set("isActive", isActive.toString());

        return this.http.get<IClientInfo[]>(`api/clients`, { params: params });
    }

    getClientAccessInfo(clientId: number) {
        return this.http.get<IClientAccessInfo>(`${this.API_BASE}/clients/${clientId}`);
    }

    getUserList(clientId: number) {
        return this.http.get<IContact[]>(`${this.API_BASE}/users-list`, { params: new HttpParams().set("clientId", clientId.toString())});
    }

    getUserAccessInfo(userId: number, clientId: number | null = null) {
        var params = new HttpParams();
        if (clientId !== null)
            params = params.set('clientId', clientId.toString());
        return this.http.get<IUserAccessInfo>(`${this.API_BASE}/users/${userId}`, { params: params });
    }
}
