import { Component, OnInit, Input } from '@angular/core';
import { DateRange } from '@ds/analytics/shared/models/DateRange.model';
import { AnalyticsApiService } from '@ds/analytics/shared/services/analytics-api.service';
import { MatDialog } from '@angular/material/dialog';
import * as moment from "moment";
import { UserTypeDialogComponent } from './user-type-dialog/user-type-dialog.component';
import { AccountService } from '@ds/core/account.service';
import { MOMENT_FORMATS, UserInfo, UserType } from '@ds/core/shared';
import { UserPerformanceDashboard, UserPerformanceDashboardEmployee } from '@ds/analytics/models';
import { AnalyticsService } from '@ds/analytics/shared/services/analytics.service';
import { switchMap, tap } from 'rxjs/operators';

@Component({
  selector: 'ds-user-type',
  templateUrl: './user-type.component.html',
  styleUrls: ['./user-type.component.css']
})
export class UserTypeComponent implements OnInit {
  @Input() employeeIds: number[];
  @Input() dateRange: DateRange;

  //card info
  cardType: string = "graph";
  title: string = "User Types";

  //empty state info
  loaded: boolean;
  emptyState: boolean = false;

  //data
  userData: UserPerformanceDashboardEmployee[] = [];
  companyAdminData: UserPerformanceDashboardEmployee[] = [];
  supervisorData: UserPerformanceDashboardEmployee[] = [];
  employeeData: UserPerformanceDashboardEmployee[] = [];
  companyAdmin: number = 0;
  supervisor: number = 0;
  employee: number = 0;
  user: UserInfo;

  constructor(
    private analyticsService: AnalyticsService,
    private accountService: AccountService,
    private dialog: MatDialog) { }

  ngOnInit() {
    this.accountService.getUserInfo()
        .pipe(
            switchMap(user => {
                this.user = user;
                return this.analyticsService.getUserPerformanceDashboard(
                    this.user.selectedClientId(),
                    this.dateRange.StartDate,
                    this.dateRange.EndDate,
                    this.employeeIds);
            }),
            tap(res => {
                if (res && res.isInFlight) return;
                if (!res) {
                    this.emptyState = true;
                    this.loaded = true;
                    return;
                }

                const emps = res.activeUsers.filter(u => u.userTypeId === UserType.employee);
                const sups = res.activeUsers.filter(u => u.userTypeId === UserType.supervisor);
                const cas = res.activeUsers.filter(u => u.userTypeId === UserType.companyAdmin);

                this.employeeData = emps;
                this.supervisorData = sups;
                this.companyAdminData = cas;

                this.employee = emps != null ? emps.length : 0;
                this.supervisor = sups != null ? sups.length : 0;
                this.companyAdmin = cas != null ? cas.length : 0;

                this.loaded = true;
            })
        )
        .subscribe();
  }

  openComanyAdminDialog(){
    var config = {
      width: '1000px',
      data: {
        employees: this.companyAdminData,
        title: 'Company Administrator',
      }
    };
    const dialogRef = this.dialog.open(UserTypeDialogComponent, config);
  }

  openSupervisorDialog(){
    var config = {
      width: '1000px',
      data: {
        employees: this.supervisorData,
        title: 'Supervisor',
      }
    };
    const dialogRef = this.dialog.open(UserTypeDialogComponent, config);
  }

  openEmployeeDialog(){
    var config = {
      width: '1000px',
      data: {
        employees: this.employeeData,
        title: 'Employee',
      }
    };
    const dialogRef = this.dialog.open(UserTypeDialogComponent, config);
  }

}
