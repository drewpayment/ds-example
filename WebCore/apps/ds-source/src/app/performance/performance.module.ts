import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

// NGX IMPORTS
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { PerformanceComponent } from './performance.component';
import { EmployeePerformanceAppModule } from './employees/employees.module';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsExpansionModule } from '@ds/core/ui/ds-expansion';
import { EvaluationOutletComponent } from './evaluations/evaluation-outlet/evaluation-outlet.component';
import { CompanyAppModule } from './company/company.module';
import { DsProgressModule } from '@ds/core/ui/ds-progress';
import { PerformanceSetupModule } from './setup/setup.module';
import { PerformanceManagerModule } from './manager/manager.module';
import { PrintableEvaluationComponent } from '@ds/performance/evaluations/printable-evaluation/printable-evaluation.component';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { ChartsModule } from 'ng2-charts';
import { DsCoreEmployeesModule } from '@ds/core/employees';
import { EvalSummaryGuard } from '@ds/performance/evaluations/evaluation-summary/eval-summary.guard';
import { RatingsEditComponent } from '@ds/performance/ratings/ratings-edit/ratings-edit.component';
import { CompetencySetupComponent } from '@ds/performance/competencies/competency-setup/competency-setup.component';
import { CompetencyModelComponent } from '@ds/performance/competencies/competency-model/competency-model.component';
import { FeedbackSetupListComponent } from '@ds/performance/feedback/feedback-setup-list/feedback-setup-list.component';
import { EmployeeGoalsListComponent } from '@ds/performance/goals/employee-goals-list/employee-goals-list.component';
import { EvaluationDetailComponent } from '@ds/performance/evaluations/evaluation-detail/evaluation-detail.component';
import { EvaluationSummaryComponent } from '@ds/performance/evaluations/evaluation-summary/evaluation-summary.component';
import { RatingsEditDialogComponent } from '@ds/performance/ratings/ratings-edit-dialog/ratings-edit-dialog.component';
import { CompetencyEditDialogComponent } from '@ds/performance/competencies/competency-edit-dialog/competency-edit-dialog.component';
import { DefaultCompetenciesDialogComponent } from '@ds/performance/competencies/default-competencies-dialog/default-competencies-dialog.component';
import { CompetencyDeleteConfirmDialogComponent } from '@ds/performance/competencies/competency-delete-confirm-dialog/competency-delete-confirm-dialog.component';
import { AddGoalDialogComponent } from '@ds/performance/goals/add-goal-dialog/add-goal-dialog.component';
import { FeedbackSetupDialogComponent } from '@ds/performance/feedback/feedback-setup-dialog/feedback-setup-dialog.component';
import { ReviewTemplateDialogComponent } from '@ds/performance/review-policy';
import { ReviewSummaryDialogComponent } from '@ds/performance/performance-manager/review-summary-dialog/review-summary-dialog.component';
import { DefaultFeedbacksDialogComponent } from '@ds/performance/feedback/default-feedbacks-dialog/default-feedbacks-dialog.component';
import { PerformanceModule } from '@ds/performance/performance.module';
import { PayrollRequestReportComponent } from '@ds/performance/performance-manager/payroll-requests/payroll-request-report/payroll-request-report.component';
import { EmployeeAttachmentViewerComponent } from '@ds/performance/attachments/employee-attachment-viewer/employee-attachment-viewer.component';
import { CommonModule } from '@angular/common';

// some ajs emp search functionality needs to know the route to the EvaluationDetailComponent
export const PerformanceBase = 'performance';
export const EvaluationFormOutlet = 'evaluations';
export const EvaluationDetail = 'detail';

/**
 * Made into export const so it can be imported into Ng2UrlHandlingStrategy to
 * only use NGX router when the URL qualifies a NGX route
 *
 */
export const routes: Routes = [
  {
    path: PerformanceBase,
    children: [
      { path: '', component: PerformanceComponent },
      { path: 'setup/ratings', component: RatingsEditComponent },
      { path: 'setup/competencies', component: CompetencySetupComponent },
      {
        path: 'setup/competencies/models',
        component: CompetencyModelComponent,
      },
      { path: 'setup/feedback', component: FeedbackSetupListComponent },
      {
        path: 'goals',
        component: EmployeeGoalsListComponent,
        data: { isEmployeeGoals: false, isEss: false },
      }, // TODO: Update with company goals component
      {
        path: EvaluationFormOutlet + '/:evaluationId',
        component: EvaluationOutletComponent,
        children: [
          { path: '', redirectTo: 'detail', pathMatch: 'full' },
          { path: EvaluationDetail, component: EvaluationDetailComponent },
          {
            path: 'summary',
            component: EvaluationSummaryComponent,
            canActivate: [EvalSummaryGuard],
          },
          { path: 'attachments', component: EmployeeAttachmentViewerComponent },
        ],
      },
      { path: 'print-evaluation', component: PrintableEvaluationComponent },
      { path: 'print-total-payout', component: PayrollRequestReportComponent },
    ],
  },
];

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    EmployeePerformanceAppModule,
    PerformanceSetupModule,
    PerformanceManagerModule,
    CompanyAppModule,
    DsExpansionModule,
    DsCardModule,
    DsProgressModule,
    DsCoreFormsModule,
    ChartsModule,
    DsCoreEmployeesModule,
    PerformanceModule,

    RouterModule.forChild(routes),
  ],
  declarations: [PerformanceComponent, EvaluationOutletComponent],
  entryComponents: [
    PerformanceComponent,
    RatingsEditDialogComponent,
    CompetencyEditDialogComponent,
    DefaultCompetenciesDialogComponent,
    CompetencyDeleteConfirmDialogComponent,
    AddGoalDialogComponent,
    FeedbackSetupDialogComponent,
    ReviewTemplateDialogComponent,
    ReviewSummaryDialogComponent,
    DefaultFeedbacksDialogComponent,
  ],
})
export class PerformanceAppModule {}
