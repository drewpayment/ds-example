import * as angular from "angular";
import { Component, OnInit } from '@angular/core';
import { ClientService } from '@ds/core/clients/shared';
import { AccountService } from '@ds/core/account.service';
import { UserInfo } from "@ds/core/shared/user-info.model";
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { tap, switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs/internal/Observable';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute ,Params, Router} from '@angular/router';
import { EmployeeDataService } from "apps/ds-company/src/app/services/employee-data.service";
import { IEmployeeSearchResult,    IEmployeeSearchResultResponseData, IEmployeeNavInfo } from '@ds/employees/search/shared/models/employee-search-result';
import {DashboardService} from "apps/ds-company/src/app/services/dashboard.service";
import { CountryStateService } from '@ds/onboarding/shared/country-state.service';
import { ICountry,IState } from '@ds/onboarding/shared/country.model';
import { EMPTY, iif, of, forkJoin } from 'rxjs';
import { IOnboardingEmployee, IOnboardingAdminTask } from 'apps/ds-company/src/app/models/onboarding-employee.model';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { CloseOnboardingDialogComponent } from './close-onboarding/close-onboarding-dialog.component';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { ImportAdminTasklistDialogComponent } from './import-admin-tasklist/import-admin-tasklist-dialog.component';
import { CompletionStatusType } from '@ds/core/shared';
import { CertifyI9DialogComponent } from '@ds/onboarding/certify-I9/certify-I9-trigger/certify-I9-trigger.component';
import { EmployeeAttachmentFolderDetail } from '@ds/performance/attachments/shared/models/';
import { EmployeeActionTypes } from '@ds/employees/header/ngrx/actions';
import { EmployeeAttachmentApiService } from '@ds/core/employees/employee-attachments/employee-attachment-api.service';
import { IEmployeeAttachment } from '@ds/core/employees/employee-attachments/employee-attachment.model';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';
import { AddAttachmentDialogComponent } from './add-attachment-dialog/add-attachment-dialog.component';
import { OnboardingSummaryDialogComponent } from './onboarding-summary-dialog/onboarding-summary-dialog.component';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
  selector: 'ds-dashboard-detail',
  templateUrl: './dashboard-detail.component.html',
  styleUrls: ['./dashboard-detail.component.scss']
})
export class DashboardDetailComponent implements OnInit {

    isLoading: boolean = true;
    isLoadingAttachments: boolean = false;
    employeeId:number;
    employee:IOnboardingEmployee;
    enableCertifyI9: boolean;
    showCertifyI9: boolean;
    showEditWorkflow:boolean;
    allowMarkEmployeePayrollReady: boolean;
    completedPct: number;
    form:FormGroup;

    resources:Array<IEmployeeAttachment> = [];
    onboardingFolders:Array<EmployeeAttachmentFolderDetail> = [];
    workflowData:any[] = [];

    countries: ICountry[];
    states: IState[];
    documents: any[];
    companyUrl: string;

    private get tasksFormArray(): FormArray {
      return this.form.get('tasks') as FormArray;
    }

    private searchList:IEmployeeSearchResultResponseData;
    private user: UserInfo;
    private onboardingEmployees$:Observable<IOnboardingEmployee[]>;

    constructor(
      private accountService: AccountService,
      private clientService: ClientService,
      private dashboardService: DashboardService,
      private msgSvc: NgxMessageService,
      private dialog: MatDialog,
      private route: ActivatedRoute,
      private router: Router,
      private dsEmployeeData: EmployeeDataService,
      private confirmDialog: ConfirmDialogService,
      private fb: FormBuilder,
      private countryStateService: CountryStateService,
      private attachmentService: EmployeeAttachmentApiService,
      ) {}

    ngOnInit() {

        this.employeeId = parseInt( this.route.snapshot.paramMap.get('employeeId') );
        this.isLoadingAttachments = true;
        this.isLoading = true;

        // always load the employee data
        this.checkCurrentUser().pipe(
            switchMap(x => this.loadEmployeeData() ),
            switchMap( x => {
              if(!x || x.length == 0) return EMPTY;
              else return this.loadEmployeeResources();
            }),
            ).subscribe(()=>{}, (err: HttpErrorResponse) => {
              this.isLoadingAttachments = false;
              this.isLoading = false;

              if (err.error && err.error.errors && err.error.errors.length) 
                this.msgSvc.setErrorMessage(err.error.errors[0].msg);
              else 
                this.msgSvc.setErrorMessage(err.message );
            }); 

        forkJoin(this.countryStateService.getCountryList(), this.countryStateService.getStatesForUSA(), this.dashboardService.getI9Documents(), this.accountService.getSiteUrls())
        .subscribe(data => {
          this.countries = data[0];
          this.states = data[1];
          this.documents = data[2];
          let company = data[3].find(
            (s) => s.siteType === ConfigUrlType.Company
          );
          this.companyUrl = company ? company.url : '';
        });

        this.accountService.canPerformActions('Onboarding.AllowI9Certification').subscribe((data) => { 
          this.enableCertifyI9 = true; 
        });
        this.accountService.canPerformActions('Onboarding.AllowMarkEmployeePayrollReady').subscribe((data) => { 
          this.allowMarkEmployeePayrollReady = true;
        });
    }

