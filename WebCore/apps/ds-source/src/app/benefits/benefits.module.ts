import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdvanceEnrollmentReportDialogComponent, AdvanceEnrollmentReportTriggerComponent, DsAdvanceEnrollmentReportModule } from '@ds/benefits/enrollments/advance-enrollment-report';
import { DsBenefitsCopyPlansModule } from '@ds/benefits/plans/copy-plans/copy-plans.module'
import { CopyPlansTriggerComponent, CopyPlansDialogComponent } from '@ds/benefits/plans/copy-plans/';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
    declarations: [],
    imports: [
        CommonModule,
        HttpClientModule,
		DsAdvanceEnrollmentReportModule,
        DsBenefitsCopyPlansModule,
    ],
    entryComponents: [
    	AdvanceEnrollmentReportTriggerComponent,
      	AdvanceEnrollmentReportDialogComponent,
        CopyPlansTriggerComponent,
        CopyPlansDialogComponent
    ]
})
export class DsBenefitsAppModule { }
