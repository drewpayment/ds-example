import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Client } from './models/client';
import { ClientPayroll } from './models/clientPayroll';

@Injectable({
  providedIn: 'root'
})
export class ClientStatisticsApiService {

    constructor(private httpClient: HttpClient) { }

    getTotalActiveClients(){
        return this.httpClient.get<Client[]>('api/clients/feature-list');
    }

    getClientsRunningPayroll(){
        return this.httpClient.get<ClientPayroll[]>('api/clients/clients-running-payroll');
    }

}
