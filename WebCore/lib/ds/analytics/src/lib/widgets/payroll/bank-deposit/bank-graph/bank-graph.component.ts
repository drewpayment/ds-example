import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from "@angular/core";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { ChartType, ChartOptions } from "chart.js";
import { AccountService } from '@ds/core/account.service';
import { AnalyticsApiService } from "@ds/analytics/shared/services/analytics-api.service";
import { IBankDepositInfo } from '@ds/payroll';
import { switchMap, tap } from 'rxjs/operators';
import { coerceNumberProperty } from '@angular/cdk/coercion';

@Component({
    selector: 'ds-bank-graph',
    templateUrl: './bank-graph.component.html',
    styleUrls: ['./bank-graph.component.css']
})

export class BankGraphComponent implements OnInit, OnChanges {
    @Input() dateRange: DateRange;
    @Input() data: IBankDepositInfo;
    @Output() breakdown = new EventEmitter();

    loaded: boolean;

    cardType: string = "info";

    infoData: InfoData;

    bankDepositTotal: number;
    taxPaymentTotal: number;
    employeePaymentTotal: number;
    vendorPaymentTotal: number;

    tempLabels: string[] = ["Employee Payments", "Tax Payments", "Vendor Payments"];
    breakdownLabels: string[] = ["Employee Payments", "Tax Liability", "Vendor Payments"];

    ClientCalendarTaxFileID: number;
    nonEscrowClient: boolean = false;

    //Chart Variables
    public pieChartOptions: ChartOptions = {
        responsive: true,
        tooltips: {
            callbacks: {
                label: (tooltipItem) => {
                    return this.pieChartLabels[tooltipItem.index];
                },
            }
        },
        legend: {
            position: "right",
            labels: {
                usePointStyle: true,
                padding: 15,
            },
        }
    };
    public pieChartData: number[] = [];
    public pieChartLabels: string[] = ["Employee Payments", "Tax Payments", "Vendor Payments"];
    public pieChartType: ChartType = "pie";
    public pieChartColors = [
        {
            backgroundColor: [
                "#6dbb2c",
                "#caec73",
                "#2a8d33",
            ],
        },
    ];

    constructor(private analyticsApi: AnalyticsApiService, private accountService: AccountService) { }

    ngOnInit() {
        this.bankDepositTotal = this.data.bankDepositTotal;
        this.taxPaymentTotal = this.data.taxPaymentTotal;
        this.employeePaymentTotal = this.data.eePaymentTotal;
        this.vendorPaymentTotal = this.data.vendorPaymentTotal;
        this.infoData = {
            icon: '',
            color: 'success',
            value: `${this.bankDepositTotal.toLocaleString('en-US', { style: 'currency', currency: 'USD' })}`,
            title: 'Bank Deposit',
            tooltip: 'A breakdown of the employee, vendor, and tax payments for the selected pay date\'s check deposit.',
            showBottom: true
        };

        // Check Client Tax Service Type
        this.accountService.getUserInfo()
            .pipe(
                switchMap(user =>
                    this.analyticsApi.getClientCalendarTaxFileID(user.clientId)),
                tap((calendarTaxFileId: number) => {
                    this.ClientCalendarTaxFileID = coerceNumberProperty(calendarTaxFileId);
                    this.nonEscrowClient = this.ClientCalendarTaxFileID !== 4;
                })
            )
            .subscribe();

        this.setData();
    }

    ngOnChanges(changes: SimpleChanges) {
        if (changes.data != null && !changes.data.firstChange) {
            this.loaded = false;

            this.bankDepositTotal = changes.data.currentValue.bankDepositTotal;
            this.taxPaymentTotal = changes.data.currentValue.taxPaymentTotal;
            this.employeePaymentTotal = changes.data.currentValue.eePaymentTotal;
            this.vendorPaymentTotal = changes.data.currentValue.vendorPaymentTotal;
            this.infoData = {
                icon: '',
                color: 'success',
                value: `${this.bankDepositTotal.toLocaleString('en-US', { style: 'currency', currency: 'USD' })}`,
                title: 'Bank Deposit',
                tooltip: 'A breakdown of the employee, vendor, and tax payments for the selected pay date\'s check deposit.',
                showBottom: true
            };

            this.setData();
        }
    }

    setData() {
        this.pieChartData = [this.employeePaymentTotal, this.taxPaymentTotal, this.vendorPaymentTotal];

        //Concat data and labels for Legend
        this.concatLabels(this.tempLabels, this.pieChartData);

        this.loaded = true;
    }

    concatLabels(Labels: String[], data) {
        for (var x = 0; x < Labels.length; x++) {
            this.pieChartLabels[x] = `${data[x].toLocaleString('en-US', { style: 'currency', currency: 'USD' })} ` + Labels[x];
        }
    }

    changeBreakdown(event) {
        if (event.active.length > 0) {
            let breakdownType = this.breakdownLabels[event.active[0]._index];
            this.breakdown.emit(breakdownType);
        }
    }
}

