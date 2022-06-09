import { MessageService } from './message.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IClientMachine, IPushMachine, IUserInfo, ITemplate, ITransaction, IDeviceCommand } from '@models';

@Injectable({
    providedIn: 'root'
})

export class MachineService {
    private api = `api/machine`;

    constructor(private http: HttpClient, private msg: MessageService){}

    getClientMachines(clientId: number): Observable<IClientMachine[]>{
        const url = `${this.api}/machines?clientId=${clientId}`;
        return this.http.get<IClientMachine[]>(url);
    }

    updateClientMachine(model: IClientMachine, clientId: number): Observable<IClientMachine> {
        const url = `${this.api}/client/${clientId}/machine/update`;
        return this.http.post<IClientMachine>(url, model);
    }

    deleteClientMachine(clientId: number, clientMachineId: number) {
        var url = `${this.api}/client/${clientId}/machine/delete/${clientMachineId}`;
        return this.http.post<IClientMachine>(url, null);
    }

    getUnassignedMachines(){
        var url = `${this.api}/unassigned`;
        return this.http.get<IPushMachine[]>(url);
    }

    getMachineUsersByClientId(clientId: number): Observable<IUserInfo[]> {
        var url = `${this.api}/users?clientId=${clientId}`;
        return this.http.get<IUserInfo[]>(url);
    }

    getTemplatesByClientId(clientId: number): Observable<ITemplate[]> {
        var url = `${this.api}/templates?clientId=${clientId}`;
        return this.http.get<ITemplate[]>(url);
    }

    getTransactionsByClientId(clientId: number): Observable<ITransaction[]> {
        var url = `${this.api}/transactions?clientId=${clientId}`;
        return this.http.get<ITransaction[]>(url);
    }

    rebootMachines(serialNumbers: string[]) {
        const url = `api/push/machines/reboot`;
        return this.http.post<IDeviceCommand[]>(url, serialNumbers);
    }

    checkUploadNewDataForMachines(serialNumbers: string[]){
        const url = `api/push/machines/checkuploadnewdataformachines`;
        return this.http.post<IDeviceCommand[]>(url, serialNumbers);
    }

    refreshInfoForMachines(serialNumbers: string[]): Observable<IDeviceCommand[]> {
        const url = `api/push/machines/refreshinfoformachines`;
        return this.http.post<IDeviceCommand[]>(url, serialNumbers);
    }
    clearTransactionsForMachines(serialNumbers: string[]): Observable<IDeviceCommand[]> {
        const url = `api/push/machines/cleartransactionsformachines`;
        return this.http.post<IDeviceCommand[]>(url, serialNumbers);
    }
}
