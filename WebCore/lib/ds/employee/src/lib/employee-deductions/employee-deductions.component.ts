import { Component, OnInit, Input, Output, Inject } from '@angular/core';
import { EmployeeDeductionsApiService } from './shared/employee-deductions-api.service';
import { IDeductions, Deductions, IClientDeductionInfo } from './models/Deductions';
import { UserType, UserInfo, UrlParts } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { DOCUMENT } from '@angular/common';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ConfigUrlType, ConfigUrl } from '@ds/core/shared/config-url.model';
import { APP_CONFIG, AppConfig, joinWithSlash } from '@ds/core/app-config/app-config';

@Component({
  selector: 'ds-employee-deductions',
  templateUrl: './employee-deductions.component.html',
  styleUrls: ['./employee-deductions.component.scss']
})
export class EmployeeDeductionsComponent implements OnInit {

    showInactive: boolean;
    rawDeductionsList: any;
    deductionsList: Deductions[] = [];
    earningsList: Deductions[] = [];
    directDepositsList: Deductions[] = [];

    userInfo: UserInfo;
    employeeID: number;
    clientID: number;
    userTypeID: number;
    baseURL: string;
    userHasReadAccess: boolean;
    userHasAccountWriteAccess: boolean;
    userHasAmountWriteAccess: boolean;
    isOwnDDAndIsSupervisor: boolean;

    hideEarnings = false;
    NO_VALUE_SELECTED =  -2147483648;

    loadingData = true;

    reminderChecked = false;
    reminderForm: FormGroup;
    minDate: Date;

    disableReminderCalendar = true;


    clientDeductionInformation: IClientDeductionInfo;
    deductionsThatAllowDDsList: Array<number> = [];

    employeeKey: string;
    baseUrl = this.config.baseSite.url;
    breadcrumb: string;
    hasSelectedEmployee = false;

    constructor(
        private deductionsService: EmployeeDeductionsApiService,
        private acctService: AccountService,
        @Inject(DOCUMENT) private document: Document,
        @Inject(APP_CONFIG) private config: AppConfig,
    ) {}

    ngOnInit() {
        this.breadcrumb =
            joinWithSlash(this.baseUrl, 'ChangeEmployee.aspx?SubMenu=Employee&Force=True&URL=EMDeduction.aspx#/employee/deductions');
        this.minDate = new Date();
        this.minDate.setHours(0, 0, 0, 0);

        this.getUserInfoAndPopulateTables();
        this.reminderForm = new FormGroup({
            reminderDate: new FormControl(new Date())
        });
        this.disableReminderCalendar = true;
    }

    setupSessionStorage() {
        this.employeeKey = `${this.employeeID}-displayInActive`;
        const ses = sessionStorage.getItem(this.employeeKey);
        if (ses === null) {
            sessionStorage.setItem(this.employeeKey, 'false');
            this.showInactive = false;
        } else if (ses === 'true') {
            this.showInactive = true;
        } else {
            this.showInactive = false;
        }
    }

    changeActiveSessionStorage() {
        this.showInactive = !this.showInactive;
        const ses = sessionStorage.getItem(this.employeeKey);
        if (ses === 'false') {
            sessionStorage.setItem(this.employeeKey, 'true');
        } else {
            sessionStorage.setItem(this.employeeKey, 'false');
        }
    }

    parseEmployeeDeductionList(employeeID: number) {
        this.deductionsService.getEmployeeDeductionsList(employeeID).subscribe(
            res  => {
                this.rawDeductionsList = res;
                this.directDepositsList = [];
                this.earningsList = [];
                this.deductionsList = [];

                for ( let i = 0; i < this.rawDeductionsList.length; i++) {
                    if ( res[i].groupSequence == 3) {
                        this.directDepositsList.push(new Deductions(res[i]));
                    } else if (res[i].groupSequence == 2) {
                        this.deductionsList.push(new Deductions(res[i]));
                    } else if (res[i].groupSequence == 1) {
                        this.earningsList.push(new Deductions(res[i]));
                    }
                }

                this.loadingData = false;
            },
            err => {
                console.log(err);
            }
        );
    }

    getClientDeductionInfo() {
        this.deductionsService.getClientDeductionInformation(this.clientID, this.userTypeID, this.employeeID, this.userInfo.userId)
            .subscribe((data: IClientDeductionInfo) => {
                this.clientDeductionInformation = data;
                this.toggleAccessBasedOnPermission(); // function to leave page if user does not have appropriate permissions
            });

        this.deductionsService.getClientDeductionThatAllowDDs(this.clientID).subscribe((deducIDsThatAllowDDs: Array<number>) => {
            this.deductionsThatAllowDDsList = deducIDsThatAllowDDs;
        });
    }

