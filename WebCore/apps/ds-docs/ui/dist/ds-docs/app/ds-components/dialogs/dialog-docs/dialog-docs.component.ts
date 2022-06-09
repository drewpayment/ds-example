import { Component } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BasicDialogComponent } from '../basic-dialog/basic-dialog.component';
import { BorderedDialogComponent } from '../bordered-dialog/bordered-dialog.component';
import { NavDialogComponent } from '../nav-dialog/nav-dialog.component';

@Component({
  selector: 'ds-dialog-docs',
  templateUrl: './dialog-docs.component.html',
  styleUrls: ['./dialog-docs.component.scss']
})
export class DialogDocsComponent {

  toggleModal = false
  toggleBorderModal = false
  toggleTabModal = false
  TabModalExample = 1;
  TabBorderExample = 1;
  TabExample = 1;

  constructor(public dialog: MatDialog) { }

  openDialog(): void {
    const dialogRef = this.dialog.open(BasicDialogComponent, {
      width: '500px'
    });
  }

  openBorderDialog(): void {
    const dialogRef = this.dialog.open(BorderedDialogComponent, {
      width: '500px'
    })
  }

  openNavDialog(): void {
    const dialogRef = this.dialog.open(NavDialogComponent, {
      width: '500px'
    })
  }
}
