import { Component, OnInit, Inject, AfterViewInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormBuilder, FormGroup, Validators, AbstractControl } from "@angular/forms";

import * as _ from 'lodash';
import * as moment from 'moment';
import { UserInfo } from "@ds/core/shared";
import { IEmployeeDependent, IEmployeeDependentRelationship } from '@ds/employees/profile/shared/employee-dependent.model';
import { EmployeeProfileService } from '../shared/employee-profile-api.service';
import { AccountService } from "@ds/core/account.service";
import { calculateBirthdateWithText } from '@util/dateUtilities';

interface DialogData {
    user: UserInfo,
    employeeDependent: IEmployeeDependent,
    hasEditPermissions: boolean
}

@Component({
  selector: 'ds-employee-dependents-form',
  templateUrl: './employee-dependents-form.component.html',
  styleUrls: ['./employee-dependents-form.component.scss']
})
export class EmployeeDependentsFormComponent implements OnInit, AfterViewInit {
    user: UserInfo;
    employeeDependent: IEmployeeDependent;
    hasEditPermissions: boolean;
    age: string;
    f: FormGroup;
    formSubmitted: boolean;
    pageTitle: string;
    private _editMode: boolean;
    employeeDependentRelationshipsList: IEmployeeDependentRelationship[];
    isConfirmSSNValid: boolean;

    constructor(
        public dialogRef: MatDialogRef<EmployeeDependentsFormComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
        private fb: FormBuilder,
        private accountService: AccountService,
        private employeeProfileService: EmployeeProfileService
    ) { }

    ngOnInit(): void {
        this.user = this.data.user;
        this.hasEditPermissions = this.data.hasEditPermissions;
        this.isConfirmSSNValid = true;
        if (!this.user)
            this.accountService.getUserInfo().subscribe(u => {
                this.user = u;
                this.initializePage();
            });
        else
            this.initializePage();
    }

    private initializePage() {
        this.employeeProfileService.getRelationshipList().subscribe( x=> {
            this.employeeDependentRelationshipsList = x;
            this.patchRelationShipValue();
        });

        this.employeeDependent = _.cloneDeep(this.data.employeeDependent) || this.createEmptyEmployeeDependent();
        this.employeeDependent.unmaskedConfirmSocialSecurityNumber=this.employeeDependent.unmaskedSocialSecurityNumber;
        this.pageTitle = (this.employeeDependent != null && this.employeeDependent.employeeDependentId > 0) ? `Edit Dependent` : `Add Dependent`;

        this.createForm();
        this.calculateAge();
    }

    private createEmptyEmployeeDependent(): IEmployeeDependent {
        return {
            employeeDependentId: null,
            clientId: null,
            firstName: null,
            middleInitial: null,
            lastName: null,
            employeeId: null,
            unmaskedSocialSecurityNumber: null,
            maskedSocialSecurityNumber: null,
            unmaskedConfirmSocialSecurityNumber: null,
            relationship: null,
            gender: null,
            comments: null,
            birthDate: null,
            insertStatus: null,
            lastModifiedByDescription: null,
            lastModifiedDate: null,
            isAStudent: null,
            hasADisability: null,
            tobaccoUser: null,
            isSelected: null,
            primaryCarePhysician: null,
            hasPcp: null,
            employeeDependentsRelationshipId: null,
            isChild: null,
            isSpouse: null,
            isInactive: null,
            inactiveDate: null
        }
    }

    private patchRelationShipValue():void {
        let relId = this.employeeDependent.employeeDependentsRelationshipId;

        if(!relId){
            let relShip = this.employeeDependentRelationshipsList.find(x =>x.description.toLowerCase()
                == this.employeeDependent.relationship.toLowerCase() );
            if(relShip){
                relId = relShip.employeeDependentsRelationshipId;
            }
        }

        this.f.patchValue({
            relationshipId: relId,
        });
    }

