import { Component, OnInit, Input } from '@angular/core';
import { InfoData } from '@ds/analytics/shared/models/InfoData.model';
import { DateRange } from '@ds/analytics/shared/models/DateRange.model';
import { AccountService } from '@ds/core/account.service';
import { PayrollService } from '@ds/payroll/shared/payroll.service';
import * as moment from 'moment';
import { MOMENT_FORMATS, UserInfo } from '@ds/core/shared';
import { Observable } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';
import { IBankDepositInfo, IBasicPayrollHistory } from '@ds/payroll';

@Component({
    selector: 'ds-bank-deposit',
    templateUrl: './bank-deposit.component.html',
    styleUrls: ['./bank-deposit.component.css']
})
export class BankDepositComponent implements OnInit {
    private user: UserInfo;
    @Input() employeeIds: Number[];
    @Input() dateRange: DateRange;

    loaded: boolean;
    emptyState = false;

    cardType = 'graph';

    infoData: InfoData;

    public title = 'Check Date: ';

    public graphData: IBankDepositInfo;
    currentPayrollId: number;
    currentCheckDate: string;
    breakdownType = 'Tax Liability';

    headerDropdownOptions$: Observable<{ id: number, val: any }[]>;
    headerDropdownOptions: { id: number, val: any }[] = [];
    clientPayrolls: any[];

    initialPayroll: number;

    constructor(
        private payrollApi: PayrollService,
        private accountService: AccountService,
    ) { }

    ngOnInit() {
        this.headerDropdownOptions$ = this.accountService.getUserInfo()
            .pipe(
                tap(user => this.user = user),
                switchMap(user =>
                    this.payrollApi.getBasicPayrollHistoryByStatus(user.clientId)),
                map(payrolls => {
                    if (payrolls != null && payrolls.length) {
                        return payrolls;
                    }

                    this.emptyState = true;
                    this.loaded = true;
                    return payrolls;
                }),
                switchMap((payrolls: IBasicPayrollHistory[]) => {
                    this.clientPayrolls = payrolls;

                    payrolls.forEach((x, i, a) => {
                        const p = {
                            id: x.payrollId,
                            val: moment(x.checkDate).format(MOMENT_FORMATS.US)
                        };

                        if (i === payrolls.length-1) {
                            this.initialPayroll = p.id;
                            this.setCurrentCheckDate(this.initialPayroll);
                            this.currentPayrollId = this.initialPayroll;
                        }

                        this.headerDropdownOptions.unshift(p);
                        return p;
                    });
                    return this.payrollApi.getPayrollHistoryBankDepositInfoByPayrollId(this.initialPayroll);
                }),
                map(depositInfo => {
                    this.graphData = depositInfo;

                    if (depositInfo == null) this.emptyState = true;

                    this.loaded = true;

                    return this.headerDropdownOptions;
                }),
            );
    }

    changePayroll(event) {
        const pID = event.value;
        this.payrollApi.getPayrollHistoryBankDepositInfoByPayrollId(pID)
            .subscribe((data: any) => {
                if (data != null) {
                    this.emptyState = false;
                }
                this.graphData = data;
            });
        this.currentPayrollId = pID;
        this.setCurrentCheckDate(pID);
    }

    setCurrentCheckDate(payrollId: number) {
        const payroll = this.clientPayrolls.find(p => p.payrollId === payrollId);
        if (!payroll) return;

        this.currentCheckDate = moment(payroll.checkDate).format(MOMENT_FORMATS.US);
    }

    changeBreakdown(type) {
        this.breakdownType = type;
    }

    formatPayPeriod(startDate, endDate) {
        const start = moment(startDate).format(MOMENT_FORMATS.US);
        const end = moment(endDate).format(MOMENT_FORMATS.US);
        return start + ' - ' + end;
    }
}

