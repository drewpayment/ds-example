import { NgModule } from "@angular/core";
import { CommonModule, CurrencyPipe } from "@angular/common";
import { MaterialModule } from "@ds/core/ui/material/material.module";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AjsUpgradesModule } from "@ds/core/ajs-upgrades/ajs-upgrades.module";
import { CoreModule } from "@ds/core/core.module";
import { DateTimeModule } from "@ds/core/ui/datetime/datetime.module";
import { DsCoreFormsModule } from "@ds/core/ui/forms/forms.module";
import { DsCardModule } from "@ds/core/ui/ds-card";
import { ReviewStatusCardComponent } from "@ds/performance/performance-manager/review-status-card/review-status-card.component";
import { ReviewGroupStatusViewComponent } from "@ds/performance/performance-manager/review-group-status-view/review-group-status-view.component";
import { ReviewSummaryDialogComponent } from "@ds/performance/performance-manager/review-summary-dialog/review-summary-dialog.component";
import { ReviewGroupAnalyticsComponent } from "@ds/performance/performance-manager/review-group-analytics/review-group-analytics.component";
import { ReviewScoringGraphComponent } from "@ds/performance/performance-manager/review-scoring-graph/review-scoring-graph.component";
import { EmployeesRatingListDialogComponent } from "@ds/performance/performance-manager/review-scoring-graph/employees-rating-list-dialog/employees-rating-list-dialog.component";
import { ReviewCompetencyGraphComponent } from "@ds/performance/performance-manager/review-scoring-graph/review-competency-graph.component";
import { SharedModule } from "@ds/performance/shared/shared.module";
import { PayrollRequestsComponent } from "./payroll-requests/payroll-requests.component";
import { TextFeedbacksComponent } from "@ds/performance/performance-manager/review-group-status-view/text-feedbacks/text-feedbacks.component";
import { YesNoFeedbacksGraphComponent } from "@ds/performance/performance-manager/review-group-status-view/yesno-feedbacks-graph/yesno-feedbacks-graph.component";
import { ChartsModule } from "ng2-charts";
import { MultiSelectFeedbacksGraphComponent } from "./review-group-status-view/multiselect-feedbacks-graph/multiselect-feedbacks-graph.component";
import { EmployeeEvaluationsDialogComponent } from "./review-group-status-view/employee-evaluations/employee-evaluations-dialog.component";
import {
  ProposalApprovalStatusPipe,
  ProposalItemsApprovalStatusPipe,
} from "./payroll-requests/review-approval-status-pipe/proposal-approval-status.pipe";
import { PayrollRequestRequestTypePipe } from "./payroll-requests/review-approval-status-pipe/payroll-request-request-type.pipe";
import { PayrollRequestReportComponent } from "./payroll-requests/payroll-request-report/payroll-request-report.component";
import { PayrollRequestReportHeaderComponent } from "./payroll-requests/payroll-request-report/presentation/payroll-request-report-header/payroll-request-report-header.component";
import { PayrollRequestEmpSectionComponent } from "./payroll-requests/payroll-request-report/presentation/payroll-request-emp-section/payroll-request-emp-section.component";
import { PayrollRequestReportCurrencyPipe } from "./payroll-requests/payroll-request-report/shared/payroll-request-report-currency.pipe";
import { SafeStylePipe } from "./review-scoring-graph/safe-style.pipe";
import { LoadingMessageModule } from "@ds/core/ui/loading-message/loading-message.module";
import { AvatarModule } from "@ds/core/ui/avatar/avatar.module";

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    AjsUpgradesModule,
    DateTimeModule,
    DsCoreFormsModule,
    DsCardModule,
    CoreModule,
    ChartsModule,
    SharedModule,
    LoadingMessageModule,
    AvatarModule
  ],
  declarations: [
    ReviewStatusCardComponent,
    ReviewGroupStatusViewComponent,
    ReviewSummaryDialogComponent,
    ReviewGroupAnalyticsComponent,
    ReviewScoringGraphComponent,
    ReviewCompetencyGraphComponent,
    EmployeesRatingListDialogComponent,
    TextFeedbacksComponent,
    YesNoFeedbacksGraphComponent,
    MultiSelectFeedbacksGraphComponent,
    EmployeeEvaluationsDialogComponent,
    PayrollRequestsComponent,
    ProposalApprovalStatusPipe,
    ProposalItemsApprovalStatusPipe,
    PayrollRequestRequestTypePipe,
    PayrollRequestReportComponent,
    PayrollRequestReportHeaderComponent,
    PayrollRequestEmpSectionComponent,
    PayrollRequestReportCurrencyPipe,
    SafeStylePipe,
  ],
  entryComponents: [
    EmployeesRatingListDialogComponent,
    EmployeeEvaluationsDialogComponent,
  ],
  exports: [
    ReviewStatusCardComponent,
    ReviewGroupStatusViewComponent,
    ReviewSummaryDialogComponent,
    ReviewGroupAnalyticsComponent,
    ReviewScoringGraphComponent,
    ReviewCompetencyGraphComponent,
    EmployeesRatingListDialogComponent,
    TextFeedbacksComponent,
    YesNoFeedbacksGraphComponent,
    MultiSelectFeedbacksGraphComponent,
    EmployeeEvaluationsDialogComponent,
  ],
  providers: [CurrencyPipe],
})
export class DsPerformanceManagerModule {}
