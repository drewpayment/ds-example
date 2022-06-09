import { NgModule } from '@angular/core';
import { CommonModule, CurrencyPipe, DecimalPipe } from '@angular/common';
import { ShiftsComponent } from './shifts/shifts.component';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { UpgradeModule } from '@angular/upgrade/static';
import { DsCoreResourcesModule } from '@ds/core/resources';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { MaterialModule } from '@ds/core/ui/material';
import { IConfig, NgxMaskModule } from 'ngx-mask';
import { DsWidgetModule } from '@ds/core/ui/ds-widget/ds-widget.module';
import { DepartmentsComponent } from './departments/departments.component';
import { TrackFormChangesDirective } from '@ds/core/ui/forms/change-track/track-form-changes.directive';
import { DivisionLocationDialogComponent } from './divisions/division-location-dialog/division-location-dialog.component';
import { DivisionsComponent } from './divisions/divisions.component';
import { EmployeeExportComponent } from '../employee-export/employee-export.component';
import { EmployeeExportModalComponent } from '../employee-export/employee-export-modal/employee-export-modal.component';

export const options: Partial<IConfig> | (() => Partial<IConfig>) = {};
@NgModule({
  declarations: [ShiftsComponent, DepartmentsComponent, DivisionsComponent, DivisionLocationDialogComponent, EmployeeExportComponent, EmployeeExportModalComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    DsCardModule,
    DsCoreFormsModule,
    UpgradeModule,
    ScrollingModule,
    LoadingMessageModule,
    DsCoreResourcesModule,
    NgxMaskModule.forRoot(options),
    DsWidgetModule,
  ],
  providers: [
    CurrencyPipe,
    DecimalPipe
  ],
  entryComponents: [
    DivisionLocationDialogComponent
  ]
})
export class LaborModule { }
