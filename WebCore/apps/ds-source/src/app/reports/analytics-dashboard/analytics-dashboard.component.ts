import {
    Component,
    OnInit,
    ComponentFactoryResolver,
    ViewChild,
    ViewContainerRef,
    AfterViewInit,
    ElementRef,
    ComponentRef,
    Renderer2,
} from "@angular/core";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { ComponentLoaderService } from "../shared/services/component-loader.service";
import { DashboardApiService } from "../shared/services/dashboard-api.service";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { AccountService } from "@ds/core/account.service";
import { AnalyticsDashboardFilter } from "@ds/analytics/models/filter.model";
import { DashboardFilterOption } from "@ds/analytics/models/dashboard-filter-option.model";
import { AnalyticsService } from "@ds/analytics/shared/services/analytics.service";
import { map, switchMap, tap } from "rxjs/operators";
import { UserInfo, UserType } from "@ds/core/shared";
import { iif, of } from 'rxjs';

@Component({
    selector: "ds-analytics-dashboard",
    templateUrl: "./analytics-dashboard.component.html",
    styleUrls: ["./analytics-dashboard.component.scss"],
})
export class AnalyticsDashboardComponent implements OnInit {
    public DEFAULT_DATE_RANGE = "Current Month";

    user: UserInfo;
    currentDashboard: string;
    currentDateRangeType: string;
    currentDateRangeCustom: DateRange = {};

    employeeIds: number[];
    currentFilters: any = {}; //TODO: Replace any with interface after structure is defined
    dateRange: DateRange;
    dateRanges: string[] = [
        "Last 12 Months",
        "Current Week",
        "Last Week",
        "Current Month",
        "This Year",
        "Last Year",
        "Custom",
    ];
    dashboardNames = [];
    dashboardList;
    widgetList;
    sessionReady: boolean;
    filters: AnalyticsDashboardFilter[] = [];

    @ViewChild("widgets", { read: ViewContainerRef, static: false })
    widgets: ViewContainerRef;

    constructor(
        private accountService: AccountService,
        private renderer2: Renderer2,
        private cfr: ComponentFactoryResolver,
        private componentLoader: ComponentLoaderService,
        private dashboardApi: DashboardApiService,
        private analyticsService: AnalyticsService
    ) {}

    ngOnInit() {
        this.accountService
            .getUserInfo()
            .pipe(
                tap(user => this.user = user),
                switchMap(() =>
                    this.dashboardApi.GetFilters()),
                switchMap((searchFilters) => {
                    for (var i = 0; i < searchFilters.length; i++) {
                        searchFilters[i].filterOptions.sort((a, b) =>
                            a.name > b.name ? 1 : -1
                        );
                        this.filters.push({
                            title: searchFilters[i].description,
                            items: searchFilters[i].filterOptions,
                        });
                    }

                    return this.dashboardApi.GetDashboardList();
                }),
                map((dashboards: any) => {
                    this.dashboardList = dashboards;
                    return dashboards.map((x) => x.name);
                }),
                switchMap((dashboards: string[]) => {
                    this.dashboardNames =
                        this.user.userTypeId === UserType.supervisor
                            ? dashboards.filter((name) => name != "Payroll")
                            : dashboards;
                    return this.dashboardApi.GetDashboardSession();
                }),
                switchMap(session => {
                    if(!session || this.isFilterSessionValid(session) )
                        return of(session);
                    else {
                        /*Update the session with nullified values */
                        return this.dashboardApi.SaveDashboardSession({
                            DashboardId: session.dashboardId,
                            FilterData: session.filterData,
                        });
                    }
                } ),
                tap((session) => {
                    if (session) {
                        session.filterData = JSON.parse(session.filterData);

                        this.currentFilters = session.filterData;

                        this.currentDateRangeType = session.filterData.dateRangeType
                            ? session.filterData.dateRangeType : this.DEFAULT_DATE_RANGE;

                        if (session.filterData.dateRange)
                            this.currentDateRangeCustom = session.filterData.dateRange;

                        if (this.dashboardList && this.dashboardList.length) {
                            const myDashboard = this.dashboardList.find(d => d.dashboardId == session.dashboardId);

                            if (myDashboard) {
                                this.currentDashboard = myDashboard.name;
                            }
                        }
                    } else {
                        this.currentDateRangeType = this.DEFAULT_DATE_RANGE;
                        this.currentDashboard = this.dashboardNames[0];
                    }

                    this.sessionReady = true;
                }),
            )
            .subscribe();
    }

