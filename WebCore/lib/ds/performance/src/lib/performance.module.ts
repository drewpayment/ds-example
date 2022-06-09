import { NgModule } from '@angular/core';
import { CompetenciesModule } from '@ds/performance/competencies/competencies.module';
import { RatingsModule } from '@ds/performance/ratings/ratings.module';
import { GoalsModule } from '@ds/performance/goals/goals.module';
import { ReviewsModule } from '@ds/performance/reviews/reviews.module';
import { DsEvaluationsModule } from '@ds/performance/evaluations/evaluations.module';
import { DsPerformanceFeedbackModule } from '@ds/performance/feedback/feedback.module';
import { DsPerformanceReviewPolicyModule } from './review-policy/review-policy.module';
import { DsPerformanceReviewProfilesModule } from './review-profiles/review-profiles.module';
import { DsPerformanceManagerModule } from './performance-manager/performance-manager.module';
import { AttachmentsModule } from './attachments/attachments.module';

@NgModule({
  imports: [],
  exports: [
      CompetenciesModule,
      RatingsModule,
      GoalsModule,
      ReviewsModule,
      DsEvaluationsModule,
      DsPerformanceFeedbackModule,
      DsPerformanceReviewProfilesModule,
      DsPerformanceReviewPolicyModule,
      DsPerformanceManagerModule,
      AttachmentsModule
  ],
  declarations: []
})
export class PerformanceModule { }
