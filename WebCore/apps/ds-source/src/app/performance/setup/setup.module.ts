import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PerformanceSetupRoutingModule } from './setup-routing.module';
import { ReviewProfilesOutletComponent } from './review-profiles-outlet/review-profiles-outlet.component';
import { ReviewProfileFormOutletComponent } from './review-profile-form-outlet/review-profile-form-outlet.component';
import { ReviewPolicyOutletComponent } from './review-policy-outlet/review-policy-outlet.component';
import { ReviewPolicyFormOutletComponent } from './review-policy-form-outlet/review-policy-form-outlet.component';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { DsPerformanceReviewProfilesModule } from '@ds/performance/review-profiles/review-profiles.module';
import { DsPerformanceReviewPolicyModule } from '@ds/performance/review-policy/review-policy.module';

@NgModule({
  imports: [
    CommonModule,
    DsPerformanceReviewProfilesModule,
    DsPerformanceReviewPolicyModule,
    PerformanceSetupRoutingModule,
    DsCardModule,
    MaterialModule
  ],
  declarations: [
    ReviewProfilesOutletComponent, 
    ReviewProfileFormOutletComponent, 
    ReviewPolicyOutletComponent,
    ReviewPolicyFormOutletComponent
  ]
})
export class PerformanceSetupModule { }
