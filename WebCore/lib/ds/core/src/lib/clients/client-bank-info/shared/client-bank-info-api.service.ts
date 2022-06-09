import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IClientBankSetupInfo } from './client-bank-setup-info.model';
import { IBankBasicInfo } from '@ds/core/banks';


@Injectable({
    providedIn: 'root'
})
export class ClientBankInfoApiService {

    constructor(private http: HttpClient) { }

    getClientBankSetupInfo(clientId: number) {
        return this.http.get<IClientBankSetupInfo>(`api/clients/${clientId}/banks/setup-info`);
    }

    getClientBanks(clientId: number) {
        return this.http.get<IBankBasicInfo[]>(`api/clients/${clientId}/banks`);
    }

    saveClientBanks(clientId: number, banks: IBankBasicInfo[]) {
        return this.http.post<IBankBasicInfo[]>(`api/clients/${clientId}/banks`, banks);
    }
}
