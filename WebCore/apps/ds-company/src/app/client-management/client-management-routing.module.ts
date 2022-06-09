import { NgModule } from '@angular/core';
import { Routes, RouterModule, Route } from '@angular/router';
import { AutomaticBillingComponent } from './automatic-billing/automatic-billing.component';
import { BillingComponent } from './billing/billing.component';
import { FeatureComponent } from './feature/feature.component';
import { UserTypeGuard } from '../guards/user-type.guard';
import { UserType } from '@ds/core/shared';
import { ActionTypes } from '@ds/core/constants/action-types';
import { DeviceListComponent as ClientMachineListComponent } from './clock/client-machine-list/device-list.component';
import { UserListComponent } from './clock/user-list/user-list.component';
import { HardwareComponent as ClockAdministrationComponent } from './clock/hardware/hardware.component';
import { TemplateListComponent } from './clock/template-list/template-list.component';
import { TransactionListComponent } from './clock/transaction-list/transaction-list.component';
import { ClientMachineComponent } from './clock/client-machine/client-machine.component';
import { BankHolidaysComponent } from './maintenance/bank-holidays/bank-holidays.component';
import { NPSDashboardComponent } from './nps/nps-dashboard.component';
import { OrganizationClientComponent } from './organization/organization-client/organization-client.component';
import { ClientGlControlComponent } from "./general-ledger/client-gl-control/client-gl-control.component";
import { GlClassGroupsComponent } from "./general-ledger/gl-class-groups/gl-class-groups.component";
import { ActionTypeGuard } from '../guards/action-type.guard';
import { ClientAccrualsModule } from './client-accruals/client-accruals.module';
import { W2ProcessingComponent } from "./w2/w2-processing/w2-processing.component";
import { AttachmentsComponent } from "../employee-management/attachments/attachments.component";

/** All admin routes are system Admin components only. Enforced by SystemAdminGuard */
export const clientManagementRoutes: Routes = [
  {
    path: "sys",
    canActivate: [UserTypeGuard],
    data: {
      userTypes: [UserType.systemAdmin],
    },
    children: [
      {
        path: 'billing/setup',
        component: AutomaticBillingComponent,
        canActivate: [ActionTypeGuard],
        data: {
          actionTypes: [ActionTypes.SystemAdmin.ReadWriteAutomaticBilling],
        },
      },
      { path: 'bank-holidays', component: BankHolidaysComponent },
      { path: 'billing', component: BillingComponent },
      { path: 'features', component: FeatureComponent },
      { path: 'nps-dashboard', component: NPSDashboardComponent },
      { path: '', redirectTo: 'billing/setup', pathMatch: 'full' },
      { path: 'attachments/:pageType', component: AttachmentsComponent },
      // { path: "clock", loadChildren: () => import("./clock/clock.module").then((module) => module.ClockModule) }
      // { path: "clock", loadChildren: './clock/clock.module#ClockModule', data: { applyPreload: true }},
      {
        path: 'clock',
        children: [
          { path: 'hardware', component: ClockAdministrationComponent },
          {
            path: 'hardware/device-list',
            component: ClientMachineListComponent,
          },
          { path: 'hardware/user-list', component: UserListComponent },
          { path: 'hardware/template-list', component: TemplateListComponent },
          {
            path: 'hardware/transaction-list',
            component: TransactionListComponent,
          },
          {
            path: 'hardware/clientmachine/:id',
            component: ClientMachineComponent,
          },
        ],
      },
      {
        path: 'leave-management',
        children: [
          {
            path: 'client-accruals',
            loadChildren: () => ClientAccrualsModule,
          },
          {
            path: '',
            redirectTo: 'client-accruals',
            pathMatch: 'full',
          },
        ],
      },
      {
        path: 'organization',
        component: OrganizationClientComponent,
        canActivate: [UserTypeGuard],
        data: {
          userTypes: [UserType.systemAdmin]
        },
      },
      {
        path: 'gl',
        children: [
          {
            path: 'control',
            component: ClientGlControlComponent,
          },
          {
            path: 'class-groups',
            component: GlClassGroupsComponent,
          }
        ]
        },
        {
            path: 'w2-processing', component: W2ProcessingComponent,
            canActivate: [UserTypeGuard], data: { UserType: [UserType.systemAdmin] }
        },
    ],
  },
];

/** Define NgModule */
@NgModule({
  imports: [RouterModule.forChild(clientManagementRoutes)],
  exports: [RouterModule],
})
export class ClientManagementRoutingModule { }
