import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormControl, FormGroup, Validators, FormBuilder,  } from '@angular/forms';
import { IOnboardingAdminTaskList } from '../../../../company-management/shared/onboarding-admin-task-list.model';
import { OnboardingAdminService } from '../../../../company-management/shared/onboarding-admin.service';

@Component({
    selector: 'ds-import-admin-tasklist-dialog',
    templateUrl: './import-admin-tasklist-dialog.component.html',
    styleUrls: ['./import-admin-tasklist-dialog.component.scss']
  })
  export class ImportAdminTasklistDialogComponent implements OnInit {
    form1: FormGroup;
    formSubmitted: boolean;
    allAdminTaskList: Array<IOnboardingAdminTaskList> = [];
    clientId: number;
    isLoading:boolean;
    noData:boolean = false;

    constructor(
        private onboardingSvc: OnboardingAdminService,
        private ref:MatDialogRef<ImportAdminTasklistDialogComponent>,
        private fb: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data:number,
    ) {
        
        this.clientId = this.data;
    }
    

    ngOnInit() {
        /// Build the form here
        this.form1 = this.fb.group({
            taskList:   [null , Validators.required],
        });

        this.isLoading = true;
        this.onboardingSvc.getOnboardingAdminTaskList(this.clientId)
        .subscribe((data) => {
            this.allAdminTaskList = data;
            this.isLoading = false;
            this.noData = !this.allAdminTaskList || this.allAdminTaskList.length == 0;
        });
    }
    clear() {
        this.ref.close(null);
    }
    import() {
        this.formSubmitted = true;
        if(this.form1.valid){
            let tlId = this.form1.value.taskList;
            this.ref.close(this.allAdminTaskList.find(x=>x.onboardingAdminTaskListId == tlId));
        }
    }
}