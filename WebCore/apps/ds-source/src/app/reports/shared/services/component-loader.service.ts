import { Injectable, Component, Type } from '@angular/core';
import { EmployeeTurnoverComponent } from '@ds/analytics/widgets/workforce/employee-turnover/employee-turnover.component';
import { DemographicsComponent } from '@ds/analytics/widgets/workforce/demographics/demographics.component';
import { EmployeeRetentionComponent } from '@ds/analytics/widgets/workforce/employee-retention/employee-retention.component';
import { ActiveEmployeesComponent } from '@ds/analytics/widgets/workforce/active-employees/active-employees.component';
import { TerminatedEmployeesComponent } from '@ds/analytics/widgets/workforce/terminated-employees/terminated-employees.component';
import { EmployeeGrowthRateComponent } from '@ds/analytics/widgets/workforce/employee-growth-rate/employee-growth-rate.component';
import { EventsComponent } from '@ds/analytics/widgets/workforce/events/events.component';
import { PunchTypeComponent } from '@ds/analytics/widgets/time-attendance/punch-type/punch-type.component';
import { HistoryComponent } from '@ds/analytics/widgets/workforce/history/history.component';
import { OpenTimeCardComponent } from '@ds/analytics/widgets/time-attendance/open-time-card/open-time-card.component';
import { ExceptionsComponent } from '@ds/analytics/widgets/time-attendance/exceptions/exceptions.component';
import { PointsTotalsComponent } from '@ds/analytics/widgets/time-attendance/points-totals/points-totals.component';
import { EmployeesClockedInComponent } from '@ds/analytics/widgets/time-attendance/employees-clocked-in/employees-clocked-in.component';
import { ScheduledVsWorkedHoursComponent } from '@ds/analytics/widgets/time-attendance/scheduled-vs-worked-hours/scheduled-vs-worked-hours.component';
import { OvertimeComponent } from '@ds/analytics/widgets/time-attendance/overtime/overtime.component';
import { PayrollHistoryComponent } from '@ds/analytics/widgets/payroll/payroll-history/payroll-history.component';
import { BreakdownComponent } from '@ds/analytics/widgets/payroll/breakdown/breakdown.component';
// import { PtoWidgetComponent } from '@ds/analytics/widgets/workforce/pto-widget/pto-widget.component';
import { BankDepositComponent } from '@ds/analytics/widgets/payroll/bank-deposit/bank-deposit.component';
import { UserTypeComponent } from '@ds/analytics/widgets/user/user-type/user-type.component';
import { LockedUsersComponent } from '@ds/analytics/widgets/user/locked-users/locked-users.component';
import { EmployeesWithUserIDComponent } from '@ds/analytics/widgets/user/employees-with-user-id/employees-with-user-id.component';
import { EmployeesWithoutUserIdComponent } from '@ds/analytics/widgets/user/employees-without-user-id/employees-without-user-id.component';
import { UserInfoComponent } from '@ds/analytics/widgets/user/user-info/user-info.component';

@Injectable({
  providedIn: 'root'
})
export class ComponentLoaderService {

  public components = [ ]

  constructor() { 
    this.components = [
      {
        id: 1,
        component: EventsComponent
      },
      {
        id: 2,
        component: ActiveEmployeesComponent
      },
      {
        id: 3,
        component: TerminatedEmployeesComponent
      },
      {
        id: 4,
        component: EmployeeTurnoverComponent
      },
      {
        id: 5,
        component: EmployeeRetentionComponent
      },      
      {
        id: 6,
        component: EmployeeGrowthRateComponent
      },
      {
        id: 7,
        component: HistoryComponent
      },
      {
        id: 8,
        component: DemographicsComponent
      },
      // {
      //   id: 9,
      //   component: PtoWidgetComponent
      // },
      {
        id: 10,
        component: BreakdownComponent
      },
      {
        id: 11,
        component: BankDepositComponent
      },
      {
        id: 12,
        component: PayrollHistoryComponent
      },
      {
        id: 13,
        component: EmployeesClockedInComponent
      },
      {
        id: 14,
        component: OvertimeComponent
      },
      {
        id: 15,
        component: OpenTimeCardComponent
      },
      {
        id: 16,
        component: PunchTypeComponent
      },
      {
        id: 17,
        component: ScheduledVsWorkedHoursComponent
      },
      {
        id: 18,
        component: ExceptionsComponent
      },
      {
        id: 19,
        component: PointsTotalsComponent
      },
      {
        id: 20,
        component: UserTypeComponent
      },
      {
        id: 21,
        component: EmployeesWithUserIDComponent
      },
      {
        id: 22,
        component: EmployeesWithoutUserIdComponent
      },
      {
        id: 23,
        component: LockedUsersComponent
      },
      {
        id: 24,
        component: UserInfoComponent
      },
    ]
  }

  GetComponentById(id): Type<any>{
    var obj: any = this.components.filter(x => x.id === id)[0];

    if(obj){ return obj.component }

    return null;
  }
}
