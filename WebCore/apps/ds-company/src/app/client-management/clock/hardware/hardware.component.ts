import { AccountService } from '@ds/core/account.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { catchError, switchMap } from 'rxjs/operators';
import { MachineService } from '../../../services/machine.service';
import { Observable, throwError } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { DeviceListDataService } from '../../services/device-list-data.service';
import { IClientMachine, IDeviceCommand } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
    selector: 'ds-hardware',
    templateUrl: './hardware.component.html',
    styleUrls: ['./hardware.component.scss']
})
export class HardwareComponent implements OnInit {
    createForm(): FormGroup {
        return this.formBuilder.group({
            filter: this.formBuilder.control(''),
            data: this.formBuilder.control('')
        })
    };
    formGroup: FormGroup = this.createForm();
    formSubmitted = false;
    get selectedData() { return this.formGroup.get('data') as FormControl; }
    get filter() { return this.formGroup.get('filter') as FormControl; }
    datas: string[] = ['Clocks', 'Employees', 'Fingerprints', 'Transactions'];
    get clientId() { return this.deviceListDataService.clientId as number; }
    set clientId(value: number) { this.deviceListDataService.clientId = value; }
    get devices(): IClientMachine[] {
        return this.deviceListDataService.devices;
    }
    get selectedMachineSns(): string[] {
        return this.devices.filter(x => {
            return x.isSelected === true
        }).map(a => a.pushMachine.devSn);
    }

    constructor(private formBuilder: FormBuilder, private accountService: AccountService, private deviceListDataService: DeviceListDataService,
        private ngxMsg: NgxMessageService, private hardwareService: MachineService, private activatedRt: ActivatedRoute, private router: Router) { }

    ngOnInit() {
        this.accountService.getUserInfo().subscribe(user => {
            this.clientId = user.lastClientId;
        });

        this.filterDevices('');
        this.selectedData.setValue("Clocks");
        this.activatedRt.queryParams.subscribe(param => {
            if(param["filter"]){
                this.filter.setValue(param["filter"]);
            }
          });
    }

    routeToForm() {
      this.router.navigateByUrl('/sys/clock/hardware/clientmachine/0');
    }

    filterDevices(filterText: string) {
        this.router.navigate([], {
            relativeTo: this.activatedRt,
            queryParams: {filter: filterText},
            queryParamsHandling: 'merge',
          });

        this.filter.setValue(filterText);
    }
    addClockHardware(clientId?: number): void {
        if (clientId) {
            // this.modalTrigger.openClockHardware(clientId as number, null);
        }
    }

    rebootSelected = (selectedMachineSns: string[]): Observable<IDeviceCommand[]> => {
        return this.hardwareService.rebootMachines(selectedMachineSns);
    }

    checkUploadNewDataSelected = (selectedMachineSns: string[]): Observable<IDeviceCommand[]> => {
        return this.hardwareService.checkUploadNewDataForMachines(selectedMachineSns);
    }

    clearTransactionsSelected = (selectedMachineSns: string[]): Observable<IDeviceCommand[]> => {
        return this.hardwareService.clearTransactionsForMachines(selectedMachineSns);
    }

    refreshInfoSelected = (selectedMachineSns: string[]): Observable<IDeviceCommand[]> => {
        return this.hardwareService.refreshInfoForMachines(selectedMachineSns);
    }

    sendDeviceCommand(fn: (selectedMachineSns: string[]) => Observable<IDeviceCommand[]>) {
        if(this.selectedData.value !== "Clocks"){
            return;
        }

        if(this.selectedMachineSns.length === 0){
            this.deviceListDataService.displayCheckboxValidationError = true;
            return;
        }

        this.ngxMsg.setSuccessMessage('sending');

        fn(this.selectedMachineSns).pipe(
            catchError((error) => {
                const errorMsg = error.error.errors != null && error.error.errors.length
                    ? error.error.errors[0].msg
                    : error.message;

                this.ngxMsg.setErrorMessage(errorMsg);
                return throwError(error);
            }),
            switchMap((result: IDeviceCommand[]) => {
                if (result && result.length > 0) {
                    let resultSns = result.map(function (x) {
                        return x.devSn;
                    }).join(", ");

                    this.ngxMsg.setSuccessMessage(result.length === 1 ? 'Command sent to ' + resultSns : 'Commands sent to ' + resultSns);

                    return result;
                }
            })
        ).subscribe();
    }
}
