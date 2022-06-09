import { Component, OnInit, Inject} from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Moment } from 'moment';
import * as moment from 'moment/moment';
import * as _ from "lodash";
import { findIndex } from 'lodash';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { AdvanceEnrollmentReportService } from '../../shared/advance-enrollment-report.service';
import {
    IAdvanceEnrollmentReportDialogData,
    IEnrollmentTypeData,
    IAdvanceEnrollmentReportConfigData,
    IEmployeeData,
    IPlanData,
    IProviderData
} from '../../shared/advance-enrollment-report.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'ds-advance-enrollment-report-dialog',
  templateUrl: './advance-enrollment-report-dialog.component.html',
  styleUrls: ['./advance-enrollment-report-dialog.component.scss']
})
export class AdvanceEnrollmentReportDialogComponent implements OnInit {

    enrollmentTypes: IEnrollmentTypeData[] = [
        { enrollmentTypeId: 1, description: "Open Enrollments" },
        { enrollmentTypeId: 2, description: "Life Events" }
    ];
    config: IAdvanceEnrollmentReportConfigData;
    employeeList: IEmployeeData[];
    planList: IPlanData[];
    planProviderList: IProviderData[];

    constructor(
        private msgSvc: DsMsgService,
        private dialogRef: MatDialogRef<AdvanceEnrollmentReportDialogComponent, IAdvanceEnrollmentReportDialogData>,
        private aerService: AdvanceEnrollmentReportService,
        @Inject(MAT_DIALOG_DATA)
        private dialogData: IAdvanceEnrollmentReportDialogData) { }

    ngOnInit() {
        this.config = {
            enrollmentTypeId: null,
            employeeId: null,
            planId: null,
            providerId: null,
            startDate: null,
            endDate: null,
            clientId: this.dialogData.clientId
        };

        this.aerService.getEmployeeList(this.dialogData.clientId).subscribe(s => {
            this.employeeList = _.sortBy(s, ["firstName", "lastName"]);
        });
        this.aerService.getPlanList(this.dialogData.clientId).subscribe(s => {
            this.planList = s;
        });
        this.aerService.getPlanProviderList(this.dialogData.clientId).subscribe(s => {
            this.planProviderList = s;
        });
    }

    generateReport() {
        this.aerService.generateReport(this.config);
    }

    cancel() {
        this.dialogRef.close();
    }


}
