import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AjsUpgradesModule } from '@ds/core/ajs-upgrades/ajs-upgrades.module';
import {
  ReviewEditDialogComponent,
  SupervisorSelectComponent,
  ReviewEditMeetingComponent,
  EmployeeNameComponent,
  EmployeeDateRangeComponent,
} from '@ds/performance/reviews/review-edit-dialog/review-edit-dialog.component';
import { ReviewListComponent } from '@ds/performance/reviews/review-list/review-list.component';
import { DateTimeModule } from '@ds/core/ui/datetime/datetime.module';
import { CoreModule } from '@ds/core/core.module';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsAutocompleteModule } from '@ds/core/ui/ds-autocomplete';
import { ReviewMeetingDialogComponent } from '@ds/performance/reviews/review-meeting-dialog/review-meeting-dialog.component';
import { SharedModule } from '@ds/performance/shared/shared.module';
import { EmployeeNotesModule } from '@ds/core/employees/employee-notes/employee-notes.module';
import { AvatarModule } from '@ds/core/ui/avatar/avatar.module';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';

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
    DsAutocompleteModule,
    SharedModule,
    CoreModule,
    EmployeeNotesModule,
    AvatarModule,
    LoadingMessageModule,
  ],
  declarations: [
    ReviewEditDialogComponent,
    ReviewListComponent,
    ReviewMeetingDialogComponent,
    SupervisorSelectComponent,
    ReviewEditMeetingComponent,
    EmployeeNameComponent,
    EmployeeDateRangeComponent,
  ],
  exports: [
    ReviewEditDialogComponent,
    ReviewListComponent,
    ReviewMeetingDialogComponent,
  ],
  entryComponents: [
    SupervisorSelectComponent,
    ReviewEditMeetingComponent,
    EmployeeNameComponent,
    EmployeeDateRangeComponent,
  ],
})
export class ReviewsModule {}
