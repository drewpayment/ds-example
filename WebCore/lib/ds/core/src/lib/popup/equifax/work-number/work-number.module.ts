import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkNumberModalComponent } from './work-number-modal.component';
import { WorkNumberModalTriggerComponent } from './work-number-trigger.component';
import { MaterialModule } from '@ds/core/ui/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    WorkNumberModalComponent,
    WorkNumberModalTriggerComponent
  ],
  entryComponents: [
    WorkNumberModalComponent,
    WorkNumberModalTriggerComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule
  ],
  exports: [
    WorkNumberModalComponent,
    WorkNumberModalTriggerComponent
  ]
})
export class WorkNumberModule { }
