import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BankSetupModule, BankSetupFormComponent } from '@ds/core/banks/bank-setup';

@NgModule({
    declarations: [],
    imports: [
        CommonModule,
        BankSetupModule
    ],
    entryComponents: [
        BankSetupFormComponent
    ]
})
export class BanksAppModule { }
