import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DsExpansionModule } from '@ds/core/ui/ds-expansion';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { AccountSettingsComponent } from './account-settings.component';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';

@NgModule({
    imports: [
        BrowserModule,
        MaterialModule,
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        DsExpansionModule,
        DsCardModule,
        LoadingMessageModule
  ],
  declarations: [AccountSettingsComponent],
  entryComponents: [],
    exports: [AccountSettingsComponent]
})
export class DsEmployeeAccountSettingsModule { }
