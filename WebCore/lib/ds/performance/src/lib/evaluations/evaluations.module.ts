import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AjsUpgradesModule } from '@ds/core/ajs-upgrades/ajs-upgrades.module';
import { CoreModule } from '@ds/core/core.module';
import { DateTimeModule } from '@ds/core/ui/datetime/datetime.module';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsProgressModule } from '@ds/core/ui/ds-progress';
import { RouterModule } from '@angular/router';
import { EvaluationSummaryDialogComponent } from '@ds/performance/evaluations/evaluation-summary-dialog/evaluation-summary-dialog.component';
import { GoalWeightingDialogComponent } from '@ds/performance/evaluations/goal-weighting/goal-weighting-dialog/goal-weighting-dialog.component';
import { PrintableEvaluationComponent } from '@ds/performance/evaluations/printable-evaluation/printable-evaluation.component';
import { ChartsModule } from 'ng2-charts';
import { EvaluationHeaderComponent } from '@ds/performance/evaluations/evaluation-header/evaluation-header.component';
import { EvaluationDetailComponent } from '@ds/performance/evaluations/evaluation-detail/evaluation-detail.component';
import { EvaluationSummaryComponent } from '@ds/performance/evaluations/evaluation-summary/evaluation-summary.component';
import { EvaluationPayrollRequestComponent } from '@ds/performance/evaluations/evaluation-payroll-request/evaluation-payroll-request.component';
import { ScoringGraphComponent } from '@ds/performance/evaluations/scoring-graph/scoring-graph.component';
import { EvalScoreToStringPipe } from '@ds/performance/evaluations/printable-evaluation/eval-score-to-string.pipe';
import { StatusPipe } from './evaluation-detail/approval-process/status.pipe';
import { ApprovalProcessSummaryDialogComponent } from './approval-process-summary-dialog/approval-process-summary-dialog.component';
import { DsPerformanceFeedbackModule } from '../feedback/feedback.module';
import { RecommendBonusContainerComponent } from './recommend-bonus-container/recommend-bonus-container.component';
import { RecommendedBonusComponent } from './recommend-bonus-container/recommended-bonus/recommended-bonus.component';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    AjsUpgradesModule,
    DateTimeModule,
    DsCoreFormsModule,
    DsCardModule,
    DsProgressModule,
    DsPerformanceFeedbackModule,
    ChartsModule,
    CoreModule,
  ],
  declarations: [
    EvaluationSummaryDialogComponent,
    GoalWeightingDialogComponent,
    PrintableEvaluationComponent,
    EvaluationHeaderComponent,
    EvaluationDetailComponent,
    EvaluationSummaryComponent,
    EvaluationPayrollRequestComponent,
    ScoringGraphComponent,
    EvalScoreToStringPipe,
    StatusPipe,
    ApprovalProcessSummaryDialogComponent,
    RecommendBonusContainerComponent,
    RecommendedBonusComponent,
  ],
  exports: [
    EvaluationSummaryDialogComponent,
    GoalWeightingDialogComponent,
    EvaluationHeaderComponent,
    EvaluationDetailComponent,
    EvaluationSummaryComponent,
    EvaluationPayrollRequestComponent,
    ApprovalProcessSummaryDialogComponent,
  ],
  entryComponents: [
    EvaluationSummaryDialogComponent,
    GoalWeightingDialogComponent,
    ApprovalProcessSummaryDialogComponent,
  ],
})
export class DsEvaluationsModule {}
