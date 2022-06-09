import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
    AccountOptionInfoWithClientSelectionByControlIdName,
    IAccountOptionInfoWithClientSelection
} from './account-option-info-with-client-selection.model';
import { IClientOptionItem } from './client-option-item.model';
import { ClientDepartmentDto } from '@ajs/ds-external-api/models';
import { IClientData } from '@ajs/onboarding/shared/models';
import { ClientEssOptions } from 'apps/ds-company/ajs/models';
import { ClientAccountFeature } from './client-account-feature.model';

@Injectable({
    providedIn: 'root'
})
export class ClientService {

    constructor(private http: HttpClient) { }

    private api = 'api/client';
    private clientsApi = 'api/clients';

    getClientOptions(page: string): Observable<IAccountOptionInfoWithClientSelection[]> {
        const url = `${this.api}/options`;
        let params = new HttpParams();
        params = params.append('page', page);
        return this.http.get<IAccountOptionInfoWithClientSelection[]>(url, { params: params });
    }

    getAccountOptionInfoWithClientSelectionByControlIds(page: string)
    : Observable<AccountOptionInfoWithClientSelectionByControlIdName> {
        const url = `${this.api}/options-by-control-ids`;
        let params = new HttpParams();
        params = params.append('page', page);
        return this.http.get<AccountOptionInfoWithClientSelectionByControlIdName>(url, { params: params });
    }

    saveClientAccountOption(dtos: IAccountOptionInfoWithClientSelection[]) {
        const url = `${this.api}/save/account/options`;
        return this.http.post<IAccountOptionInfoWithClientSelection[]>(url, dtos);
    }

    saveAccountOptionInfoWithClientSelectionByControlIds(dictionary: AccountOptionInfoWithClientSelectionByControlIdName)
    : Observable<AccountOptionInfoWithClientSelectionByControlIdName> {
        const url = `${this.api}/save/account/options-by-control-ids`;
        return this.http.post<AccountOptionInfoWithClientSelectionByControlIdName>(url, dictionary);
    }

    getClientAccountOption(clientOptionsControl: number) {
        const url = `${this.api}/account/options/status`;
        let params = new HttpParams();
        params = params.append('clientOptionControlId', clientOptionsControl.toString());
        return this.http.get<IClientOptionItem>(url, { params: params });
    }
    getClientDepartmentList(clientId: number): Observable<ClientDepartmentDto[]> {
        const url = `${this.api}/${clientId}/client-departments`;
        return this.http.get<ClientDepartmentDto[]>(url);
    }

    getClientAccountFeatureByFeatureId(clientId: number, featureId: number): Observable<ClientAccountFeature> {
        const url = `${this.api}/get/account/feature`;
        let params = new HttpParams();
        params = params.append('clientId', clientId.toString());
        params = params.append('featureId', featureId.toString());

        return this.http.get<ClientAccountFeature>(url, { params: params });
    }

    getClientById(clientId: number) {
        const url = `${this.clientsApi}/${clientId}`;
        return this.http.get<IClientData>(url);
    }

    getClientEssOptions(clientId){
        const url = `${this.clientsApi}/${clientId}/ess-options`;
        return this.http.get<ClientEssOptions>(url);
    }

    updateClientEssOptions(dto: ClientEssOptions) {
        const url = `${this.clientsApi}/${dto.clientId}/ess-options`;
        return this.http.post<ClientEssOptions>(url, dto);
    }

}
