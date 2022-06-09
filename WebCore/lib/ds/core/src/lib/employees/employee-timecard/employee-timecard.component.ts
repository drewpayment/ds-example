import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import * as moment from 'moment';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { tap, switchMap, skipWhile, catchError } from 'rxjs/operators';
import { MatInput } from '@angular/material/input';
import { AccountService } from '@ds/core/account.service';
import { Observable, forkJoin, Subject, of, concat, throwError } from 'rxjs';
import { PayrollService } from '@ds/payroll/shared/payroll.service';
import { ClockService } from '@ds/core/employee-services/clock.service';
import { IBasicPayrollHistory }    from '@ds/payroll/shared/payroll-history-info.model';
import { EmployeeSearchService } from "@ajs/employee/search/shared/employee-search.service";
import { HttpErrorResponse } from '@angular/common/http';
import { PayFrequencyTypeEnum } from '@ajs/employee/hiring/shared/models';
import { UserType } from '@ds/core/shared';

@Component({
  selector: 'ds-employee-timecard',
  templateUrl: './employee-timecard.component.html',
  styleUrls: ['./employee-timecard.component.scss'],
  providers: [],
})
export class EmployeeTimecardComponent implements OnInit {
  @ViewChild('fromInput', { read: MatInput, static: false }) fromInput: MatInput;
  @ViewChild('toInput',   { read: MatInput, static: false }) toInput:   MatInput;

  @Input() employeeId:  number;

  payPeriods:           IBasicPayrollHistory[];
  currPeriod:           IBasicPayrollHistory;
  fromDate:             Date;
  toDate:               Date;

  userinfo:             any;
  isUserEmployee:       boolean;
  loading:              boolean;
  selectedPayrollId:    number;

  clientId:             number;
  payrollId:            number;
  isCustomPeriod:       boolean;

  subscriptionHandler$: Observable<any>;

  isFilteringData = false;

  constructor(
    private api: PayrollService,
    private msgSvc: DsMsgService,
    private accountService: AccountService,
    public clockService: ClockService,
    private searchService: EmployeeSearchService,
  ) { }

  ngOnInit() {
    this.loading = true;

    this.subscriptionHandler$ = this.accountService.getUserInfo().pipe(
        switchMap(user => {
            this.clientId = user.clientId;
            if ( user.userTypeId == UserType.employee) //if user is employee, do not account for lastEmployeeId
              this.employeeId = this.employeeId ? this.employeeId : ( user.employeeId || user.userEmployeeId );
            else
              this.employeeId = this.employeeId ? this.employeeId : (user.lastEmployeeId || user.employeeId || user.userEmployeeId);
            this.isUserEmployee = user.userTypeId == UserType.employee;
            return this.api.getPayrollHistoryByEmployeeId(this.clientId, this.employeeId);
        }),
        skipWhile(periods => !periods),
        tap(periods => {
          this.refreshFilters(periods);
          this.loading = false;
        }),
        catchError((err:HttpErrorResponse, caught) => {
          this.refreshFilters(null);
          this.loading = false;

          if(err.error.errors && err.error.errors.length > 0)
            this.msgSvc.setTemporaryMessage(err.error.errors[0].msg, this.msgSvc.messageTypes.error, 6000);
          else
            this.msgSvc.showWebApiException(err.error);

          return throwError("Error Fetching Pay Periods");
        })
      );

    // Employee Selection changed
    this.searchService.hookToEmployeeChanged((newEmpId: number)=>{
      this.employeeId = newEmpId;
      this.loading = true;

      // update the Principal Dominion object with selected EmployeeId
      (<HTMLInputElement>document.querySelector("input[id$='hdnEmployeeId']")).value = newEmpId.toString();
      (<HTMLInputElement>document.querySelector("input[id$='btnRefreshPage']")).click();

      this.api.getPayrollHistoryByEmployeeId(this.clientId, this.employeeId).subscribe(periods => {
        this.refreshFilters(periods);
        this.loading = false;
      },(err)=>{
        this.refreshFilters(null);
          this.loading = false;

          if(err.error.errors && err.error.errors.length > 0)
            this.msgSvc.setTemporaryMessage(err.error.errors[0].msg, this.msgSvc.messageTypes.error, 6000);
          else
            this.msgSvc.showWebApiException(err.error);

          return throwError("Error Fetching Pay Periods");
      });
    });
  }

