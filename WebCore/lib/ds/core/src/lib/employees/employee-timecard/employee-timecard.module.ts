import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeeTimecardComponent } from './employee-timecard.component';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { DsAutocompleteModule } from '@ds/core/ui/ds-autocomplete';
import { DsCardModule } from '@ds/core/ui/ds-card/ds-card.module';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { EmployeeClockTimecardWidgetComponent } from './employee-clock-timecard-widget.component';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';

@NgModule({
  imports: [
    CommonModule,
    DsCoreFormsModule,
    FormsModule,
    DsCardModule,
    MaterialModule,
    ReactiveFormsModule,
    DsAutocompleteModule,
    LoadingMessageModule
  ],
  declarations: [
    EmployeeTimecardComponent,
    EmployeeClockTimecardWidgetComponent
  ],
  exports: [
    EmployeeTimecardComponent,
    EmployeeClockTimecardWidgetComponent
  ],
  entryComponents: [
  ]
})
export class EmployeeTimecardModule { }
