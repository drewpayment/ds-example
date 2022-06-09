import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '@ds/core/ui/material';
import { MatDialogModule } from '@angular/material/dialog';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { UpgradeModule } from '@angular/upgrade/static';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { DsCoreResourcesModule } from '@ds/core/resources';
import { DropdownItemSelectorComponent } from './dropdown-item-selector/dropdown-item-selector.component';
import { ClientGlReportOptionsTriggerComponent } from './dialogs/client-gl-report-options-trigger/client-gl-report-options-trigger.component';
import { ClientGlReportOptionsDialogComponent } from './dialogs/client-gl-report-options-dialog/client-gl-report-options-dialog.component';
import { AddAttachmentFolderDialogComponent } from "./dialogs/add-attachment-folder-dialog/add-attachment-folder-dialog.component";
import { AddEmployeeAttachmentDialogComponent } from "./dialogs/add-attachment-dialog/add-attachment-dialog.component";

@NgModule({
  declarations: [
    DropdownItemSelectorComponent,
    ClientGlReportOptionsTriggerComponent,
    ClientGlReportOptionsDialogComponent,
    AddAttachmentFolderDialogComponent,
    AddEmployeeAttachmentDialogComponent
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
  ],
  entryComponents: [
    DropdownItemSelectorComponent,
    ClientGlReportOptionsDialogComponent,
    AddAttachmentFolderDialogComponent,
    AddEmployeeAttachmentDialogComponent
  ],
  exports: [
    DropdownItemSelectorComponent,
    ClientGlReportOptionsTriggerComponent,
  ],
})
export class SharedModule {
  constructor() { }
}
