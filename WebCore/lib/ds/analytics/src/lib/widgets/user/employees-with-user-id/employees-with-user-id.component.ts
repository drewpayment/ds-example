import { Component, OnInit, Input, OnDestroy } from "@angular/core";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { ChartType, ChartOptions } from "chart.js";
import { AnalyticsApiService } from "@ds/analytics/shared/services/analytics-api.service";
import { MatDialog } from "@angular/material/dialog";
import * as moment from "moment";
import { EmployeesWithUserIDDialogComponent } from "./employees-with-user-id-dialog/employees-with-user-id-dialog.component";
import { AccountService } from "@ds/core/account.service";
import { MOMENT_FORMATS, UserInfo, UserType } from "@ds/core/shared";
import { AnalyticsService } from '@ds/analytics/shared/services/analytics.service';
import { switchMap, tap } from 'rxjs/operators';
import { UserPerformanceDashboardEmployee, UserPerformanceDashboard } from '@ds/analytics/models';

@Component({
    selector: "ds-employees-with-user-id",
    templateUrl: "./employees-with-user-id.component.html",
    styleUrls: ["./employees-with-user-id.component.css"],
})
export class EmployeesWithUserIDComponent implements OnInit, OnDestroy {
    user: UserInfo;
    @Input() employeeIds: number[];
    @Input() dateRange: DateRange;

    //card info
    cardType: string = "info";

    //empty state info
    loaded: boolean;
    emptyState: boolean = false;

    //data
    userData: UserPerformanceDashboardEmployee[] = [];
    companyAdminData: UserPerformanceDashboardEmployee[] = [];
    supervisorData: UserPerformanceDashboardEmployee[] = [];
    employeeData: UserPerformanceDashboardEmployee[] = [];
    companyAdminCount: number = 0;
    supervisorCount: number = 0;
    employeeCount: number = 0;

    //graph data
    infoData: InfoData;

    constructor(
        private analyticsApi: AnalyticsApiService,
        private accountService: AccountService,
        private dialog: MatDialog,
        private analyticsService: AnalyticsService,
    ) {}

    ngOnInit() {
        this.accountService.getUserInfo()
            .pipe(
                switchMap(user => {
                    this.user = user;
                    return this.analyticsService.getUserPerformanceDashboard(
                        this.user.selectedClientId(),
                        moment(this.dateRange.StartDate),
                        moment(this.dateRange.EndDate),
                        this.employeeIds);
                }),
                tap(res => {
                    if (res && res.isInFlight) return;
                    if (res == null) {
                        this.emptyState = true;
                        this.loaded = true;
                        return;
                    }

                    this.userData = res.eesWithUser;
                    this.setUserTypeCounts(this.userData);

                    this.infoData = {
                        icon: "people",
                        color: "success",
                        value: `${res.eesWithUserCount}`,
                        title: "Employees with user profile",
                        showBottom: false,
                    };
                    this.loaded = true;
                }),
            )
            .subscribe();
    }

    ngOnDestroy() {
        this.analyticsService.destroy();
    }

    setUserTypeCounts(data: UserPerformanceDashboardEmployee[]) {
        const employees = data.filter(x => x.userTypeId === UserType.employee);
        this.employeeCount = this.employeeData.length;

        const supervisors = data.filter(x => x.userTypeId === UserType.supervisor);
        this.supervisorCount = this.supervisorData.length;

        const companyAdmins = data.filter(x => x.userTypeId === UserType.companyAdmin);
        this.companyAdminCount = this.companyAdminData.length;
    }

    openDialog() {
        this.dialog.open(EmployeesWithUserIDDialogComponent, {
            width: "1000px",
            data: {
                employees: this.userData,
                title: `EMPLOYEES WITH USER PROFILE: ${this.userData.length}`,
            },
        });
    }
}
