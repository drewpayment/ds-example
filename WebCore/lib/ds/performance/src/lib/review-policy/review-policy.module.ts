import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { MaterialModule } from 'lib/ds/core/src/public_api';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AjsUpgradesModule } from '@ds/core/ajs-upgrades/ajs-upgrades.module';
import { CoreModule } from '@ds/core/core.module';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import {
  IsRecurringToStringPipe,
  ReviewPolicySetupFormComponent,
  ReviewTypeToStringPipe,
  ToggleTemplatesPipe,
  filterTemplatesNotInListPipe,
} from './review-policy-setup-form/review-policy-setup-form.component';
import { ReviewTemplateDialogComponent } from './review-template-dialog/review-template-dialog.component';
import { DsAutocompleteModule } from '@ds/core/ui/ds-autocomplete';
import { ReferenceDateComponent } from './reference-date/reference-date.component';
import { SharedModule } from '../shared/shared.module';
import {
  ReviewTemplateListComponent,
  FilterByTypePipe,
} from './review-policy-setup-form/review-template-list/review-template-list.component';
import {
  IconWidgetComponent,
  RefDateToMatIconPipe,
  RefDateToColorPipe,
} from './review-policy-setup-form/icon-widget/icon-widget.component';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { DateTimeModule } from '@ds/core/ui/datetime/datetime.module';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';

export const options: Partial<IConfig> | (() => Partial<IConfig>) = {};

@NgModule({
  imports: [
    CommonModule,
    DsCardModule,
    DsAutocompleteModule,
    MaterialModule,
    AjsUpgradesModule,
    FormsModule,
    ReactiveFormsModule,
    DateTimeModule,
    DsCoreFormsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    CoreModule,
    SharedModule,
    NgxMaskModule.forRoot(options),
  ],
  declarations: [
    ReviewPolicySetupFormComponent,
    ReviewTemplateDialogComponent,
    ReferenceDateComponent,
    ReviewTypeToStringPipe,
    IsRecurringToStringPipe,
    ReviewTemplateListComponent,
    IconWidgetComponent,
    RefDateToMatIconPipe,
    FilterByTypePipe,
    RefDateToColorPipe,
    ToggleTemplatesPipe,
    filterTemplatesNotInListPipe,
  ],
  exports: [ReviewPolicySetupFormComponent, ReviewTemplateDialogComponent],
})
export class DsPerformanceReviewPolicyModule {}
