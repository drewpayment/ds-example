import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { MachineService } from '../../../services/machine.service';
import * as moment from 'moment';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { DeviceListDataService } from '../../services/device-list-data.service';
import { IClientMachine, IPushMachine, Timezone } from '@models';
import { coerceNumberProperty } from '@angular/cdk/coercion';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
    selector: 'ds-client-machine',
    templateUrl: './client-machine.component.html',
    styleUrls: ['./client-machine.component.scss']
})
export class ClientMachineComponent implements OnInit {
    clientMachineId: number;
    isLoading$: Observable<boolean>;
    hardware: IClientMachine;
    formGroup: FormGroup;
    unassignedMachines: IPushMachine[] = [];
    get Description() { return this.formGroup.get('description') as FormControl; }
    get Id() { return this.formGroup.get('pushMachine.id') as FormControl; }
    get DevSn() { return this.formGroup.get('pushMachine.devSn') as FormControl; }
    get DevIp() { return this.formGroup.get('pushMachine.devIp') as FormControl; }
    get TransTimes() { return this.formGroup.get('pushMachine.transTimes') as FormControl; }
    get TransInterval() { return this.formGroup.get('pushMachine.transInterval') as FormControl; }
    get AttLogStamp() { return this.formGroup.get('pushMachine.attLogStamp') as FormControl; }
    get OperLogStamp() { return this.formGroup.get('pushMachine.operLogStamp') as FormControl; }
    get PushMachineId() { return this.formGroup.get('pushMachineId') as FormControl; }
    get TimeZone() { return this.formGroup.get('pushMachine.timeZone') as FormControl; }
    get SyncTime() { return this.formGroup.get('pushMachine.syncTime') as FormControl; }
    get Warranty() { return this.formGroup.get('warranty') as FormControl; }
    get WarrantyEnd() { return this.formGroup.get('warrantyEnd') as FormControl; }
    get PurchaseDate() { return this.formGroup.get('purchaseDate') as FormControl; }
    get IsRental() { return this.formGroup.get('isRental') as FormControl; }
    get clientId() { return this.deviceListDataService.clientId as number; }
    get clientCode() { return this.deviceListDataService.clientCode as string; }
    set clientId(value: number) { this.deviceListDataService.clientId = value; }
    set clientCode(value: string) { this.deviceListDataService.clientCode = value; }
    get pageSize() { return this.deviceListDataService.pageSize as number; }
    get pageIndex() { return this.deviceListDataService.pageIndex as number; }
    get rowNumber() { return this.deviceListDataService.rowNumber as number; }
    get filter() { return this.deviceListDataService.filter as string; }


    changeMachine$: Observable<any>;
    formSubmitted = false;
    usTimezones: Timezone[];

    constructor(private fb: FormBuilder,
        private hardwareService: MachineService,
        private deviceListDataService: DeviceListDataService,
        private ngxMsg: NgxMessageService, private route: ActivatedRoute,
        private router: Router) { }
    ngOnInit() {
        if(!this.deviceListDataService){
            this.router.navigate(['/sys/clock/hardware']);
        }


        this.route.params.subscribe(
            (params: Params) => {
                this.clientMachineId = coerceNumberProperty(params['id']);
            }
          );

        this.hardware = this.deviceListDataService.devices.find(x => x.clientMachineId === this.clientMachineId) as IClientMachine;
        this._initList(this.hardwareService.getUnassignedMachines(), this.unassignedMachines).subscribe();
        this.populateTimezoneList();

        if (this.hardware) {
            this.clientId = this.hardware.clientId;
            this.createForm(this.hardware);
            if (this.hardware.pushMachineId && this.hardware.pushMachine) {
                this.DevSn.setValue(this.hardware.pushMachine.devSn);
                this.populateMachineForm(this.hardware.pushMachine.devSn);
            }
        } else {
            this.hardware = <IClientMachine>{ clientMachineId: 0, clientId: this.clientId }
            this.createForm(this.hardware)
            this.PurchaseDate.setValue(moment());
            this.Warranty.setValue(moment());
            this.WarrantyEnd.setValue(moment());
            this.IsRental.setValue("false");
        }
    }