  refreshFilters(periods:IBasicPayrollHistory[]){
    this.selectedPayrollId = 0;
    this.isCustomPeriod = false;
    //this.loading = true;

    if(periods && periods.length > 0){
      let today = moment();
      if( moment(periods[0].periodEnded) > today){
        this.currPeriod = periods.shift();
        this.payPeriods = periods;
      } else {
        // In case if there is no payroll for current date, make one
        this.currPeriod = <IBasicPayrollHistory>{
          periodStart: periods[0].periodStart,
          periodEnded: periods[0].periodEnded,
          payrollId: 0,
          payFrequencyType: periods[0].payFrequencyType,
        };
        let daysCount = moment( this.currPeriod.periodEnded).diff(moment(this.currPeriod.periodStart), 'days') + 1;

        if(this.currPeriod.payFrequencyType == PayFrequencyTypeEnum.AlternateBiWeekly ||
          this.currPeriod.payFrequencyType == PayFrequencyTypeEnum.BiWeekly ||
          this.currPeriod.payFrequencyType == PayFrequencyTypeEnum.Weekly){
          // Move to a date range that falls today
          while( moment(this.currPeriod.periodEnded) < today ){
            this.currPeriod.periodEnded = this.addDays(this.currPeriod.periodEnded, daysCount);
            this.currPeriod.periodStart = this.addDays(this.currPeriod.periodStart, daysCount);
          }
        }
        else
        {
          this.currPeriod.periodStart = new Date(this.currPeriod.periodStart);
          this.currPeriod.periodEnded = new Date(this.currPeriod.periodEnded);

          // Move to a date range that falls today
          while( moment(this.currPeriod.periodEnded) < today ){
            switch (this.currPeriod.payFrequencyType) {
              case PayFrequencyTypeEnum.Quarterly:
                this.currPeriod.periodStart.setMonth(this.currPeriod.periodStart.getMonth()+3);

                this.currPeriod.periodEnded.setDate(1);
                this.currPeriod.periodEnded.setMonth(this.currPeriod.periodEnded.getMonth()+4);
                this.currPeriod.periodEnded = this.addDays(this.currPeriod.periodEnded, -1);
                break;
              case PayFrequencyTypeEnum.Monthly:
                this.currPeriod.periodStart.setMonth(this.currPeriod.periodStart.getMonth()+1);

                this.currPeriod.periodEnded.setDate(1);
                this.currPeriod.periodEnded.setMonth(this.currPeriod.periodEnded.getMonth()+2);
                this.currPeriod.periodEnded = this.addDays(this.currPeriod.periodEnded, -1);
                break;
              case PayFrequencyTypeEnum.SemiMonthly:
                  if(this.currPeriod.periodStart.getDate() == 1){
                    this.currPeriod.periodEnded.setMonth(this.currPeriod.periodEnded.getMonth()+1);
                    this.currPeriod.periodEnded.setDate(1);
                    this.currPeriod.periodEnded = this.addDays(this.currPeriod.periodEnded, -1);

                    this.currPeriod.periodStart.setDate(16);
                  } else {
                    this.currPeriod.periodStart.setDate(1);
                    this.currPeriod.periodStart.setMonth(this.currPeriod.periodStart.getMonth()+1);
                    this.currPeriod.periodEnded.setDate(15);
                    this.currPeriod.periodEnded.setMonth(this.currPeriod.periodEnded.getMonth()+1);
                  }
                  break;
              default:
                break;
            }
          }
        }

        this.payPeriods = periods;
      }
      this.clockService.setPeriodFilterParams(0, false, this.currPeriod.periodStart, this.currPeriod.periodEnded);
      this.fromDate = this.currPeriod.periodStart;
      this.toDate   = this.currPeriod.periodEnded;
    } else {
      // No payroll periods for this User, so initialize weekly timecard
      this.initWeeklyActivity();
    }
    //this.loading = false;
  }

  initWeeklyActivity(){
    this.fromDate = new Date();
    this.toDate   = new Date();
    var curDayStart = this.fromDate.getDay();
    this.fromDate.setDate(this.fromDate.getDate() - curDayStart);
    this.toDate.setDate(this.fromDate.getDate() + 6);

    this.currPeriod = this.currPeriod = <IBasicPayrollHistory>{
      periodStart: this.fromDate,
      periodEnded: this.toDate,
      payrollId: 0,
    };

    this.clockService.setPeriodFilterParams(0, false, this.currPeriod.periodStart, this.currPeriod.periodEnded);
  }

  renderDateRange(k:IBasicPayrollHistory){
    return moment(k.periodStart).format('MM/DD/YYYY') + ' - ' + moment(k.periodEnded).format('MM/DD/YYYY');
  }
  public addDays(currentDate: Date, days: number): Date {
    var k = new Date(currentDate);
    return new Date(k.getTime() + (days * 24 * 60 * 60 * 1000));
  }

  payPeriodSelectionChanged()
  {
    this.isCustomPeriod = false;
    if(this.selectedPayrollId == -1){
      this.isCustomPeriod = true;
    }
    else {
      if(this.selectedPayrollId > 0){
        let selectedPayPeriod = this.payPeriods.find(x => x.payrollId == this.selectedPayrollId);
        this.clockService.setPeriodFilterParams(this.selectedPayrollId, false, selectedPayPeriod.periodStart, selectedPayPeriod.periodEnded);
      } else {
        this.clockService.setPeriodFilterParams(0, false, this.currPeriod.periodStart, this.currPeriod.periodEnded);
      }
    }
  }
  goClick()
  {
    if(this.fromDate <= this.toDate) {
        this.isFilteringData = true;
        this.clockService.setPeriodFilterParams(-1, true, this.fromDate, this.toDate);
    }

  }


}
