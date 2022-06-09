import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule, UrlSerializer } from '@angular/router';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DsExpansionModule } from '@ds/core/ui/ds-expansion';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { PerformanceComponent } from './performance.component';
import { MainMenuComponent } from './main-menu/main-menu.component';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { GoalsComponent } from './goals/goals.component';
import { FeatureGuard } from './feature.guard';
import { EmployeeReviewsComponent } from './reviews/reviews.component';
import { EmployeeEvaluationViewComponent } from './evaluations/evaluations.component';
import { CompanyGoalsComponent } from './company-goals/company-goals.component';
import { EvaluationDetailComponent } from '@ds/performance/evaluations/evaluation-detail/evaluation-detail.component';
import { EvaluationSummaryComponent } from '@ds/performance/evaluations/evaluation-summary/evaluation-summary.component';
import { PerformanceModule } from '@ds/performance/performance.module';
import { CompetenciesModule } from '@ds/performance/competencies/competencies.module';
import { GoalsModule } from '@ds/performance/goals/goals.module';
import { HeaderComponent } from '@ds/core/ui/menu-wrapper-header/header/header.component';
import { LowerCaseUrlSerializer } from '@ds/core/utilities';

export const PerformanceRoutes: Routes = [
  {
    path: 'performance',
    children: [
      {
        path: '',
        component: PerformanceComponent,
        canActivate: [FeatureGuard],
      },
      { path: '', component: SidebarComponent, outlet: 'sidebar' },
      { path: '', component: HeaderComponent, outlet: 'header' },
      { path: 'goals', component: GoalsComponent, canActivate: [FeatureGuard] },
      { path: 'reviews', component: EmployeeReviewsComponent },
      {
        path: 'evaluations/:evaluationId',
        component: EmployeeEvaluationViewComponent,
        children: [
          { path: '', redirectTo: 'detail', pathMatch: 'full' },
          { path: 'detail', component: EvaluationDetailComponent },
          { path: 'summary', component: EvaluationSummaryComponent },
        ],
      },
      {
        path: 'company-goals',
        component: CompanyGoalsComponent,
        canActivate: [FeatureGuard],
      },
    ],
  },
];

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    PerformanceModule,
    DsExpansionModule,
    DsCardModule,
    CompetenciesModule,
    GoalsModule,

    RouterModule.forChild(PerformanceRoutes),
  ],
  declarations: [
    PerformanceComponent,
    GoalsComponent,
    MainMenuComponent,
    EmployeeReviewsComponent,
    EmployeeEvaluationViewComponent,
    CompanyGoalsComponent,
  ],
  entryComponents: [],
  providers: [
    {
      provide: UrlSerializer,
      useClass: LowerCaseUrlSerializer,
    },
  ],
})
export class PerformanceAppModule {}
