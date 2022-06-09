import { Component, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { PublicApiDataService } from "../../services/public-api/public-api-data.service";
import { UserInfo, UserType } from "@ds/core/shared";
import { AccountService } from "@ds/core/account.service";
import { switchMap, takeUntil, tap } from "rxjs/operators";
import { ActionTypes } from "@ds/core/constants/action-types";
import { EMPTY, NEVER, of, Subject } from "rxjs";
import { IntegrationsService } from "../../services/intergrations.service";
import { HttpErrorResponse } from "@angular/common/http";
import { EmployeeNavigatorEmpRequiredFields, IntegrationSyncLog } from '@models';
import { MatSort, Sort } from '@angular/material/sort';
import { min } from "lodash";
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
  selector: "ds-employee-navigator-dashboard",
  templateUrl: "./integration-sync-dashboard.component.html",
  styleUrls: ["./integration-sync-dashboard.component.scss"],
})
export class IntegrationSyncDashboardComponent implements OnInit {
  destroy$ = new Subject();
  user: UserInfo;
  isSystemAdmin = false;
  hasEmployeeNavigatorFeature = false;
  animationState = false;
  animationWithState = false;
  tableData = null;
  tableColumns = ["date", "time", "from", "status"];
  tableFilters = this.createTableFilters();

  showReportsButton = false;
  invalidEmployees: EmployeeNavigatorEmpRequiredFields[];
  filteredEmployees: EmployeeNavigatorEmpRequiredFields[];
  invalidEmpsColumns = ["employee", "separationDate", "missingFields"];
  isLoading = true;

  @ViewChild(MatSort, { static: false }) sort: MatSort;

  constructor(
    private fb: FormBuilder,
    private service: PublicApiDataService,
    private account: AccountService,
    private integrations: IntegrationsService,
    private message: NgxMessageService,
  ) {}

  ngOnInit() {
    this.account.getUserInfo()
      .pipe(
        takeUntil(this.destroy$),
        switchMap(user => {
          this.user = user;
          this.isSystemAdmin = this.user.userTypeId === UserType.systemAdmin;
          return this.account.canPerformActions(ActionTypes.EmployeeNavigator.ViewReports);
        }),
        switchMap(res => {
          this.showReportsButton = res === true;
          if (this.showReportsButton) {
            this.hasEmployeeNavigatorFeature = true;
            return this.integrations.getInvalidEmployeeNavigatorEmployees();
          } else {
            this.hasEmployeeNavigatorFeature = false;
          }

          return of(EMPTY);
        })
      ).subscribe((emps: EmployeeNavigatorEmpRequiredFields[]) => {
        if (emps && emps.length) this.invalidEmployees = emps;
        this.filteredEmployees = emps;
        this.changeEmployeeStatus(1)
        this.isLoading = false;
      });
  }

  sendForRetry(row: IntegrationSyncLog) {
    console.dir(row);

    setTimeout(() => {
      this.animationState = true;
      this.animationWithState = !this.animationWithState;
    }, 1);
  }

  downloadDemographicReport() {
    this.account
      .getUserInfo()
      .pipe(
        tap(() => this.message.loading(true, 'Downloading report...')),
        switchMap((user) =>
          this.service.getEmployeeDemographicReport(user.lastClientId)
        ),
        tap(() => this.message.loading(false))
      )
      .subscribe((reportData) => {
        window.open(window.URL.createObjectURL(reportData))
      }, (error: HttpErrorResponse) => {
        this.message.setErrorMessage("An error occurred trying to download the report.");
      });
  }

  downloadEmployeeDeductionReport() {
    this.account
      .getUserInfo()
      .pipe(
        tap(() => this.message.loading(true, 'Downloading report...')),
        switchMap((user) =>
          this.service.getEmployeeDeductionsReport(user.lastClientId)
        ),
        tap(() => this.message.loading(false))
      )
      .subscribe((reportData) => {
        window.open(window.URL.createObjectURL(reportData))
      }, (error: HttpErrorResponse) => {
        this.message.setErrorMessage("An error occurred trying to download the report.");
      });
  }

  downloadClientDeductionReport() {
    this.account
      .getUserInfo()
      .pipe(
        tap(() => this.message.loading(true, 'Downloading report...')),
        switchMap((user) =>
          this.service.getClientDeductionReport(user.lastClientId)
        ),
        tap(() => this.message.loading(false))
      )
      .subscribe((reportData) => {
        window.open(window.URL.createObjectURL(reportData))
      }, (error: HttpErrorResponse) => {
        this.message.setErrorMessage("An error occurred trying to download the report.");
      });
  }
  
  changeEmployeeStatus(value: number) {
    this.invalidEmployees = this.filteredEmployees;
    if (value == 0) {
      this.invalidEmployees = this.invalidEmployees.filter(x => x.isActive || !x.isActive)
    }
    else if (value == 1) {
      this.invalidEmployees = this.invalidEmployees.filter(x => x.isActive);
    }
  }

  sortData(sort: Sort) {
    const data = this.invalidEmployees.slice();
    if (!sort.active || sort.direction === '') {
      this.invalidEmployees = data.sort((a, b) => {
        return this.compare(a.employeeId.toString(), b.employeeId.toString(), true);
      });
      return;
    }

    this.invalidEmployees = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'employee':
          return this.compare(a.lastName.toLowerCase(), b.lastName.toLowerCase(), isAsc);
        case 'separationDate':
          if (a.isActive && a.separationDate !== null) {
            a.separationDate = null;
          } if (b.isActive && b.separationDate !== null) {
            b.separationDate = null;
          }
           return this.compare(new Date(a.separationDate).getTime(), new Date(b.separationDate).getTime(), isAsc);
        case 'missingFields':
          return this.compare(a.missingFields[0].toLowerCase(), b.missingFields[0].toLowerCase(), isAsc);
      }
    });

  }

  compare(a: number | string, b: number | string, isAsc: boolean) {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }

  private createTableFilters(): FormGroup {
    return this.fb.group({
      from: this.fb.control(""),
      to: this.fb.control(""),
      fromClient: this.fb.control(""),
      status: this.fb.control(""),
    });
  }
}
