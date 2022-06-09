import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ClientStatisticsComponent } from './client-statistics/client-statistics/client-statistics.component';
import { ChartsModule } from 'ng2-charts';
import { FeaturesChartComponent } from './client-statistics/features-chart/features-chart.component';
import { FeaturesChartModalComponent } from './client-statistics/features-chart-modal/features-chart-modal.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MaterialModule } from '@ds/core/ui/material';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { PayrollTableComponent } from './client-statistics/payroll-table/payroll-table.component';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { Routes, RouterModule } from '@angular/router';

import { CompanyAdminGuard } from './guards/company-admin.guard';

@NgModule({
  declarations: [
    ClientStatisticsComponent,
    FeaturesChartComponent,
    FeaturesChartModalComponent,
    PayrollTableComponent,
  ],
  imports: [
    CommonModule,
    ChartsModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    DsCardModule,
  ],
  entryComponents: [ClientStatisticsComponent, FeaturesChartModalComponent],
  exports: [
    ClientStatisticsComponent,
    FeaturesChartComponent,
    FeaturesChartModalComponent,
  ],
})
export class AdminModule {}
