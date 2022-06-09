import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DsExpansionModule } from '@ds/core/ui/ds-expansion';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DirectDepositListComponent } from './direct-deposit-list/direct-deposit-list.component';
import { EmployeeDirectDepositsComponent } from './employee-direct-deposits/employee-direct-deposits.component';
import { EmployeeDirectDepositsFormComponent } from './employee-direct-deposits-form/employee-direct-deposits-form.component';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';

export const options: Partial<IConfig> | (() => Partial<IConfig>) = {};

@NgModule({
  imports: [
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    DsExpansionModule,
    DsCardModule,
    LoadingMessageModule,
    DsCoreFormsModule,

    NgxMaskModule.forRoot(options),
  ],
  declarations: [
    DirectDepositListComponent,
    EmployeeDirectDepositsComponent,
    EmployeeDirectDepositsFormComponent,
  ],
  entryComponents: [EmployeeDirectDepositsFormComponent],
  exports: [DirectDepositListComponent, EmployeeDirectDepositsComponent],
})
export class DsEmployeeDirectDepositsModule {}
