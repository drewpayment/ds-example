import { Component, OnInit, Input }   from '@angular/core';
import { IPayrollHistoryInfo } from '../shared/index';
import { PayrollService }      from '../shared/payroll.service';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'ds-payroll-information-widget',
    templateUrl: './payroll-information-widget.component.html',
    styleUrls: ['./payroll-information-widget.component.scss']
})
export class PayrollInformationWidgetComponent implements OnInit {

    isLoading: Boolean = true;
    payrollHistoryInfo: IPayrollHistoryInfo
    @Input('payrollId') payrollId:number;

    /********************************************
     * @constructor
     * 
     * @param { PaycheckListService } api
     ********************************************/
    constructor(private payrollApi: PayrollService, private route: ActivatedRoute) { }

    ngOnInit() {
        if(this.payrollId == null || this.payrollId == 0)
            this.payrollId = +this.route.snapshot.paramMap.get("payrollId");
        this.payrollApi.getPayryollInfoHistoryByPayrollId(this.payrollId).subscribe(info => {
            this.payrollHistoryInfo = info;
            if(this.payrollHistoryInfo.payrollRunDescription == "Normal Payroll with Salaries")
                this.payrollHistoryInfo.payrollRunDescription = "Normal";
            if(this.payrollHistoryInfo.payrollRunDescription == "Special Payroll (No Salary)")
                this.payrollHistoryInfo.payrollRunDescription = "Special (No Salary)";
            if(this.payrollHistoryInfo.isFrequencyBiWeekly) {
                if(this.payrollHistoryInfo.frequencyBiWeeklyPeriodStart == null)
                    this.payrollHistoryInfo.frequencyBiWeeklyPeriodStart = this.payrollHistoryInfo.periodStart;
                if(this.payrollHistoryInfo.frequencyBiWeeklyPeriodEnded == null)
                    this.payrollHistoryInfo.frequencyBiWeeklyPeriodEnded = this.payrollHistoryInfo.periodEnded;
            }
            if(this.payrollHistoryInfo.isFrequencyAltBiWeekly) {
                if(this.payrollHistoryInfo.frequencyAltBiWeeklyPeriodStart == null)
                    this.payrollHistoryInfo.frequencyAltBiWeeklyPeriodStart = this.payrollHistoryInfo.periodStart;
                if(this.payrollHistoryInfo.frequencyAltBiWeeklyPeriodEnded == null)
                    this.payrollHistoryInfo.frequencyAltBiWeeklyPeriodEnded = this.payrollHistoryInfo.periodEnded;
            }
            if(this.payrollHistoryInfo.isFrequencyMonthly) {
                if(this.payrollHistoryInfo.frequencyMonthlyPeriodStart == null)
                    this.payrollHistoryInfo.frequencyMonthlyPeriodStart = this.payrollHistoryInfo.periodStart;
                if(this.payrollHistoryInfo.frequencyMonthlyPeriodEnded == null)
                    this.payrollHistoryInfo.frequencyMonthlyPeriodEnded = this.payrollHistoryInfo.periodEnded;
            }
            if(this.payrollHistoryInfo.isFrequencyQuarterly) {
                if(this.payrollHistoryInfo.frequencyQuarterlyPeriodStart == null)
                    this.payrollHistoryInfo.frequencyQuarterlyPeriodStart = this.payrollHistoryInfo.periodStart;
                if(this.payrollHistoryInfo.frequencyQuarterlyPeriodEnded == null)
                    this.payrollHistoryInfo.frequencyQuarterlyPeriodEnded = this.payrollHistoryInfo.periodEnded;
            }
            if(this.payrollHistoryInfo.isFrequencySemiMonthly) {
                if(this.payrollHistoryInfo.frequencySemiMonthlyPeriodStart == null)
                    this.payrollHistoryInfo.frequencySemiMonthlyPeriodStart = this.payrollHistoryInfo.periodStart;
                if(this.payrollHistoryInfo.frequencySemiMonthlyPeriodEnded == null)
                    this.payrollHistoryInfo.frequencySemiMonthlyPeriodEnded = this.payrollHistoryInfo.periodEnded;
            }
            this.isLoading = false;
        });
    }

}
