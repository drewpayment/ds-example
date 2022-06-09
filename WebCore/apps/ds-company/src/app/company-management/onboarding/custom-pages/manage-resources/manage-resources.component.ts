import { Component, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { AccountService } from '@ds/core/account.service';
import { CustomPagesService } from '../../../../services/custom-pages.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Observable } from 'rxjs/internal/Observable';
import { iif, of, Subscription } from 'rxjs';
import { tap } from 'rxjs/operators';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { ResourceService } from 'apps/ds-company/src/app/services/resource.service';
import { ManageResourcesDialogComponent } from 'apps/ds-company/src/app/company-management/onboarding/custom-pages/manage-resources/manage-resources-dialog/manage-resources-dialog.component';
import { Utilities } from 'apps/ds-company/src/app/shared/utils/utilities';
import { IOnboardingWorkflowTask, ICompanyResource } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-manage-resources',
  templateUrl: './manage-resources.component.html',
  styleUrls: ['./manage-resources.component.scss']
})
export class ManageResourcesComponent implements OnInit, AfterViewInit {
  isLoading: boolean = true;
  userinfo: UserInfo;
  form: FormGroup;
  formSubmitted: boolean;
  isNewTask: boolean = false;
  paramsSubscription: Subscription;
  currentTask: IOnboardingWorkflowTask = <IOnboardingWorkflowTask>{};
  selectedResources: Array<ICompanyResource> = [];
  previewResource: string;
  pageType: number = 1;
  pageFrom: any = null;
  selectedClientOrganization: number;
  routeText: string;
  workflowEmployeeId: number = 0;
  workflowClientId: number = 0;
  jobProfileId: number = 0;
  clientId: number = 0;


  constructor(private accountService: AccountService,
    private customPagesService: CustomPagesService,
    private msg: NgxMessageService,
    private dialog: MatDialog,
    private confirmDialog: MatDialog,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer,
    private ref: ChangeDetectorRef,
    private router: Router,
    private resourceService: ResourceService, ) { }

  ngOnInit() {
    this.isLoading = true;
    this.pageType = this.route.snapshot.params['pageType']; //Have to change this to get it from the passed object instead
    this.workflowEmployeeId = this.route.snapshot.params['employeeId'] ? this.route.snapshot.params['employeeId']: 0;
    this.workflowClientId = this.route.snapshot.params['clientId'] ? this.route.snapshot.params['clientId']: 0;
    this.clientId = this.route.snapshot.params['clientId'];
    this.jobProfileId = this.route.snapshot.params['jobProfileId'];

    this.routeText = Utilities.getRouteByPageType(this.pageType);

    this.checkCurrentUser().pipe(tap(userInfo => {
      this.userinfo = userInfo;
      this.selectedClientOrganization = (this.workflowClientId > 0) ? this.workflowClientId : this.userinfo.lastClientId || this.userinfo.clientId;
      if (this.route.snapshot.params['taskId'] != 0) {
        this.customPagesService.getOnboardingWorkflowTask(this.route.snapshot.params['taskId'])
          .pipe(tap( task => {
            this.currentTask = task;
            if (this.currentTask) {
              this.currentTask.userMustCheckAgreement = this.currentTask.signatureDescription ? true : false;
              this.selectedResources = this.currentTask.resources;
              if (this.selectedResources && this.selectedResources.length > 0) {
                  this.setPreviewResource(this.selectedResources[0]);
              }
            }

            // Build the form here
            this.buildForm();
            this.isLoading = false;
          })
        ).subscribe();
      }
      else {
        this.isLoading = true;
        this.initializeCurrentTask(this.pageType);
        //Build the form here
        this.buildForm();
        this.isLoading = false;
      }
    })).subscribe();

    this.paramsSubscription = this.route.params
      .subscribe(
        (params: Params) => {
          this.currentTask.onboardingWorkflowTaskId = params['taskId'];
          this.pageType = params['pageType'];
        }
    );
  }

