import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'ds-communication-dialog',
  templateUrl: './communication-dialog.component.html',
  styleUrls: ['./communication-dialog.component.scss']
})
export class CommunicationDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<CommunicationDialogComponent>,
  ) { }

  close(): void {
    this.dialogRef.close();
  }

}