    FillWidgets(widgetList, employeeIds, dateRange) {
        this.widgets.clear();

        if (!widgetList || widgetList.length == 0) {
            this.dashboardApi
                .GetDashboardWidgets(
                    this.dashboardList.filter(
                        (d) => d.name === this.currentDashboard
                    )[0].dashboardId
                )
                .subscribe((data) => {
                    this.widgetList = data;
                    this.FillWidgets(
                        this.widgetList,
                        this.employeeIds,
                        this.dateRange
                    );
                });
            return;
        }

        widgetList.sort((a, b) => (a.sequence > b.sequence ? 1 : -1));

        for (var i = 0; i < widgetList.length; i++) {
            var comp = this.componentLoader.GetComponentById(
                widgetList[i].dashboardWidgetId
            );
            if (comp) {
                var factory = this.cfr.resolveComponentFactory(comp);
                var compRef: ComponentRef<any> = this.widgets.createComponent(
                    factory
                );
                compRef.instance.employeeIds = employeeIds;
                compRef.instance.dateRange = dateRange;

                this.renderer2.addClass(
                    compRef.location.nativeElement,
                    `col-md-${widgetList[i].size}`
                );

                compRef.changeDetectorRef.detectChanges();
            }
        }
    }

    dashboardChanged(event) {
        this.currentDashboard = event.value;

        this.saveSession();
    }

    submittedFilter(event) {
        //Children watch input change: https://stackoverflow.com/questions/38571812/how-to-detect-when-an-input-value-changes-in-angular

        this.currentFilters = event.value;
        this.saveSession();

        this.dashboardApi
            .GetDashboardWidgets(
                this.dashboardList.filter(
                    (d) => d.name === this.currentDashboard
                )[0].dashboardId
            )
            .subscribe((data) => {
                this.widgetList = data;

                var e: DashboardFilterOption[] = [];

                for (var [key, val] of Object.entries(event.value)) {
                    if (val) {
                        e.push({
                            Type: key,
                            Id: val != "" ? parseInt(val.toString()) : null,
                        });
                    }
                }

                //TODO: Remove get employees list calls so that we aren't passing huge employee id lists back and forth...
                // we should just be passing the filters and letting the API resolved the employee id list on the server instead...
                // safer and smaller payloads
                this.dashboardApi.setSearchFilters(e);

                // PRELOAD USER PERFORMANCE EMPLOYEE INFO
                this.analyticsService.getUserPerformanceDashboard(this.user.selectedClientId(), this.dateRange.StartDate, this.dateRange.EndDate, this.employeeIds);

                //TODO: don't do this - let's remove it. - Drew
                this.dashboardApi
                    .GetEmployeeIdsFromFilters(e)
                    .subscribe((eids: any) => {
                        this.employeeIds = eids.map((x) => x.employeeId);

                        this.FillWidgets(
                            this.widgetList,
                            this.employeeIds,
                            this.dateRange
                        );
                    });
            });
    }

    dateRangeChanged(event) {
        this.dateRange = event.value;
        if (this.widgetList) {
            this.FillWidgets(this.widgetList, this.employeeIds, this.dateRange);
        }

        this.currentDateRangeType = event.type;

        if (this.currentDateRangeType == "Custom") {
            this.currentDateRangeCustom = event.value;
        }

        this.saveSession();
    }

    isFilterSessionValid( sess: any){
        let arr = JSON.parse(sess.filterData);
        
        var valid = true;
        for(var i=0; i< this.filters.length; i++){
            let cf = arr[ this.filters[i].title.toString() ];
            
            if(cf){
                let item = this.filters[i].items.find(x => x.id == cf);
                if(!item){
                    // It seems the filters have changed and the session is retaining old values
                    valid = false;
                    arr[ this.filters[i].title.toString() ] = null;
                }
            }
        }
        if(!valid)  sess.filterData = JSON.stringify(arr);
        
        return valid;
    }

    saveSession() {
        var dashboardId = this.dashboardList.filter(
            (x) => x.name == this.currentDashboard
        )[0].dashboardId;
        var filterData = this.currentFilters;
        filterData.dateRangeType = this.currentDateRangeType;
        filterData.dateRange = this.currentDateRangeCustom;

        this.dashboardApi
            .SaveDashboardSession({
                DashboardId: dashboardId,
                FilterData: JSON.stringify(filterData),
            })
            .subscribe(() => {});
    }
}
