import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { FormControl, FormGroup, Validators, FormBuilder, AsyncValidatorFn, ValidatorFn, ValidationErrors } from '@angular/forms';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { tap } from 'rxjs/operators';
import { Utilities } from 'apps/ds-company/src/app/shared/utils/utilities';
import {DashboardService} from "apps/ds-company/src/app/services/dashboard.service";
import { IJobProfileDefaultData } from "apps/ds-company/src/app/models/job-profile.model";
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
  selector: 'ds-job-title-confirm-dialog',
  templateUrl: './job-title-confirm-dialog.component.html',
  styleUrls: ['./job-title-confirm-dialog.component.scss']
})
export class JobTitleConfirmDialogComponent implements OnInit {
  form: FormGroup;
  formSubmitted: boolean;
  isLoading: boolean = true;
  userAccount: UserInfo;
  clientId: number;
  jobProfileId: number;
  allData: any;
  jobProfileData: any;
  profileData:Array<IJobProfileDefaultData>;
  lblDesc: string = '';


  constructor(
    private ref:MatDialogRef<JobTitleConfirmDialogComponent>,
    private dashboardApi: DashboardService,
    private msg: NgxMessageService,
    private dialog: MatDialog,
    private confirmDialog: MatDialog,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data:any,
  ) { 
    this.allData = data;
    this.jobProfileId = data.jobProfileId;
    this.clientId = data.clientId;
  }

  ngOnInit(): void {
    this.dashboardApi.getJobProfileData(this.jobProfileId, this.clientId).pipe(
      tap(jobProfileData => {
        this.jobProfileData = jobProfileData;
        this.profileData = [];
        var lblDesc = '';
        this.isLoading = false;

        if (jobProfileData.data && jobProfileData.data.jobProfileDto && jobProfileData.data.jobProfileDto.classifications) {
          var classifications = jobProfileData.data.jobProfileDto.classifications;
          if (classifications.clientCostCenterId) {
            lblDesc = jobProfileData.data.clientCostCenterList.find( x => x.clientCostCenterId == classifications.clientCostCenterId).description;
            this.profileData.push({ id: 1, label: "COST CENTER", description: lblDesc });
          }

          if (classifications.clientDivisionId) {
            lblDesc = jobProfileData.data.clientDivisionList.find( x => x.clientDivisionId == classifications.clientDivisionId).name;
              this.profileData.push({ id: 2, label: "DIVISION", description: lblDesc });
          }

          if (classifications.clientDepartmentId) {
            lblDesc = classifications.departments.find( x => x.clientDepartmentId == classifications.clientDepartmentId).name;
            this.profileData.push({ id: 3, label: "DEPARTMENT", description: lblDesc });
          }

          if (classifications.clientGroupId) {
            lblDesc = jobProfileData.data.clientGroupList.find( x => x.clientGroupId == classifications.clientGroupId).description;
            this.profileData.push({ id: 4, label: "GROUP", description: lblDesc });
          }

          if (classifications.clientWorkersCompId) {
            lblDesc = jobProfileData.data.clientWorkersCompList.find( x => x.clientWorkersCompId == classifications.clientWorkersCompId).description;
            this.profileData.push({ id: 5, label: "CLIENT WORKERS COMP", description: lblDesc });
          }

          if (classifications.employeeStatusId) {
            lblDesc = jobProfileData.data.employeeStatusList.find( x => x.employeeStatusId == classifications.employeeStatusId).description;
            this.profileData.push({ id: 7, label: "EMPLOYEE STATUS", description: lblDesc });
          }

          if (classifications.clientShiftId) {
            lblDesc = jobProfileData.data.clientShiftList.find( x => x.clientShiftId == classifications.clientShiftId).description;
            this.profileData.push({ id: 8, label: "CLIENT SHIFT", description: lblDesc });
          }

          if (classifications.eeocLocationId) {
            lblDesc = jobProfileData.data.eeocLocationList.find( x => x.eeocLocationId == classifications.eeocLocationId).eeocLocationDescription;
            this.profileData.push({ id: 9, label: "EEOC LOCATION", description: lblDesc });
          }

          if (classifications.eeocJobCategoryId) {
            lblDesc = jobProfileData.data.eeocJobCategoryList.find( x => x.jobCategoryId == classifications.eeocJobCategoryId).description;
            this.profileData.push({ id: 10, label: "EEOC JOB CATEGORY", description: lblDesc });
          }

          if (classifications.directSupervisorId) {
            lblDesc = jobProfileData.data.supervisorAndCompAdminList.find( x => x.userId == classifications.directSupervisorId).firstName;
            this.profileData.push({ id: 14, label: "DIRECT SUPERVISOR", description: lblDesc });
          }           
        }

        if (jobProfileData.data && jobProfileData.data.jobProfileDto && jobProfileData.data.jobProfileDto.compensation) {
          var compensation = jobProfileData.data.jobProfileDto.compensation;
          if (compensation.payFrequencyID) {
            lblDesc = jobProfileData.data.payFrequencyList.find( x => x.payFrequencyId == compensation.payFrequencyID).name;
            this.profileData.push({ id: 11, label: "PAY FREQUENCY", description: lblDesc });
          }            

          if (compensation.employeeTypeID == 1) {
              lblDesc = "Hourly";
              this.profileData.push({ id: 12, label: "PAY TYPE", description: lblDesc });
          }
          else if (compensation.employeeTypeID == 2) {
              lblDesc = "Salary";
              this.profileData.push({ id: 12, label: "PAY TYPE", description: lblDesc });
          }
        }

        if (this.profileData.length == 0) {
          setTimeout(()=>{
            this.ref.close(null);
          },3000);
        }
      })
    ).subscribe();    

    this.buildForm();
  }

  buildForm() {
    this.form = this.fb.group({

    });
  }

  save() {
    this.formSubmitted = true;
    if (!this.form.valid) {
      return;
    }

    this.ref.close(this.jobProfileData);
  }

  clear() {
    this.ref.close(null);
  }
}