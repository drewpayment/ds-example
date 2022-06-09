import * as _ from 'lodash';
import { Component, Inject, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { UserInfo } from "@ds/core/shared";
import { MaintenanceApiService } from '../../../services/maintenance-api.service';
import { AccountService } from '@ds/core/account.service';
import { IBankHoliday } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

interface DialogData {
    user: UserInfo,
    bankHoliday: IBankHoliday
}

@Component({
  selector: 'ds-bank-holiday-edit-dialog',
  templateUrl: './bank-holiday-edit-dialog.component.html',
  styleUrls: ['./bank-holiday-edit-dialog.component.scss']
})
export class BankHolidayEditDialogComponent implements OnInit {

    user:UserInfo;
    bankHoliday:IBankHoliday;
    formSubmitted:boolean;
    form:FormGroup;
    editBankHolidayTitle:string;

    constructor(
        public dialogRef:MatDialogRef<BankHolidayEditDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data:DialogData,
        private fb:FormBuilder,
        private service:MaintenanceApiService,
        private msg:NgxMessageService,
        private accountService: AccountService
    ) {}

    ngOnInit():void {
        this.user = this.data.user;
        if (!this.user) {
            this.accountService.getUserInfo().subscribe(u => {
                this.user = u;
            });
        }

        this.bankHoliday = this.data.bankHoliday != null ? _.cloneDeep(this.data.bankHoliday) : this.createEmptyBankHoliday();
        this.createForm();
    }

    saveBankHoliday(): void {
        this.formSubmitted = true;
        this.form.updateValueAndValidity();
        if (this.form.invalid) return;

        const dto = this.prepareModel();
        this.dialogRef.close(dto);
    }

    ngAfterViewInit() {

    }

    onNoClick():void {
        this.dialogRef.close();
    }

    private createEmptyBankHoliday():IBankHoliday {
        return {
            bankHolidayId: null,
            name: null,
            date: null,
            modified: null,
            modifiedBy: null
        }
    }

    private createForm():void {
        this.form = this.fb.group({
            name: this.fb.control(this.bankHoliday.name || '', [Validators.required]),
            date: this.fb.control(this.bankHoliday.date || '', [Validators.required]),
        });
    }

    private prepareModel():IBankHoliday {
        return {
            bankHolidayId: this.bankHoliday != null ? this.bankHoliday.bankHolidayId : null,
            name: this.form.value.name,
            date: this.form.value.date,
            modified: new Date(),
            modifiedBy: this.user.employeeId
        };
    }

    getFormControlError(field: string, errorCodes: string[]): boolean {
        const control = this.form.get(field);
        let flag: boolean = false;
        _.forEach(errorCodes, (errorCode) => {
            flag = control.hasError(errorCode) && (control.touched || this.formSubmitted);
            if (flag === true)
                return false;
        });
        return flag;
    }
}
