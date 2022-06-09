import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { FormControl, FormGroup, Validators, FormBuilder, AsyncValidatorFn, ValidatorFn, ValidationErrors } from '@angular/forms';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { tap } from 'rxjs/operators';
import { CustomPagesService } from "apps/ds-company/src/app/services/custom-pages.service";
import { ResourceService } from 'apps/ds-company/src/app/services/resource.service';
import { Utilities } from 'apps/ds-company/src/app/shared/utils/utilities';
import { JobProfileApiService } from "apps/ds-company/src/app/services/job-profile-api.service";
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
  selector: 'ds-job-profile-title-dialog',
  templateUrl: './job-profile-title-dialog.component.html',
  styleUrls: ['./job-profile-title-dialog.component.css']
})
export class JobProfileTitleDialogComponent implements OnInit {
  form: FormGroup;
  formSubmitted: boolean;
  jobProfileTitle: string;
  isLoading: boolean = true;
  userAccount: UserInfo;
  clientId: number;
  jobProfileId?: number;
  title: string;
  allData: any;

  constructor(
    private ref:MatDialogRef<JobProfileTitleDialogComponent>,
    private jobProfileService: JobProfileApiService,
    private resourceService: ResourceService, 
    private msg: NgxMessageService,
    private dialog: MatDialog,
    private confirmDialog: MatDialog,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data:any,
  ) { 
    this.allData = data;
    this.title = data.title;
  }

  ngOnInit(): void {
    this.buildForm();
    this.isLoading = false;
  }

  buildForm() {
    this.form = this.fb.group({
      jobProfileTitle:       [this.title, Validators.compose([Validators.required, Validators.maxLength(50)])],
    });

    this.form.get('jobProfileTitle').valueChanges.subscribe(val => {
      this.jobProfileTitle = this.form.get('jobProfileTitle').value;
    });
  }

  save() {
    this.formSubmitted = true;
    if (!this.form.valid) {
      return;
    }

    this.ref.close(this.jobProfileTitle);
  }

  clear() {
    this.ref.close(null);
  }
}