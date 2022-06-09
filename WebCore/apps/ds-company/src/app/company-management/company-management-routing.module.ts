import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserTypeGuard } from '../guards/user-type.guard';
import { ActionTypeGuard } from '../guards/action-type.guard';
import { UserType } from '@ds/core/shared';
import { IntegrationSyncDashboardComponent } from './integration-sync-dashboard/integration-sync-dashboard.component';
import { AdminTaskListComponent } from './admin-task/admin-task-list.component';
import { AdminTaskFormComponent } from './admin-task/admin-task-form/admin-task-form.component';
import { ShiftsComponent } from './labor/shifts/shifts.component';
import { DirtyCheckGuard } from '../guards/dirty-check.guard';
import { DepartmentsComponent } from './labor/departments/departments.component';
import { ResourcesComponent } from './resources/resources.component';
import { ClientGlAccountAssignmentComponent } from './general-ledger/client-gl-account-assignment/client-gl-account-assignment.component';
import { ClientGlGlobalMappingComponent } from './general-ledger/client-gl-global-mapping/client-gl-global-mapping.component';
import { ActionTypes } from '@ds/core/constants/action-types';
import { ClientGlAccountImportComponent } from './general-ledger/client-gl-account-import/client-gl-account-import.component';
import { DivisionsComponent } from './labor/divisions/divisions.component';
import { ManageCorrespondenceTemplateComponent } from "./onboarding/correspondence-template/manage-correspondence-template.component";
import { CustomPagesComponent } from './onboarding/custom-pages/custom-pages.component';
import { ManageResourcesComponent } from './onboarding/custom-pages/manage-resources/manage-resources.component';
import { ManageWelcomeMessageComponent } from './onboarding/welcome-message/manage-welcome-message.component';
import { ManageFinalDisclaimerComponent } from './onboarding/final-disclaimer/manage-final-disclaimer.component';
import { EmployeeExportComponent } from './employee-export/employee-export.component';


export const companyManagementRoutes: Routes = [
  {
    path: 'admin',
    canActivate: [ UserTypeGuard ],
    data: {
      userTypes: [ UserType.systemAdmin, UserType.companyAdmin ],
    },
    children: [
      { path: '', redirectTo: '', pathMatch: 'full', },
      { path: 'integrations', component: IntegrationSyncDashboardComponent, },
      { path: 'admin-tasks', component: AdminTaskListComponent, },
      { path: 'resources', component: ResourcesComponent },
      { path: 'employee-export', component: EmployeeExportComponent},
      {
        path: 'integrations',
        component: IntegrationSyncDashboardComponent,
      },
      {
        path: '',
        redirectTo: '',
        pathMatch: 'full',
      },
      { 
        path: 'labor', 
        children: [
          {
            path: 'shifts', 
            component: ShiftsComponent,
            canDeactivate: [ DirtyCheckGuard ]
          },
          {
            path: 'departments', 
            component: DepartmentsComponent,
            canDeactivate: [ DirtyCheckGuard ]
          },
          { path: 'divisions', 
            component: DivisionsComponent,
            canDeactivate: [ DirtyCheckGuard ]
          },
        ]
      },
      {
        path: 'gl',
        children: [
          {
            path: 'account',
            component: ClientGlAccountAssignmentComponent,
            canActivate: [ ActionTypeGuard ],
              data: {
                actionTypes: [ActionTypes.GeneralLedger.GeneralLedgerAdministrator],
              },
          },
          {
            path: 'mapping',
            component: ClientGlGlobalMappingComponent,
            canActivate: [ ActionTypeGuard ],
              data: {
                actionTypes: [ActionTypes.GeneralLedger.GeneralLedgerAdministrator],
              },
          },
          { path: 'account-import', component: ClientGlAccountImportComponent },
        ]
      },
      {
        path: 'onboarding',
        children: [
         { path: 'custom-pages', component: CustomPagesComponent},
          { path: "manage-resources", component: ManageResourcesComponent },
          { path: "manage-resources/:taskId/:pageType/add", component: ManageResourcesComponent },
          { path: "manage-resources/:taskId/:pageType/add/workflow/:clientId/:employeeId", component: ManageResourcesComponent },
          { path: "manage-resources/:taskId/:pageType/add/client/:clientId/job-profile/:jobProfileId", component: ManageResourcesComponent },
          { path: "manage-resources/:taskId/:pageType/edit", component: ManageResourcesComponent },
          { path: "welcome-message", component: ManageWelcomeMessageComponent }, 
          { path: "final-disclaimer", component: ManageFinalDisclaimerComponent },
          { path: "correspondence-template/:pageType", component: ManageCorrespondenceTemplateComponent  },
      ]
     }
    ],
  },
];
@NgModule({
  imports: [RouterModule.forChild(companyManagementRoutes)],
  exports: [RouterModule],
})
export class CompanyManagementRoutingModule {}
