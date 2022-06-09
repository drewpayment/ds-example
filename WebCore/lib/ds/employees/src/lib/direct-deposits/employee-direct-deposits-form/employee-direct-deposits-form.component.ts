import { Component, OnInit, Inject, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EmployeeDirectDepositsService } from '../shared/employee-direct-deposit-api.service';
import { IEmployeeDirectDepositInfo } from '../shared/employee-direct-deposit-info.model';
import * as _ from 'lodash';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { routingNumberValidator } from '@ds/core/ui/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

interface DialogData {
    user: UserInfo;
    employeeDirectDeposit: IEmployeeDirectDepositInfo;
    maxAccountsCountAllowed: number;
    accountsCount: number;
}

@Component({
  selector: 'ds-employee-direct-deposits-form',
  templateUrl: './employee-direct-deposits-form.component.html',
  styleUrls: ['./employee-direct-deposits-form.component.scss']
})
export class EmployeeDirectDepositsFormComponent implements OnInit, AfterViewInit {
    user: UserInfo;
    employeeDirectDeposit: IEmployeeDirectDepositInfo;
    maxAccountsCountAllowed: number;
    accountsCount: number;
    f: FormGroup;
    formSubmitted: boolean;
    pageTitle: string;
    private _editMode: boolean;
    isConfirmAccountValid: boolean = true;
    isConfirmRoutingValid: boolean = true;

    constructor(
        public dialogRef: MatDialogRef<EmployeeDirectDepositsFormComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
        private fb: FormBuilder,
        private accountService: AccountService,
        private employeeDirectDepositsService: EmployeeDirectDepositsService
    ) { }

    ngOnInit(): void {
        this.user = this.data.user;
        this.maxAccountsCountAllowed = this.data.maxAccountsCountAllowed;
        this.accountsCount = this.data.accountsCount;
        if (!this.user)
            this.accountService.getUserInfo().subscribe(u => {
                this.user = u;
                this.initializePage();
            });
        else
            this.initializePage();
    }

    private initializePage() {
        this.employeeDirectDeposit = _.cloneDeep(this.data.employeeDirectDeposit) || this.createEmptyEmployeeDirectDeposit();
        this.employeeDirectDeposit.confirmRoutingNumber = this.employeeDirectDeposit.routingNumber;
        this.employeeDirectDeposit.confirmAccountNumber = this.employeeDirectDeposit.accountNumber;
        this.pageTitle = (this.employeeDirectDeposit != null && this.employeeDirectDeposit.employeeDeductionId > 0) ? `Edit Account` : `Add Account`;

        this.createForm();
    }

    private createEmptyEmployeeDirectDeposit(): IEmployeeDirectDepositInfo {
        return {
            employeeId: this.user.employeeId,
            isPrenote: null,
            employeeDeductionId: null,
            employeeBankId: null,
            accountType: 1,
            nickname: null,
            amount: null,
            amountType: -3,
            accountName: '',
            accountNumber: null,
            routingNumber: null,
            sortOrderIndex: null,
            confirmAccountNumber: null,
            confirmRoutingNumber: null
        };
    }

    private createForm(): void {
        this.f = this.fb.group({
            accountName: this.fb.control(this.employeeDirectDeposit.accountName || '', [Validators.maxLength(100)]),
            accountType: this.fb.control(this.employeeDirectDeposit.accountType, []),
            routingNumber: this.fb.control(this.employeeDirectDeposit.routingNumber || '', [Validators.required, routingNumberValidator()]),
            accountNumber: this.fb.control(this.employeeDirectDeposit.accountNumber || '', [Validators.required]),
            amountType: this.fb.control({value: this.employeeDirectDeposit.amountType || '-3', disabled: this.employeeDirectDeposit.employeeDeductionId != null} , []),
            confirmRoutingNumber: this.fb.control(this.employeeDirectDeposit.confirmRoutingNumber || '', []),
            confirmAccountNumber: this.fb.control(this.employeeDirectDeposit.confirmAccountNumber || '', []),
        });
    }

    private prepareModel(): IEmployeeDirectDepositInfo {
        return {
            employeeDeductionId: this.employeeDirectDeposit != null ? this.employeeDirectDeposit.employeeDeductionId : null,
            isPrenote: this.employeeDirectDeposit.isPrenote,
            employeeId: this.employeeDirectDeposit.employeeId,
            employeeBankId: this.employeeDirectDeposit != null ? this.employeeDirectDeposit.employeeBankId : null,
            accountType: this.f.value.accountType,
            nickname: this.f.value.accountType == '1' ? 'Checking' : 'Savings',
            amount: this.employeeDirectDeposit != null ? this.employeeDirectDeposit.amount : 0,
            amountType: this.f.controls['amountType'].value, // have to handled it bit different as this control may be disabled sometimes
            accountName: this.f.value.accountName,
            accountNumber: this.f.value.accountNumber,
            routingNumber: this.f.value.routingNumber,
            sortOrderIndex: this.employeeDirectDeposit != null ? this.employeeDirectDeposit.sortOrderIndex : 0,
            confirmRoutingNumber: this.f.value.confirmRoutingNumber,
            confirmAccountNumber: this.f.value.confirmAccountNumber
        };
    }

    getFormControlError(field: string, errorCodes: string[]): boolean {
        const control = this.f.get(field);
        let flag = false;
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

    saveEmployeeDirectDeposit(): void {
        this.formSubmitted = true;
        this.f.updateValueAndValidity();
        if (this.f.invalid) return;

        const dto = this.prepareModel();
        this.dialogRef.close(dto);
    }
}