  checkCurrentUser(): Observable<UserInfo> {
    return iif(() => this.userinfo == null,
      this.accountService.getUserInfo().pipe(tap(u => {
        this.userinfo = u;
      })),
      of(this.userinfo));
  }

  initializeCurrentTask(pageType: number) {
    this.currentTask = {
      onboardingWorkflowTaskId: 0,
      route: Utilities.getRouteByPageType(pageType),
      route1: Utilities.getRouteByPageType(pageType),
      linkToState: Utilities.getLinkToStateByPageType(pageType),
      modifiedBy: 0,
      modified: new Date(),
      clientId: this.selectedClientOrganization,
      resources: [],
      workflowTitle: null,
      description: null,
      adminDescription: null,
      adminMustSelect: true,
      signatureDescription: null,
      isReferred: false,
      hasActiveWorkflowReference: false,
      requireWorkFlowTaskId: false,
      uploadDescription: null,
      userMustUpload: false,
      userMustUploadResource: false,
      userMustCheckAgreement: false
    };
  }

  buildForm() {
    this.form = this.fb.group({
      workflowTitle:       [this.currentTask.workflowTitle, Validators.compose([Validators.required, Validators.maxLength(50)])],
      description:         [this.currentTask.description],
      cbEmpDocUpload:      [this.currentTask.userMustUpload],
      cbAgreement:         [this.currentTask.userMustCheckAgreement],
      uploadDescription:   [this.currentTask.uploadDescription, this.currentTask.userMustUpload ? Validators.required : []],
      signatureDescription:[this.currentTask.signatureDescription, this.currentTask.userMustCheckAgreement ? Validators.required : []],
    });

    this.form.get('cbEmpDocUpload').valueChanges.subscribe(val => {
      if (val)
        this.form.controls['uploadDescription'].setValidators([Validators.required]);
      else
        this.form.controls['uploadDescription'].clearValidators();

      this.form.controls['uploadDescription'].updateValueAndValidity();
    });

    this.form.get('cbAgreement').valueChanges.subscribe(val => {
      if (val)
        this.form.controls['signatureDescription'].setValidators([Validators.required]);
      else
        this.form.controls['signatureDescription'].clearValidators();

      this.form.controls['signatureDescription'].updateValueAndValidity();
    });
  }

  ngAfterViewInit() {
    //this.ref.detach()
  }

  toggleCbEmpDocUpload(event) {
    if ( event.target.checked )
      this.currentTask.userMustUpload = true;
    else
      this.currentTask.userMustUpload = false;
  }

  toggleCbAgreement(event) {
    if ( event.target.checked )
      this.currentTask.userMustCheckAgreement = true;
    else
      this.currentTask.userMustCheckAgreement = false;
  }

  setPreviewResource(selectedResource: ICompanyResource) {
    if (selectedResource.resourceId) {
      if (this.pageType == 1)
        this.previewResource = "api/resources/" + selectedResource.resourceId + "?isDownload=false";
      else if (this.pageType == 4)
        this.previewResource = selectedResource.source;
    }
    this.setActiveResource(selectedResource);
  }

  setActiveResource(selectedResource) {
    this.selectedResources.forEach(obj => {
      if (obj.resourceId == selectedResource.resourceId) {
        obj.previewResourceCssClass = true;
      }
      else {
        obj.previewResourceCssClass = false;
      }
    });
  }

  download(selectedResource: ICompanyResource) {
    this.resourceService.downloadResource(selectedResource).subscribe((res) => { },
      (error) => this.msg.setErrorMessage(this.errorMessage(error))
    );
  }

  removeResource(selectedResource: ICompanyResource) {
    this.selectedResources.forEach((currResource, index) => {
      if (currResource.resourceId == selectedResource.resourceId) {
        //currResource.IsSelectedResource = false;
        currResource.previewResourceCssClass = false;
        this.selectedResources.splice(index, 1);
        if (this.selectedResources && this.selectedResources.length > 0) {
          this.setPreviewResource(this.selectedResources[0]);
        }
      }
    });
  }

