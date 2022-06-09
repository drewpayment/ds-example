import { Component, OnInit } from '@angular/core';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { AccountService } from '@ds/core/account.service';
import { CustomPagesService } from 'apps/ds-company/src/app/services/custom-pages.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { tap, switchMap } from 'rxjs/operators';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { Utilities } from 'apps/ds-company/src/app/shared/utils/utilities';
import { DsConfirmDialogContentComponent } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.component';
import { DsStorageService } from '@ds/core/storage/storage.service';
import { IOnboardingWorkflowTask } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-custom-pages',
  templateUrl: './custom-pages.component.html',
  styleUrls: ['./custom-pages.component.scss']
})
export class CustomPagesComponent implements OnInit {
    currentPageType: number;
    currentTask: IOnboardingWorkflowTask = null;
    isLoading: boolean = true;
    userInfo: UserInfo;
    tasks:Array<IOnboardingWorkflowTask> = [];
    searchTasks: string;
    includeInactiveTasks: boolean = false;
    filters: {includeInactiveTasks: boolean};
    customPagesFilterKeys = "custom-pages-filters";
    form: FormGroup;
    formSubmitted: boolean;

    constructor(private accountService: AccountService,
      private customPagesService: CustomPagesService,
      private msg: NgxMessageService,
      private dialog: MatDialog,
      private confirmDialog: MatDialog,
      private fb: FormBuilder,
      private router: Router,
      private storeService: DsStorageService) {
    }

    ngOnInit() {
      this.isLoading = true;

      const storeResult = this.storeService.get(this.customPagesFilterKeys);
      if (storeResult.success) {
        this.filters = storeResult.data;
        this.includeInactiveTasks = this.filters.includeInactiveTasks;
      }
      else {
        this.includeInactiveTasks = false;
      }

      this.accountService.getUserInfo().pipe(
        tap(userInfo => {
          this.userInfo = userInfo;
        }),
        switchMap(userInfo => this.customPagesService.getCompanyTasksByClient(userInfo.lastClientId || userInfo.clientId)),
          tap(tasks => {
            this.tasks = tasks;
            this.tasks.sort((a, b) => (a.workflowTitle.toLowerCase() > b.workflowTitle.toLowerCase()) ? 1 : -1)
            this.buildForm();
            this.isLoading = false;
          })
      ).subscribe();
    }

    buildForm() {
      this.form = this.fb.group({
        searchTasks: [''],
        includeInactiveTasks: [this.includeInactiveTasks],
      });

      this.form.get('searchTasks').valueChanges.subscribe(val => {
        this.searchTasks = this.form.get('searchTasks').value;
      });

      this.form.get('includeInactiveTasks').valueChanges.subscribe(val => {
        this.includeInactiveTasks = this.form.get('includeInactiveTasks').value;
        this.setFilters();
      });
    }

    setFilters() {
      this.filters = {
          includeInactiveTasks: this.form.get('includeInactiveTasks').value
      };
      this.storeService.set(this.customPagesFilterKeys, this.filters);
    }

    getActionText(task: IOnboardingWorkflowTask) : string {
      let confirm = "";

      if (!task.isReferred && !task.hasActiveWorkflowReference)
        confirm = "Delete";
      else if (task.isReferred && !task.hasActiveWorkflowReference)
        confirm = "Set as Inactive";
      else //Has some Active Reference. So deletion is not allowed.
        confirm = "Set as Inactive";

      return confirm;
    }

