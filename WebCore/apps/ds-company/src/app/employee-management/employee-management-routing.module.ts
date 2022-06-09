import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { UserTypeGuard } from "../guards/user-type.guard";
import { UserType } from "@ds/core/shared";
import { TimeModule } from './time/time.module';
import { TimeAndAttendanceComponent } from '@ds/payroll/time-and-attendance/time-and-attendance.component';

import { TaxesComponent } from './taxes/taxes.component';
import { DirtyCheckGuard } from "../guards/dirty-check.guard";
import { EmergencyContactComponent } from './emergency-contact/emergency-contact.component';
import { DashboardComponent } from './onboarding/dashboard/dashboard.component';
import { DashboardDetailComponent } from './onboarding/dashboard-detail/dashboard-detail.component';
import { AddEmployeeComponent } from "./onboarding/add-employee/add-employee.component";
import { EmployeeAccrualsComponent } from "./employee-accruals/employee-accruals.component";
import { EventsComponent } from './events/events.component';
import { AttachmentsComponent } from "./attachments/attachments.component";
import { EmployeeDependentsComponent } from "./employee-dependents/employee-dependents.component";

export const employeeManagementRoutes: Routes = [
  {
    path: "manage",
    canActivate: [UserTypeGuard],
    data: {
      userTypes: [
        UserType.systemAdmin,
        UserType.companyAdmin,
        UserType.supervisor,
      ],
    },
    children: [
      { path: 'time', loadChildren: () => TimeModule },
      { path: 'taxes', component: TaxesComponent, canDeactivate: [DirtyCheckGuard] },
      { path: 'attachments/:pageType', component: AttachmentsComponent },
      {
        path: 'time-and-attendance',
        children: [
          { path: 'supervisor/:supervisorId/is-approved/:isApproved', component: TimeAndAttendanceComponent, },
          { path: '', component: TimeAndAttendanceComponent, },
        ],
      },
      { path: 'emergency-contacts', component: EmergencyContactComponent, canDeactivate: [DirtyCheckGuard] },
      {
        path: 'onboarding',
        children: [
          { path: "dashboard", component: DashboardComponent },	
          { path: "dashboard-detail/:employeeId", component: DashboardDetailComponent },
          { path: "add-employee/:clientId/:employeeId/:paneId/add", component: AddEmployeeComponent, canDeactivate: [DirtyCheckGuard] },
          { path: "add-workflow/:clientId/:employeeId/:paneId/add", component: AddEmployeeComponent, canDeactivate: [DirtyCheckGuard] },
          { path: "add-workflow/:clientId/:employeeId/:paneId/edit", component: AddEmployeeComponent, canDeactivate: [DirtyCheckGuard], data: { returnsToDetail: true} },
          { path: "add-email-template/:clientId/:employeeId/:paneId/add", component: AddEmployeeComponent, canDeactivate: [DirtyCheckGuard] },
          
        ]
      },
      { path: 'events', component: EventsComponent, canDeactivate: [DirtyCheckGuard]},
      { path: 'benefits', component: EmployeeAccrualsComponent, canDeactivate: [DirtyCheckGuard] },
      { path: 'dependents', component: EmployeeDependentsComponent, canDeactivate: [DirtyCheckGuard] },
      { path: '', redirectTo: '', pathMatch: 'full', },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(employeeManagementRoutes)],
  exports: [RouterModule],
})
export class EmployeeManagementRoutingModule {}
