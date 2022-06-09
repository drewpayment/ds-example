import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'ds-basic-dialog',
  templateUrl: './basic-dialog.component.html',
  styleUrls: ['./basic-dialog.component.scss']
})
export class BasicDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<BasicDialogComponent>,
  ) { }

  close(): void {
    this.dialogRef.close();
  }

}
