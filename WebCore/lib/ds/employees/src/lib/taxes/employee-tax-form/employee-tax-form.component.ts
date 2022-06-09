import { Component, OnInit, Inject, ElementRef, AfterViewInit, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

import * as _ from 'lodash';
import { UserInfo } from "@ds/core/shared";
import { FilingStatus } from '../shared/filing-status';
import { IFilingStatus } from '../shared/filing-status.model';
import { IEmployeeTax, IFormSignatureDefinition } from '../shared/employee-tax.model';
import { IEmployeeOnboardingTax } from '../shared/employee-onboarding-tax.model';
import { EmployeeTaxExemptionService } from '../shared/employee-tax-api.service';
import { AccountService } from "@ds/core/account.service";
import { IFormStatusData, IFormSignatureStatusData } from '@ajs/onboarding/shared/models';

interface DialogData {
    user: UserInfo,
    employeeTax: IEmployeeTax,
    maritalStatusList: IFilingStatus[],
    useFederal2020Form: boolean
}

@Component({
  selector: 'ds-employee-tax-form',
  templateUrl: './employee-tax-form.component.html',
  styleUrls: ['./employee-tax-form.component.scss']
})
export class EmployeeTaxFormComponent implements OnInit, AfterViewInit {
    user: UserInfo;
    employeeTax: IEmployeeTax;
    maritalStatusList: IFilingStatus[];
    f: FormGroup;
    fed2020FormGroup: FormGroup;
    formSubmitted: boolean;
    pageTitle: string;
    use2020FederalForm: boolean;
    showDependentsQuestions: boolean = false;
    isWaiverChecked: boolean = false;
    private _editMode: boolean;
    currentW4FormStatus: IFormStatusData;
    hasAdjusments : boolean = true;
    formSignatureDefinition: IFormSignatureDefinition;
    currentYear: string = (new Date()).getFullYear().toString();
    @ViewChild('maritalStatus', { static: false, read:ElementRef }) maritalStatusElement: ElementRef;

    constructor(
        public dialogRef: MatDialogRef<EmployeeTaxFormComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
        private fb: FormBuilder,
        private accountService: AccountService,
        private taxExemptionService: EmployeeTaxExemptionService
    ) {}

    ngOnInit(): void {
        this.user = this.data.user;
        this.use2020FederalForm = this.data.useFederal2020Form;

        this.taxExemptionService.getCurrentFormAndSignDefinitionForFedW4().subscribe(dto => {
            this.formSignatureDefinition = dto;
        });

        if (!this.user)
            this.accountService.getUserInfo().subscribe(u => {
                this.user = u;
                this.initializePage();
            });
        else
            this.initializePage();
    }

    private initializePage() {
        this.employeeTax = _.cloneDeep(this.data.employeeTax) || this.createEmptyEmployeeTax();

        this.maritalStatusList = _.filter(this.data.maritalStatusList, (status) => {
            return status.filingStatus === FilingStatus.Single ||
                status.filingStatus === FilingStatus.Married ||
                status.filingStatus === FilingStatus.HeadOfHousehold ||
                status.filingStatus === this.employeeTax.filingStatusId;
        });

        var filingStatusPresent = _.filter(this.data.maritalStatusList, (status) => {
            return status.filingStatus === this.employeeTax.filingStatusId;
        }).length > 0;
        if(!filingStatusPresent) this.employeeTax.filingStatusId = null;

        this.pageTitle = (this.employeeTax != null && this.employeeTax.employeeTaxId > 0) ? `Edit ${this.employeeTax.description} Employee Tax` : `Add Employee Tax`;

        this.createForm();
        this.loadFormStatus();
    }

    ngAfterViewInit() {

    }

    saveEmployeeTax(): void {
        let retObj = {
            forms: null,
            tax: null
        }

        if (this.use2020FederalForm) {
            this.fed2020FormGroup.updateValueAndValidity();
            if (this.fed2020FormGroup.invalid) return;
        }

        //RON SAYS THAT 0s SHOULD BE SAVED
        // if (this.f.value.percent == '0' && this.f.value.flat == 0) {
        //     this.f.patchValue({ percent: '' }, { emitEvent: false });
        //     this.f.patchValue({ flat: '' }, { emitEvent: false });
        // }
        if(this.use2020FederalForm == false){
            this.f.updateValueAndValidity();
            if (this.f.invalid) return;
        }

        const taxDto = this.prepareModel();
        //console.log(taxDto);
        const formStatusDto = this.prepareFormStatus()
        this.formSubmitted = true;

        retObj.tax = taxDto;
        retObj.forms = formStatusDto;
        //console.log(retObj);
        this.dialogRef.close(retObj);
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    previewForm(): void {

        let formDefinitionId: number = this.formSignatureDefinition.formDefinitionId; //this.use2020FederalForm == false ? 54 : 57
        let formData = this.prepareModel();

        this.taxExemptionService.previewW4FormWithData(this.employeeTax.employeeId, formDefinitionId, formData)
    }

    viewOriginalForm(): void {
        let formDefinitionId: number = this.formSignatureDefinition.formDefinitionId; //this.use2020FederalForm == false ? 54 : 57
        this.taxExemptionService.viewOriginalForm(formDefinitionId);
    }

    private createEmptyEmployeeTax(): IEmployeeTax {
        return {
            employeeTaxId: null,
            employeeId: null,
            filingStatusId: null,
            filingStatusDescription: null,
            numberOfExemptions: null,
            additionalAmountTypeId: null,
            additionalAmount: null,
            additionalPercent: null,
            description: null,
            clientId: null,
            taxCredit: null,
            otherTaxableIncome: null,
            wageDeduction: null,
            hasMoreThanOneJob: null,
            using2020FederalW4Setup: null
        }
    }

    private createForm(): void {
        if( this.use2020FederalForm == false){
            this.f = this.fb.group({
                maritalStatus: this.fb.control(this.employeeTax.filingStatusId || '', [Validators.required]),
                exemptions: this.fb.control(this.employeeTax.numberOfExemptions || '', [Validators.required, Validators.pattern("^[0-9]*$")]),
                //percent: this.fb.control(this.employeeTax.additionalPercent || '0', [Validators.required, Validators.pattern("[0-9]*\.?[0-9]+")]),
                flat: this.fb.control(this.employeeTax.additionalAmount || '0', [Validators.required, Validators.pattern("^[0-9]*$")]),
            });

            // this.f.get('percent').valueChanges.subscribe(val => {
            //     if (this.f.value.percent) {
            //         this.f.patchValue({ flat: '0' }, { emitEvent: false });
            //     }
            // });

            // this.f.get('flat').valueChanges.subscribe(val => {
            //     if (this.f.value.flat) {
            //         this.f.patchValue({ percent: '0' }, { emitEvent: false });
            //     }
            // });
        }

        else{
            this.fed2020FormGroup = this.fb.group({
                maritalStatus: this.fb.control(null, [Validators.required]),
                hasMoreThanOneJob: this.fb.control(false),
                qualifyingChildren: this.fb.control(0, [Validators.pattern("^[0-9]*$")]),
                otherDependents: this.fb.control(0, [Validators.pattern("^[0-9]*$")]),
                otherIncome: this.fb.control(0, [Validators.pattern("^[0-9]*$")]),
                wageDeduction: this.fb.control(0, [Validators.pattern("^[0-9]*$")]),
                extraWithholding: this.fb.control(0, [Validators.pattern("^[0-9]*$")]),
                esign: this.fb.control(null, [Validators.required]),
                submitWaiver: this.fb.control(null, [Validators.required])
            });
            setTimeout(()=>{
            this.maritalStatusElement.nativeElement.focus();
            },0);
        }
    }

    loadFormStatus(): void {
        this.currentW4FormStatus = null;
        this.taxExemptionService.getFormStatusData(this.user.employeeId).subscribe(
            res => {
                for(let form of res){
                    if (form.formTypeId == 10) //If Federal W4
                        this.currentW4FormStatus = form;
                }
            }
        );
    }

    private prepareModel() {
        if( this.use2020FederalForm == false){
            console.log(this.employeeTax);
            let returnDto: IEmployeeTax = {
                employeeTaxId: this.employeeTax != null ? this.employeeTax.employeeTaxId : null,
                employeeId: this.employeeTax.employeeId,
                filingStatusDescription: this.employeeTax.filingStatusDescription,
                additionalAmountTypeId: this.employeeTax.additionalAmountTypeId,
                description: this.employeeTax.description,
                clientId: this.employeeTax.clientId,

                numberOfExemptions: this.f.value.exemptions,
                filingStatusId: this.f.value.maritalStatus,
                additionalAmount: this.f.value.flat,

                //The following are unused for state taxes
                additionalPercent: 0,
                taxCredit: 0,
                otherTaxableIncome: 0,
                wageDeduction: 0,
                hasMoreThanOneJob: false,
                using2020FederalW4Setup: false
            };
            return returnDto;
        }
        else{
            let returnDto: IEmployeeOnboardingTax;

            let filingStatusDesc = null;
            for(let status of this.maritalStatusList){
                if(status.filingStatus == this.fed2020FormGroup.controls['maritalStatus'].value){
                    filingStatusDesc = status.description
                }
            }

            let qualifyingChildren = this.fed2020FormGroup.controls['qualifyingChildren'].value == null ? 0 : this.fed2020FormGroup.controls['qualifyingChildren'].value; //declaring here for code brevity below
            let otherDependents = this.fed2020FormGroup.controls['otherDependents'].value == null ? 0 : this.fed2020FormGroup.controls['otherDependents'].value; //declaring here for code brevity below
            let newFilingStatus = null;
            if( this.fed2020FormGroup.controls['hasMoreThanOneJob'].value){ //user has more than one job
                if(this.fed2020FormGroup.controls['maritalStatus'].value == 1){ //married
                    newFilingStatus = 19 //married w/ box2
                }
                else if(this.fed2020FormGroup.controls['maritalStatus'].value == 2){ //single
                    newFilingStatus = 17 //single w/ box2
                }
                else if(this.fed2020FormGroup.controls['maritalStatus'].value == 3){ //Hoh
                    newFilingStatus = 21 //Hoh w/ box2
                }
            }
            else{ //user didnt check has more than one job
                if(this.fed2020FormGroup.controls['maritalStatus'].value == 1){ //married
                    newFilingStatus = 20 //married w/o box2
                }
                else if(this.fed2020FormGroup.controls['maritalStatus'].value == 2){ //single
                    newFilingStatus = 18 //single w/o box2
                }
                else if(this.fed2020FormGroup.controls['maritalStatus'].value == 3){ // Hoh
                    newFilingStatus = 22 //Hoh w/o box2
                }
            }

            returnDto = {
                taxCategory: 1, //1 for federal taxes
                empTaxId: null,//not the same as employee tax id, this is emptaxId from employeeOnboardingW4
                employeeId: this.employeeTax.employeeId,
                filingStatus: newFilingStatus,
                isAdditionalAmountWithheld: this.fed2020FormGroup.controls['extraWithholding'].value == 0 || this.fed2020FormGroup.controls['extraWithholding'].value == null? false : true,
                additionalWithholdingAmt: this.fed2020FormGroup.controls['extraWithholding'].value == null ? 0 : this.fed2020FormGroup.controls['extraWithholding'].value,

                qualifyingChildren: qualifyingChildren,
                qualifyingChildrenAmount: qualifyingChildren*2000,
                otherDependents: otherDependents,
                otherDependentsAmount: otherDependents*500,
                taxCredit: qualifyingChildren*2000 + otherDependents*500,

                otherTaxableIncome: this.fed2020FormGroup.controls['otherIncome'].value == null ? 0 : this.fed2020FormGroup.controls['otherIncome'].value,
                wageDeduction:  this.fed2020FormGroup.controls['wageDeduction'].value == null ? 0 : this.fed2020FormGroup.controls['wageDeduction'].value,
                hasMoreThanOneJob: this.fed2020FormGroup.controls['hasMoreThanOneJob'].value,
                using2020FederalW4Setup: true,

                createDt: null,
                stateId: null,
                allowances: 0

                //numberOfExemptions: 0,
                //filingStatusDescription: filingStatusDesc,
                //description: this.employeeTax.description,
                //clientId: this.employeeTax.clientId,
                //additionalPercent: 0, //2020 only allows for flat amount per pay period,
            }
            return returnDto
        }
    }

    private prepareFormStatus(): IFormStatusData[] {
        let sig: IFormSignatureStatusData[] = [{
            isSigned: false,
            roleIdentifier: "employee",
            roleName: "Employee",
            signatureDate: new Date(),
            signatureDefinitionId:  this.formSignatureDefinition.signatureDefinitionId,
            signatureId: null,
            signatureName: this.user.firstName + " " + this.user.lastName,
            signeeFirstName: this.user.firstName,
            signeeLastName: this.user.lastName,
            signeeInitials: this.user.firstName[0].toUpperCase() + this.user.lastName[0].toUpperCase(),
            signeeMiddle: this.user.middleInitial,
            signeeTitle: null
        }];

        let retForm: IFormStatusData[] = [{
            formId: null,  //always a new form
            employeeId: this.user.employeeId,
            clientId: this.user.clientId,
            formTypeId: 10,
            formDefinitionId: this.formSignatureDefinition.formDefinitionId, //this.use2020FederalForm == false ? 54 : 57, //54 is for 2019 w4 form, 57 is for 2020 w4 form
            formName: "Federal W-4",
            formVersion: (new Date()).getFullYear().toString(), //this.use2020FederalForm == false ? "2019" : "2020",
            isComplete: false,
            isCurrentVersion: true,
            DefinitionInfo: null,
            Signatures: sig
        }];
        return retForm;
    }

    getFormControlError(field: string, errorCodes: string[], form:FormGroup = null): boolean {
        const control = form?form.get(field) : this.f.get(field);
        let flag: boolean = false;
        _.forEach(errorCodes, (errorCode) => {
            flag = control.hasError(errorCode) && (control.touched || this.formSubmitted);
            if (flag === true)
                return false;
        });
        return flag;
    }

    filingOptionChange() {
        if (this.f.value.maritalStatus)
            this.employeeTax.filingStatusDescription = FilingStatus[this.f.value.maritalStatus];
        else
            this.employeeTax.filingStatusDescription = '';
    }

    toggleDependentsQuestions(checked):void {
        this.showDependentsQuestions = checked;
        if(!this.showDependentsQuestions){
            this.fed2020FormGroup.controls['qualifyingChildren'].setValue(0);
            this.fed2020FormGroup.controls['otherDependents'].setValue(0);
        }
    }
    toggleHasAdjusments(checked):void{
        this.hasAdjusments = checked;
    }
    waiverCheck(checked):void {
        this.isWaiverChecked = checked;
    }
}