    checkCurrentUser(): Observable<UserInfo> {
      return iif(() => this.user == null, 
        this.accountService.getUserInfo().pipe(tap(u => {
          this.user = u;
        })),
        of(this.user));
    }

    refresh(navType: EmployeeActionTypes){
      if(navType == EmployeeActionTypes.GoToPreviousEmployee && !!this.searchList.nav.prev)
      {
        this.dsEmployeeData.selectEmployee(this.searchList.nav.prev.employeeId);
        this.employeeId = this.searchList.nav.prev.employeeId;
      }
      else if(navType == EmployeeActionTypes.GoToNextEmployee && !!this.searchList.nav.next)
      {
        this.dsEmployeeData.selectEmployee(this.searchList.nav.next.employeeId);
        this.employeeId = this.searchList.nav.next.employeeId;
      }
      else{
        this.employeeId = this.dsEmployeeData.getCurrentEmployeeId();
      }

      if(!this.employeeId)
        this.router.navigate(['manage/onboarding/dashboard']);
      else{
        this.router.navigate(['manage/onboarding/dashboard-detail' , this.employeeId ]);

        this.isLoading = true;
        this.isLoadingAttachments = true;

        this.loadEmployeeData().pipe(
        switchMap( x => {
          if(!x || x.length == 0) return EMPTY;
          else return this.loadEmployeeResources();
        }),
        ).subscribe( x => {
        } , (err: HttpErrorResponse) => {
          this.isLoadingAttachments = false;
          this.isLoading = false;

          if (err.error && err.error.errors && err.error.errors.length) 
            this.msgSvc.setErrorMessage(err.error.errors[0].msg);
          else 
            this.msgSvc.setErrorMessage(err.message );
        } ); 
      }
  }

  loadEmployeeData():Observable<any[]>{
    return this.dashboardService.getEmployeeList(this.user.clientId, {employeeId: this.employeeId}).pipe(
      tap(x => {
        if(x && x.length > 0){
          if (  !this.dsEmployeeData.employeeList || this.dsEmployeeData.employeeList.length == 0 || 
                !this.dsEmployeeData.employeeList.find(x=>x.employeeId == this.employeeId) )
            this.dsEmployeeData.setEmployeeList(x);

          this.dsEmployeeData.selectEmployee(this.employeeId);
          this.employee = x[0];
          this.searchList = this.dsEmployeeData.mapOnboardingEmployeeToSearchResponse(this.employee);
          this.reloadAdminTaskStatus();
          this.isLoading = false;
        }
        
      }))
  }

  loadEmployeeResources():Observable<any[]>{
    return this.attachmentService.getEmployeeAttachmentFolderList( this.employeeId, this.employee.clientId, true).pipe(tap( data => {
      this.resources = [];
      if(data){
        let folders:any[] = <Array<any>>data;
        
        if (folders && folders.length > 0) {
            this.onboardingFolders = folders.filter(f => f.isDefaultOnboardingFolder);
            
            folders.forEach( folder => {
                if (folder.attachmentCount > 0) {
                    folder.attachments.forEach( (attachmnet) => {
                        this.resources.push(attachmnet);
                    });
                }
            });
        }
      }
      this.isLoadingAttachments = false;
    }))
  }

  reloadAdminTaskStatus() {
    // Employee Workflow Tasks
    this.workflowData = [];
    this.completedPct = 0;
    this.employee.employeeWorkflow = this.employee.employeeWorkflow || [];
    this.employee.employeeWorkflow.forEach((item) => {
      const hasChildren = this.employee.employeeWorkflow.filter( (workFlow) => workFlow.onboardingTask.mainTaskId == item.onboardingWorkflowTaskId ).length > 0;
      if (!hasChildren) this.workflowData.push(item);
    });
    
    const totalTask = this.workflowData.length;
    const totalCompletedTask = this.workflowData.filter(v => v.isCompleted).length;
    if (totalTask != 0) this.completedPct = Math.round((100 * totalCompletedTask) / totalTask);
    this.showEditWorkflow = this.workflowData.filter(empTask => empTask.isCompleted == null).length == this.workflowData.length;

    // Admin Tasks
    this.employee.taskList = this.employee.taskList || [];
    this.showCertifyI9 = this.workflowData.filter(value => value.onboardingTask.linkToState == "ess.onboarding.i9").length > 0;
    this.employee.taskList.forEach(task => {
      if (task.completionStatus == 3) task.isComplete = true;
      else task.isComplete = false;
      task.isEditing = false;
    });
    
    this.buildTaskListForm();
    this.calculateAdminPct();
  }