    addCustomPage(route: string) {
      this.currentPageType = Utilities.getPageTypeByRoute(route);

      this.currentTask = {
        onboardingWorkflowTaskId: 0,
        route: route,
        route1: route,
        linkToState: Utilities.getLinkToStateByRoute(route),
        modifiedBy: 0,
        modified: new Date(),
        clientId: this.userInfo.lastClientId || this.userInfo.clientId, //Have to check if page redirected from workflow and if so use the selectedClientId passed from there.
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

      this.router.navigate(['admin/onboarding/manage-resources', this.currentTask.onboardingWorkflowTaskId, this.currentPageType, 'add']);  
    }

    redirectToCustomPages() {
      this.router.navigate(['admin/onboarding/custom-pages']);  
    }
  
    editCustomPage(currentTask: IOnboardingWorkflowTask) {
      this.currentPageType = Utilities.getPageTypeByRoute(currentTask.route1);
      this.currentTask = currentTask;
      this.router.navigate(['admin/onboarding/manage-resources', this.currentTask.onboardingWorkflowTaskId, this.currentPageType, 'edit']);  
    }

    changeStatusOfCustomPage(currentTask: IOnboardingWorkflowTask) {
      this.currentTask = currentTask;

      if (this.currentTask.isDeleted) { //Is now in deactivated state. Have to activate
        this.currentTask.isDeleted = false;
        this.customPagesService.changeStatusOfWorkflowTask(this.currentTask)
        .pipe(tap(success => {
          if (success) {
            this.currentTask.isDeleted = false;
            this.tasks.forEach((currTask, index) => {
              if (currTask.onboardingWorkflowTaskId == this.currentTask.onboardingWorkflowTaskId)
                this.tasks[index] = this.currentTask;
            });
            this.tasks = this.tasks.slice();
            this.msg.setSuccessMessage("Resource updated successfully.", 2000);
          }
          else {
            this.msg.setErrorMessage("Error occurred. Please try after some time.")
          }
        })).subscribe();
      }
      else { //Is now in activated state. Can allow delete/deactivate/nothing
        let message = "";
		    let confirm = "";
        let allowDelete = false;
		    let allowToInactivate = false;

        if (!this.currentTask.isReferred && !this.currentTask.hasActiveWorkflowReference) {
          message = "Are you sure you want to delete this document?";
          allowDelete = true;
		      confirm = "Delete";
        }
        else if (this.currentTask.isReferred && !this.currentTask.hasActiveWorkflowReference) {
          message = "This document has been used for employee's onboarding process. Marking it as inactive it will remove the document from any one it was assigned.";
          allowToInactivate = true;
		      confirm = "Mark as Inactive";
        }
        else //Has some Active Reference. So deletion is not allowed.
          message = "This document is assigned to an active employee's onboarding process. It cannot be deleted or marked as inactive.";

        let config = new MatDialogConfig<any>();
        config.width = "500px";
        config.data = {title: confirm + ' ' + this.currentTask.workflowTitle, message: message, confirm: confirm, allowAction: allowDelete || allowToInactivate};

        return this.dialog.open<DsConfirmDialogContentComponent, any, boolean>(DsConfirmDialogContentComponent, config)
        .afterClosed()
        .subscribe((result: boolean) => {
            if(result) {
              if (allowDelete)	{
                this.customPagesService.deleteOnboardingWorkflowTask(this.currentTask)
                .pipe(tap(success => {
                if (success) {
                  this.tasks.forEach((currTask, index) => {
                    if (currTask.onboardingWorkflowTaskId == this.currentTask.onboardingWorkflowTaskId)
                      this.tasks.splice(index, 1);
                  });
                  this.tasks = this.tasks.slice();
                  this.msg.setSuccessMessage("Resource updated successfully.", 2000);
                }
                else {
                  this.msg.setErrorMessage("An error occurred. Please try again later.")
                }
                })).subscribe();
              }
              else  {
                this.currentTask.isDeleted = true;
                this.customPagesService.changeStatusOfWorkflowTask(this.currentTask)
                .pipe(tap(success => {
                if (success) {
                  this.tasks.forEach((currTask, index) => {
                    if (currTask.onboardingWorkflowTaskId == this.currentTask.onboardingWorkflowTaskId)
                      this.tasks[index] = this.currentTask;
                  });
                  this.tasks = this.tasks.slice();
                  this.msg.setSuccessMessage("Resource updated successfully.", 2000);
                }
                else {
                  this.msg.setErrorMessage("An error occurred. Please try again later.")
                }
                })).subscribe();
              }
            }
        });
      }
    }
}
