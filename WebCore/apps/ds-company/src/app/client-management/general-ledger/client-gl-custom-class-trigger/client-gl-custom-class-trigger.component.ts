import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ClientGlCustomClassDialogComponent } from '../client-gl-custom-class-dialog/client-gl-custom-class-dialog.component';

@Component({
  selector: 'ds-client-gl-custom-class-trigger',
  templateUrl: './client-gl-custom-class-trigger.component.html',
  styleUrls: ['./client-gl-custom-class-trigger.component.scss']
})
export class ClientGlCustomClassTriggerComponent implements OnInit {

  constructor(private dialog: MatDialog) { }

  ngOnInit() {
  }

  openModal() {
    const dialogRef = this.dialog.open(ClientGlCustomClassDialogComponent, {
      width:"600px"
    });
  }

}
