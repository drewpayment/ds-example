import { Component, HostListener, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { YEARS } from '@ds/docs/ds-components/shared/years';

@Component({
  selector: 'app-table-edit-dialog',
  templateUrl: './table-edit-dialog.component.html',
  styleUrls: ['./table-edit-dialog.component.scss']
})
export class TableEditDialogComponent implements OnInit {
  form: FormGroup = this.createForm();
  years = YEARS;
  constructor(
    public dialogRef: MatDialogRef<TableEditDialogComponent>,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    let year = this.years.find(yr => yr.year === this.data.year)

    this.form.patchValue({
      character: this.data.firstName + ' ' + this.data.lastName,
      movie: this.data.movie,      
      year: year.id
    })
  }

  createForm() {
    return this.fb.group({
      character: null,
      movie: null,
      year: null
    })
  }
  close(): void {
    this.dialogRef.close();
  }

  @HostListener("keydown.esc")
  public onEsc() {
      this.dialogRef.close(false);
  }
}
