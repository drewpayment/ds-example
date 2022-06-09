import { Injectable } from '@angular/core';
import { IClientMachine } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';
import { BehaviorSubject } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class DeviceListDataService {
    clientId: number;
    clientCode: string;
    pageSize: number = 25;
    pageIndex: number = 0;
    filter: string;
    rowNumber?: number;
    private _devices: IClientMachine[] = [] as IClientMachine[];
    get devices(): IClientMachine[] {
        return this._devices;
    }
    set devices(value: IClientMachine[]){
        this._devices = value;
    }
    private _devices$ = new BehaviorSubject<IClientMachine[]>(this._devices);
    devicesObservable$ = this._devices$.asObservable();
    displayCheckboxValidationError: boolean = false;

    constructor(private msg: NgxMessageService) {
    }

    public RemoveById(clockClientHardwareId: number) {
        const index = this._devices.map(e => e.clientMachineId).indexOf(clockClientHardwareId);
        this._devices.splice(index, 1);
        this._devices$.next(this.devices);
    }

    public AddClockHardware(clockClientHardware: IClientMachine) {
        this._devices.push(clockClientHardware);
        this._devices$.next(this.devices);
    }

    UpdateClockHardware(clockClientHardware: IClientMachine) {
        let index = this._devices.map(e => e.clientMachineId).indexOf(clockClientHardware.clientMachineId);
        this._devices[index] = clockClientHardware;
        this._devices$.next(this.devices);
    }
}
