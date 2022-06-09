import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Route, RouterModule } from '@angular/router';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { MaterialModule } from '@ds/core/ui/material';
import { ClientAccrualsComponent } from './client-accruals.component';
import { ClientAccrualsSchedulesTabComponent } from './schedules-tab/schedules-tab.component';
import { ClientAccrualsSetupComponent } from './setup-tab/client-accruals-setup.component';

const routes: Route[] = [
  {
    path: '',
    component: ClientAccrualsComponent,
    children: [
      {
        path: "setup",
        component: ClientAccrualsSetupComponent,
      },
      {
        path: "schedules",
        component: ClientAccrualsSchedulesTabComponent,
      },
      {
        path: "",
        redirectTo: "setup",
        pathMatch: "full",
      },
    ]
  }
];

@NgModule({
  // declarations: [
  //   ClientAccrualsComponent,
  //   ClientAccrualsSetupComponent,
  //   ClientAccrualsGeneralCardComponent,
  //   ClientAccrualsPaidLeaveActCardComponent,
  //   ClientAccrualsEligibilityCardComponent,
  //   ClientAccrualsComputationCardComponent,
  //   ClientAccrualsDisplayCardComponent,
  //   ClientAccrualsAtmExportCardComponent,
  //   ClientAccrualsEarningsCardComponent,
  //   ClientAccrualsTimeOffCardComponent,
  //   ClientAccrualsFooterCardComponent,
  //   ClientAccrualsSchedulesTabComponent,
  //   ClientAccrualsAccrualSchedulesCardComponent,
  //   ClientAccrualsFirstYearAccrualSchedulesCardComponent,
  // ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    DsCardModule,
    DsCoreFormsModule,
    LoadingMessageModule,

    RouterModule.forChild(routes),
  ],
  exports: [
    RouterModule,
  ]
})
export class ClientAccrualsModule {}
