import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { AdvanceEnrollmentReportDialogComponent } from '@ds/benefits/enrollments/advance-enrollment-report/advance-enrollment-report-dialog/advance-enrollment-report-dialog.component';
import { IAdvanceEnrollmentReportDialogData } from '../../shared/advance-enrollment-report.model'

@Injectable({
    providedIn: 'root'
})
export class AdvanceEnrollmentReportDialogService {

    constructor(private dialog: MatDialog) { }

    open(clientId: number) {
        let config = new MatDialogConfig<IAdvanceEnrollmentReportDialogData>();
        config.width = "500px";
        config.data = {
            clientId: clientId
        };

        return this.dialog.open<AdvanceEnrollmentReportDialogComponent, IAdvanceEnrollmentReportDialogData>(AdvanceEnrollmentReportDialogComponent, config);
    }
}
