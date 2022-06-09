import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeePerformanceNavComponent } from './employee-performance-nav/employee-performance-nav.component';
import { EmployeePerformanceRoutingModule } from './employees-routing.module';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { EmployeeReviewsOutletComponent } from './employee-reviews-outlet/employee-reviews-outlet.component';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsEvaluationsModule } from '@ds/performance/evaluations/evaluations.module';
import { ReviewsModule } from '@ds/performance/reviews/reviews.module';
import { ReviewEditDialogComponent } from '@ds/performance/reviews/review-edit-dialog/review-edit-dialog.component';
import { ReviewMeetingDialogComponent } from '@ds/performance/reviews/review-meeting-dialog/review-meeting-dialog.component';
import { Router } from '@angular/router';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    DsCardModule,
    EmployeePerformanceRoutingModule,
    DsEvaluationsModule,
    ReviewsModule
  ],
  declarations: [
    EmployeePerformanceNavComponent,
    EmployeeReviewsOutletComponent
  ],
  entryComponents: [
    EmployeePerformanceNavComponent,
    ReviewEditDialogComponent,
    ReviewMeetingDialogComponent
  ]
})
export class EmployeePerformanceAppModule { 
  // constructor(
  //   private readonly router: Router,
  // ) {
  //   router.events.subscribe(console.log)
  // }
}
