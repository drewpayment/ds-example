import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '@ds/core/ui/material';
import { MatDialogModule } from '@angular/material/dialog';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { UpgradeModule } from '@angular/upgrade/static';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { CoreModule } from '@ds/core/core.module';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { JobProfileListComponent } from './job-profile-list/job-profile-list.component';
import { FilterJobProfilesPipe } from 'apps/ds-company/src/app/shared/filter-job-profiles.pipe';
import { JobProfileTitleDialogComponent } from './job-profile-title-dialog/job-profile-title-dialog.component';//
import { JobProfileDialogComponent } from './job-profile-dialog/job-profile-dialog.component';
import { JobProfileDetailsComponent } from './job-profile-details/job-profile-details.component';
import { EmployeeTasksComponent } from './employee-tasks/employee-tasks.component';
import { FilterOnboardingWorkflowTasksPipe } from 'apps/ds-company/src/app/shared/filter-onboarding-workflow-tasks.pipe';
import { JobProfileRoutingModule } from './job-profile-routing.module';
import { CompetenciesModule } from '@ds/performance/competencies/competencies.module';
export const options: Partial<IConfig> | (() => Partial<IConfig>) = {};

@NgModule({
  declarations: [
    JobProfileListComponent,
    FilterJobProfilesPipe,
    FilterOnboardingWorkflowTasksPipe,
    JobProfileTitleDialogComponent,
    JobProfileDialogComponent,
    JobProfileDetailsComponent,
    EmployeeTasksComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    MatDialogModule,
    DsCardModule,
    DsCoreFormsModule,
    UpgradeModule,
    LoadingMessageModule,
    CoreModule,
    CompetenciesModule,
    JobProfileRoutingModule,
    NgxMaskModule.forRoot(options)
  ],
  entryComponents: [
    JobProfileListComponent,
    JobProfileTitleDialogComponent,
    JobProfileDialogComponent,
  ],  
  exports: [
    FilterJobProfilesPipe,
    FilterOnboardingWorkflowTasksPipe,
    EmployeeTasksComponent,
    JobProfileListComponent,
  ]
})
export class JobProfileModule { }