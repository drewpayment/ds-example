import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { CompanyManagementRoutingModule } from "./company-management-routing.module";
import { DsCoreResourcesModule } from "@ds/core/resources";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { DsCardModule } from "@ds/core/ui/ds-card";
import { DsCoreFormsModule } from "@ds/core/ui/forms";
import { LoadingMessageModule } from "@ds/core/ui/loading-message/loading-message.module";
import { MaterialModule } from "@ds/core/ui/material";

import { IntegrationSyncDashboardComponent } from './integration-sync-dashboard/integration-sync-dashboard.component';
import { FormatMissingFieldPipe } from './integration-sync-dashboard/missing-fields-formatter.pipe';
import { PayrollModule } from "@ds/payroll/payroll.module";
import { AdminTaskListComponent } from './admin-task/admin-task-list.component';
import { AdminTaskFormComponent } from './admin-task/admin-task-form/admin-task-form.component';
import { CompanyAdminGuard } from '../guards/company-admin.guard';
import { LaborModule } from './labor/labor.module';
import { DsConfirmDialogModule } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.module';
import { ClientGlAccountAssignmentComponent } from './general-ledger/client-gl-account-assignment/client-gl-account-assignment.component';
import { ClientGlGlobalMappingComponent } from './general-ledger/client-gl-global-mapping/client-gl-global-mapping.component';
import { SharedModule } from '../shared/shared.module';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { ClientGlAccountImportComponent } from './general-ledger/client-gl-account-import/client-gl-account-import.component';
import { ImportLedgersDialogComponent } from './general-ledger/client-gl-account-import/import-ledgers-dialog/import-ledgers-dialog.component';
import { formatAccountDescPipe } from './shared/format-account-desc.pipe';
import { ResourcesComponent } from "./resources/resources.component";
import { AddFolderDialogComponent } from "./resources/add-folder-dialog/add-folder-dialog.component";
import { AddResourceDialogComponent } from "./resources/add-resource-dialog/add-resource-dialog.component";
import { DeleteFolderDialogComponent } from "./resources/delete-folder-dialog/delete-folder-dialog.component";
import { CoreModule } from "@ds/core/core.module";
import { PreviewWelcomeMessageDialogComponent } from './onboarding/welcome-message/preview-welome-message/preview-welcome-message-dialog.component';
import { CustomPagesComponent } from './onboarding/custom-pages/custom-pages.component';
import { ManageResourcesComponent } from './onboarding/custom-pages/manage-resources/manage-resources.component';
import { ManageResourcesDialogComponent } from './onboarding/custom-pages/manage-resources/manage-resources-dialog/manage-resources-dialog.component';
import { ManageResourceItemDialogComponent } from './onboarding/custom-pages/manage-resources/manage-resource-item-dialog/manage-resource-item-dialog.component';
import { ManageFinalDisclaimerComponent } from './onboarding/final-disclaimer/manage-final-disclaimer.component';
import { PreviewFinalDisclaimerDialogComponent } from './onboarding/final-disclaimer/preview-final-disclaimer/preview-final-disclaimer-dialog.component';
import { ManageCorrespondenceTemplateComponent } from './onboarding/correspondence-template/manage-correspondence-template.component';
import { CustomizeSenderDialogComponent } from './onboarding/correspondence-template/customize-sender/customize-sender-dialog.component';
import { PreviewCorrespondenceTemplateDialogComponent } from './onboarding/correspondence-template/preview-correspondence-template/preview-correspondence-template-dialog.component';
import { ManageWelcomeMessageComponent } from './onboarding/welcome-message/manage-welcome-message.component';
import { FilterCustomPagesPipe } from '../shared/filter-custom-pages.pipe';
import { CachedSrcDirective } from "../shared/cached-src.directive";
import { EmployeeHeaderModule } from '@ds/employees/header/employee-header.module';

@NgModule({
  declarations: [
    IntegrationSyncDashboardComponent,
    FormatMissingFieldPipe,
    ClientGlAccountImportComponent,
    ImportLedgersDialogComponent,  
    formatAccountDescPipe,
    AdminTaskListComponent,
    AdminTaskFormComponent,
    ResourcesComponent,
    CachedSrcDirective,
    AddFolderDialogComponent,
    AddResourceDialogComponent,
    DeleteFolderDialogComponent,
    ManageCorrespondenceTemplateComponent,
    CustomizeSenderDialogComponent,
    PreviewCorrespondenceTemplateDialogComponent,
    ManageResourcesDialogComponent, 
    ManageResourceItemDialogComponent, 
    ManageWelcomeMessageComponent,
    PreviewWelcomeMessageDialogComponent,
    ManageFinalDisclaimerComponent,
    PreviewFinalDisclaimerDialogComponent,
    ManageResourcesComponent, 
    CustomPagesComponent, 
    FilterCustomPagesPipe,
    ClientGlAccountAssignmentComponent,
    ClientGlGlobalMappingComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    DsCardModule,
    DsCoreFormsModule,
    LoadingMessageModule,
    DsCoreResourcesModule,
    PayrollModule,
    LaborModule,
    DsConfirmDialogModule,
    CompanyManagementRoutingModule,
    CoreModule,
    SharedModule,
    ScrollingModule,
    EmployeeHeaderModule
  ],
  entryComponents: [
    ImportLedgersDialogComponent,
    AdminTaskListComponent,
    AdminTaskFormComponent,
    AddFolderDialogComponent,
    AddResourceDialogComponent,
    DeleteFolderDialogComponent,
    CustomizeSenderDialogComponent,
    PreviewCorrespondenceTemplateDialogComponent,
    ManageResourcesDialogComponent,
    ManageResourceItemDialogComponent,
    PreviewWelcomeMessageDialogComponent,
    PreviewFinalDisclaimerDialogComponent,
    SharedModule,
    ScrollingModule,
    
  ],
  providers: [
      CompanyAdminGuard,
      formatAccountDescPipe,
  ]
})
export class CompanyManagementModule {
    constructor() {}
}
