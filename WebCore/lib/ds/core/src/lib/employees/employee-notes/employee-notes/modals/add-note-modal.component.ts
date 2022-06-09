import { Component, Input, Output, EventEmitter, Inject, Optional } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EmployeeApiService } from '../../../shared/employee-api.service';
import { IEditEmployeeNote, INoteSource } from '../../../shared/employee-notes-api.model';
import { tap } from 'rxjs/operators';
import { Console } from 'console';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'add-note-modal',
  template: `
  <div *ngIf="true === isInDDL; else add">
  <button type="button" mat-menu-item (click)="openNoteModal()">Edit</button>
  </div>
  <ng-template #add>
  <button type="button" class="btn btn-primary text-nowrap inline-md-control" (click)="openNoteModal()"><i class="material-icons">add</i> Note</button>
  </ng-template>
  `,
})
export class AddNoteTriggerComponent {
  @Input() selectedNote: IEditEmployeeNote;
  @Input() isInDDL: boolean;
  @Output() savedNote = new EventEmitter();
  constructor(public dialog: MatDialog) {}

  description: String;
  noteSourceId: number;
  remarkId: number;

  openNoteModal(): void {
    this.dialog.open(AddNoteModalComponent, {
        width: '500px',
        data: this.selectedNote
    }).afterClosed().pipe(tap(x => this.savedNote.emit(x))).subscribe();
  }
}

@Component({
  selector: 'mat-dialog-demo',
  templateUrl: './add-note-modal.component.html',
  providers: [AddNoteTriggerComponent],
})
export class AddNoteModalComponent {
  form:FormGroup;
  formSubmitted:boolean = false;

  noteText: String;
  currNoteSourceId: number;
  employeeId: number;
  currNote: IEditEmployeeNote;
  sources: INoteSource[];
  employeeName: string;

  constructor(
      public dialogRef: MatDialogRef<AddNoteModalComponent>,
      private api: EmployeeApiService,
      private fb: FormBuilder,
      @Optional() @Inject(MAT_DIALOG_DATA) public data: IEditEmployeeNote
  ){}

  ngOnInit() {
    this.form = this.createForm();
    this.currNote = this.data;
    this.noteText = this.currNote.description;
    this.currNoteSourceId = this.currNote.noteSourceId;

    this.api.getNoteSources()
    .subscribe(data => {
        this.sources = data;
        this.patchForm();
    });

    this.api.getEmployeeName(this.currNote.employeeId)
    .subscribe(data => {
        this.employeeName = data.firstName + ' ' + data.lastName;
    });
  }

  close() {
      this.dialogRef.close();
  }

  private createForm(): FormGroup {
    return new FormGroup({
      noteSource: this.fb.control({value: ''}, Validators.required),
      noteText: this.fb.control({value: ''}, [Validators.required])
    });
  }

  private patchForm() {
    this.form.patchValue({
      noteSource: this.currNote.noteSourceId,
      noteText: this.currNote.description
    });
  }

  saveNote() {
    this.formSubmitted = true;
    if(this.form.invalid) return;

    const newDescriptionText = this.form.get('noteText').value;
    if (newDescriptionText && newDescriptionText != this.noteText) {
      const newNote = {
        remarkId: this.currNote.remarkId || null,
        employeeId: this.currNote.employeeId,
        securityLevel: 1,
        description: newDescriptionText,
        noteSourceId: this.form.get('noteSource').value,
        reviewId: this.currNote.reviewId || null,
	      attachmentIds: '',
	      addedBy: null
      };
      this.noteText = newDescriptionText;
      this.api.saveEmployeeNote(newNote)
      .pipe(tap(x => this.dialogRef.close(x)))
        .subscribe();
    }
  }
}
