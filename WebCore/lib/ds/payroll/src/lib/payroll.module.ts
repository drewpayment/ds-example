import { NgModule } from '@angular/core';
import { PaycheckListComponent } from '@ds/payroll/paycheck-list/paycheck-list.component';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AjsUpgradesModule } from '@ds/core/ajs-upgrades/ajs-upgrades.module';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { PayrollInformationWidgetComponent } from '@ds/payroll/payroll-information-widget/payroll-information-widget.component';
import { BankDepositWidgetComponent } from '@ds/payroll/bank-deposit-widget/bank-deposit-widget.component';
import { PaymentCountWidgetComponent } from '@ds/payroll/payment-count-widget/payment-count-widget.component';
import { ChartsModule } from 'ng2-charts';
import { VendorMaintenanceComponent } from '@ds/payroll/vendor-maintenance/vendor-maintenance.component';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { DsCoreLocationModule } from '@ds/core/location';
import { CheckStockDialogComponent } from '@ds/payroll/check-stock/check-stock-dialog/check-stock-dialog.component';
import { CheckStockTriggerComponent } from '@ds/payroll/check-stock/check-stock-trigger/check-stock-trigger.component';
import { MaterialModule } from '@ds/core/ui/material';
import { Route, RouterModule } from '@angular/router';
import { TimeAndAttendanceComponent } from './time-and-attendance/time-and-attendance.component';
import { DateTimeModule } from '@ds/core/ui/datetime/datetime.module';
import { EmpFilterComponent } from './time-and-attendance/emp-filter/emp-filter.component';
import { TableContainerComponent } from './time-and-attendance/table-container/table-container.component';
import { IsTotalRowPipe } from './time-and-attendance/table-container/is-total-row.pipe';
import { DateWithPopoverComponent } from './time-and-attendance/table-container/date-with-popover/date-with-popover.component';
import { IsExceptionRowPipe } from './time-and-attendance/table-container/is-exception-row.pipe';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { DsAutocompleteModule } from '@ds/core/ui/ds-autocomplete';
import { TimeClockHardwareComponent } from './time-and-attendance/time-clock-hardware/time-clock-hardware.component';
import { PaycheckTableComponent } from './paycheck-table/paycheck-table.component';
import { PaycheckTableService } from './paycheck-table/paycheck-table.service';
import {
    TimeClockHardwareEditDialogComponent
} from './time-and-attendance/time-clock-hardware/time-clock-hardware-edit-dialog/time-clock-hardware-edit-dialog.component';
import { PunchMapModalComponent } from './time-and-attendance/punch-map-modal/punch-map-modal.component';
import { IsExceptionPipe } from './time-and-attendance/punch-map-modal/mark-exception.pipe';
import { HasExceptionDetailPipe } from './time-and-attendance/punch-map-modal/exception-detail.pipe';
import { AgmCoreModule } from '@agm/core';
import { HasExceptionColorPipe } from './time-and-attendance/punch-map-modal/mark-color.pipe';
import { GeofenceModule } from 'apps/ds-source/src/app/geofence/geofence.module';
import { AvatarModule } from '@ds/core/ui/avatar/avatar.module';

const routes: Route[] = [
  { path: 'list', component: PaycheckListComponent },
];

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    AjsUpgradesModule,
    DsCardModule,
    ChartsModule,
    DsCoreFormsModule,
    DsCoreLocationModule,
    DateTimeModule,
    LoadingMessageModule,
    DsAutocompleteModule,
    AgmCoreModule.forRoot({
        apiKey: 'AIzaSyDyggiB6RzKzKIWdK-v-PajBngC3vl4_P0',
        apiVersion:  'weekly',
        libraries: ['drawing', 'places']
    }),
    GeofenceModule,
    AvatarModule,

    // ROUTING - DON'T MOVE ME
    RouterModule.forChild(routes),
  ],
  declarations: [
    PaycheckListComponent,
    PayrollInformationWidgetComponent,
    BankDepositWidgetComponent,
    PaymentCountWidgetComponent,
    VendorMaintenanceComponent,
    CheckStockDialogComponent,
    CheckStockTriggerComponent,
    TimeAndAttendanceComponent,
    EmpFilterComponent,
    TableContainerComponent,
    IsTotalRowPipe,
    IsExceptionRowPipe,
    DateWithPopoverComponent,
    TimeClockHardwareComponent,
    TimeClockHardwareEditDialogComponent,
    PunchMapModalComponent,
    IsExceptionPipe,
    HasExceptionDetailPipe,
    HasExceptionColorPipe,
    PaycheckTableComponent,
  ],
  entryComponents: [
    TimeClockHardwareEditDialogComponent,
    PunchMapModalComponent
  ],
  exports: [
    PaycheckListComponent,
    RouterModule,
    PaycheckTableComponent,
    PunchMapModalComponent
  ],
  providers: [
    PaycheckTableService,
  ],
})
export class PayrollModule { }
