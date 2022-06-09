import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DsExpansionModule } from '@ds/core/ui/ds-expansion';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { ResourcesListComponent } from './resources-list/resources-list.component';
import { DsCustomFilterCallbackPipe } from '@ds/core/shared/ds-custom-filter-callback.pipe';
import { HttpClientModule } from '@angular/common/http';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';

@NgModule({
  imports: [
    HttpClientModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    DsExpansionModule,
    DsCardModule,
    LoadingMessageModule,
  ],
  declarations: [ResourcesListComponent, DsCustomFilterCallbackPipe],
  entryComponents: [],
  exports: [ResourcesListComponent, DsCustomFilterCallbackPipe],
})
export class DsEmployeeResourcesModule {}