  popupDocumentsDialog() {
    let config = new MatDialogConfig<any>();
    config.width = "700px";
    config.data = {userAccount: this.userinfo, selectedResources: this.selectedResources, pageType: this.pageType,  pageFrom: this.pageFrom, selectedClientOrganization: this.selectedClientOrganization};

    return this.dialog.open<ManageResourcesDialogComponent, any, string>(ManageResourcesDialogComponent, config)
      .afterClosed()
      .subscribe((selectedResources: any) => {
        if (selectedResources) {
          this.selectedResources = selectedResources;
          if (selectedResources) {
            if (this.pageType == 1)
              this.previewResource = "api/resources/" + selectedResources[0].resourceId + "?isDownload=false";
            else if (this.pageType == 4)
              this.previewResource = selectedResources[0].source;

            _.forEach(this.selectedResources, resource => {
              resource.previewResourceCssClass = false;
            });
            selectedResources[0].previewResourceCssClass = true;
          }
        }
      });
  }

  redirectToCustomPages() {
    if (this.workflowEmployeeId > 0) //This request is from AddWorkflow
      this.router.navigate(['manage/onboarding/', 'add-workflow', this.selectedClientOrganization, this.workflowEmployeeId, '2', 'add']);
    else if (this.clientId > 0 && this.jobProfileId > 0) //This request is from Job Profile 
      this.router.navigate(['admin/job-profile-details/' + this.clientId + '/'+ this.jobProfileId + '/edit'])      
    else
      this.router.navigate(['admin/onboarding/custom-pages']);  
  }

  save(item: IOnboardingWorkflowTask) {
    this.formSubmitted = true;
    if (!this.form.valid) {
      return;
    }

    if (!this.selectedResources || this.selectedResources.length < 1) {
      this.msg.setErrorMessage('Please select at least one resource.');
      return;
    }

    let selectedResourceIds = [];
    this.selectedResources.forEach((currResource, index) => {
        selectedResourceIds.push(currResource.resourceId);
    });

    let data = {
        workflowTaskId: item.onboardingWorkflowTaskId,
        workflowTitle: this.form.value.workflowTitle,
        type: this.pageType,
        description: this.form.value.description,
        adminDescription: this.form.value.description,
        resources: selectedResourceIds,
        signatureDescription: item.userMustCheckAgreement ? this.form.value.signatureDescription : null,
        uploadDescription: item.userMustUpload ? this.form.value.uploadDescription : null,
        userMustUpload: this.form.value.cbEmpDocUpload,
        clientId: item.clientId
    };

    this.customPagesService.updateOnboardingWorkflowTask(data)
      .pipe(tap( result => {
        this.msg.setSuccessMessage("Saved successfully.", 2000);
        setTimeout(() => {
          // if ($scope.pageFrom == 'workflow') {
          //     DsState.router.go('company.onboarding.workflow', { employeeId: dsEmployeeData.getEmployeeData().employeeId, returnRoute : '' });
          // }
          // else if($scope.pageFrom == 'workflow'){
          //     DsState.router.go('company.jobprofiledetails', { jobProfile: $scope.jobProfileId, jobProfileStatus: false, returnFrom:'customPage' });
          // }
          //else {
            //this.msg.setSuccessMessage("Resource added successfully.", 5000);
            //this.redirectToCustomPages();
          //}
          this.redirectToCustomPages();
        }, 2000);
      })
    ).subscribe();
  }

  openSite(siteUrl) {
    window.open(siteUrl, '_blank');
  }

  errorMessage(error){
      var str = error.message ;
      if(error.error.errors && error.error.errors.length > 0){
          str += '. ' + error.error.errors[0].msg;
      }
      return str;
  }
}
