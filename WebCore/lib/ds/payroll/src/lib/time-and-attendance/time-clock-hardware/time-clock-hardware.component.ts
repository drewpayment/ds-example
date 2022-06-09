import { Component, OnInit, ViewChild, Input, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { UserInfo, UserType } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { TimeAndAttendanceService } from '@ds/payroll/time-and-attendance/time-and-attendance.service';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import * as moment from 'moment';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { distinctUntilChanged, map, tap, switchMap, takeUntil } from 'rxjs/operators';
import { Observable, Subject } from 'rxjs';
import { IClockClientHardware } from '@ds/payroll/time-and-attendance/shared/ClockClientHardware.model';
import { TimeClockHardwareEditDialogComponent } from './time-clock-hardware-edit-dialog/time-clock-hardware-edit-dialog.component';
import { HttpErrorResponse } from '@angular/common/http';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'ds-time-clock-hardware',
  templateUrl: './time-clock-hardware.component.html',
  styleUrls: ['./time-clock-hardware.component.css']
})
export class TimeClockHardwareComponent implements OnInit {
  isLoading = true;
  user: UserInfo;
  clientId: number;
  hasFullUpdateAccess: boolean = false;
  hasPartialUpdateAccess: boolean = false;
  hasViewAccess: boolean = false;

  displayedColumns: string[] = ['description', 'isRental', 'serialNumber', 'ipAddress', 'actionBtn'];
  dataSource: MatTableDataSource<IClockClientHardware>;

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  constructor(
    private accountService: AccountService,
    private service: TimeAndAttendanceService,
    private confirmService: DsConfirmService,
    private msg: DsMsgService,
    private dialog:MatDialog,
   ){
    this.dataSource = new MatTableDataSource<IClockClientHardware>();
  }

  ngOnInit() {
      this.dataSource.paginator = this.paginator;

      this.accountService.getUserInfo().pipe(
        switchMap(user => {
            this.user = user;
            this.clientId = this.user.lastClientId || this.user.clientId;
            switch(this.user.userTypeId) {
              case UserType.systemAdmin: {
                this.hasFullUpdateAccess = true;
                this.hasPartialUpdateAccess = true;
                this.hasViewAccess = true;
                break;
              }
              case UserType.companyAdmin: {
                this.hasFullUpdateAccess = false;
                this.hasPartialUpdateAccess = false;
                this.hasViewAccess = true;
                break;
              }
              case UserType.supervisor: {
                this.hasFullUpdateAccess = false;
                this.hasPartialUpdateAccess = false;
                this.hasViewAccess = true;
                break;
              }
              default: {
                 break;
              }
            }
            return this.service.getClockClientHardwares(this.user.clientId);
        })
        ).subscribe(hardwares => {
          this.dataSource.data = hardwares;
          this.isLoading = false;
    });
  }

  showUpdateHardwareDialog(clockHardware: IClockClientHardware):void {
    const ref = this.renderUpdateHardwareDialog(clockHardware);

    ref.afterClosed().subscribe((result:IClockClientHardware) => {
        if(!result) return;

        this.msg.setTemporarySuccessMessage('Successfully saved Clock hardware.', 5000);
        this.service.getClockClientHardwares(this.user.lastClientId || this.user.clientId)
            .subscribe(clockHardwares => {
                this.dataSource.data = clockHardwares;
        });
    });
  }

  private renderUpdateHardwareDialog(clockHardware: IClockClientHardware):MatDialogRef<TimeClockHardwareEditDialogComponent, any> {
    return this.dialog.open(TimeClockHardwareEditDialogComponent, {
        width: '600px',
        data: {
            user: this.user,
            clockHardware: clockHardware
        }
    });
  }

  deleteClockHardware(clockHardware: IClockClientHardware):void {
    this.confirmService.show({size: 'md'}, {
        bodyText: "Are you sure you wish to delete this Company Hardware?",
        swapOkClose: true,
        actionButtonText: "Delete",
        closeButtonText: "Cancel"
    }).then(result => {
    this.service.deleteClockClientHardware(clockHardware.clockClientHardwareId)
      .subscribe(clockHardware => {
        this.msg.setTemporarySuccessMessage('Successfully deleted Clock hardware.', 5000);
        this.service.getClockClientHardwares(this.user.lastClientId || this.user.clientId)
          .subscribe(clockHardwares => {
            this.dataSource.data = clockHardwares;
        });
      },
      (error:HttpErrorResponse) => {
        this.msg.showWebApiException(error.error);
      });
    });
  }
}
