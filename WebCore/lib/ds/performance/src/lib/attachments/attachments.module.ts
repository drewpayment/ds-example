import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeeAttachmentViewerComponent } from './employee-attachment-viewer/employee-attachment-viewer.component';
import { MaterialModule } from '@ds/core/ui/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { CoreModule } from '@ds/core/core.module';
import { DateTimeModule } from '@ds/core/ui/datetime/datetime.module';



@NgModule({
  declarations: [
    EmployeeAttachmentViewerComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    DsCardModule,
    LoadingMessageModule,
    DateTimeModule,
    CoreModule
  ],
  exports: [
    EmployeeAttachmentViewerComponent
  ]
})
export class AttachmentsModule { }