    private createForm(): void {
        this.f = this.fb.group({
            firstName: this.fb.control(this.employeeDependent.firstName || '', [Validators.required, Validators.maxLength(25)]),
            middleInitial: this.fb.control(this.employeeDependent.middleInitial || '', [Validators.maxLength(1)]),
            lastName: this.fb.control(this.employeeDependent.lastName || '', [Validators.required, Validators.maxLength(25)]),
            relationshipId: this.fb.control(this.employeeDependent.employeeDependentsRelationshipId || '', [Validators.required]),
            ssn: this.fb.control(this.employeeDependent.unmaskedSocialSecurityNumber || '', [Validators.required, Validators.pattern("^\\d{3}-\\d{2}-\\d{4}$")]),
            gender: this.fb.control(this.employeeDependent.gender || '', [Validators.required]),
            birthDate: this.fb.control(this.employeeDependent.birthDate || '', [Validators.required]),
            isAStudent: this.fb.control(this.employeeDependent.isAStudent || ''),
            hasADisability: this.fb.control(this.employeeDependent.hasADisability || ''),
            tobaccoUser: this.fb.control(this.employeeDependent.tobaccoUser || ''),
            comments: this.fb.control(this.employeeDependent.comments || ''),
            confirmssn: this.fb.control(this.employeeDependent.unmaskedConfirmSocialSecurityNumber || ''),
        });
    }

    private validateDate(control: AbstractControl) {
        var m = moment(control.value, 'MM/DD/YYYY');
        return {validDate: m.isValid() };
    }

    private prepareModel(): IEmployeeDependent {
        let relId = this.f.value.relationshipId;
        let relText = '';
        let relShip = this.employeeDependentRelationshipsList.find(x =>x.employeeDependentsRelationshipId == relId );
        if(relShip) relText = relShip.description;

        return {
            employeeDependentId: this.employeeDependent != null ? this.employeeDependent.employeeDependentId : null,
            clientId: this.employeeDependent.clientId,
            firstName: this.f.value.firstName,
            middleInitial: this.f.value.middleInitial,
            lastName: this.f.value.lastName,
            employeeId: this.employeeDependent.employeeId,
            unmaskedSocialSecurityNumber: this.f.value.ssn,
            maskedSocialSecurityNumber: this.f.value.ssn,
            unmaskedConfirmSocialSecurityNumber: this.f.value.confirmssn,
            relationship: relText,
            gender: this.f.value.gender,
            birthDate: moment(this.f.value.birthDate).toDate(),
            isAStudent: this.f.value.isAStudent ? this.f.value.isAStudent : false,
            hasADisability: this.f.value.hasADisability ? this.f.value.hasADisability : false,
            tobaccoUser: this.f.value.tobaccoUser ? this.f.value.tobaccoUser : false,
            comments: this.f.value.comments,

            insertStatus: this.employeeDependent.insertStatus,
            lastModifiedByDescription: this.employeeDependent.lastModifiedByDescription,
            lastModifiedDate: this.employeeDependent.lastModifiedDate,
            isSelected: this.employeeDependent.isSelected,
            primaryCarePhysician: this.employeeDependent.primaryCarePhysician,
            hasPcp: this.employeeDependent.hasPcp,
            employeeDependentsRelationshipId: this.f.value.relationshipId,
            isChild: this.employeeDependent.isChild,
            isSpouse: this.employeeDependent.isSpouse,
            isInactive: this.employeeDependent.isInactive,
            inactiveDate: this.employeeDependent.inactiveDate
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

    saveEmployeeDependent(): void {
        this.formSubmitted = true;
        this.f.updateValueAndValidity();
        if (this.f.invalid) return;
        if(this.f.value.ssn!=this.f.value.confirmssn) {
            this.isConfirmSSNValid=false;
            return;
        }

        const dto = this.prepareModel();
        this.dialogRef.close(dto);
    }

    calculateAge() {
        if (this.f.value.birthDate) {
            let dob = moment(this.f.value.birthDate).format('MM/DD/YYYY');
            this.age = calculateBirthdateWithText(dob);
        }
        else
            this.age = '';
    }

}
