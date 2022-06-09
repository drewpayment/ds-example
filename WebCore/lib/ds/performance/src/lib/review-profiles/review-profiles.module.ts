import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AjsUpgradesModule } from '@ds/core/ajs-upgrades/ajs-upgrades.module';
import { DateTimeModule } from '@ds/core/ui/datetime/datetime.module';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { ReviewProfilesListComponent } from '@ds/performance/review-profiles/review-profiles-list/review-profiles-list.component';
import { ReviewProfileSetupFormComponent } from '@ds/performance/review-profiles/review-profile-setup-form/review-profile-setup-form.component';

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
  ],
  declarations: [ReviewProfilesListComponent, ReviewProfileSetupFormComponent],
  exports: [ReviewProfilesListComponent, ReviewProfileSetupFormComponent],
})
export class DsPerformanceReviewProfilesModule {}
