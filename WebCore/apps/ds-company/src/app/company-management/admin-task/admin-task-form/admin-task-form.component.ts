import { Component, Inject, OnInit,  ElementRef, Input, Output, EventEmitter, ViewChild} from "@angular/core";
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';

import { UserInfo } from '@ds/core/shared/user-info.model';
import { OnboardingAdminService } from '../../shared/onboarding-admin.service';
import { tap, filter } from 'rxjs/operators';
import { IOnboardingAdminTaskList, IOnboardingAdminTask, IJobProfileDetail, Modification } from '../../shared/onboarding-admin-task-list.model';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { MatSidenav } from "@angular/material/sidenav/sidenav";

@Component({
    selector: 'ds-admin-task-form',
    templateUrl: './admin-task-form.component.html',
    styleUrls: ['./admin-task-form.component.scss']
  })
  export class AdminTaskFormComponent implements OnInit {
      
    @Input() user: UserInfo;
    @Input() taskList: IOnboardingAdminTaskList;
    @Input() isModal: boolean;
    @Input() pageSubmitted: boolean;
    @Input() jobProfiles: Array<IJobProfileDetail>;

    @Output() statusChange = new EventEmitter();

    submitted: boolean;
    filteredJobProfiles: Array<IJobProfileDetail>;
    selectedJobProfile:IJobProfileDetail;

    taskForm: FormGroup;
    get taskDesc(): FormControl {
        return this.taskForm.get('taskName') as FormControl
    }

    jpForm: FormGroup;
    get profileSearchInput(): FormControl {
        return this.jpForm.get('profileName') as FormControl
    }

    listNameForm: FormGroup;
    get listDesc(): FormControl {
        return this.listNameForm.get('listName') as FormControl
    }


    constructor(
        private obEmpService: OnboardingAdminService, 
        private fb: FormBuilder,
        private dialog:ConfirmDialogService, 
    ) {
        
    }
    
    

    ngOnInit() {
        this.listNameForm = new FormGroup({
            listName: this.fb.control('', {updateOn: 'blur', validators: Validators.required } ),
          });
        this.taskForm = new FormGroup({
            taskName: this.fb.control('', [Validators.required]),
          });
        this.jpForm = new FormGroup({
            profileName: this.fb.control('', [Validators.required]),
          });

        if(this.taskList){
            this.listDesc.setValue(this.taskList.name);
        } else {
            this.obEmpService.activeTaskList.subscribe( tl => {
                if(tl){
                    this.taskList = <IOnboardingAdminTaskList>tl;
                    this.listDesc.setValue(this.taskList.name);
                    this._isAddingTask = false;
                    this._isAddingJobProfile = false;
                }
            });
        }

        this.filteredJobProfiles = [];
        this.profileSearchInput.valueChanges.pipe(tap(x => {
            if(x){
                if(typeof x === 'object') 
                    this.selectedJobProfile = x;
                else
                    this.filteredJobProfiles = this.jobProfiles
                        .filter(l => !l.onboardingAdminTaskListId )
                        .filter(m => this.taskList.jobProfiles.map(k=>k.jobProfileId).indexOf(m.jobProfileId) == -1 )
                        .filter(n => n.description.toLowerCase().indexOf(x.toLowerCase())>-1 );
            } else {
                this.filteredJobProfiles = this.jobProfiles
                    .filter(l => !l.onboardingAdminTaskListId )
                    .filter(m => this.taskList.jobProfiles.map(k=>k.jobProfileId).indexOf(m.jobProfileId) == -1 );
            }
        })).subscribe();

        this.listDesc.valueChanges.pipe(
        filter(input => this.taskList.name != input && !!input),
        tap(x=>{
            this.taskList.name = x;
            this.taskList.isDirty = true;
            this.statusChange.emit(Modification.onlyName);
        })).subscribe();

    }

    //#region "Task Add/Edit"
    public _isAddingTask:boolean;
    public addTask(){
        this._isAddingTask = true;
        this.taskList.onboardingAdminTasks.forEach(x=>x.isEditing = false);
        var count = this.taskList.onboardingAdminTasks.length;
        this.taskList.onboardingAdminTasks.push(<IOnboardingAdminTask>{
            onboardingAdminTaskListId : this.taskList.onboardingAdminTaskListId,
            onboardingAdminTaskId: -(count+1),
            isEditing: true,
        })
        
        this.taskForm.reset();
        this.focusTaskDesc();
    }

    
    private resetTaskEditing(){
        let draftItem = this.taskList.onboardingAdminTasks.find(x=>x.isEditing);
        if(draftItem){
            if(this._isAddingTask){
                this.taskList.onboardingAdminTasks.splice(this.taskList.onboardingAdminTasks.length-1,1);
                this._isAddingTask = false;
            }
        }
        this.taskList.onboardingAdminTasks.forEach(x=>x.isEditing = false);
        this.taskForm.reset();
        this.submitted = false;
    }

    
    public editTask(task:IOnboardingAdminTask, inx: number){
        this.resetTaskEditing();
        this.taskList.onboardingAdminTasks[inx].isEditing = true;
        this.taskDesc.setValue( this.taskList.onboardingAdminTasks[inx].description );
        this.focusTaskDesc();
    }

    public removeTask(task:IOnboardingAdminTask, inx: number){
        const options = {
            title: 'Are you sure you want to remove this task?',
            confirm: "Remove",
            width: "300px",
        };
        this.dialog.open(options);

        this.dialog.confirmed().pipe(
        filter(ok => !!ok),
        tap(x => {
            // Removing....
            this.resetTaskEditing();
            this.taskList.onboardingAdminTasks.splice(inx,1);
            this.taskList.isDirty = true;
        })).subscribe();
    }

    public updateTask(task:IOnboardingAdminTask, inx: number){
        this.submitted = true;
        if(this.taskForm.valid){
            task.description = this.taskDesc.value;
            
            if(this._isAddingTask) this._isAddingTask = false;
            this.taskList.onboardingAdminTasks.forEach(x=>x.isEditing = false);
            this.taskForm.reset();
            this.submitted = false;
            this.taskList.isDirty = true;
            this.statusChange.emit(Modification.onlyTask);
        }
    }

    public clearTask(task:IOnboardingAdminTask, inx: number){
        this.resetTaskEditing();
    }
    //#endregion

    //#region "JobProfile Add/Edit"
    public _isAddingJobProfile:boolean;
    public addJobProfile(){
        this._isAddingJobProfile = true;
        this.jpForm.reset();
        this.focusProfileName();
    }

    public assignJobProfile(){
        this.selectedJobProfile.onboardingAdminTaskListId = this.taskList.onboardingAdminTaskListId;
        let k = Object.assign({}, this.selectedJobProfile);
        this.taskList.jobProfiles.push(k);
        this._isAddingJobProfile = false;        
        this.selectedJobProfile = null;
        this.taskList.isDirty = true;
    }

    public removeJobProfile(jp:IJobProfileDetail, inx: number){
        const options = {
            title: 'Are you sure you want to remove this job profile from the list?',
            confirm: "Remove",
            width: "300px",
        };
        this.dialog.open(options);

        this.dialog.confirmed().pipe(
        filter(ok => !!ok),
        tap(x => {
            // Removing....
            
            let removableId = this.taskList.jobProfiles[inx].jobProfileId;
            this.taskList.jobProfiles.splice(inx,1);
            let removable = this.jobProfiles.find(x=>x.jobProfileId == removableId);
            if(removable){
                removable.onboardingAdminTaskListId = null;
            }
            this.taskList.isDirty = true;
        })).subscribe();
    }

    public clearJobProfile( ){
        this.selectedJobProfile = null;
        this._isAddingJobProfile = false;
    }
    public displayFn(jp?: IJobProfileDetail): string | undefined {
        return jp ? jp.description : undefined;
    }
    //#endregion

    focusTaskDesc(){
        setTimeout(()=>{
            let elementRef = document.getElementById('taskNameCtrl');
            if(elementRef) elementRef.focus();

        },300)
    }
    focusProfileName(){
        setTimeout(()=>{
            let elementRef = document.getElementById('profileNameCtrl');
            if(elementRef) elementRef.focus();

        },300)
    }

    clearDrawer() {
        this.statusChange.emit(0);
    }

    public dismiss(){
        document.getElementById('btnCancelI9').click();
    }
}