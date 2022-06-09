import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AutomaticBillingComponent } from './automatic-billing/automatic-billing.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '@ds/core/ui/material';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { ClientManagementRoutingModule } from './client-management-routing.module';
import { AutomaticBillingDialogComponent } from './automatic-billing/automatic-billing-dialog/automatic-billing-dialog.component';
import { BillingComponent } from './billing/billing.component';
import { BillingDialogComponent } from './billing/billing-dialog/billing-dialog.component';
import { FeatureComponent } from './feature/feature.component';
import { UpgradeModule } from '@angular/upgrade/static';
import { BillingService } from './services/billing.service';
import { ClientSelectorService } from '@ds/core/ui/menu-wrapper-header/ng-client-selector/client-selector.service';
import { DsCoreResourcesModule } from '@ds/core/resources';
import { NPSDashboardComponent } from './nps/nps-dashboard.component';
import { NpsService } from '../services/nps.service';
import { DsWidgetModule } from '@ds/core/ui/ds-widget/ds-widget.module';
import { ClientAccrualsComponent } from './client-accruals/client-accruals.component';
import { ClientAccrualsAccrualSchedulesCardComponent } from './client-accruals/schedules-tab/cards/client-accruals-accrual-schedules-card/client-accruals-accrual-schedules-card.component';
import { ClientAccrualsFirstYearAccrualSchedulesCardComponent } from './client-accruals/schedules-tab/cards/client-accruals-first-year-accrual-schedules-card/client-accruals-first-year-accrual-schedules-card.component';
import { ClientAccrualsSchedulesTabComponent } from './client-accruals/schedules-tab/schedules-tab.component';
import { ClientAccrualsAtmExportCardComponent } from './client-accruals/setup-tab/cards/client-accruals-atm-export-card/client-accruals-atm-export-card.component';
import { ClientAccrualsComputationCardComponent } from './client-accruals/setup-tab/cards/client-accruals-computation-card/client-accruals-computation-card.component';
import { ClientAccrualsDisplayCardComponent } from './client-accruals/setup-tab/cards/client-accruals-display-card/client-accruals-display-card.component';
import { ClientAccrualsEarningsCardComponent } from './client-accruals/setup-tab/cards/client-accruals-earnings-card/client-accruals-earnings-card.component';
import { ClientAccrualsEligibilityCardComponent } from './client-accruals/setup-tab/cards/client-accruals-eligibility-card/client-accruals-eligibility-card.component';
import { ClientAccrualsFooterCardComponent } from './client-accruals/setup-tab/cards/client-accruals-footer-card/client-accruals-footer-card.component';
import { ClientAccrualsGeneralCardComponent } from './client-accruals/setup-tab/cards/client-accruals-general-card/client-accruals-general-card.component';
import { ClientAccrualsPaidLeaveActCardComponent } from './client-accruals/setup-tab/cards/client-accruals-paid-leave-act-card/client-accruals-paid-leave-act-card.component';
import { ClientAccrualsTimeOffCardComponent } from './client-accruals/setup-tab/cards/client-accruals-time-off-card/client-accruals-time-off-card.component';
import { ClientAccrualsSetupComponent } from './client-accruals/setup-tab/client-accruals-setup.component';
import { ClockModule } from './clock/clock.module';
import { MatDialogModule } from '@angular/material/dialog';
import { OrganizationClientComponent } from './organization/organization-client/organization-client.component';
import { AddClientDialogComponent } from './organization/organization-client/add-client-dialog/add-client-dialog.component';
import { AddOrganizationDialogComponent } from './organization/organization-client/add-organization-dialog/add-organization-dialog.component';
import { DsConfirmDialogModule } from "@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.module";
import { BankHolidayEditDialogComponent } from './maintenance/bank-holidays/bank-holiday-edit-dialog/bank-holiday-edit-dialog.component';
import { BankHolidaysComponent } from './maintenance/bank-holidays/bank-holidays.component';
import { SharedModule } from '../shared/shared.module';
import { ClientGlControlComponent } from './general-ledger/client-gl-control/client-gl-control.component';
import { ClientGlCustomClassDialogComponent } from './general-ledger/client-gl-custom-class-dialog/client-gl-custom-class-dialog.component';
import { ClientGlCustomClassTriggerComponent } from './general-ledger/client-gl-custom-class-trigger/client-gl-custom-class-trigger.component';
import { GlClassGroupsComponent } from './general-ledger/gl-class-groups/gl-class-groups.component';
import { W2ProcessingComponent } from './w2/w2-processing/w2-processing.component';
import { NotesDialogComponent } from './w2/w2-processing/notes-dialog/notes-dialog.component';
import { AttachmentManagementModule } from "../employee-management/attachments/attachment-management.module";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    MatDialogModule,
    DsCardModule,
    DsCoreFormsModule,
    UpgradeModule,
    ScrollingModule,
    LoadingMessageModule,
    ClientManagementRoutingModule,

    // APP MODULES
    // ClientAccrualsModule,

    DsCoreResourcesModule,
    ClockModule,
    DsWidgetModule,
    DsConfirmDialogModule,
    SharedModule,
    AttachmentManagementModule
  ],
  declarations: [
    AutomaticBillingComponent,
    AutomaticBillingDialogComponent,
    BillingComponent,
    BillingDialogComponent,
    FeatureComponent,
    NPSDashboardComponent,
    ClientAccrualsComponent,
    ClientAccrualsSetupComponent,
    ClientAccrualsGeneralCardComponent,
    ClientAccrualsPaidLeaveActCardComponent,
    ClientAccrualsEligibilityCardComponent,
    ClientAccrualsComputationCardComponent,
    ClientAccrualsDisplayCardComponent,
    ClientAccrualsAtmExportCardComponent,
    ClientAccrualsEarningsCardComponent,
    ClientAccrualsTimeOffCardComponent,
    ClientAccrualsFooterCardComponent,
    ClientAccrualsSchedulesTabComponent,
    ClientAccrualsAccrualSchedulesCardComponent,
    ClientAccrualsFirstYearAccrualSchedulesCardComponent,
    OrganizationClientComponent,
    AddClientDialogComponent,
    AddOrganizationDialogComponent,
    W2ProcessingComponent,
    NotesDialogComponent,
    BankHolidaysComponent,
    BankHolidayEditDialogComponent,
    ClientGlControlComponent,
    ClientGlCustomClassDialogComponent,
    ClientGlCustomClassTriggerComponent,
    GlClassGroupsComponent,
  ],
  entryComponents: [
    AutomaticBillingDialogComponent,
    BillingDialogComponent,
    NPSDashboardComponent,
    OrganizationClientComponent,
    AddClientDialogComponent,
    AddOrganizationDialogComponent,
    NotesDialogComponent,
    BankHolidayEditDialogComponent,
    ClientGlCustomClassDialogComponent,
  ],
  providers: [
    BillingService,
    ClientSelectorService,
    NpsService,
  ],
  exports: [
    NPSDashboardComponent,
    OrganizationClientComponent,
  ]
  })
export class ClientManagementModule {
  constructor() { }
}
