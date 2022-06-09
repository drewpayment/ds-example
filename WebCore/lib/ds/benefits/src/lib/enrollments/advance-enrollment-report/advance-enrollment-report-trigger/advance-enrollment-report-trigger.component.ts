import { Component, OnInit, Input } from '@angular/core';
import { AdvanceEnrollmentReportDialogService } from '../advance-enrollment-report-dialog/advance-enrollment-report-dialog.service';

@Component({
  selector: 'ds-advance-enrollment-report-trigger',
  templateUrl: './advance-enrollment-report-trigger.component.html',
  styleUrls: ['./advance-enrollment-report-trigger.component.scss']
})
export class AdvanceEnrollmentReportTriggerComponent implements OnInit {

    private _clientId: number;

    @Input()
    set clientId(value: number) {
        this._clientId = value;
    }

    constructor(private aerDialogSvc: AdvanceEnrollmentReportDialogService) { }

    ngOnInit() {
    }
    openDialog() {
        this.aerDialogSvc.open(this._clientId);
    }
}
