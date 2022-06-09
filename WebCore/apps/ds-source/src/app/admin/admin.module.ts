import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ChartsModule } from 'ng2-charts';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MaterialModule } from '@ds/core/ui/material';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { AddAlertDialogComponent } from './company-alerts/add-alert-dialog/add-alert-dialog.component';
import { CompanyAlertsComponent } from './company-alerts/company-alerts.component';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { Routes, RouterModule } from '@angular/router';
import { AdminGuard } from './shared/admin.guard';
import { OutletComponent } from '../outlet.component';

export const AdminRoutes: Routes = [
  {
    path: 'admin',
    children: [
      {
        path: 'notifications/alerts',
        component: CompanyAlertsComponent,
        canActivate: [AdminGuard],
      },
    ],
  },
];

@NgModule({
  declarations: [CompanyAlertsComponent, AddAlertDialogComponent],
  imports: [
    CommonModule,
    ChartsModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    DsCardModule,
    LoadingMessageModule,
    RouterModule.forChild(AdminRoutes),
  ],
  entryComponents: [AddAlertDialogComponent],
  exports: [CompanyAlertsComponent, RouterModule],
})
export class AdminAppModule {}
