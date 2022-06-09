import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardWidgetComponent, WidgetContent, WidgetHeaderComponent, WidgetBottomContent } from './dashboard-widget/dashboard-widget.component';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { FilterCardComponent } from './filter-card/filter-card.component';
import { HttpClientModule } from '@angular/common/http';
import { ActiveEmployeesComponent } from './widgets/workforce/active-employees/active-employees.component';
import { DemographicsComponent } from './widgets/workforce/demographics/demographics.component';
import { EmployeeTurnoverDialogComponent } from './widgets/workforce/employee-turnover/employee-turnover-dialog/employee-turnover-dialog.component';
import { EmployeeTurnoverComponent } from './widgets/workforce/employee-turnover/employee-turnover.component';
import { EmployeeRetentionComponent } from './widgets/workforce/employee-retention/employee-retention.component';
import { ChartsModule } from 'ng2-charts';
import { ActiveEmployeeDialogComponent } from './widgets/workforce/active-employees/active-employee-dialog/active-employee-dialog.component';
import { TerminatedEmployeesComponent } from './widgets/workforce/terminated-employees/terminated-employees.component';
import { EmployeeGrowthRateComponent } from './widgets/workforce/employee-growth-rate/employee-growth-rate.component';
import { EmployeeGrowthRateDialogComponent } from './widgets/workforce/employee-growth-rate/employee-growth-rate-dialog/employee-growth-rate-dialog.component';
import { EventsComponent } from './widgets/workforce/events/events.component';
import { BirthdayEventDialogComponent } from './widgets/workforce/events/events-dialog-components/birthday-event-dialog/birthday-event-dialog.component';
import { AnniversaryEventDialogComponent } from './widgets/workforce/events/events-dialog-components/anniversary-event-dialog/anniversary-event-dialog.component';
import { NinetyDayEventDialogComponent } from './widgets/workforce/events/events-dialog-components/ninety-day-event-dialog/ninety-day-event-dialog.component';
import { PunchTypeComponent } from './widgets/time-attendance/punch-type/punch-type.component';
import { GenderGraphComponent } from './widgets/workforce/demographics/gender-graph/gender-graph.component';
import { EthnicityGraphComponent } from './widgets/workforce/demographics/ethnicity-graph/ethnicity-graph.component';
import { AgeGraphComponent } from './widgets/workforce/demographics/age-graph/age-graph.component';
import { LengthOfServiceGraphComponent } from './widgets/workforce/demographics/length-of-service-graph/length-of-service-graph.component';
import { TerminatedEmployeesDialogComponent } from './widgets/workforce/terminated-employees/terminated-employees-dialog/terminated-employees-dialog.component';
import { DemographicsListComponent } from './widgets/workforce/demographics/demographics-list/demographics-list.component';
import { HistoryComponent } from './widgets/workforce/history/history.component';
import { HistoryDialogComponent } from './widgets/workforce/history/history-dialog/history-dialog.component';
import { OpenTimeCardComponent } from './widgets/time-attendance/open-time-card/open-time-card.component';
import { OpenTimeCardDialogComponent } from './widgets/time-attendance/open-time-card/open-time-card-dialog/open-time-card-dialog.component';
import { DemographicDialogComponent } from './widgets/workforce/demographics/demographic-dialog/demographic-dialog.component';
import { ExceptionsComponent } from './widgets/time-attendance/exceptions/exceptions.component';
import { ExceptionCardComponent } from './widgets/time-attendance/exceptions/exception-card/exception-card.component';
import { ExceptionCardDialogComponent } from './widgets/time-attendance/exceptions/exception-card/exception-card-dialog/exception-card-dialog.component';
import { PunchTypeDialogComponent } from './widgets/time-attendance/punch-type/punch-type-dialog/punch-type-dialog.component';
import { PointsTotalsComponent } from './widgets/time-attendance/points-totals/points-totals.component';
import { EmployeesClockedInComponent } from './widgets/time-attendance/employees-clocked-in/employees-clocked-in.component';
import { EmployeesClockedInDialogComponent } from './widgets/time-attendance/employees-clocked-in/employees-clocked-in-dialog/employees-clocked-in-dialog.component';
import { ScheduledVsWorkedHoursComponent } from './widgets/time-attendance/scheduled-vs-worked-hours/scheduled-vs-worked-hours.component';
import { ScheduledVsWorkedHoursDialogComponent } from './widgets/time-attendance/scheduled-vs-worked-hours/scheduled-vs-worked-hours-dialog/scheduled-vs-worked-hours-dialog.component';
import { OvertimeDialogComponent } from './widgets/time-attendance/overtime/overtime-dialog/overtime-dialog.component';
import { OvertimeComponent } from './widgets/time-attendance/overtime/overtime.component';
import { PayrollHistoryComponent } from './widgets/payroll/payroll-history/payroll-history.component';
import { BreakdownComponent } from './widgets/payroll/breakdown/breakdown.component';
import { PtoWidgetComponent } from './widgets/workforce/pto-widget/pto-widget.component';
import { PtoWidgetDiaglogComponent } from './widgets/workforce/pto-widget/pto-widget-diaglog/pto-widget-diaglog.component';
import { PointsTotalsDialogComponent } from './widgets/time-attendance/points-totals/points-totals-dialog/points-totals-dialog.component';
import { BankDepositComponent } from './widgets/payroll/bank-deposit/bank-deposit.component';
import { BankGraphComponent } from './widgets/payroll/bank-deposit/bank-graph/bank-graph.component';
import { PaymentGraphComponent } from './widgets/payroll/bank-deposit/payment-graph/payment-graph.component';
import { BankDepositDialogComponent } from './widgets/payroll/bank-deposit/bank-deposit-dialog/bank-deposit-dialog.component';
import { TaxBankDepositDialogComponent } from './widgets/payroll/bank-deposit/tax-bank-deposit-dialog/tax-bank-deposit-dialog.component';
import { UserTypeComponent } from './widgets/user/user-type/user-type.component';
import { UserTypeDialogComponent } from './widgets/user/user-type/user-type-dialog/user-type-dialog.component';
import { LockedUsersComponent } from './widgets/user/locked-users/locked-users.component';
import { LockedUsersDialogComponent } from './widgets/user/locked-users/locked-users-dialog/locked-users-dialog.component';
import { EmployeesWithUserIDComponent } from './widgets/user/employees-with-user-id/employees-with-user-id.component';
import { EmployeesWithUserIDDialogComponent } from './widgets/user/employees-with-user-id/employees-with-user-id-dialog/employees-with-user-id-dialog.component';
import { EmployeesWithoutUserIdComponent } from './widgets/user/employees-without-user-id/employees-without-user-id.component';
import { EmployeesWithoutUserIdDialogComponent } from './widgets/user/employees-without-user-id/employees-without-user-id-dialog/employees-without-user-id-dialog.component';
import { UserInfoComponent } from './widgets/user/user-info/user-info.component';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { DsDialogModule } from '@ds/core/ui/ds-dialog';
import { CoreModule } from '@ds/core/core.module';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSortModule } from '@angular/material/sort';
import { AvatarModule } from '@ds/core/ui/avatar/avatar.module';

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
  EmployeesClockedInComponent,
  ExceptionsComponent,
  ScheduledVsWorkedHoursComponent,
  OvertimeComponent,
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

