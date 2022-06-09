import { Component, OnInit, Input } from "@angular/core";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { MatDialog } from "@angular/material/dialog";
import { LockedUsersDialogComponent } from "./locked-users-dialog/locked-users-dialog.component";
import { AccountService } from "@ds/core/account.service";
import { UserInfo } from "@ds/core/shared";
import { switchMap } from "rxjs/operators";
import { AnalyticsService } from "@ds/analytics/shared/services/analytics.service";
import { tap } from "rxjs/operators";
import { UserPerformanceDashboardEmployee } from "@ds/analytics/models";

@Component({
    selector: "ds-locked-users",
    templateUrl: "./locked-users.component.html",
    styleUrls: ["./locked-users.component.css"],
})
export class LockedUsersComponent implements OnInit {
    @Input() employeeIds: number[];
    @Input() dateRange: DateRange;

    //card info
    cardType: string = "info";

    //empty state info
    loaded: boolean;
    emptyState: boolean = false;

    //data
    userData: UserPerformanceDashboardEmployee[] = [];

    //graph data
    infoData: InfoData;
    user: UserInfo;

    constructor(
        private accountService: AccountService,
        private dialog: MatDialog,
        private analyticsService: AnalyticsService
    ) {}

    ngOnInit() {
        this.accountService
            .getUserInfo()
            .pipe(
                switchMap((user) => {
                    this.user = user;
                    return this.analyticsService.getUserPerformanceDashboard(
                        this.user.selectedClientId(),
                        this.dateRange.StartDate,
                        this.dateRange.EndDate,
                        this.employeeIds
                    );
                }),
                tap((res) => {
                    if (res && res.isInFlight) return;
                    if (!res) {
                        this.emptyState = true;
                        this.loaded = true;
                        return;
                    }

                    this.userData = res.activeUsers.filter((u) => u.userId > 0 && !u.isPasswordEnabled && u.isActive);

                    this.infoData = {
                        icon: "lock",
                        color: "danger",
                        value: `${res.lockedOutUsersCount}`,
                        title: "EMPLOYEES LOCKED OUT",
                        showBottom: false,
                    };
                    this.loaded = true;
                })
            )
            .subscribe();
    }

    openDialog() {
        if (this.loaded) {
        }
        var config = {
            width: "1000px",
            data: {
                users: this.userData,
                title: "Employees locked out",
            },
        };
        const dialogRef = this.dialog.open(LockedUsersDialogComponent, config);
    }
}
