import { Component, OnInit } from '@angular/core';
import { IBasicPayrollHistory, IPaycheckEarningsDetail,
    IPaycheckInfo, IPaycheckEarningsHours, IPaystubOptions } from '@ds/payroll';
import * as moment from 'moment';
import { forkJoin } from 'rxjs';
import {  UserInfo } from '@ds/core/shared';
import { IPaycheckHistory } from '@ajs/payroll/employee/shared';
import { PayrollService } from '@ds/payroll/shared/payroll.service';
import { AccountService } from '@ds/core/account.service';

@Component({
  selector: 'ds-paychecks-summary',
  templateUrl: './paychecks-summary.component.html',
  styleUrls: ['./paychecks-summary.component.scss']
})
export class PaychecksSummaryComponent implements OnInit {
  isLoading = false;
  showCheckDetails = true;
  showEarnings = true;
  showEarningsHours = true;
  showEmployerBenefits = true;
  showOptions = true;
  startDate: Date;
  endDate: Date;
  employeePaycheckHistory: IPaycheckHistory[];
  employeeCurrentPaycheck: IPaycheckHistory;
  basicPayrolls: IBasicPayrollHistory[];
  currentPayroll: IBasicPayrollHistory;
  fullPaycheckDetail: IPaycheckInfo[];
  currentPaycheckDetail: IPaycheckInfo;
  fullPaycheckEarnings: IPaycheckEarningsDetail[];
  fullPaycheckEarningsHours: IPaycheckEarningsHours[];
  paystubOptions: IPaystubOptions;


  constructor(
    private PayrollApiService: PayrollService,
    private AccountService: AccountService
  ) { }

  ngOnInit() {
    this.loadPage();
  }

  loadPage() {
    // FIXME Stolen from old Brandon code, which is where we call the GetPaycheckHistoryByEmployeeId
    this.startDate = moment().subtract(3, 'months').toDate();
    this.endDate = moment().toDate();
    this.AccountService.getUserInfo().subscribe((user: UserInfo) => {
      this.PayrollApiService.getCurrentPayrollByClientId(user.clientId)
      .subscribe((payroll: IBasicPayrollHistory) => {
        this.currentPayroll = payroll;
        forkJoin(
          this.PayrollApiService
          .getEmployeePayHistoryByEmployeeId(user.userEmployeeId,user.clientId,payroll.periodStart,payroll.periodEnded),
          this.PayrollApiService.getCurrentPaycheckWithFullDetail(user.userEmployeeId),
          this.PayrollApiService.getCurrentPaycheckEarningsDetail(user.userEmployeeId),
          this.PayrollApiService.getCurrentPaycheckEarningsHours(user.userEmployeeId),
          this.PayrollApiService.getCurrentPaycheckStubOptions(user.userEmployeeId)

        )
        .subscribe((results) => {
          this.employeePaycheckHistory = results[0];
          this.employeeCurrentPaycheck = this.employeePaycheckHistory[0];
          this.fullPaycheckDetail = results[1];
          this.currentPaycheckDetail = this.fullPaycheckDetail[0];
          if(this.fullPaycheckDetail == undefined || this.fullPaycheckDetail === undefined) {
            this.showCheckDetails = false;
            this.showEmployerBenefits = false;
          } else if (this.fullPaycheckDetail[0] == undefined ||
            this.fullPaycheckDetail[0] === null) {
            this.showEmployerBenefits = false;
            this.showCheckDetails = false;
          } else if (this.fullPaycheckDetail[0].companyPaidBenefits == undefined ||
            this.fullPaycheckDetail[0].companyPaidBenefits === null) {
            this.showEmployerBenefits = false;
          }
          this.fullPaycheckEarnings = results[2];
          if(this.fullPaycheckEarnings == undefined || this.fullPaycheckEarnings === undefined) {
            this.showEarnings = false;
          } else if(this.fullPaycheckEarnings.length < 1) {
            this.showEarnings = false;
          }
          this.fullPaycheckEarningsHours = results[3];
          if(this.fullPaycheckEarningsHours == undefined || this.fullPaycheckEarningsHours === null) {
            this.showEarningsHours = false;
          } else if(this.fullPaycheckEarningsHours.length < 1) {
            this.showEarningsHours = false;
          }
          this.paystubOptions = results[4];
          if(this.paystubOptions == undefined || this.paystubOptions === null) {
            this.showOptions = false;
          } else if(this.paystubOptions[0] == undefined || this.paystubOptions[0] === null) {
            //YEAAAA I DON'T LIKE THIS EITHER. IT IS BRINGING BACK A BLANK ARRAY EVEN IF THE ITEM IS NULL
            //THIS IS HOW I HANDLED IT I AM SORRY. IF YOU KNOW SOMETHING BETTER PLEASE DO IT.
            this.showOptions = false;
          }

          this.isLoading = true;
        });
      });
    });
  }

}
