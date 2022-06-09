import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { IReportScheduleSequencyDialogData } from './report-schedule-sequency-dialog-data.model';
import { ReportScheduleSequenceDialogComponent } from './report-schedule-sequence-dialog.component';
import { IReportScheduleSequencyDialogResult } from './report-schedule-sequency-dialog-result.model';

@Injectable({
    providedIn: 'root'
})
export class ReportScheduleSequenceDialogService {

    constructor(private dialog: MatDialog) { }

    open(emailId: number) {
        let config = new MatDialogConfig<IReportScheduleSequencyDialogData>();
        config.data = {
            emailId: emailId
        };

        config.width = "400px";

        return this.dialog.open<ReportScheduleSequenceDialogComponent, IReportScheduleSequencyDialogData, IReportScheduleSequencyDialogResult>(ReportScheduleSequenceDialogComponent, config);
    }
}
