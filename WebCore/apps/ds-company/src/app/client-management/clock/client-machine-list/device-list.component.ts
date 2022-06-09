import { AccountService } from '@ds/core/account.service';
import { AfterViewChecked, Component, ElementRef, Input, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { NEVER, of, throwError } from 'rxjs';
import { catchError, switchMap, tap } from 'rxjs/operators';
import { MachineService } from '../../../services/machine.service';
import { ChangeDetectorRef  } from '@angular/core';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { DeviceListDataService } from '../../services/device-list-data.service';
import { IClientMachine } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
    selector: 'ds-device-list',
    templateUrl: './device-list.component.html',
    styleUrls: ['./device-list.component.scss']
})
export class DeviceListComponent implements OnInit, AfterViewChecked {

    ngAfterViewChecked() {
        if(this.rowNumber){
            this.scrollToClientMachine();
        }
    }
    @ViewChildren("clientMachineRows", { read: ElementRef }) clientMachineRows: QueryList<ElementRef>;
    @Input() selectedDeviceId?: number;
    @Input() inputfilter: string;
    deviceCount: number;
    filteredDeviceCount: number;
    get devices(): IClientMachine[] {
        return this.deviceListDataService.devices;
    }
    set devices(value: IClientMachine[]){
        this.deviceListDataService.devices = value;
    }
    get displayCheckboxValidationError(): boolean {
        return this.deviceListDataService.displayCheckboxValidationError;
    }
    devicesDatasource = new MatTableDataSource<IClientMachine>([]);
    // @ViewChild('devicesPaginator', { static: false }) devicesPaginator: MatPaginator;
    @ViewChild('devicesPaginator', {static: false}) set paginator(pager:MatPaginator) {
        if (pager) this.devicesDatasource.paginator = pager;

        this.uncheckAll();
        this.selectAllCheckboxChecked = false;
        this.changeDetectorRef.detectChanges();
      }
      @ViewChild(MatSort, { static: false }) set sort(sorter: MatSort) {
        if (sorter) this.devicesDatasource.sort = sorter;
      }
    devicesDisplayedColumns: string[] = ['selected', 'description', 'devSn', 'devIp', 'realtime', 'operLogStamp', 'devFirmwareVersion', 'userCount', 'fpCount', 'attCount', 'isRental', 'edit'];
    devicesLoaded: boolean = false;
    selectAllCheckboxChecked: boolean;
    get clientId() { return this.deviceListDataService.clientId as number; }
    get clientCode() { return this.deviceListDataService.clientCode as string; }
    set clientId(value: number) { this.deviceListDataService.clientId = value; }
    set clientCode(value: string) { this.deviceListDataService.clientCode = value; }
    get pageSize() { return this.deviceListDataService.pageSize as number; }
    set pageSize(value: number) { this.deviceListDataService.pageSize = value; }
    get pageIndex() { return this.deviceListDataService.pageIndex as number; }
    set pageIndex(value: number) { this.deviceListDataService.pageIndex = value; }
    get rowNumber() { return this.deviceListDataService.rowNumber as number; }
    set rowNumber(value: number) { this.deviceListDataService.rowNumber = value; }
    get filter() { return this.deviceListDataService.filter as string; }
    set filter(value: string) { this.deviceListDataService.filter = value; }


    constructor(private formBuilder: FormBuilder, private hardwareService: MachineService,
        private accountService: AccountService, private deviceListDataService: DeviceListDataService,
        private ngxMsg: NgxMessageService, private changeDetectorRef: ChangeDetectorRef,
        private confirmDialog: ConfirmDialogService, private activatedRt: ActivatedRoute,
        private router: Router) {
            this.activatedRt.queryParams.subscribe(param => {
                this.pageIndex = param["pageIndex"];
                this.pageSize = param["pageSize"];
                this.rowNumber = param["r"];
                this.inputfilter = param["filter"];
                this.filter = param["filter"];
                let pageRequest = {
                 pageSize: this.pageSize,
                 pageIndex: this.pageIndex,
                 e: this.rowNumber
                }
                this.changePage(pageRequest);
                this.filterDevices(this.inputfilter);
              });
        }

