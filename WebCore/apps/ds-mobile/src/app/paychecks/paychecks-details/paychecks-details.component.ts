import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
    IPaycheckInfo, IPaycheckEarningsDetail,
    IPaycheckEarningsHours, IPaycheckEarningsHoursList, IPaystubOptions, IPaycheckEarnings
} from '@ds/payroll';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { PayrollService } from '@ds/payroll/shared/payroll.service';

@Component({
    selector: 'ds-paychecks-details',
    templateUrl: './paychecks-details.component.html',
    styleUrls: ['./paychecks-details.component.scss']
})
export class PaychecksDetailsComponent implements OnInit {
    id: number;
    isLoading = false;
    employeePaychecksInfo: IPaycheckInfo[];
    currentPaycheckInfo: IPaycheckInfo;
    paycheckEarningsDetails: IPaycheckEarnings[];
    currentPaycheckDetails: IPaycheckEarningsDetail;
    employeePaycheckHours: IPaycheckEarningsHours[];
    currentPaycheckHours: IPaycheckEarningsHours;
    paycheckHoursList: IPaycheckEarningsHoursList[];
    paystubOptions: IPaystubOptions;
    displayedColumns = ['description', 'currentAmount', 'ytdAmount'];
    earningDetailsColumns = ['description', 'rate', 'hours', 'amount', 'ytdAmount'];
    displayedColumns2s = ['payCode', 'earning', 'rate', 'hours', 'pay'];
    displayedColumns3 = ['header1', 'header2', 'description', 'groupCode', 'displayOrder', 'YTDTotalAmount', 'currentRate'];
    displayedColumns4 = ['description', 'current', 'endYtd'];
    columns4 = ['description', 'YTDTotalAmount'];
    displayedColumns5 = ['pointBalance'];
    groupIds = {};
    headers: { [id: number]: string[] } = {};
    values: { [id: number]: { value1: any, value2: any }[] } = {};
    rows: { [id: number]: any[] } = {};

    get groups() {
        return Object.keys(this.groupIds);
    }

    constructor(
        private route: ActivatedRoute,
        private payrollApiService: PayrollService,
        private accountService: AccountService
    ) { }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.id = params['id'];
            this.accountService.getUserInfo().subscribe((user: UserInfo) => {
                if (this.id == 2) {
                    // this.payrollApiService.getCurrentPaycheckEarningsDetail(user.employeeId)
                    //     .subscribe(results => {
                        // TODO: This subscribe is returning an array of earning details???
                    //         // this.employeePaycheckDetails = results;
                    //         this.paycheckEarningsDetails = results;
                    //         // this.currentPaycheckDetails = this.paycheckEarningsDetails;
                    //     });
                } else if (this.id == 3) {
                    this.payrollApiService.getCurrentPaycheckEarningsHours(user.userEmployeeId)
                        .subscribe(results => {
                            this.employeePaycheckHours = results;
                            this.employeePaycheckHours.forEach(x => {
                                this.groupIds[x.groupCode] = true;
                            });

                            this.employeePaycheckHours.forEach(x => {
                                const existing = this.headers[x.groupCode];
                                if (null == existing) {
                                    this.headers[x.groupCode] = [x.header1];
                                } else {
                                    this.headers[x.groupCode].push(x.header2);
                                }
                            });
                            this.employeePaycheckHours.forEach(x => {
                                const existing = this.values[x.groupCode];
                                if (null == existing) {
                                    this.values[x.groupCode] = [{ value1: x.description, value2: x.ytdTotalAmount }];
                                } else {
                                    this.values[x.groupCode].push({ value1: x.description, value2: x.ytdTotalAmount });
                                }
                            });

                            this.employeePaycheckHours.forEach(x => {
                                const existing = this.rows[x.groupCode];
                                if (null == existing) {
                                    this.rows[x.groupCode] = [0];
                                } else {
                                    this.rows[x.groupCode].push(0);
                                }
                            });
                        });

                } else if (this.id == 5) {
                    this.payrollApiService.getCurrentPaycheckStubOptions(user.userEmployeeId)
                        .subscribe(results => {
                            this.paystubOptions = results;
                        });
                }

                this.payrollApiService.getCurrentPaycheckWithFullDetail(user.userEmployeeId)
                    .subscribe(result => {
                        this.employeePaychecksInfo = result;
                        this.currentPaycheckInfo = this.employeePaychecksInfo[0];
                        this.paycheckEarningsDetails = this.currentPaycheckInfo.earnings;
                        this.isLoading = true;
                    });

            });

        });
    }

}

