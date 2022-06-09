import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { DateRange } from '@ds/analytics/shared/models/DateRange.model';
import { AnalyticsApiService } from '@ds/analytics/shared/services/analytics-api.service';
import * as moment from "moment";
import { MatSort } from '@angular/material/sort';
import { AccountService } from '@ds/core/account.service';
import { MOMENT_FORMATS, UserInfo } from '@ds/core/shared';
import { switchMap, tap } from 'rxjs/operators';
import { AnalyticsService } from '@ds/analytics/shared/services/analytics.service';
import { UserPerformanceDashboard, UserPerformanceDashboardEmployee } from '@ds/analytics/models';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'ds-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.scss']
})
export class UserInfoComponent implements OnInit {
  @Input() employeeIds: number[];
  @Input() dateRange: DateRange;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  dataSource: MatTableDataSource<UserPerformanceDashboardEmployee>;
  displayedColumns: string[] = ['firstName', 'userType','username', 'login', 'assignedEmployee', 'supervisor', 'email', 'enabled', 'tempAccess', 'payTypes', 'payRates', 'i9', 'addEmployee', 'resetPassword', 'approveTimeCards'];

  //card info
  cardType: string = "graph";
  title: string = "Employee Self Service"; //"Active Users";

  //empty state info
  loaded: boolean = false;
  emptyState: boolean = false;

  clientId: number = 0;
  companyAdminData: string[] = [];
  supervisorData: string[] = [];
  employeeData: string[] = [];
  companyAdmin: number = 0;
  supervisor: number = 0;
  employee: number = 0;
  user: UserInfo;

  constructor(
    private analyticsService: AnalyticsService,
    private accountService: AccountService,
    ) { }

  ngOnInit() {
      this.accountService.getUserInfo()
        .pipe(
            switchMap(user => {
                this.user = user;
                this.clientId = this.user.selectedClientId();
                return this.analyticsService.getUserPerformanceDashboard(
                    this.user.selectedClientId(),
                    this.dateRange.StartDate,
                    this.dateRange.EndDate,
                    this.employeeIds
                );
            }),
            tap(res => {
                if (res && res.isInFlight) return;
                if (!res) {
                    this.emptyState = true;
                    this.loaded = true;
                    return;
                }

                // combine these two lists and then sort them... this will be all users/non-users compiled
                const users = [...res.eesWithUser, ...res.eesWoUser].sort((a, b) => a.lastName < b.lastName ? -1 : 1);
                this.dataSource = new MatTableDataSource<UserPerformanceDashboardEmployee>(users);
                this.dataSource.sort = this.sort;
                this.dataSource.sortingDataAccessor = this.applySort();
                this.dataSource.paginator = this.paginator;
                this.loaded = true;
            })
        )
        .subscribe();
  }

  private applySort() {
      return (item, sortColumn) => {
        switch (sortColumn) {
            case 'firstName':
                return item.lastName;
            case 'userType':
                return item.userTypeId;
            case 'username':
                return item.username;
            case 'login':
              return item.lastLoginDate;
            case 'assignedEmployee':
                return item.assignedEmployee;
            case 'supervisor':
                return item.supervisorId;
            case 'email':
                return item.emailAddress;
            case 'enabled':
                return item.isPasswordEnabled;
            case 'tempAccess':
                return item.tempEnableToDate;
            case 'payTypes':
              return item.viewEmployeePayTypes;
            case 'payRates':
                return item.viewEmployeeRateTypes;
            case 'i9':
                return item.certifyI9;
            case 'addEmployee':
                return item.addEmployee;
            case 'resetPassword':
                return item.resetPassword;
            case 'approveTimeCards':
                return item.approveTimecards;
            default:
                return item[this.sort.active];
        }
      };
  }
}
