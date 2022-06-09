import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'ds-no-paycheck-modal',
  templateUrl: './no-paycheck-modal.component.html',
  styleUrls: ['./no-paycheck-modal.component.scss']
})
export class NoPaycheckModalComponent implements OnInit {

  constructor( public dialogRef: MatDialogRef<NoPaycheckModalComponent>) { }

  ngOnInit() {
  }

  close(): void {
    this.dialogRef.close();
  }

}
