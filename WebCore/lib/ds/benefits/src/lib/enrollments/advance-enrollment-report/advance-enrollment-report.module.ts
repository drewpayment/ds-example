import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdvanceEnrollmentReportTriggerComponent } from './advance-enrollment-report-trigger/advance-enrollment-report-trigger.component';
import { AdvanceEnrollmentReportDialogComponent } from './advance-enrollment-report-dialog/advance-enrollment-report-dialog.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AjsUpgradesModule } from '@ds/core/ajs-upgrades/ajs-upgrades.module';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { DateTimeModule } from '@ds/core/ui/datetime/datetime.module';

@NgModule({
    declarations: [AdvanceEnrollmentReportTriggerComponent, AdvanceEnrollmentReportDialogComponent],
    imports: [
        CommonModule,
        MaterialModule,
        FormsModule,
        ReactiveFormsModule,
        AjsUpgradesModule,
        DateTimeModule,
        DsCoreFormsModule,
        DsCardModule
    ],
    exports: [AdvanceEnrollmentReportTriggerComponent, AdvanceEnrollmentReportDialogComponent]
})
export class DsAdvanceEnrollmentReportModule { }
