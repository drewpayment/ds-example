import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CopyPlansDialogComponent } from './copy-plans-dialog/copy-plans-dialog.component';
import { CopyPlansTriggerComponent } from './copy-plans-trigger/copy-plans-trigger.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AjsUpgradesModule } from '@ds/core/ajs-upgrades/ajs-upgrades.module';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { DateTimeModule } from '@ds/core/ui/datetime/datetime.module';

@NgModule({
    declarations: [
        CopyPlansDialogComponent,
        CopyPlansTriggerComponent
    ],
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
    exports: [
        CopyPlansDialogComponent,
        CopyPlansTriggerComponent
    ]
})
export class DsBenefitsCopyPlansModule { }
