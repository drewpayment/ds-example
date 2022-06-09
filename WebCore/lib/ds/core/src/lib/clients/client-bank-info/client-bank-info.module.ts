import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClientBankInfoSetupFormComponent } from '@ds/core/clients/client-bank-info/client-bank-info-setup-form/client-bank-info-setup-form.component';
import { ClientBankRelateFormComponent } from '@ds/core/clients/client-bank-info/client-bank-relate-form/client-bank-relate-form.component';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        DsCardModule,
        DsCoreFormsModule
    ],
    declarations: [
        ClientBankInfoSetupFormComponent, 
        ClientBankRelateFormComponent
    ],
    exports: [
        ClientBankInfoSetupFormComponent,
        ClientBankRelateFormComponent
    ]
})
export class ClientBankInfoModule { }
