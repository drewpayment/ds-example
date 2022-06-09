import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@ds/core/ui/material';
import { MomentDateModule } from '@angular/material-moment-adapter';
import { IConfig, NgxMaskModule } from 'ngx-mask';
import { ReactiveFormsModule } from '@angular/forms';
import { DatePickerComponent } from './date-picker/date-picker.component';
import { DatePickerDocsComponent } from './date-picker-docs/date-picker-docs.component';
import { MarkdownModule } from 'ngx-markdown';

export const options: Partial<IConfig> | (() => Partial<IConfig>) = {};

@NgModule({
  declarations: [DatePickerComponent, DatePickerDocsComponent],
  imports: [
    MaterialModule,
    CommonModule,
    MomentDateModule,
    NgxMaskModule.forRoot(options),
    ReactiveFormsModule,
    MarkdownModule.forChild(),
  ],
  entryComponents: [
    DatePickerComponent
  ]
})
export class DatePickerModule { }
