import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportRunnerComponent } from './report-runner/report-runner.component';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AjsUpgradesModule } from '@ds/core/ajs-upgrades/ajs-upgrades.module';
import { DsExpansionModule } from '@ds/core/ui/ds-expansion';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsProgressModule } from '@ds/core/ui/ds-progress';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { DsAutocompleteModule } from '@ds/core/ui/ds-autocomplete';

@NgModule({
  declarations: [ReportRunnerComponent],
  exports: [ReportRunnerComponent],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    AjsUpgradesModule,
    DsExpansionModule,
    DsCardModule,
    DsProgressModule,
    DsCoreFormsModule,
    DsAutocompleteModule
  ]
})
export class ChangeHistoryModule { }
