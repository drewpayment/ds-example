import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ClientOutletComponent } from './client-outlet/client-outlet.component';
import { ClientOptionsComponent } from '@ds/core/clients/client-options/client-options.component';
import { ClientsModule } from '@ds/core/clients/clients.module';
import { ClientBankInfoModule, ClientBankInfoSetupFormComponent, ClientBankRelateFormComponent } from '@ds/core/clients/client-bank-info';
import { TaxDeferralsComponent } from './tax-deferrals/tax-deferrals.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { MaterialModule } from '@ds/core/ui/material';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { AddDeferralDialogComponent } from './tax-deferrals/add-deferral-dialog/add-deferral-dialog.component';
import { BrowserModule } from '@angular/platform-browser';
import { MatDialogModule } from '@angular/material/dialog';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { TermsAndConditionsComponent } from './terms-and-conditions/terms-and-conditions.component';

export const clientRoutes: Routes = [
  { path: 'company', children: [
      { path: '', component: ClientOutletComponent, children:
          [
              { path: '', redirectTo: 'options/:page', pathMatch:'full' },
              { path: 'options/:page', component: ClientOptionsComponent },
              { path: 'termsAndConditions', component: TermsAndConditionsComponent }
          ]
      },
  ]}
];

@NgModule({
  imports: [
    CommonModule,
    ClientsModule,
    MaterialModule,
    MatDialogModule,
    ClientBankInfoModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    DsCardModule,
    DsCoreFormsModule,
    ScrollingModule,
    LoadingMessageModule,
    RouterModule.forChild(clientRoutes)
  ],
  declarations: [
    ClientOutletComponent,
    TaxDeferralsComponent,
    AddDeferralDialogComponent,
    TermsAndConditionsComponent
  ],
  entryComponents:[
    ClientBankInfoSetupFormComponent,
    ClientBankRelateFormComponent,
    AddDeferralDialogComponent,
  ]
})
export class ClientModule { }
