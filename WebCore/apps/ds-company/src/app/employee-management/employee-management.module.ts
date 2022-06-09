import { NgModule, Directive, Component, Injector, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeeManagementRoutingModule } from './employee-management-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '@ds/core/ui/material';
import { MatDialogModule } from '@angular/material/dialog';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { UpgradeModule } from '@angular/upgrade/static';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { DsCoreResourcesModule } from '@ds/core/resources';
import { DashboardComponent } from './onboarding/dashboard/dashboard.component';
import { DashboardDetailComponent } from './onboarding/dashboard-detail/dashboard-detail.component';

import { CertifyI9FormComponent } from '@ds/onboarding/certify-I9/certify-I9-form/certify-I9-form.component';
import { CertifyI9TriggerComponent, CertifyI9DialogComponent } from '@ds/onboarding/certify-I9/certify-I9-trigger/certify-I9-trigger.component';
import { I9DocumentTypePipe } from '@ds/onboarding/certify-I9/i9-document-type.pipe';
import { HireDateFilterPipe, OnboardingEmployeeFilterPipe } from 'apps/ds-company/src/app/shared/onboarding-employee.pipe';
import { SharedModule } from '../shared/shared.module';

import { PayrollModule } from '@ds/payroll/payroll.module';
import { TaxesComponent } from './taxes/taxes.component';
import { ConfigureCostCentersDialogComponent } from './taxes/configure-cost-centers-dialog/configure-cost-centers-dialog.component';
import { CoreModule } from '@ds/core/core.module';
import { EmployeeHeaderModule } from '@ds/employees/header/employee-header.module';
import { AddTaxDialogComponent } from './taxes/add-tax-dialog/add-tax-dialog.component';
import { EmergencyContactComponent } from './emergency-contact/emergency-contact.component';
import { EmergencyContactFormComponent } from './emergency-contact/emergency-contact-form/emergency-contact-form.component';
import { CloseOnboardingDialogComponent } from './onboarding/dashboard-detail/close-onboarding/close-onboarding-dialog.component';
import { ImportAdminTasklistDialogComponent } from './onboarding/dashboard-detail/import-admin-tasklist/import-admin-tasklist-dialog.component';
import { AddAttachmentDialogComponent } from './onboarding/dashboard-detail/add-attachment-dialog/add-attachment-dialog.component';
import { OnboardingSummaryDialogComponent } from './onboarding/dashboard-detail/onboarding-summary-dialog/onboarding-summary-dialog.component';

import { JobProfileModule } from '../job-profile/job-profile.module';
import { ClientSelectorDialogComponent } from './onboarding/add-employee/client-selector-dialog/client-selector-dialog.component';
import { JobTitleConfirmDialogComponent } from './onboarding/add-employee/job-title-confirm-dialog/job-title-confirm-dialog.component';
import { AddEmployeeComponent } from './onboarding/add-employee/add-employee.component';
import { DsProgressModule } from '@ds/core/ui/ds-progress';
import { AvatarModule } from '@ds/core/ui/avatar/avatar.module';
import { EmployeeAccrualsComponent } from './employee-accruals/employee-accruals.component';
import { AddClassDialogComponent } from './employee-accruals/add-class-dialog/add-class-dialog.component';
import { EventsComponent } from './events/events.component';
import { EmployeeSelfServiceModule } from '../employee-self-service/employee-self-service.module';
import { DsEventsModule } from '@ds/employees/events/ds-events.module';
import { AttachmentManagementModule } from "./attachments/attachment-management.module";

import { EmployeeDependentFormComponent } from './employee-dependents/employee-dependent-form/employee-dependent-form.component';
import { EmployeeDependentsComponent } from './employee-dependents/employee-dependents.component';

@NgModule({
  declarations: [
    I9DocumentTypePipe,
    CertifyI9FormComponent,
    CertifyI9DialogComponent,
    CertifyI9TriggerComponent,
    HireDateFilterPipe, OnboardingEmployeeFilterPipe,
    DashboardDetailComponent,
    DashboardComponent, 
    AddEmployeeComponent,
    ClientSelectorDialogComponent,
    JobTitleConfirmDialogComponent,
    CloseOnboardingDialogComponent,
    ImportAdminTasklistDialogComponent,
    AddAttachmentDialogComponent,
    OnboardingSummaryDialogComponent,
    TaxesComponent,
    ConfigureCostCentersDialogComponent,
    AddTaxDialogComponent,
    EventsComponent,    
    EmergencyContactComponent,
    EmergencyContactFormComponent,
    EmployeeAccrualsComponent,
    AddClassDialogComponent,  
    EmployeeDependentsComponent,
    EmployeeDependentFormComponent,    
  ],
  imports: [
    CommonModule,
    SharedModule,
    EmployeeManagementRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    MatDialogModule,
    DsCardModule,
    DsCoreFormsModule,
    UpgradeModule,
    LoadingMessageModule,
    DsCoreResourcesModule,
    JobProfileModule,
    DsProgressModule,
    AvatarModule,
    EmployeeHeaderModule,
    PayrollModule,
    CoreModule,
    DsEventsModule,
    AttachmentManagementModule,
  ],
  entryComponents: [
    CertifyI9DialogComponent,
    CertifyI9TriggerComponent,
    CloseOnboardingDialogComponent,
    ImportAdminTasklistDialogComponent,
    AddAttachmentDialogComponent,
    OnboardingSummaryDialogComponent,
    ClientSelectorDialogComponent,
    JobTitleConfirmDialogComponent,
    AddClassDialogComponent,
  ],
})
export class EmployeeManagementModule {
  constructor() { }
}
