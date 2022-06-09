import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BankSetupFormComponent } from '@ds/core/banks/bank-setup/bank-setup-form/bank-setup-form.component';
import { FormsModule } from '@angular/forms';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { AjsUpgradesModule } from '@ds/core/ajs-upgrades/ajs-upgrades.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    DsCardModule,
    DsCoreFormsModule,
    AjsUpgradesModule,
  ],
  declarations: [BankSetupFormComponent],
  exports: [BankSetupFormComponent],
})
export class BankSetupModule {}
