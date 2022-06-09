import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownModule } from 'ngx-markdown';
import { DocsMaterialModule } from '@ds/docs/material.module';

import { CalendarDocsComponent } from './calendar-docs/calendar-docs.component';
import { CalendarComponent } from './calendar/calendar.component';
import { CalendarVerticalComponent } from './calendar-vertical/calendar-vertical.component';
import { CalendarChipComponent } from './calendar-chip/calendar-chip.component';
import { CalendarChipIconComponent } from './calendar-chip-icon/calendar-chip-icon.component';

@NgModule({
  declarations: [
    CalendarDocsComponent,
    CalendarComponent,
    CalendarVerticalComponent,
    CalendarChipComponent,
    CalendarChipIconComponent
  ],
  imports: [
    CommonModule,
    MarkdownModule.forChild(),
    DocsMaterialModule
  ]
})
export class CalendarModule { }
