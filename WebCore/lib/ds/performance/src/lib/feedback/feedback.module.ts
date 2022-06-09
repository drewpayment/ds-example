import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { DateTimeModule } from '@ds/core/ui/datetime/datetime.module';
import { AjsUpgradesModule } from '@ds/core/ajs-upgrades/ajs-upgrades.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import {
  FeedbackSetupListComponent,
  ShouldArchiveFeedbackPipe,
} from '@ds/performance/feedback/feedback-setup-list/feedback-setup-list.component';
import { FeedbackSetupDialogComponent } from '@ds/performance/feedback/feedback-setup-dialog/feedback-setup-dialog.component';
import { FeedbackDirective } from '@ds/performance/feedback/helper/feedback.directive';
import {
  FormatCommentPipe,
  NoCommentsEnteredPipe,
} from '@ds/performance/feedback/shared/format-comment.pipe';
import {
  FeedbackYesNoComponent,
  TextComponent,
  DateComponent,
  ListComponent,
  MultiSelectComponent,
} from '@ds/performance/feedback/helper/dynamic-feedback-components';
import { DefaultFeedbacksDialogComponent } from '@ds/performance/feedback/default-feedbacks-dialog/default-feedbacks-dialog.component';

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
  declarations: [
    FeedbackSetupListComponent,
    FeedbackSetupDialogComponent,
    FeedbackDirective,
    FeedbackYesNoComponent,
    ListComponent,
    MultiSelectComponent,
    DateComponent,
    TextComponent,
    FormatCommentPipe,
    NoCommentsEnteredPipe,
    DefaultFeedbacksDialogComponent,
    ShouldArchiveFeedbackPipe,
  ],
  exports: [
    FeedbackDirective,
    FeedbackSetupListComponent,
    ListComponent,
    MultiSelectComponent,
    DateComponent,
    TextComponent,
    FormatCommentPipe,
    NoCommentsEnteredPipe,
    DefaultFeedbacksDialogComponent,
  ],
  providers: [FormatCommentPipe, NoCommentsEnteredPipe],
  entryComponents: [
    ListComponent,
    MultiSelectComponent,
    DateComponent,
    TextComponent,
    FeedbackYesNoComponent,
  ],
})
export class DsPerformanceFeedbackModule {}