        changePage(event?: any) {
            this.router.navigate([], {
                relativeTo: this.activatedRt,
                queryParams: {pageSize: event.pageSize, pageIndex: event.pageIndex, r: this.rowNumber, filter: this.inputfilter},
                queryParamsHandling: 'merge'
                });

            return event;
          }

    ngOnInit() {
        this.accountService.getUserInfo().subscribe(user => {
            this.clientId = user.lastClientId;
            this.clientCode = user.lastClientCode;
            this.hardwareService.getClientMachines(user.lastClientId).pipe(
                // catchError((error) => {
                //     this.msg.setTemporaryMessage('Sorry, this operation failed: \'Get Devices By Client Id\'', MessageTypes.error, 60000);
                //     return throwError(error);
                // }),
                tap(x => {
                    this.devices = x;
                    this.devices.forEach(function(el){el.isSelected = false;})
                    this.deviceCount = this.devices.length;
                    this.filterDevices(this.inputfilter);
                    this.devicesLoaded = true;
                })
            ).subscribe();
        });

        this.deviceListDataService.devicesObservable$.subscribe(x => {
            this.filterDevices(this.inputfilter);
        });

        this.configureSortProperties();
    }

    ngOnChanges() {
        if (this.devicesLoaded) {
            this.filterDevices(this.inputfilter);
        }
    }

    filterDevices(filterText: string) {

        let filteredDevices: IClientMachine[];
        if (filterText) {
            var filter = filterText.trim().toLowerCase();

            filteredDevices = this.devices.filter(
                device => (device && device.description && device.description.trim().toLowerCase().includes(filter)) ||
                    (device.pushMachine && device.pushMachine.devSn && device.pushMachine.devSn.trim().toLowerCase().includes(filter)) ||
                    (device.pushMachine && device.pushMachine.devIp && device.pushMachine.devIp.trim().toLowerCase().includes(filter))
            );
        } else {
            filteredDevices = this.devices;
        }
        filteredDevices.push();
        this.filteredDeviceCount = filteredDevices.length;
        this.devicesDatasource.data = filteredDevices;
    }

    deleteDevice(machine: IClientMachine){
        //let shouldDelete = false;
        const options = {
            title: 'Are you sure you want to delete this clock?',
            confirm: "Delete"
        };
        this.confirmDialog.open(options);

        this.confirmDialog.confirmed().pipe(
            switchMap(confirmed => {
                if ( confirmed ) {
                    this.ngxMsg.setSuccessMessage('sending');

                    return this.hardwareService.deleteClientMachine(this.clientId, machine.clientMachineId);
                    //return of({} as IClientMachine); // for testing without deleting
                } else {
                    return NEVER;
                }
            }),
            catchError((error) => {
                this.ngxMsg.setErrorMessage(error.error.errors[0].msg);
                return throwError(error);
            }),
        ).subscribe(result => {
            const index = this.devices.map(e => e.clientMachineId).indexOf(machine.clientMachineId);
            if (index > -1) {
                this.devices.splice(index, 1);
                this.filterDevices(this.inputfilter)
                this.ngxMsg.setSuccessMessage('Deleted clock ' + machine.description);
            }
        });
    }

    configureSortProperties() {
        this.devicesDatasource.sortingDataAccessor = (item, property) => {
            switch (property) {
                case 'devSn': {
                    return item.pushMachine ? item.pushMachine.devSn : "";
                }
                case 'devIp': {
                    return item.pushMachine ? item.pushMachine.devIp : "";
                }
                case 'realtime': {
                    return item.pushMachine ? item.pushMachine.realtime : "";
                }
                case 'operLogStamp': {
                    return item.pushMachine ? item.pushMachine.operLogStamp : "";
                }
                case 'devFirmwareVersion': {
                    return item.pushMachine ? item.pushMachine.devFirmwareVersion : "";
                }
                case 'userCount': {
                    return item.pushMachine ? item.pushMachine.userCount : "";
                }
                case 'fpCount': {
                    return item.pushMachine ? item.pushMachine.fpCount : "";
                }
                case 'attCount': {
                    return item.pushMachine ? item.pushMachine.attCount : "";
                }
                default: {
                    return item[property];
                }
            }
        }
    }