    private _initList<T>(source: Observable<T[]>, target: T[]) {
        return source.pipe(
            tap(data => {
                for (let item of data) {
                    target.push(item);
                }
            })
        )
    }

    saveForm(): void {
        this.formSubmitted = true;

        if (this.formGroup.invalid) return;

        if (!this.clientId) return;

        this.hardwareService.updateClientMachine(this.formGroup.value, this.clientId).pipe(
            catchError((error) => {
                this.ngxMsg.setErrorMessage(error.error.errors[0].msg);
                return throwError(error);
            }),
            tap(x => {
                // this.data.clockClientHardware = x;

                if(this.hardware.clientMachineId == 0){
                    this.deviceListDataService.AddClockHardware(x);
                } else{
                    this.deviceListDataService.UpdateClockHardware(x);
                }

                if (this.hardware.clientMachineId == 0) {
                    this.ngxMsg.setSuccessMessage(x.description + ' added successfully!');
                }
                else {
                    this.ngxMsg.setSuccessMessage('Changes to ' + x.description + ' saved successfully!');
                }
            })
        ).subscribe(() => {
            this.goBackToList();
        });
    }

    private createForm(hardware: any): void {
        this.formGroup = this.fb.group({
            clientMachineId: [hardware.clientMachineId],
            description: [hardware.description, [Validators.required, Validators.maxLength(50)]],
            isRental: [`${ hardware.isRental }`, Validators.required],
            purchaseDate: [hardware.purchaseDate],
            total: [hardware.total],
            clientId: [this.clientId],
            clientCode: [this.clientCode],
            warranty: [hardware.warranty],
            warrantyEnd: [hardware.warrantyEnd, Validators.required],
            pushMachineId: [hardware.pushMachineId],
            pushMachine: this.fb.group({
                id: [''],
                devSn: ['', Validators.required],
                devIp: [''],
                transTimes: [''],
                transInterval: [''],
                attLogStamp: [''],
                operLogStamp: [''],
                timeZone: [''],
                syncTime: ['']
            })
        });

        this.changeMachine$ = this.DevSn.valueChanges.pipe(tap(x => this.populateMachineForm(x)));
    }

    populateMachineForm(devSn: string): void {
        let selectedMachine = this.hardware.pushMachine ? this.hardware.pushMachine : this.getSelectedPushMachine(devSn);
        this.PushMachineId.setValue(selectedMachine ? selectedMachine.id : "");
        this.Id.setValue(selectedMachine ? selectedMachine.id : "");
        this.DevIp.setValue(selectedMachine ? selectedMachine.devIp : "");
        this.TransTimes.setValue(selectedMachine ? selectedMachine.transTimes : "");
        this.TransInterval.setValue(selectedMachine ? selectedMachine.transInterval : "");
        this.AttLogStamp.setValue(selectedMachine ? selectedMachine.attLogStamp : "");
        this.OperLogStamp.setValue(selectedMachine ? selectedMachine.operLogStamp : "");
        this.TimeZone.setValue(selectedMachine ? selectedMachine.timeZone : "");
        this.SyncTime.setValue(selectedMachine ? selectedMachine.syncTime : "");

    }

    private getSelectedPushMachine(devSn: string) {
        return this.unassignedMachines.find(x => x.devSn === devSn);
    }

    private populateTimezoneList() {
        let tzList: Timezone[] = [];
        tzList.push(new Timezone('Eastern', '-05:00'));
        tzList.push(new Timezone('Central', '-06:00'));
        tzList.push(new Timezone('Mountain', '-07:00'));
        tzList.push(new Timezone('Pacific', '-08:00'));
        tzList.push(new Timezone('Alaska', '-09:00'));
        tzList.push(new Timezone('Hawaii', '-10:00'));

        this.usTimezones = tzList;
    }

    goBackToList(){
        this.router.navigate(['/sys/clock/hardware'], {
            queryParams: {pageSize: this.pageSize, pageIndex: this.pageIndex, r: this.rowNumber, filter: this.filter}
        });
    }
}
