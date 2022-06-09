import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { Utilities } from 'apps/ds-company/src/app/shared/utils/utilities';
import { JobProfileApiService } from "apps/ds-company/src/app/services/job-profile-api.service";
import { IJobProfileBasicInfoData } from 'apps/ds-company/src/app/models/job-profile.model';


@Component({
  selector: 'ds-job-profile-dialog',
  templateUrl: './job-profile-dialog.component.html',
  styleUrls: ['./job-profile-dialog.component.css']
})
export class JobProfileDialogComponent implements OnInit {
    currentDate: string = new Date().toLocaleString();
    jobProfileData: any;
    jobProfileId:number;
    clientId:number;

    constructor(
		private ref:MatDialogRef<JobProfileDialogComponent>,
        private api: JobProfileApiService, 
        private dialog: MatDialog,
		@Inject(MAT_DIALOG_DATA) public data:any,
    ) {
        this.clientId = data.clientId;
        this.jobProfileId = data.jobProfileId;
    }

    ngOnInit(): void {
        this.api.getJobProfileDetails(this.jobProfileId, this.clientId)
        .subscribe((result: any) => {
            let basicInfo:IJobProfileBasicInfoData = result.data.jobProfileDto;
            basicInfo.jobProfileSkills = result.data.jobProfileDto.classifications.jobSkills||[];
            basicInfo.jobProfileResponsibilities = result.data.jobProfileDto.classifications.jobResponsibilities||[];

            this.jobProfileData = basicInfo;
            this.jobProfileData.jobProfileSkills.sort((x, y) => x.description.localeCompare(y.description) );
            this.jobProfileData.jobProfileResponsibilities.sort((x, y) => x.description.localeCompare(y.description) );
        });
    }

    printGrid() {
        var printContent = document.getElementById('pnlToPrint');
        var printWindow = window.open("", "Print Panel", 'left=50000,top=50000,width=0,height=0');
        printWindow.document.write(printContent.innerHTML);
        printWindow.document.close();
        printWindow.focus();
        printWindow.print();
        printWindow.close();
    }

    convertHtmlPageToPdf() {
        var printContent = document.getElementById('pnlToPrint');
        var dto = { fileName: this.jobProfileData.description, htmlContent: '<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>' + printContent.innerHTML };
        this.api.convertHtmlPageToPdf(dto).subscribe();
    }


    public cancel() {
        this.ref.close(null);
    }
}
