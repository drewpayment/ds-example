import { HttpErrorResponse } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';
import { IClientW2ProcessingNotes } from 'apps/ds-company/src/app/models/w2/client-w2-processing-notes';
import { W2Service } from 'apps/ds-company/src/app/services/w2.service';

@Component({
  selector: 'ds-notes-dialog',
  templateUrl: './notes-dialog.component.html',
  styleUrls: ['./notes-dialog.component.css']
})
export class NotesDialogComponent implements OnInit {
  isLoading: boolean = true;
  form: FormGroup;
  isSubmitted: boolean = false;
  clientName: string;

  get AnnualNotes() {
    return this.form.get("annualNotes") as FormControl;
  }

  get MiscNotes() {
    return this.form.get("miscNotes") as FormControl;
  }

  constructor(private fb: FormBuilder, private w2Service: W2Service, private dialogRef: MatDialogRef<NotesDialogComponent>, private ngxMsg: NgxMessageService, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.initForm();
    this.isLoading = false;
    this.clientName = this.data.clientInfo.clientName;
    this.AnnualNotes.setValue(this.data.clientInfo.annualNotes);
    this.MiscNotes.setValue(this.data.clientInfo.miscNotes);
  }

  initForm() {
    this.form = this.fb.group({
      annualNotes: this.fb.control(""),
      miscNotes: this.fb.control("")
    });
  }

  save() {
    this.isSubmitted = true;
    const w2NotesInfo = <IClientW2ProcessingNotes>({
      year: this.data.year,
      clientId: this.data.clientInfo.clientId,
      annualNotes: this.AnnualNotes.value,
      miscNotes: this.MiscNotes.value
    });

    this.w2Service.saveW2Notes(w2NotesInfo).subscribe(() => {
      this.ngxMsg.setSuccessMessage("Notes Saved Successfully!")
      this.dialogRef.close(w2NotesInfo);
      this.isSubmitted = false;
    }, (error: HttpErrorResponse) => {
      this.ngxMsg.setErrorResponse(error);
      this.cancel();
    });
  }

  cancel() {
    this.dialogRef.close(null);
  }

}
