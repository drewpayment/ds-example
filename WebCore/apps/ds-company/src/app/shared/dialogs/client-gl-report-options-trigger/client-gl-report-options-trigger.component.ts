import { Component, OnInit } from '@angular/core';
import { ClientGlReportOptionsDialogComponent } from '../client-gl-report-options-dialog/client-gl-report-options-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'ds-client-gl-report-options-trigger',
  templateUrl: './client-gl-report-options-trigger.component.html',
  styleUrls: ['./client-gl-report-options-trigger.component.scss']
})
export class ClientGlReportOptionsTriggerComponent implements OnInit {

  constructor(private dialog: MatDialog) { }

  ngOnInit() {
  }

  openReportingOptions() {
    const dialogRef = this.dialog.open(ClientGlReportOptionsDialogComponent, {
      width:"600px"
    });
  }
}
