import { Component, OnInit } from '@angular/core';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { AccountService } from '@ds/core/account.service';
import { JobProfileApiService } from 'apps/ds-company/src/app/services/job-profile-api.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { tap, switchMap } from 'rxjs/operators';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { Utilities } from 'apps/ds-company/src/app/shared/utils/utilities';
import { DsConfirmDialogContentComponent } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.component';
import { DsStorageService } from '@ds/core/storage/storage.service';
import { JobProfileTitleDialogComponent } from '../job-profile-title-dialog/job-profile-title-dialog.component';
import { HttpErrorResponse } from '@angular/common/http';
import { IJobDetailData } from 'apps/ds-company/src/app/models/job-profile.model';
import { UserType } from '@ds/core/shared';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-job-profiles',
  templateUrl: './job-profile-list.component.html',
  styleUrls: ['./job-profile-list.component.scss']
})
export class JobProfileListComponent implements OnInit {
    currentPageType: number;
    currentJobProfile: IJobDetailData = null;
    isLoading: boolean = true;
    userInfo: UserInfo;
    isApplicantAdmin: boolean = true;
    searchJobProfiles: string;
    includeInactiveJobProfiles: boolean = false;
    filters: {includeInactiveJobProfiles: boolean};
    jobProfilesFilterKeys = "job-profiles-filters";
    form: FormGroup;
    formSubmitted: boolean;
    jobProfiles:Array<IJobDetailData> = [];

    constructor(private accountService: AccountService,
      private jobProfileService: JobProfileApiService, 
      private msg: NgxMessageService,
      private dialog: MatDialog,
      private confirmDialog: MatDialog,
      private fb: FormBuilder,
      private router: Router,
      private storeService: DsStorageService) {
    }

    ngOnInit() {
      this.isLoading = true;

      const storeResult = this.storeService.get(this.jobProfilesFilterKeys);
      if (storeResult.success) {
        this.filters = storeResult.data;
        this.includeInactiveJobProfiles = this.filters.includeInactiveJobProfiles;
      }
      else {
        this.includeInactiveJobProfiles = false;
      }

      this.accountService.getUserInfo().pipe(
        tap(userInfo => {
          this.userInfo = userInfo;
          if (userInfo.userTypeId == UserType.supervisor)
            this.isApplicantAdmin = userInfo.isApplicantAdmin;
        }), 
        switchMap(userInfo => this.jobProfileService.getJobProfileListByClient(userInfo.lastClientId || userInfo.clientId)),
          tap(jobProfiles => {
            this.jobProfiles = jobProfiles;
            this.jobProfiles.sort((a, b) => (a.description > b.description) ? 1 : -1)
            this.buildForm();
            this.isLoading = false;
          })
      ).subscribe();
    }

    popupJobProfileTitleDialog() {
      let config = new MatDialogConfig<any>();
      config.width = "500px";
      config.data = {title: ''};
  
      return this.dialog.open<JobProfileTitleDialogComponent, any, string>(JobProfileTitleDialogComponent, config)
        .afterClosed()
        .subscribe((jobProfileTitle: any) => {
          if (jobProfileTitle) {

            let data = {
              clientId: this.userInfo.lastClientId || this.userInfo.clientId,
              jobProfileId: 0,
              description: jobProfileTitle,
              isActive: true
            };

            this.jobProfileService.saveJobProfile(data)
            .subscribe((result) => {
              if (!result.hasError && result.data)
                this.router.navigate(['admin/job-profile-details', result.data.clientId, result.data.jobProfileId, 'edit']);  
            }, (error: HttpErrorResponse) => {
                this.msg.setErrorResponse(error);
            });    
          }
        });
    }

    buildForm() {
      this.form = this.fb.group({
        searchJobProfiles: [''],
        includeInactiveJobProfiles: [this.includeInactiveJobProfiles],
      });
  
      this.form.get('searchJobProfiles').valueChanges.subscribe(val => {
        this.searchJobProfiles = this.form.get('searchJobProfiles').value;
      });

      this.form.get('includeInactiveJobProfiles').valueChanges.subscribe(val => {
        this.includeInactiveJobProfiles = this.form.get('includeInactiveJobProfiles').value;
        this.setFilters();
      });
    }

    setFilters() {
      this.filters = {
          includeInactiveJobProfiles: this.form.get('includeInactiveJobProfiles').value
      };
      this.storeService.set(this.jobProfilesFilterKeys, this.filters);	
    }
 
    editJobProfile(currentJobProfile: IJobDetailData) {
      this.currentJobProfile = currentJobProfile;
      this.router.navigate(['admin/job-profile-details', this.currentJobProfile.clientId, this.currentJobProfile.jobProfileId, 'edit']);  
    }

    updateJobProfileStatus(jobProfile) {
      let data = {
          jobProfileId: jobProfile.jobProfileId,
          isActive: !jobProfile.isActive
      };
  
      this.jobProfileService.updateJobProfileStatus(data)
      .subscribe((result) => {
        this.msg.setSuccessMessage("Job profile status updated successfully.");
        jobProfile.isActive = !jobProfile.isActive;
      }, (error: HttpErrorResponse) => {
          this.msg.setErrorResponse(error);
      });    
    }
    
    copyJobProfile(jobProfileId: number) {
      let config = new MatDialogConfig<any>();
      config.width = "500px";
      config.data = {title: '', message: 'Are you sure you want to copy this Job Profile?', confirm: 'Copy'};

      return this.dialog.open<DsConfirmDialogContentComponent, any, boolean>(DsConfirmDialogContentComponent, config)
      .afterClosed()
      .subscribe((canDelete: boolean) => {
          if(canDelete) {
            this.jobProfileService.copyJobProfile(jobProfileId)
            .subscribe((result) => {
              this.jobProfiles.push(result.data);
              this.jobProfiles.sort((a, b) => (a.description > b.description) ? 1 : -1)
              this.msg.setSuccessMessage("Job profile copied successfully.");
            }, (error: HttpErrorResponse) => {
                this.msg.setErrorResponse(error);
            });    
          }
      });
    }

    trackJobProfile (index: number, jobProfile: IJobDetailData) {
      return jobProfile.jobProfileId;
    }    
}