var ModalList = [
  EmployeeTurnoverDialogComponent,
  ActiveEmployeeDialogComponent,
  EmployeeGrowthRateDialogComponent,
  BirthdayEventDialogComponent,
  AnniversaryEventDialogComponent,
  NinetyDayEventDialogComponent,
  TerminatedEmployeesDialogComponent,
  GenderGraphComponent,
  EthnicityGraphComponent,
  AgeGraphComponent,
  LengthOfServiceGraphComponent,
  DemographicsListComponent,
  HistoryDialogComponent,
  OpenTimeCardDialogComponent,
  TerminatedEmployeesDialogComponent,
  DemographicDialogComponent,
  ExceptionCardComponent,
  ExceptionCardDialogComponent,
  PunchTypeDialogComponent,
  PointsTotalsComponent,
  EmployeesClockedInDialogComponent,
  ScheduledVsWorkedHoursDialogComponent,
  OvertimeDialogComponent,
  PayrollHistoryComponent,
  PtoWidgetDiaglogComponent,
  PointsTotalsDialogComponent,
  BankDepositDialogComponent,
  TaxBankDepositDialogComponent,
  UserTypeDialogComponent,
  LockedUsersDialogComponent,
  EmployeesWithUserIDDialogComponent,
  EmployeesWithoutUserIdDialogComponent,
]

@NgModule({
  declarations: [
    DashboardWidgetComponent,
    WidgetContent,
    WidgetHeaderComponent,
    WidgetBottomContent,
    FilterCardComponent,
    WidgetList,
    ModalList,
    ],

  imports: [
    CommonModule,
    MatIconModule,
    MatMenuModule,
    DsCardModule,
    CoreModule,
    ReactiveFormsModule,
    FormsModule,
    MatDatepickerModule,
    HttpClientModule,
    ChartsModule,
    MatDialogModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatSortModule,
    DsDialogModule,
    LoadingMessageModule,
    AvatarModule
  ],
  entryComponents: [
    FilterCardComponent,
    ModalList,
    FilterCardComponent,
    EmployeeTurnoverDialogComponent
  ],

  exports: [
    DashboardWidgetComponent,
    WidgetContent,
    WidgetHeaderComponent,
    WidgetBottomContent,
    FilterCardComponent,
    WidgetList
  ]
})
export class AnalyticsModule { }