  deleteEmployee(){
    const options = {
      title: "Are you sure you want to delete this employee?",
      message: "",
      confirm: "Yes, Remove",
    };
    this.confirmDialog.open(options);
    this.confirmDialog.confirmed().subscribe((confirmed) => {
      if (confirmed) {
        this.dashboardService.putEmployeeFromESSRemove(this.employee.employeeId).subscribe( (affectedRows) => {
          if (affectedRows.data > 0) {
            this.dsEmployeeData.deleteEmployee(this.employee.employeeId);
            this.refresh(EmployeeActionTypes.UpdateState);

              this.msgSvc.setSuccessMessage("Employee removed from onboarding successfully.");
          } else if (affectedRows.data === -2) {
            this.msgSvc.setErrorMessage('This employee can not be deleted because pay records already exist for this employee.');
          } else if (affectedRows.data === -3) {
            this.msgSvc.setErrorMessage('This employee can not be deleted because punch records already exist for this employee.');
          } else {
            this.msgSvc.setErrorMessage('Delete employee process failed.');
          }
      });
      }
    });
  }

  resendEmail(){
    this.router.navigate(['manage/onboarding/', 'add-email-template', this.employee.clientId, this.employee.employeeId, 3, 'add']);
  }

  payrollReady() {
      if (this.completedPct == 100 && this.employee.essActivated == null) {
          this.dashboardService.putEmployeeOnboardingRemove(this.employee.employeeId)
              .subscribe( () => {
                  this.employee.essActivated = new Date();
                  this.employee.isSelfOnboardingComplete = true;
                  this.calculateAdminPct();
              });
      } else if (this.completedPct == 100 && this.employee.essActivated != null) {
          this.dashboardService.putRollbackPayrollReady(this.employee.employeeId)
              .subscribe( () => {
                  this.employee.essActivated = null;
                  this.employee.isSelfOnboardingComplete = false;
                  this.calculateAdminPct();
              });
      }
  }

  completeOnboarding() {
    let eo = this.employee;
      if (this.completedPct == 100 && eo.adminPctComplete == 100 && !eo.isOnboardingCompleted) {
        this.dashboardService.completeOnboarding(eo).subscribe( () => {
              eo.isOnboardingCompleted = true;
              this.msgSvc.setSuccessMessage('The employee has completed onboarding.');
          });
      } else if ((this.completedPct < 100 || eo.adminPctComplete < 100) && !eo.isOnboardingCompleted) {
        let config = new MatDialogConfig<any>();
        config.width = '500px';
        config.data = { separationDate: this.employee.separationDate, employeeStatusId: this.employee.employeeStatus };

        return this.dialog
          .open<CloseOnboardingDialogComponent>(
            CloseOnboardingDialogComponent,
            config
          )
          .afterClosed()
          .pipe(switchMap(x => {
            if(x){
              eo.employeeStatus = x.employeeStatusId;
              eo.separationDate = x.separationDate; 
              return this.dashboardService.completeOnboarding(eo);
            }
            else
              return EMPTY;
          }))
          .subscribe((result: any) => {
            if(result){
              this.employee.isOnboardingCompleted = true;
              this.msgSvc.setSuccessMessage('The employee has completed onboarding.');
            }
          });
      }
  }

  displayI9Modal(){
    if (this.employee.isFinalized && !this.employee.isI9AdminComplete) {
      let config = new MatDialogConfig<any>();
        config.width = "700px";
        config.data = { user     : this.user, 
                        employee : this.employee ,
                        documents : this.documents ,
                        countries : this.countries ,
                        states  : this.states};

        
        return this.dialog.open<CertifyI9DialogComponent, any, string>(CertifyI9DialogComponent, config)
        .afterClosed()
        .subscribe( statusChanged => {
          if(statusChanged){
            this.employee.isI9AdminComplete = true;
            this.calculateAdminPct();
          }
        });
    }
  }

