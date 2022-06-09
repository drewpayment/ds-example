import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeeNotesComponent } from './employee-notes/employee-notes.component';
import { AddNoteModalComponent, AddNoteTriggerComponent,  } from './employee-notes/modals/add-note-modal.component';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { ShareNoteModalComponent, ShareNoteTriggerComponent} from './employee-notes/modals/share-note-modal.component';
import { DsAutocompleteModule } from '@ds/core/ui/ds-autocomplete';
import { EmployeeAttachmentsComponent } from '../employee-attachments/employee-attachments.component';
import { DsCardModule } from '@ds/core/ui/ds-card/ds-card.module';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { AssignTagsModalComponent,AssignTagsTriggerComponent } from './employee-notes/modals/assign-tags-modal.component';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';

@NgModule({
  imports: [
    CommonModule,
    DsCoreFormsModule,
    FormsModule,
    MatMenuModule,
    MatIconModule,
    MatTooltipModule,
    DsCardModule,
    MaterialModule,
    ReactiveFormsModule,
    DsAutocompleteModule,
  ],
  declarations: [
    EmployeeNotesComponent,
    AddNoteModalComponent,
    AddNoteTriggerComponent,
    AssignTagsModalComponent,
    AssignTagsTriggerComponent,
    EmployeeAttachmentsComponent,
    ShareNoteModalComponent,
    ShareNoteTriggerComponent,
  ],
  exports: [
    EmployeeNotesComponent,
    AddNoteModalComponent,
    AddNoteTriggerComponent,
    AssignTagsModalComponent,
    AssignTagsTriggerComponent,
    EmployeeAttachmentsComponent,
    ShareNoteModalComponent,
    ShareNoteTriggerComponent
  ],
  entryComponents: [
    AddNoteModalComponent,
    AssignTagsModalComponent,
    EmployeeAttachmentsComponent,
    ShareNoteModalComponent
  ]
})
export class EmployeeNotesModule { }
