import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormControl, FormGroup, Validators, FormBuilder, AsyncValidatorFn } from '@angular/forms';
import { date } from '@ajs/applicantTracking/application/inputComponents';
import {DashboardService} from "apps/ds-company/src/app/services/dashboard.service";
import { IEmployeeStatus } from '@ajs/employee/models';

@Component({
    selector: 'ds-close-onboarding-dialog',
    templateUrl: './close-onboarding-dialog.component.html',
    styleUrls: ['./close-onboarding-dialog.component.scss']
  })
  export class CloseOnboardingDialogComponent implements OnInit {
    form1: FormGroup;
    formSubmitted: boolean;

    separationDate: date;
    employeeStatusId: number;
    allStatuses: Array<IEmployeeStatus> = [];

    constructor(
        private dashboardService: DashboardService,
        private ref:MatDialogRef<CloseOnboardingDialogComponent>,
        private fb: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data:any,
    ) {
        this.separationDate = data.separationDate;
        this.employeeStatusId = data.employeeStatusId;
    }
    

    ngOnInit() {
        /// Build the form here
        this.form1 = this.fb.group({
            separationDate:   [this.separationDate , ],
            employeeStatus:   [this.employeeStatusId , ],
        });

        this.dashboardService.getEmployeeStatusList().subscribe((data) => {
            this.allStatuses = data;
        });
    }
    clear() {
        this.ref.close(null);
    }
    save() {
        this.formSubmitted = true;
        if(this.form1.valid){
            this.ref.close({
                separationDate: this.form1.value.separationDate, 
                employeeStatusId: this.form1.value.employeeStatus
            });
        }
    }

    getFormControlError(field: string, errorCodes: string[]): boolean {
        const control = this.form1.get(field);
        let flag: boolean = false;
        _.forEach(errorCodes, (errorCode) => {
            flag = control.hasError(errorCode) && (control.touched || this.formSubmitted);
            if (flag === true)
                return false;
        });
        return flag;
    }
}