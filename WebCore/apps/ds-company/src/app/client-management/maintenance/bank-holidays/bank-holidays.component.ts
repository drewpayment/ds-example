import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { DsStyleLoaderService } from '@ajs/ui/ds-styles/ds-styles.service';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { AccountService } from "@ds/core/account.service";
import { BankHolidayEditDialogComponent } from './bank-holiday-edit-dialog/bank-holiday-edit-dialog.component';
import { UserInfo } from '@ds/core/shared';
import { ConfirmModalComponent } from '@ds/core/resources/confirm-modal/confirm-modal.component';
import { MaintenanceApiService } from '../../services/maintenance-api.service';
import { IBankHoliday } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-bank-holidays',
  templateUrl: './bank-holidays.component.html',
  styleUrls: ['./bank-holidays.component.scss']
})
export class BankHolidaysComponent implements OnInit {
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;

    user: UserInfo;

    bankHoliday: IBankHoliday;
    bankHolidays: IBankHoliday[] = [];
    tableData: any;

    searchLength: number;
    loading: boolean = true;

    displayedColumns: string[] = ['name', 'date', 'actionsColumn'];
    pageOptions = [];

    selectedYear: number;
    years: any;

    @Input()
    clientId:number;

    constructor(
      private dialog:MatDialog,
      private accountService:AccountService,
      private apiSvc:MaintenanceApiService,
      private ngxMsgSvc:NgxMessageService
      ) {
    }

    ngOnInit() {
      const now = new Date().getUTCFullYear();
      this.years = Array(now - (now - 4)).fill('').map((v, idx) => now + idx) as Array<number>;

      this.accountService.getUserInfo().subscribe(user => {
        this.user = user;
        this.loadData(now);
      });
    }

    ngAfterViewChecked() {
      //this.styles.useMainStyleSheet();
    }

    loadData(year: number) {
      this.loading = true;
      this.selectedYear = year;
      this.apiSvc.getBankHolidays(year).subscribe(
        (data) => {
        this.bankHolidays = data;
        this.searchLength = data.length;
        this.tableData = new MatTableDataSource<IBankHoliday>(data);
        this.pageOptions = [10, 25, 50 ];
        setTimeout(() => this.tableData.paginator = this.paginator);
        setTimeout(() => this.tableData.sort = this.sort);
        this.loading = false;
      },
      (error: HttpErrorResponse) => {
          this.loading = false;
          this.ngxMsgSvc.setErrorResponse(error.error);
      });
    }

    onYearSelected(year: number){
      this.loadData(year);
    }

    deleteHoliday(bankHolidayId: number) {
      const confirmDialogRef = this.dialog.open(ConfirmModalComponent, {
          width: "350px",
          data: {
              displayText: 'Are you sure you want to delete this holiday?',
              confirmButtonText: 'Delete',
              cancelButtonText: 'Cancel',
              swapOkClose: true
          }
      });

      confirmDialogRef.afterClosed().subscribe((confirmed: boolean) => {
          if (confirmed) {
            this.loading = true;
            this.apiSvc.deleteBankHoliday(bankHolidayId)
                .subscribe(data => {
                    _.remove(this.bankHolidays, h => h.bankHolidayId === bankHolidayId);

                    this.searchLength = this.bankHolidays.length;
                    this.tableData = new MatTableDataSource<IBankHoliday>(this.bankHolidays);
                    setTimeout(() => this.tableData.paginator = this.paginator);
                    setTimeout(() => this.tableData.sort = this.sort);

                    this.loading = false;
                    this.ngxMsgSvc.setSuccessMessage('Holiday deleted successfully.', 5000);
                },
                (error: HttpErrorResponse) => {
                    this.loading = false;
                    this.ngxMsgSvc.setErrorResponse(error.error);
                });
          }
      });
  }

    copyBankHolidays(year: number) {
      this.loading = true;
      this.apiSvc.copyBankHolidays(year)
          .subscribe(data => {
              this.bankHolidays = data;
              this.tableData = new MatTableDataSource<IBankHoliday>(this.bankHolidays);
              this.loading = false;
              this.ngxMsgSvc.setSuccessMessage('Holiday(s) copied successfully.', 5000);
          },
          (error: HttpErrorResponse) => {
              this.ngxMsgSvc.setErrorResponse(error.error);
          });
    }

    showAddEditDialog(bankHoliday: IBankHoliday): void {
      this.dialog.open(BankHolidayEditDialogComponent, {
          width: '410px',
          data: {
              user: this.user,
              bankHoliday: bankHoliday
          }
      })
      .afterClosed()
          .subscribe(result => {
          if (result == null) return;

          this.loading = true;
          this.apiSvc.saveBankHoliday(result).subscribe(data => {
            if (!_.isEmpty(data)) {
              this.loadData(this.selectedYear);
              this.loading = false;
              this.ngxMsgSvc.setSuccessMessage('Holiday saved successfully.', 5000);
            }
          },
          (error: HttpErrorResponse) => {
              this.loading = false;
              this.loadData(this.selectedYear);
              if (error.error.errors[0].msg.indexOf("UNIQUE KEY constraint") !== -1)
                this.ngxMsgSvc.setErrorMessage('A holiday with this date already exists.', 5000);
              else
                this.ngxMsgSvc.setErrorResponse(error.error);
          });
      });
  }
}