    checkUncheckAllPaged(checked: boolean) {
        if(this.devicesDatasource.paginator){
            this.selectAllCheckboxChecked = checked;
            let pagedClientMachineIds = this.getClientMachinesOnCurrentPage();

            pagedClientMachineIds.map(function (x) {
                return x.clientMachineId;
            }).forEach(clientMachineId => {
                let itemIndex = this.devices.findIndex(item => item.clientMachineId == clientMachineId);
                this.devices[itemIndex].isSelected = checked;
            });

            this.filterDevices(this.inputfilter);
        }
    }

    private getClientMachinesOnCurrentPage() {
        const skip = this.devicesDatasource.paginator.pageSize * this.devicesDatasource.paginator.pageIndex;

        //Only toggle current page of devices based on paginator.
        let pagedClientMachines = this.devices.map(x => x).filter((u, i) => i >= skip).filter((u, i) => i < this.devicesDatasource.paginator.pageSize).map(function (x) {
            return x;
        });
        return pagedClientMachines;
    }

    uncheckAll(){
        this.devices.forEach(clientMachine => {
            let itemIndex = this.devices.findIndex(item => item.clientMachineId == clientMachine.clientMachineId);
            this.devices[itemIndex].isSelected = false;
        });

        this.filterDevices(this.inputfilter);
    }

    onCheckChange(clientMachineId: number, checked: boolean) {
        this.deviceListDataService.displayCheckboxValidationError = false;
      let index = this.devices.findIndex((x) => x.clientMachineId == clientMachineId);
      this.devices[index].isSelected = checked;
      if(this.allCheckboxesOnPageChecked()){
        this.selectAllCheckboxChecked = true
      } else{
        this.selectAllCheckboxChecked = false
      }
      }

      onCheckAllChange() {
          if(this.devicesDatasource.paginator){
            const skip = this.devicesDatasource.paginator.pageSize * this.devicesDatasource.paginator.pageIndex;
            let pagedClientMachineIds = this.devices.map(x => x).filter((u, i) => i >= skip).filter((u, i) => i < this.devicesDatasource.paginator.pageSize).map(function(x){
                return x.clientMachineId;
            });

            pagedClientMachineIds.forEach(clientMachineId => {
                let itemIndex = this.devices.findIndex(item => item.clientMachineId == clientMachineId);
                this.devices[itemIndex].isSelected = true;
            });

            this.filterDevices(this.inputfilter);
          }
    }

    allCheckboxesOnPageChecked(){
        let clientMachines = this.getClientMachinesOnCurrentPage();
        let selectedClientMachineCount = 0;

        clientMachines.forEach(x => {
            if(x.isSelected){
                selectedClientMachineCount++;
            }
        });

        if(selectedClientMachineCount === this.devices.length){
            return true;
        };

        return false;
    }

    unSelectAllDevices(){
        this.devices.map(a=>a.isSelected=false);
    }

    scrollToClientMachine() {
        if (this.clientMachineRows && this.clientMachineRows.length > 0 && this.rowNumber && this.rowNumber >= 0) {
            if(this.clientMachineRows.find(x => x.nativeElement.id === 'selected_' + this.rowNumber)){
                let elementRef = this.clientMachineRows.find(x => x.nativeElement.id === 'selected_' + this.rowNumber);
                elementRef.nativeElement.scrollIntoView({ block: 'center' })
            }
            this.rowNumber = null;
        }
    }

    openClientMachine(clientMachineId: number, rowNumber: number){
        this.rowNumber = rowNumber;
        this.router.navigate(['/sys/clock/hardware/clientmachine/' + clientMachineId]);
    }
}