  editTask(task:IOnboardingAdminTask, inx: number){
    task.isEditing = true;
    let taskRow = (<FormGroup>(this.tasksFormArray).at(inx));
  }
  addTask(){
    let newTask:IOnboardingAdminTask = <IOnboardingAdminTask>{
      employeeOnboardingTaskId:0,
      employeeId: this.employee.employeeId,
      description:'',
      dueDate:null, 
      isEditing:true,
      isComplete:false
    };
    this.employee.taskList = this.employee.taskList || [];
    this.employee.taskList.push(newTask);
    this.tasksFormArray.push(this.createAdminTaskForm(newTask));
  }

  deleteTask(task:IOnboardingAdminTask, inx: number){
    const options = {
      title: `Are you sure you want to delete "${task.description}" admin task?`,
      message: "",
      confirm: "Yes, Remove",
    };
    this.confirmDialog.open(options);
    this.confirmDialog.confirmed().pipe(switchMap(confirmed => {
      if (confirmed) {
        return this.dashboardService.deleteEmployeeOnboardingAdminTask(task);
      } else return EMPTY;
    })).subscribe(x => {
      task.isEditing = false;
      this.tasksFormArray.removeAt(inx);
      this.employee.taskList.splice(inx,1);
    }, err => {
      if (err.error && err.error.errors && err.error.errors.length) 
        this.msgSvc.setErrorMessage(err.error.errors[0].msg);
      else 
        this.msgSvc.setErrorMessage(err.message );
    });
  }

  saveTask(task:IOnboardingAdminTask, inx: number){
    let taskRow = (<FormGroup>(this.tasksFormArray).at(inx));

    if(taskRow.valid){
      task.isComplete = taskRow.controls["chkComplete"].value;
      task.completionStatus = task.isComplete ? CompletionStatusType.Done : CompletionStatusType.InProgress;
      task.description = taskRow.controls["inpDesc"].value;
      task.dueDate = taskRow.controls["inpDueDate"].value;

      this.dashboardService.updateEmployeeOnboardingAdminTask(task)
      .subscribe(x => {
        task.isEditing = false;
        if(!task.employeeOnboardingTaskId){
          task.employeeOnboardingTaskId = x.employeeOnboardingTaskId;
          task.taskId = x.taskId;
        }
      }, err => {
        if (err.error && err.error.errors && err.error.errors.length) 
          this.msgSvc.setErrorMessage(err.error.errors[0].msg);
        else 
          this.msgSvc.setErrorMessage(err.message );
      });
    }
  }

  cancelEditTask(task:IOnboardingAdminTask, inx:number){
    task.isEditing = false;
    let taskRow = (<FormGroup>(this.tasksFormArray).at(inx));
    taskRow.reset();
    if(!task.employeeOnboardingTaskId){
      this.tasksFormArray.removeAt(inx);
      this.employee.taskList.splice(inx,1);
    } else {
      this.updateAdminTaskForm(task, inx);
    }
  }

  updateAdminTaskStatus(task:IOnboardingAdminTask, inx:number){
    this.saveTask(task, inx);
    this.calculateAdminPct();
  }

  importTaskList(){
    let config = new MatDialogConfig<any>();
        config.width = '500px';
        config.data = this.employee.clientId;

    this.dialog
      .open<ImportAdminTasklistDialogComponent>(
        ImportAdminTasklistDialogComponent,
        config
      )
      .afterClosed()
      .pipe(switchMap(tl => {
        if(tl)
          return this.dashboardService.importTaskLists(tl, this.employee.employeeId);
        else
          return EMPTY;
      }))
      .subscribe((tlresult: any) => {
        if(tlresult.data){
          this.employee.taskList = tlresult.data;
          this.buildTaskListForm();
          this.calculateAdminPct();
          this.msgSvc.setSuccessMessage('Admin task list imported successfully.');
        }
      });
  }

  calculateAdminPct() {
    let totalAdminItems = 0;
    let totalCompletedAdminItems = 0;

    totalAdminItems++;
    if (this.employee.isSelfOnboardingComplete) {
        totalCompletedAdminItems++;
    }

    if (this.employee.isI9Required) {
        totalAdminItems++;
        if (this.employee.isI9AdminComplete) {
            totalCompletedAdminItems++;
        }
    }

    const totalTasks = this.employee.taskList.length;
    totalAdminItems += totalTasks;

    const completedTasks = this.employee.taskList.filter( x => x.isComplete).length;

    totalCompletedAdminItems += completedTasks;

    if (totalAdminItems == 0) {
        this.employee.adminPctComplete = 100;
    } else {
        this.employee.adminPctComplete = Math.round((100 * totalCompletedAdminItems) / totalAdminItems);
    }
  }

