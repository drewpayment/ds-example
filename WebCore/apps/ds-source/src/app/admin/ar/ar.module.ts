import {
  NgModule,
  Type,
  ComponentFactoryResolver,
  Inject,
  ApplicationRef,
} from '@angular/core';
import { CommonModule, DOCUMENT } from '@angular/common';
import { ArReportingComponent } from './ar-reporting/ar-reporting.component';
import { ArManualInvoiceComponent } from './ar-manual-invoice/ar-manual-invoice.component';
import { ArDepositsComponent } from './ar-deposits/ar-deposits.component';
import { RouterModule, Routes } from '@angular/router';
import { ArOutletComponent } from './ar-outlet/ar-outlet.component';
import { MaterialModule } from '@ds/core/ui/material';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ClientSelectorComponent } from './client-selector/client-selector.component';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { EditPostingDialogComponent } from './edit-posting-dialog/edit-posting-dialog.component';
import { EditPostingTriggerComponent } from './edit-posting-dialog/edit-posting-trigger.component';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { ArDominionCheckPaymentComponent } from './ar-dominion-check-payment/ar-dominion-check-payment.component';
import { ArClientCheckPaymentComponent } from './ar-client-check-payment/ar-client-check-payment.component';
import { ArDominionCheckPaymentGuard } from './ar-dominion-check-payment/ar-dominion-check-payment.guard';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSortModule } from '@angular/material/sort';

export const ArRoutes: Routes = [
  {
    path: 'ar',
    children: [
      { path: 'reporting', component: ArReportingComponent },
      { path: 'deposits', component: ArDepositsComponent },
      { path: 'manualInvoice', component: ArManualInvoiceComponent },
      {
        path: 'deposits/dominionCheckPayment/:id',
        component: ArDominionCheckPaymentComponent,
        canActivate: [ArDominionCheckPaymentGuard],
      },
      {
        path: 'deposits/clientCheckPayment/:id',
        component: ArClientCheckPaymentComponent,
      },
      { path: '', component: ArDepositsComponent },
    ],
  },
];

@NgModule({
  declarations: [
    ArReportingComponent,
    ArManualInvoiceComponent,
    ArDepositsComponent,
    ArOutletComponent,
    ClientSelectorComponent,
    ArDominionCheckPaymentComponent,
    ArClientCheckPaymentComponent,
    EditPostingDialogComponent,
    EditPostingTriggerComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(ArRoutes),
    DsCardModule,
    FormsModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MaterialModule,
    DsCoreFormsModule,
    LoadingMessageModule,
    MatSortModule,
  ],
  entryComponents: [EditPostingDialogComponent, EditPostingTriggerComponent],
  exports: [RouterModule],
  providers: [ArDominionCheckPaymentGuard],
})
export class ArModule {}