    getUserInfoAndPopulateTables() {
        this.acctService.getUserInfo().subscribe(
            res => {
                this.userInfo = res; // get users info from endpoint and set to this.userInfo
                this.employeeID = res.selectedEmployeeId();
                this.clientID = res.selectedClientId();
                this.userTypeID = res.userTypeId;
                this.getClientDeductionInfo();
                this.hideEarningComponentIfNoneAvailable();

                if (this.employeeID == 0 || this.employeeID == null) {
                    /**
                     * Should NOT be enabled until this page stands alone as an angular route in a SPA. Currently,
                     * this is referenced on the EMDeductions.aspx and the code behind on this page already redirects them
                     * if the user doesn't have an employee selected.
                     */
                    // this.document.location.href =
                    //     this.config.baseSite.url + '/ChangeEmployee.aspx?SubMenu=Employee&Force=True&URL=EMDeduction.aspx';
                    console.warn('No employee selected');
                } else {
                    this.parseEmployeeDeductionList(this.employeeID);
                }

                this.setupSessionStorage();
            },
            err => { // if we can't get userInfo we cant't verify the user should be on this page, send them away.

                /**
                 * Should NOT be enabled until this page stands alone as an angular route in a SPA. Currently,
                 * this is referenced on the EMDeductions.aspx and the code behind on this page already redirects them
                 * if the user doesn't have an employee selected.
                 */
                // this.document.location.href =
                //         this.config.baseSite.url + '/ChangeEmployee.aspx?SubMenu=Employee&Force=True&URL=EMDeduction.aspx';
                console.warn('No employee selected');
            }
        );
    }

    refreshData() {
        this.parseEmployeeDeductionList(this.employeeID);
    }

    refreshClientDeductionInfo() {
        this.getClientDeductionInfo();
    }

    hideEarningComponentIfNoneAvailable(): void {
        this.deductionsService.getDeductionDescriptionList(
            this.userInfo.selectedEmployeeId(),
            -1,
            this.NO_VALUE_SELECTED,
            1,
            this.userInfo.selectedClientId(),
            'null'
        ).subscribe(
            (res: any[]) => {
                if (res.length == 0) {
                    this.hideEarnings = true;
                }
            },
            error => {
                console.log(error);
            }
        );
    }

    reminderCheckboxClicked(reminderChecked: boolean) {
        this.reminderChecked = reminderChecked;
        if (this.reminderChecked) {
            this.disableReminderCalendar = false;
        } else {
            this.disableReminderCalendar = true;
        }
    }

    toggleAccessBasedOnPermission(): void {
        if (this.userInfo.userTypeId === UserType.systemAdmin) {
            this.userHasReadAccess = true;
            this.userHasAccountWriteAccess = true;
            this.userHasAmountWriteAccess = true;
            this.isOwnDDAndIsSupervisor = false;
        } else if (this.userInfo.userTypeId === UserType.companyAdmin) {
            this.userHasReadAccess = true;
            this.userHasAccountWriteAccess = !this.userInfo.isEmployeeSelfServiceViewOnly;
            this.userHasAmountWriteAccess = !this.userInfo.isEmployeeSelfServiceViewOnly;
            this.isOwnDDAndIsSupervisor = false;
        } else if (this.userInfo.userTypeId === UserType.supervisor) { // supervisors have access if BlockDeductionsPage is false
            this.deductionsService.getSupervisorHasBlockedDeductions(this.userInfo.userId).subscribe(
                (result: any) => {
                    // for cases when a supervisor is viewing deductions for users they have access to
                    if (result.data != null && result.data !== undefined) {
                        if (result.data == 0) { // full access
                            this.userHasAccountWriteAccess = true;
                            this.userHasAmountWriteAccess = true;
                            this.userHasReadAccess = true;
                        } else if (result.data == 1) { // no access
                            this.userHasReadAccess = false;
                            this.userHasAccountWriteAccess = false;
                            this.userHasAmountWriteAccess = false;
                        } else if (result.data == 2) { // readonly access
                            this.userHasReadAccess = true;
                            this.userHasAmountWriteAccess = false;
                            this.userHasAccountWriteAccess = false;
                        }
                    }

                    // for cases when a supervisor is viewing deductions for themselves via this page
                    if (this.userInfo.employeeId === this.userInfo.lastEmployeeId) {
                        // write access is governed by clientEssOptions rather than SupervisorHasBlockedDeductions like above, per EM-647
                        // read access is always true(just for DD) if they are a supervisor and are viewing their own deductions
                        this.isOwnDDAndIsSupervisor = true;
                        this.userHasReadAccess = true;
                        if (this.clientDeductionInformation.clientEssOptions.manageDirectDepositAmountAndAccountInfo) {
                            this.userHasAccountWriteAccess = true;
                            this.userHasAmountWriteAccess = true;
                        } else if (this.clientDeductionInformation.clientEssOptions.manageDirectDepositAmount) {
                            this.userHasAmountWriteAccess = true;
                            this.userHasAccountWriteAccess = false;
                        } else {
                            this.userHasAmountWriteAccess = false;
                            this.userHasAccountWriteAccess = false;
                        }
                    } else {
                        this.isOwnDDAndIsSupervisor = false;
                    }
                }
            );
        }
    }
}
