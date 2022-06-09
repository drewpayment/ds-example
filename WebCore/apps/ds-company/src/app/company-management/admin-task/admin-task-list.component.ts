import { Component, OnInit, Inject } from '@angular/core';
import { OnboardingAdminService } from '../shared/onboarding-admin.service';
import { AccountService } from '@ds/core/account.service';
import { DOCUMENT } from '@angular/common';
import { Observable, iif, of, EMPTY } from 'rxjs';
import { tap, switchMap } from 'rxjs/operators';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { HttpErrorResponse } from '@angular/common/http';
import { ConfirmDialogService } from "@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service";
import { Client } from '@ds/core/employee-services/models';
import { IOnboardingAdminTaskList, IJobProfileDetail, Modification } from '../shared/onboarding-admin-task-list.model';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';
import { changeDrawerHeightOnOpen, matDrawerAfterHeightChange } from '@ds/core/ui/animations/drawer-auto-height-animation';
import { xor } from 'lodash';

@Component({
  selector: 'ds-admin-task-list',
  templateUrl: './admin-task-list.component.html',
  styleUrls: ['./admin-task-list.component.scss'],
  animations: [
    changeDrawerHeightOnOpen,
    matDrawerAfterHeightChange
  ]
})
export class AdminTaskListComponent implements OnInit {
    
    hasError = false;
    showBreadCrumb = false;
    onboardingUrl = ""; 

    isOnboardingCompleted: boolean;
    relatedClients: Array<Client> = [];

    isLoading: boolean = true;
    showFiles: boolean = false;
    allTaskList:Array<IOnboardingAdminTaskList>;
    activeTaskList:IOnboardingAdminTaskList = null;
    activeTaskListId:number = 0;
    taskListKlicked:IOnboardingAdminTaskList = null;
    submitted:boolean;

    userinfo: UserInfo;
    totalCount: number = 0;
    jobProfiles: Array<IJobProfileDetail> = [];

    constructor(private accountService: AccountService,
        private onboardingAdminService: OnboardingAdminService, 
        private msg: NgxMessageService,
        private confirmSvc:ConfirmDialogService,
        @Inject(DOCUMENT) private document: Document) {
        
    }
    ngOnInit() {
        this.isLoading = true;
        this.showFiles = false;
        
        this.allTaskList = [];
        this.activeTaskList = null;
        this.activeTaskListId = 0;

        this.isLoading = true;

        this.checkCurrentUser().pipe(
            switchMap(userInfo => this.onboardingAdminService.getOnboardingAdminTaskList(userInfo.lastClientId || userInfo.clientId) ),
            tap( x => {
                this.allTaskList = x;
                this.isLoading = false; 

                let parentAnchor = (document.querySelector('mat-sidenav a[href$="/Onboarding"]') as HTMLAnchorElement);
                this.onboardingUrl = '/onboarding';
            })
        ).subscribe();

        this.checkCurrentUser().pipe(
            switchMap(userInfo => this.onboardingAdminService.getJobProfiles(userInfo.lastClientId || userInfo.clientId) ),
            tap( x => {
                this.jobProfiles   = x;
            })
        ).subscribe();

        this.accountService.canPerformActions('Onboarding.OnboardingAdministrator').subscribe( canPerform => {
            if( canPerform )
                this.showBreadCrumb = true;
        }, (error: HttpErrorResponse) => {
            this.showBreadCrumb = false;
        });
    }

    checkCurrentUser(): Observable<UserInfo>{
        return iif(() => this.userinfo == null, 
            this.accountService.getUserInfo().pipe(tap(u => {
                this.userinfo = u;
            })),
            of(this.userinfo));
    }
    getNext(){
        let minId = Math.min(...this.allTaskList.map(x=>x.onboardingAdminTaskListId));
        if(!minId || minId>0) minId = 0;
        minId = (minId-1);
        var newName = "New " + (-minId);
        while(this.allTaskList.find(x=>x.name == newName) )
            newName = "New " + newName;
        return {id:minId,name:newName};
    }
    addTaskList(){
        //this.clearDrawer();
        var count = this.allTaskList.length;
        var next = this.getNext();

        let newTaskList = <IOnboardingAdminTaskList>{
            onboardingAdminTaskListId : next.id,  
            onboardingAdminTasks:[],
            jobProfiles:[],
            name : next.name,
            isDirty:true,
            clientId: this.userinfo.lastClientId || this.userinfo.clientId,
         };
        this.allTaskList.push(newTaskList);
        this.toggleActiveTaskList(next.id);
    }

