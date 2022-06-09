import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'ds-nav-dialog',
  templateUrl: './nav-dialog.component.html',
  styleUrls: ['./nav-dialog.component.scss']
})
export class NavDialogComponent implements OnInit {

  activeTab = 1;

  constructor(
    public dialogRef: MatDialogRef<NavDialogComponent>
  ) { }

  ngOnInit() {
    
  }
  close(): void {
    this.dialogRef.close();
  }
 
}
