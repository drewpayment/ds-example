import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@ds/core/ui/material';
import { NPSSurveyDialogComponent } from './nps-survey/nps-survey-dialog.component';
import { NPSSurveyTriggerComponent } from './nps-survey/nps-survey-trigger.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DeviceDetectorModule } from 'ngx-device-detector';

@NgModule({
  declarations: [NPSSurveyDialogComponent, NPSSurveyTriggerComponent],
  imports: [
    CommonModule,
    MaterialModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    DeviceDetectorModule.forRoot(),
  ],
  exports: [NPSSurveyDialogComponent, NPSSurveyTriggerComponent],
  entryComponents: [NPSSurveyDialogComponent],
})
export class NpsModule {}
