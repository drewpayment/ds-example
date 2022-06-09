import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DsCoreResourcesModule } from '@ds/core/resources/resources.module';
import { DsAutocompleteModule } from '@ds/core/ui/ds-autocomplete';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { MaterialModule } from '@ds/core/ui/material';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { NgxIndexedDBModule } from 'ngx-indexed-db';
import { DbStoreService } from './db-store/db-store.service';
import { EmployeeHeaderComponent } from './employee-header.component';
import { EmployeeEffects } from './ngrx/effects';
import * as fromReducer from './ngrx/reducer';
import { EmployeeHeaderEmployeeStatusPipe } from './pipes/ee-header-emp-status.pipe';
import { EmployeeHeaderFilterGroupNamePipe } from './pipes/ee-header-filter-group.pipe';
import { EmployeeHeaderFilterTypePipe } from './pipes/ee-header-filter-type.pipe';
import { MapEmpSearchResultToIEmployeeImagePipe } from './pipes/ee-header-map-to-ee-image.pipe';
import { EmployeeHeaderNavArrowsTooltipPipe } from './pipes/ee-header-nav-arrows-tooltip.pipe';
import { EmployeeHeaderPayTypePipe } from './pipes/ee-header-pay-type.pipe';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    LoadingMessageModule,
    DsCardModule,
    DsAutocompleteModule,
    StoreModule.forFeature(
      fromReducer.employeeSearchStoreKey,
      fromReducer.EmployeeReducer
    ),
    EffectsModule.forFeature([EmployeeEffects]),
    NgxIndexedDBModule.forRoot(DbStoreService.configFactory()),
    DsCoreResourcesModule,
  ],
  exports: [
    EmployeeHeaderComponent,
    EmployeeHeaderComponent,
    EmployeeHeaderPayTypePipe,
    EmployeeHeaderEmployeeStatusPipe,
    EmployeeHeaderNavArrowsTooltipPipe,
    EmployeeHeaderFilterGroupNamePipe,
    EmployeeHeaderFilterTypePipe,
    MapEmpSearchResultToIEmployeeImagePipe,
  ],
  declarations: [
    EmployeeHeaderComponent,
    EmployeeHeaderPayTypePipe,
    EmployeeHeaderEmployeeStatusPipe,
    EmployeeHeaderNavArrowsTooltipPipe,
    EmployeeHeaderFilterGroupNamePipe,
    EmployeeHeaderFilterTypePipe,
    MapEmpSearchResultToIEmployeeImagePipe,
  ],
  providers: [EmployeeEffects, DbStoreService],
})
export class EmployeeHeaderModule {}
