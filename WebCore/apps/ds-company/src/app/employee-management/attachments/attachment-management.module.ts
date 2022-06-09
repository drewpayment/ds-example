import { NgModule, Directive, Component, Injector, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '@ds/core/ui/material';
import { MatDialogModule } from '@angular/material/dialog';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { UpgradeModule } from '@angular/upgrade/static';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { DsCoreResourcesModule } from '@ds/core/resources';
import { CoreModule } from '@ds/core/core.module';
import { EmployeeHeaderModule } from '@ds/employees/header/employee-header.module';
import { DsProgressModule } from '@ds/core/ui/ds-progress';
import { AvatarModule } from '@ds/core/ui/avatar/avatar.module';
import { AttachmentsComponent } from "./attachments.component";

@NgModule({
  declarations: [
    AttachmentsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    MatDialogModule,
    DsCardModule,
    DsCoreFormsModule,
    UpgradeModule,
    LoadingMessageModule,
    DsCoreResourcesModule,
    DsProgressModule,
    AvatarModule,
    EmployeeHeaderModule,
    CoreModule,
  ],
  entryComponents: [
  ],
})
export class AttachmentManagementModule {
  constructor() { }
}
