import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IBankInfo, IBankBasicInfo } from './bank-info.model';

@Injectable({
    providedIn: 'root'
})
export class BanksApiService {

    api = "api/banks";  
    constructor(private http:HttpClient) { }

    /**
     * GETs all bank setup information.
     */
    getBanks() {
        return this.http.get<IBankInfo[]>(`${this.api}`);
    }

    /**
     * GETs name and routing for all banks.
     */
    getBankBasicList() {
        return this.http.get<IBankBasicInfo[]>(`${this.api}/basic`);
    }

    /**
     * Saves changes to a particular bank setup.
     */
    saveBank(bank: IBankInfo) {
        let url = `${this.api}`

        if (bank.bankId)
            url += `/${bank.bankId}`;

        return this.http.post<IBankInfo>(url, bank);
    }

    /**
     * Attempts to delete a bank.
     */
    deleteBank(bankId: number) {
        return this.http.delete(`${this.api}/${bankId}`);
    }
}
