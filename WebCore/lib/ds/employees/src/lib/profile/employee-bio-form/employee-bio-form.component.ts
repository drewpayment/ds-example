import { Component, OnInit, Inject, AfterViewInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

import * as _ from 'lodash';
import { UserInfo } from "@ds/core/shared";
import { IEmployeePersonalInfo } from '../shared/employee-personal-info.model';
import { EmployeeProfileService } from '../shared/employee-profile-api.service';
import { AccountService } from "@ds/core/account.service";

interface DialogData {
    user: UserInfo,
    employeePersonalInfo: IEmployeePersonalInfo
}

@Component({
  selector: 'ds-employee-bio-form',
  templateUrl: './employee-bio-form.component.html',
  styleUrls: ['./employee-bio-form.component.scss']
})
export class EmployeeBioFormComponent implements OnInit, AfterViewInit {
    user: UserInfo;
    employeePersonalInfo: IEmployeePersonalInfo;
    f: FormGroup;
    formSubmitted: boolean;
    pageTitle: string;
    private _editMode: boolean;

    constructor(
        public dialogRef: MatDialogRef<EmployeeBioFormComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
        private fb: FormBuilder,
        private accountService: AccountService,
        private employeeProfileService: EmployeeProfileService
    ) {

    }

    ngOnInit(): void {
        this.user = this.data.user;
        if (!this.user)
            this.accountService.getUserInfo().subscribe(u => {
                this.user = u;
                this.initializePage();
            });
        else
            this.initializePage();
    }

    private initializePage() {
        this.employeePersonalInfo = _.cloneDeep(this.data.employeePersonalInfo) || this.createEmptyEmployeePersonalInfo();
        this.pageTitle = "Edit About Me";

        this.createForm();
    }

    ngAfterViewInit() {

    }

    saveEmployeeBio(): void {
        this.formSubmitted = true;
        this.f.updateValueAndValidity();
        if (this.f.invalid) return;

        const dto = this.prepareModel();
        this.dialogRef.close(dto);
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    private createEmptyEmployeePersonalInfo(): IEmployeePersonalInfo {
        return {
            employeeId: null,
            bio: null,
            modified: null,
            modifiedBy: null
        }
    }

    private createForm(): void {
        this.f = this.fb.group({
            bio: this.fb.control(this.employeePersonalInfo.bio || '', [Validators.required]),
        });
    }

    private prepareModel(): IEmployeePersonalInfo {
        return {
            employeeId: this.employeePersonalInfo.employeeId,
            bio: this.f.value.bio,
            modified: new Date(),
            modifiedBy: this.user.employeeId
        };
    }

    getFormControlError(field: string, errorCodes: string[]): boolean {
        const control = this.f.get(field);
        let flag: boolean = false;
        _.forEach(errorCodes, (errorCode) => {
            flag = control.hasError(errorCode) && (control.touched || this.formSubmitted);
            if (flag === true)
                return false;
        });
        return flag;
    }

}
