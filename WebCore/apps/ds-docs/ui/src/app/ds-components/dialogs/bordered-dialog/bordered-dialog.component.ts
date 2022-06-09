import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'ds-bordered-dialog',
  templateUrl: './bordered-dialog.component.html',
  styleUrls: ['./bordered-dialog.component.scss']
})
export class BorderedDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<BorderedDialogComponent>,
  ) { }

  ngOnInit() {
  }

  close(): void {
    this.dialogRef.close();
  }
  
}