    toggleActiveTaskList(listId: number) {
        if (this.activeTaskListId == listId) {
          this.clearDrawer();
        } else {
          this.activeTaskListId = listId;
          this.activeTaskList = this.allTaskList.find(x => {
            return (x.onboardingAdminTaskListId == this.activeTaskListId);
          });
         this.onboardingAdminService.setActiveTaskList(this.activeTaskList);
        }    
    }
    
    clearDrawer() {
        this.activeTaskListId = 0;
        this.activeTaskList = null;
    }

    refresh(eventType: Modification){
        if(eventType == Modification.none)
            this.clearDrawer();
        else
            this.save(eventType);
    }

    deleteTLDialog(tl:IOnboardingAdminTaskList){
        const options = {
            title: `Are you sure you want to delete ${tl.name} task list?`,
            message: "",
            confirm: "Delete",
          };
    
        this.confirmSvc.open(options);
        this.confirmSvc.confirmed()
        .pipe(
        switchMap((confirmed) => {
            if (confirmed) {
                if(tl.onboardingAdminTaskListId < 0 ) 
                return of(true);
                else 
                return this.onboardingAdminService.deleteActiveTaskList(tl);
            } else {
            return EMPTY;
            }
        })
        )
        .subscribe(done => {
            if(done) {
                // remove from the list
                var inx = this.allTaskList.findIndex(
                    (x) => x.onboardingAdminTaskListId == tl.onboardingAdminTaskListId
                );
                this.allTaskList.splice(inx, 1);
    
                // unmap job profiles
                let removables = this.jobProfiles.filter(x=>x.onboardingAdminTaskListId == tl.onboardingAdminTaskListId);
                if(removables && removables.length){
                    removables.forEach(x=>x.onboardingAdminTaskListId = null);
                }
    
                this.msg.setSuccessMessage('Task list deleted successfully.');
                if(this.allTaskList.length){
                    this.clearDrawer();
                    //this.toggleActiveTaskList(this.allTaskList[this.allTaskList.length-1].onboardingAdminTaskListId);
                }
            }}, (error: HttpErrorResponse) => {
                this.msg.setErrorResponse(error);
            });
    }

    save(eventType: Modification){
        if(this.activeTaskList && !this.activeTaskList.name){
            this.submitted = true;
            return;
        }

        let modifiedTaskLists = this.allTaskList.filter(x=>x.isDirty);
        if(modifiedTaskLists.length == 0){
            this.msg.setSuccessMessage("Task lists saved successfully.", 5000);
        }
        else{
            this.onboardingAdminService.putMultipleAdminTaskList(modifiedTaskLists).pipe(
                tap(savedList => {
                    modifiedTaskLists.forEach(x=> {
                        let inx = this.allTaskList.findIndex(y => y.onboardingAdminTaskListId == x.onboardingAdminTaskListId);
                        this.allTaskList[inx] = savedList.find(y => y.name == this.allTaskList[inx].name);
                    });
                    
                    if(eventType == Modification.all){
                        this.msg.setSuccessMessage("Task lists saved successfully.", 5000);
                    }
                    else if(eventType == Modification.onlyName){
                        this.msg.setSuccessMessage("Task list name saved successfully.", 5000);                        
                    }
                    else if(eventType == Modification.onlyTask){
                        this.msg.setSuccessMessage("Task saved successfully.", 5000);
                    }
                    
                    this.activeTaskList = this.allTaskList.find(x => {
                        return (x.name == this.activeTaskList.name);
                        });

                    this.activeTaskListId = this.activeTaskList.onboardingAdminTaskListId;
                    if (this.activeTaskListId > 0) this.onboardingAdminService.setActiveTaskList(this.activeTaskList);
                })
            ).subscribe( x=> {}, (error: HttpErrorResponse) => {
                this.msg.setErrorResponse(error);
            });
        }
        this.submitted = false;
    }
}