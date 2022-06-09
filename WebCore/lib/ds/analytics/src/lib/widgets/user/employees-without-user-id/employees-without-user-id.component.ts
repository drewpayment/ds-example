import { Component, OnInit, Input } from "@angular/core";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { ChartType, ChartOptions } from "chart.js";
import { AnalyticsApiService } from "@ds/analytics/shared/services/analytics-api.service";
import { MatDialog } from "@angular/material/dialog";
import { EmployeesWithoutUserIdDialogComponent } from "./employees-without-user-id-dialog/employees-without-user-id-dialog.component";
import { AccountService } from "@ds/core/account.service";
import { MOMENT_FORMATS, UserInfo } from "@ds/core/shared";
import { AnalyticsService } from "@ds/analytics/shared/services/analytics.service";
import { switchMap, tap } from 'rxjs/operators';
import { UserPerformanceDashboardEmployee, UserPerformanceDashboard } from '@ds/analytics/models';

@Component({
    selector: "ds-employees-without-user-id",
    templateUrl: "./employees-without-user-id.component.html",
    styleUrls: ["./employees-without-user-id.component.css"],
})
export class EmployeesWithoutUserIdComponent implements OnInit {
    @Input() employeeIds: number[];
    @Input() dateRange: DateRange;

    //card info
    cardType: string = "info";

    //empty state info
    loaded: boolean;
    emptyState: boolean = false;

    //data
    userData: UserPerformanceDashboardEmployee[] = [];
    user: UserInfo;

    //graph data
    infoData: InfoData;
    public pieChartOptions: ChartOptions = {
        responsive: true,
        legend: {
            position: "right",
            labels: {
                usePointStyle: true,
                padding: 15,
            },
        },
        plugins: {
            datalabels: {
                formatter: (value, ctx) => {
                    const label = ctx.chart.data.labels[ctx.dataIndex];
                    return label;
                },
            },
        },
    };

    public pieChartData: number[] = [];
    public pieChartType: ChartType = "pie";
    public pieChartColors = [
        {
            backgroundColor: [
                "#fc621a",
                "#da2121",
                "#ffb627",
                "#ff821c",
                "#ff4949",
                "#b04001",
                "#ffd73b",
                "#f53c05",
            ],
        },
    ];
    public pieChartLabels: string[] = [];

    constructor(
        private analyticsApi: AnalyticsApiService,
        private accountService: AccountService,
        private dialog: MatDialog,
        private analyticsService: AnalyticsService
    ) {}

    ngOnInit() {
        this.accountService.getUserInfo()
            .pipe(
                switchMap(user => {
                    this.user = user;
                    return this.analyticsService.getUserPerformanceDashboard(this.user.selectedClientId(), this.dateRange.StartDate, this.dateRange.EndDate, this.employeeIds);
                }),
                tap(res => {
                    if (res && res.isInFlight) return;
                    if (res == null) {
                        this.emptyState = true;
                        this.loaded = !res.isInFlight;
                        return;
                    }

                    this.userData = res.eesWoUser;
                    this.infoData = {
                        icon: "people_outline",
                        color: "warning",
                        value: `${res.eesWoUserCount}`,
                        title:
                            "Employees with no user profile",
                        showBottom: false,
                    };
                    this.loaded = true;
                }),
            )
            .subscribe();
    }

    openDialog() {
        if (this.loaded) {
        }
        var config = {
            width: "1000px",
            data: {
                employees: this.userData,
                title: `EMPLOYEES WITH NO USER PROFILE: ${this.userData.length}`,
            },
        };
        const dialogRef = this.dialog.open(
            EmployeesWithoutUserIdDialogComponent,
            config
        );
    }
}
