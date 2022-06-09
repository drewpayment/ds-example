import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportScheduleSequenceDialogComponent } from './report-schedule-sequence-dialog/report-schedule-sequence-dialog.component';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { ReportScheduleServiceProvider } from './reports-ajs-upgrades';
import { ChangeHistoryModule, ReportRunnerComponent } from '@ds/reports/change-history';
import { AnalyticsDashboardComponent } from './analytics-dashboard/analytics-dashboard.component';
import { AnalyticsModule } from '@ds/analytics';
import { FilterCardComponent } from '@ds/analytics/filter-card/filter-card.component';
import { DashboardWidgetComponent, WidgetContent, WidgetHeaderComponent, WidgetBottomContent } from '@ds/analytics/dashboard-widget/dashboard-widget.component';
import { HttpClientModule } from '@angular/common/http';
import { OpenTimeCardComponent } from '@ds/analytics/widgets/time-attendance/open-time-card/open-time-card.component';
import { EmployeeTurnoverComponent } from '@ds/analytics/widgets/workforce/employee-turnover/employee-turnover.component';
import { DemographicsComponent } from '@ds/analytics/widgets/workforce/demographics/demographics.component';
import { EmployeeRetentionComponent } from '@ds/analytics/widgets/workforce/employee-retention/employee-retention.component';
import { ActiveEmployeesComponent } from '@ds/analytics/widgets/workforce/active-employees/active-employees.component';
import { TerminatedEmployeesComponent } from '@ds/analytics/widgets/workforce/terminated-employees/terminated-employees.component';
import { EmployeeGrowthRateComponent } from '@ds/analytics/widgets/workforce/employee-growth-rate/employee-growth-rate.component';
import { EventsComponent } from '@ds/analytics/widgets/workforce/events/events.component';
import { PunchTypeComponent } from '@ds/analytics/widgets/time-attendance/punch-type/punch-type.component';
import { HistoryComponent } from '@ds/analytics/widgets/workforce/history/history.component';
import { ExceptionsComponent } from '@ds/analytics/widgets/time-attendance/exceptions/exceptions.component';
import { PointsTotalsComponent } from '@ds/analytics/widgets/time-attendance/points-totals/points-totals.component';
import { EmployeesClockedInComponent } from '@ds/analytics/widgets/time-attendance/employees-clocked-in/employees-clocked-in.component';
import { ScheduledVsWorkedHoursComponent } from '@ds/analytics/widgets/time-attendance/scheduled-vs-worked-hours/scheduled-vs-worked-hours.component';
import { OvertimeComponent } from '@ds/analytics/widgets/time-attendance/overtime/overtime.component';
import { PayrollHistoryComponent } from '@ds/analytics/widgets/payroll/payroll-history/payroll-history.component';
import { BreakdownComponent } from '@ds/analytics/widgets/payroll/breakdown/breakdown.component';
import { PtoWidgetComponent } from '@ds/analytics/widgets/workforce/pto-widget/pto-widget.component';
import { BankDepositComponent } from '@ds/analytics/widgets/payroll/bank-deposit/bank-deposit.component';
import { BankGraphComponent } from '@ds/analytics/widgets/payroll/bank-deposit/bank-graph/bank-graph.component';
import { PaymentGraphComponent } from '@ds/analytics/widgets/payroll/bank-deposit/payment-graph/payment-graph.component';
import { UserTypeComponent } from '@ds/analytics/widgets/user/user-type/user-type.component';
import { LockedUsersComponent } from '@ds/analytics/widgets/user/locked-users/locked-users.component';
import { EmployeesWithUserIDComponent } from '@ds/analytics/widgets/user/employees-with-user-id/employees-with-user-id.component';
import { EmployeesWithoutUserIdComponent } from '@ds/analytics/widgets/user/employees-without-user-id/employees-without-user-id.component';
import { UserInfoComponent } from '@ds/analytics/widgets/user/user-info/user-info.component';


var WidgetList = [
    EmployeeTurnoverComponent,
    DemographicsComponent,
    EmployeeRetentionComponent,
    ActiveEmployeesComponent,
    TerminatedEmployeesComponent,
    EmployeeGrowthRateComponent,
    EventsComponent,
    PunchTypeComponent,
    HistoryComponent,
    OpenTimeCardComponent,
    ExceptionsComponent,
    PointsTotalsComponent,
    EmployeesClockedInComponent,
    ScheduledVsWorkedHoursComponent,
    OvertimeComponent,
    PayrollHistoryComponent,
    BreakdownComponent,
    PtoWidgetComponent,
    BankDepositComponent,
    BankGraphComponent,
    PaymentGraphComponent,
    UserTypeComponent,
    LockedUsersComponent,
    EmployeesWithUserIDComponent,
    EmployeesWithoutUserIdComponent,
    UserInfoComponent,
]

@NgModule({
    imports: [
        CommonModule,
        MaterialModule,
        ChangeHistoryModule,
        AnalyticsModule,
        HttpClientModule
    ],
    providers: [
        ReportScheduleServiceProvider
    ],
    declarations: [
        ReportScheduleSequenceDialogComponent,
        AnalyticsDashboardComponent
    ],
    entryComponents: [
        ReportScheduleSequenceDialogComponent,
        ReportRunnerComponent,
        AnalyticsDashboardComponent,
        DashboardWidgetComponent,
        WidgetContent,
        WidgetHeaderComponent,
        WidgetBottomContent,
        AnalyticsDashboardComponent,
        FilterCardComponent,
        WidgetList
    ]
})
export class DsReportsAppModule { }
