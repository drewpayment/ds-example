import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PerformanceManagerRoutingModule } from './manager-routing.module';
import { ManagerHeaderComponent } from './manager-header/manager-header.component';
import { EmployeeSchedulerOutletComponent } from './employee-scheduler-outlet/employee-scheduler-outlet.component';
import { ReviewStatusOutletComponent } from './review-status-outlet/review-status-outlet.component';
import { ArchiveOutletComponent } from './archive-outlet/archive-outlet.component';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MultiReviewEditDialogComponent } from './multi-review-edit-dialog/multi-review-edit-dialog.component';
import { SupervisorOutletComponent } from './supervisor-outlet/supervisor-outlet.component';
import { CompetencyDialogComponent } from './competency-dialog/competency-dialog.component';
import { ReviewAnalyticsOutletComponent } from './review-analytics-outlet/review-analytics-outlet.component';
import { CanUserViewPayrollRequestsPipe } from './manager-header/can-user-view-payroll-requests.pipe';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { CoreModule } from '@ds/core/core.module';
import { DateTimeModule } from '@ds/core/ui/datetime/datetime.module';
import { DsPerformanceManagerModule } from '@ds/performance/performance-manager/performance-manager.module';
import { DsPerformanceReviewPolicyModule } from '@ds/performance/review-policy/review-policy.module';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { AvatarModule } from '@ds/core/ui/avatar/avatar.module';
import { EmployeeHeaderModule } from '@ds/employees/header/employee-header.module';

@NgModule({
    imports: [
        CommonModule,
        PerformanceManagerRoutingModule,
        MaterialModule,
        DsCardModule,
        DsPerformanceManagerModule,
        DsPerformanceReviewPolicyModule,
        FormsModule,
        ReactiveFormsModule,
        DsCoreFormsModule,
        CoreModule,
        DateTimeModule,
        LoadingMessageModule,
        AvatarModule,
        EmployeeHeaderModule,
    ],
    declarations: [
        ManagerHeaderComponent,
        EmployeeSchedulerOutletComponent,
        ReviewStatusOutletComponent,
        ArchiveOutletComponent,
        MultiReviewEditDialogComponent,
        SupervisorOutletComponent,
        CompetencyDialogComponent,
        ReviewAnalyticsOutletComponent,
        CanUserViewPayrollRequestsPipe
    ],
    entryComponents: [
        MultiReviewEditDialogComponent,
        SupervisorOutletComponent,
        CompetencyDialogComponent
    ]
})
export class PerformanceManagerModule { }
