import { Component, OnInit, Inject } from '@angular/core';
import { IReportScheduleSequencyDialogData } from './report-schedule-sequency-dialog-data.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IReportScheduleSequencyDialogResult } from './report-schedule-sequency-dialog-result.model';
import { ReportScheduleService } from '@ajs/reportscheduling/reportschedule.service';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

interface IReportSeqInfo {
    scheduledReportId: number;
    sequenceNum: number;
    name: string;
}

@Component({
    selector: 'ds-report-schedule-sequence-dialog',
    templateUrl: './report-schedule-sequence-dialog.component.html',
    styleUrls: ['./report-schedule-sequence-dialog.component.scss']
})
export class ReportScheduleSequenceDialogComponent implements OnInit {

    reports: IReportSeqInfo[];

    constructor(
        private dialog: MatDialogRef<ReportScheduleSequenceDialogComponent, IReportScheduleSequencyDialogResult>,
        @Inject(MAT_DIALOG_DATA) private data: IReportScheduleSequencyDialogData,
        private reportSvc: ReportScheduleService
    ) { }

    ngOnInit() {
        this.reportSvc.getScheduledReport(this.data.emailId).then((data: IReportSeqInfo[]) => {
            this.reports = data.sort((a, b) => {
                return a.sequenceNum > b.sequenceNum ? 1 : -1;
            });
        });
    }

    reorderReports(event: CdkDragDrop<IReportSeqInfo[]>) {
        moveItemInArray(this.reports, event.previousIndex, event.currentIndex);
    }

    save() {
        const dto = [];

        for (let i = 0; i < this.reports.length; i++) {
            dto[i] = this.reports[i].scheduledReportId;
        }

        this.reportSvc.updateReportSequence(dto).then((data) => {
            const result: IReportScheduleSequencyDialogResult = {
                success: true
            };
            this.dialog.close(result);
        });
    }

    cancel() {
        this.dialog.close();
    }
}
