import { Component, OnInit, Inject, AfterViewInit } from '@angular/core';
import { UserInfo } from '@ds/core/shared';
import { DsStyleLoaderService, IStyleAsset } from '@ajs/ui/ds-styles/ds-styles.service';
import { FormBuilder, FormGroup, Validators, FormArray, FormControl, AbstractControl } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { IDsConfirmOptions } from '@ajs/ui/confirm/ds-confirm.interface';
import { EmployeeProfileService } from '../../shared/employee-profile-api.service';
import { JobProfileService } from '../../shared/job-profile-api.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { IJobProfileBasicInfo } from '../../shared/job-profile-basic-info.model';
import { EmployeeCommonService } from '../../../common/shared/employee-common-api.service';
import { DsCustomFilterCallbackPipe } from '@ds/core/shared/ds-custom-filter-callback.pipe';
import { Observable, Subject } from "rxjs";
import * as _ from 'lodash';
import * as moment from 'moment';
import { AccountService } from '@ds/core/account.service';

interface DialogData {
    user: UserInfo,
    jobProfileData: IJobProfileBasicInfo;
}
@Component({
  selector: 'ds-job-profile-modal',
  templateUrl: './job-profile-modal.component.html',
  styleUrls: ['./job-profile-modal.component.scss']
})
export class JobProfileModalComponent implements OnInit, AfterViewInit {
    mainStyle: IStyleAsset;
    user: UserInfo;
    jobProfileData: IJobProfileBasicInfo;
    f: FormGroup;
    formSubmitted: boolean;
    pageTitle: string;
    currentDate: string = new Date().toLocaleString();

    constructor(
        public dialogRef: MatDialogRef<JobProfileModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
        private fb: FormBuilder,
        private styles: DsStyleLoaderService,
        private service: EmployeeProfileService,
        private accountService: AccountService,
        private msgSvc: DsMsgService,
        private confirmService: DsConfirmService,
        private api: JobProfileService
    ) {
    }

    ngOnInit() {
        this.jobProfileData = _.cloneDeep(this.data.jobProfileData);
        this.pageTitle = "Job Profile";
        this.f = this.fb.group({
        });

        this.accountService.getUserInfo().subscribe(user => {
            this.user = user;
        });
    }

    printGrid() {
        var printContent = document.getElementById('pnlToPrint').innerHTML;
       var printWindow = window.open('Job Profile Details', 'Print' + (new Date()).getTime(), 'left=20,top=20,width=0,height=0');
       printWindow.document.write('<!DOCTYPE html><html><head>');
       for (var i = 0; i < document.styleSheets.length; i++) {
           if (document.styleSheets[i].href) {
               printWindow.document.write('<link href= "' + document.styleSheets[i].href + '" rel="stylesheet" type="text/css" >');
           }
       }
       printWindow.document.write('</head><body style="background-color:#fff;">');
       printWindow.document.write(printContent);
       printWindow.document.write('</body></html>');
       printWindow.document.close();
       printWindow.focus();
       setTimeout(() => {
           printWindow.print();
           printWindow.close();
       }, 500);
    }

    convertHtmlPageToPdf() {
        var printContent = document.getElementById('pnlToPrint');
        var dto = { fileName: this.jobProfileData.description, htmlContent: '<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>' + printContent.innerHTML };
        this.api.convertHtmlPageToPdf(dto);

    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    ngAfterViewInit() {

    }
}
