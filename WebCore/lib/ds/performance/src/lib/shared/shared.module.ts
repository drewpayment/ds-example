import { NgModule } from '@angular/core';
import { ReopenEvaluationComponent } from '@ds/performance/shared/buttons/reopen-eval/reopen-evaluation.component';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { SendCompleteEvalReminderComponent } from '@ds/performance/shared/buttons/send-reminder/send-complete-eval-reminder.component';
import { ToFriendlyTimeDiffPipe } from '@ds/performance/shared/to-friendly-time-diff.pipe';
import { CloseReviewComponent } from '@ds/performance/shared/buttons/close-review/close-review.component';
import { CalendarYearFormComponent, ContactInputDirective } from './forms/calendar-year-form/calendar-year-form.component';
import { CoreModule } from '@ds/core/core.module';
import { DateTimeModule } from '@ds/core/ui/datetime/datetime.module';
import { ReactiveFormsModule } from '@angular/forms';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { MeritIncreaseComponent } from './forms/merit-increase/merit-increase.component';
import { DateRangeComponent, DateRangeOnPushComponent } from './forms/date-range/date-range.component';

/**
 * Contains the resources that are used in multiple multiple modules of performance reviews.
 *
 */
@NgModule({
  declarations: [
    ReopenEvaluationComponent,
    SendCompleteEvalReminderComponent,
    ToFriendlyTimeDiffPipe,
    CloseReviewComponent,
    CalendarYearFormComponent,
    ContactInputDirective,
    MeritIncreaseComponent,
    DateRangeComponent,
    DateRangeOnPushComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    DateTimeModule,
    ReactiveFormsModule,
    DsCoreFormsModule,
    CoreModule
  ],
  exports: [
    ReopenEvaluationComponent,
    SendCompleteEvalReminderComponent,
    ToFriendlyTimeDiffPipe,
    CloseReviewComponent,
    CalendarYearFormComponent,
    MeritIncreaseComponent,
    DateRangeComponent,
    DateRangeOnPushComponent
  ],
  entryComponents: [
    MeritIncreaseComponent,
    DateRangeComponent,
    DateRangeOnPushComponent
  ]
})
export class SharedModule { }
