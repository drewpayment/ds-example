import { Component, OnInit, Inject, AfterViewInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

import * as _ from 'lodash';
import { UserInfo } from "@ds/core/shared";
import { IEmergencyContact } from '../../shared/emergency-contact.model';
import { EmployeeProfileService } from '../../shared/employee-profile-api.service';
import { ICountry } from '../../../common/shared/country.model';
import { IState } from '../../../common/shared/state.model';
import { EmployeeCommonService } from '../../../common/shared/employee-common-api.service';
import { AccountService } from "@ds/core/account.service";

interface DialogData {
    user: UserInfo,
    emergencyContact: IEmergencyContact,
    hasEditPermissions: boolean
}


@Component({
  selector: 'ds-emergency-contact-form',
  templateUrl: './emergency-contact-form.component.html',
  styleUrls: ['./emergency-contact-form.component.scss']
})
export class EmergencyContactFormComponent implements OnInit, AfterViewInit {
    user: UserInfo;
    emergencyContact: IEmergencyContact;
    hasEditPermissions: boolean;
    countries: ICountry[];
    states: IState[];
    f: FormGroup;
    formSubmitted: boolean;
    pageTitle: string;
    private _editMode: boolean;

    constructor(
        public dialogRef: MatDialogRef<EmergencyContactFormComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
        private fb: FormBuilder,
        private accountService: AccountService,
        private employeeProfileService: EmployeeProfileService,
        private commonService: EmployeeCommonService
    ) { }

    ngOnInit() {
        this.countries = [];
        this.states = [];

        this.user = this.data.user;
        this.hasEditPermissions = this.data.hasEditPermissions;
        if (!this.user)
            this.accountService.getUserInfo().subscribe(u => {
                this.user = u;
                this.initializePage();
            });
        else
            this.initializePage();
    }

    private initializePage() {
        this.emergencyContact = _.cloneDeep(this.data.emergencyContact) || this.createEmptyEmergencyContact();

        this.pageTitle = (this.emergencyContact != null && this.emergencyContact.employeeEmergencyContactId > 0) ? `Edit Emergency Contact` : `Add Emergency Contact`;

        this.createForm();
    }

    loadStates(e) {
        this.f.value.state = "";
        this.states = [];
        if (e.target.value) {
            this.commonService.getStatesByCountryId(e.target.value).subscribe(states => {
                this.states = states;
            });
        }
    }

    private createEmptyEmergencyContact(): IEmergencyContact {
        return {
            employeeEmergencyContactId: null,
            employeeId: null,
            clientId: null,
            homePhoneNumber: null,
            cellPhoneNumber: null,
            relation: null,
            emailAddress: null,
            insertApproved: 0,
            firstName: null,
            lastName: null
        }
    }

    private createForm(): void {
        this.f = this.fb.group({
            firstName: this.fb.control(this.emergencyContact.firstName || '', [Validators.required, Validators.maxLength(25)]),
            lastName: this.fb.control(this.emergencyContact.lastName || '', [Validators.required, Validators.maxLength(25)]),
            relationship: this.fb.control(this.emergencyContact.relation || '', [Validators.required, Validators.maxLength(20)]),
            homePhone: this.fb.control(this.emergencyContact.homePhoneNumber || '', [Validators.required, Validators.pattern("^\\d{3}-\\d{3}-\\d{4}$")]),
            cellPhone: this.fb.control(this.emergencyContact.cellPhoneNumber || '', [Validators.pattern("^\\d{3}-\\d{3}-\\d{4}$")]),
            emailAddress: this.fb.control(this.emergencyContact.emailAddress || '', [Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")]),
        });
    }

    private prepareModel(): IEmergencyContact {
        return {
            insertApproved: this.emergencyContact.insertApproved,
            employeeEmergencyContactId: this.emergencyContact != null ? this.emergencyContact.employeeEmergencyContactId : null,
            clientId: this.emergencyContact.clientId,
            firstName: this.f.value.firstName,
            lastName: this.f.value.lastName,
            employeeId: this.emergencyContact.employeeId,
            relation: this.f.value.relationship,
            homePhoneNumber: this.f.value.homePhone,
            cellPhoneNumber: this.f.value.cellPhone,
            emailAddress: this.f.value.emailAddress,
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

    ngAfterViewInit() {

    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    saveEmergencyContact(): void {
        this.formSubmitted = true;
        this.f.updateValueAndValidity();
        if (this.f.invalid) return;

        const dto = this.prepareModel();
        this.dialogRef.close(dto);
    }
}
