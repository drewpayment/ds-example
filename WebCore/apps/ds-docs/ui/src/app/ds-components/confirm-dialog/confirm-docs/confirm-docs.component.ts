import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CommunicationDialogComponent } from '../communication-dialog/communication-dialog.component';

@Component({
  selector: 'ds-confirm-docs',
  templateUrl: './confirm-docs.component.html',
  styleUrls: ['./confirm-docs.component.scss']
})
export class ConfirmDocsComponent {

  toggleConfirm: false;
  toggleConfirmAJS: false;
  toggleCommunication: false;
  ConfirmExample = 1;
  ConfirmExampleAJS = 1;

  constructor(
    public dialog: MatDialog
  ) { }

}