  buildTaskListForm(){
    let taskArr:FormArray = this.fb.array([]);
    this.employee.taskList.forEach(x => {
      taskArr.push(this.createAdminTaskForm(x));
    });
    this.form = this.fb.group({
      tasks:taskArr
    });
  }

  createAdminTaskForm( x: IOnboardingAdminTask ): FormGroup {
    return this.fb.group({
      chkComplete: this.fb.control(x.isComplete,[]),
      inpDueDate: this.fb.control(x.dueDate, []),
      inpDesc: this.fb.control(x.description, [Validators.required]),
    });
  }
  updateAdminTaskForm( x: IOnboardingAdminTask, inx: number ): void {
    this.tasksFormArray.at(inx).reset();
    this.tasksFormArray.at(inx).patchValue({
      chkComplete: x.isComplete,
      inpDueDate: x.dueDate,
      inpDesc: x.description,
    });
  }

  editWorkflow() {
    const returnRoute = 'returns-to-detail'; // 'company.onboarding.detail';
    //dsState.router.go('company.onboarding.workflow', { employeeId: $scope.employee.employeeId, returnRoute: returnRoute });
    this.router.navigate(['manage/onboarding/', 'add-workflow', this.employee.clientId, this.employeeId, 2, 'edit']);
  }

  downloadResource(resource: IEmployeeAttachment) {
    let api:Observable<Blob> = null;
    if(resource.sourceType === 2 && resource.isATFile)
    {
        let encodedCss: string = document.querySelector("link[id$='StyleSheetMain']").attributes["href"].value;
        
        encodedCss = window.btoa( this.absoluteUrl( encodedCss) ).replace(/\//gi, '_');
        let url = resource.source.replace(/\{cssUrl\}/g,encodedCss);

        api = this.attachmentService.getUrlToDownload( 'api/' + url, resource.name + resource.extension);
    }
    else if (resource.sourceType == 2 || resource.sourceType == 4) {
        window.open(resource.source, '_blank', '');
        return;
    } else {
      api = this.attachmentService.getFileResourceToDownload(resource);
    }

    api.subscribe( x => { } , 
      (err: HttpErrorResponse) => {
        if (err.error && err.error.errors && err.error.errors.length) 
          this.msgSvc.setErrorMessage(err.error.errors[0].msg);
        else 
          this.msgSvc.setErrorMessage(err.message );
      } ); 
  };

  absoluteUrl(url: string){
    return location.protocol + '//' + location.host + url
  }

  addAttacment(currResource:IEmployeeAttachment):void{
    var currentResource: IEmployeeAttachment = <IEmployeeAttachment>{};

    if (!currResource) {
      var currentFolderId = this.onboardingFolders[0].employeeFolderId;

      currentResource = <IEmployeeAttachment>{
        folderId: currentFolderId,
        employeeId: this.employee.employeeId,
        resourceId: 0,
        sourceType: 1,
        name: '',
        isNew: true,
        extension: '',
        
        isViewableByEmployee: true,
        clientId: this.employee.clientId,
        isAzure: false,
        isATFile: false,
        isCompanyAttachment: false,
        azureAccount: 0,
        cssClass: '',
        source: '',
        
        onboardingWorkflowTaskId: 0,
        
        addedDate: new Date(),
        addedByUsername: this.user.userName,
        hovered: false,
      };
    } else {
      currentResource = Object.assign({}, currResource);
    }
    let config = new MatDialogConfig<any>();
    config.width = '500px';
    config.data = { allFolders: this.onboardingFolders, allAttachments: this.resources, attachment: currentResource };

    this.dialog
      .open<AddAttachmentDialogComponent>(
        AddAttachmentDialogComponent,
        config
      )
      .afterClosed()
      .subscribe((attachment: IEmployeeAttachment) => {
        if (attachment) {
          var resourceInx = this.resources.findIndex(x=> x.resourceId == attachment.resourceId);
          if(resourceInx>-1)
              this.resources[resourceInx] = attachment;
          else{
              this.resources.push(attachment);
          }
                
          if (currentResource.resourceId == 0) {
            this.msgSvc.setSuccessMessage('Attachment added successfully.');
          } else {
            this.msgSvc.setSuccessMessage('Attachment updated successfully.');
          }
        }
      });
  }
  summarize():void{
    let config = new MatDialogConfig<any>();
    config.width = '700px';
    config.data = { employee: this.employee, resources: this.resources };

    this.dialog
      .open<OnboardingSummaryDialogComponent>(
        OnboardingSummaryDialogComponent,
        config
      )
      .afterClosed()
      .subscribe((x) => {
      });
  }
}
