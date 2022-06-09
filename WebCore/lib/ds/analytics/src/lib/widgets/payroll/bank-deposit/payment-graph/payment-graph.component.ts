import { Component, OnInit, Input } from "@angular/core";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { AnalyticsApiService } from "@ds/analytics/shared/services/analytics-api.service";
import { ChartType, ChartOptions } from "chart.js";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { IBankDepositData } from '@ds/analytics/shared/models/IBankDepositData.model';
import { BankDepositDialogComponent } from '../bank-deposit-dialog/bank-deposit-dialog.component';
import { TaxBankDepositDialogComponent } from '../tax-bank-deposit-dialog/tax-bank-deposit-dialog.component';
import { LoadingDialogService } from '@ds/analytics/shared/services/loading-dialog.service';
import { HttpErrorResponse } from '@angular/common/http';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';

/*
 * Main component for widget, data is accessed here using 'getPayrollPaycheckHistory' and 'getPayrollTaxHistory'
 * Graph on the right side of the widget. Opens 'bank-deposit-dialog' and 'tax-bank-deposit-dialog'
 * @Input 'currentPayrollID' and 'breakdownType' from bank-deposit.component.ts
 */

@Component({
  selector: 'ds-payment-graph',
  templateUrl: './payment-graph.component.html',
  styleUrls: ['./payment-graph.component.css']
})

export class PaymentGraphComponent implements OnInit {
    @Input() dateRange: DateRange;
    @Input() currentPayrollId: any;
    @Input() breakdownType: any;
    @Input() currentCheckDate: any;

    //State variables
    loaded: boolean;
    emptyState: boolean;

    vendors: string[] = [];
    employees: string[] = [];

    previousPayrollID: number;
    previousBreakdownType: string;

    //Card variables
    cardType: string = "info";
    infoData: InfoData;

    //Dialog variables
    bankDepositData: any;

    //Chart variables
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
        },
        plugins: {
            datalabels: {
                formatter: (value, ctx) => {
                    const label = ctx.chart.data.labels[ctx.dataIndex];
                    return label;
                },
            },
        },
    };
    public pieChartData: number[] = [];
    public pieChartType: ChartType = "pie";
    public pieChartColors = [
        {
            backgroundColor: [
              "#04b5b0",
              "#86d1cf",
              "#08778d",
              "#42c3cf",
              "#18568b",
              "#4ca7cf",
              "#8bdbf6",
              "#237fb7",
              "#04b5b0",
              "#86d1cf",
              "#08778d",
              "#42c3cf",
              "#18568b",
              "#4ca7cf",
              "#8bdbf6",
              "#237fb7",
            ],
        },
    ];
    public pieChartLabels: string[] = [];
    public employeeLabels: string[] = ["Hourly", "Salary", "Other"];
    public taxLabels: string[] = ["Federal", "State", "Local"];
    public vendorLabels: string[] = [];
    public title: string = "Gender";

    employeeData: any [] = [];
    vendorData: any [] = [];
    taxData: any [] = [];

    //Empty State Info Data
    emptyStateInfoData = {
      icon: '',
      color: 'success',
      value: '$0.00',
      title: 'Tax Liability',
      tooltip: 'Taxes are grouped together by whether they are federal, state, or local. Clicking on a piece of the chart will bring up a modal with the individual tax payments for that category.',
      showBottom: true
    };

    constructor(
      private analyticsApi: AnalyticsApiService,
      private dialog: MatDialog,
      private loadingSvc: LoadingDialogService,
      private messageService:DsMsgService,
    ) {}

    ngOnInit() {
      this.previousPayrollID = this.currentPayrollId;
      this.previousBreakdownType = this.breakdownType;
      //Tax Data
      this.analyticsApi.getPayrollTaxHistory(this.currentPayrollId).subscribe((taxes: any) => {
        if (taxes == null || taxes == [] || taxes.length <= 0){
          this.emptyState = true;
        }
        this.taxData = taxes;
        //Paycheck (Vendor and Employee) Data
        this.analyticsApi.getPayrollPaycheckHistory(this.currentPayrollId).subscribe((paychecks: any) => {
          this.vendorData = paychecks.filter(x => x.isVendor == true);
          this.employeeData = paychecks.filter(x => x.isVendor == false);
          //Initialize to the Tax Graph
          this.setTaxGraph(this.taxData);
          this.loaded = true;
        });
      });
    }

    ngOnChanges(){
      if (this.loaded == true){
        //Payroll Change
        if (this.previousPayrollID !== this.currentPayrollId){
          this.loaded = false;
          this.analyticsApi.getPayrollTaxHistory(this.currentPayrollId).subscribe((taxes: any) => {
            this.taxData = taxes;

            this.analyticsApi.getPayrollPaycheckHistory(this.currentPayrollId).subscribe((paychecks: any) => {
              this.vendorData = paychecks.filter(x => x.isVendor == true);
              this.employeeData = paychecks.filter(x => x.isVendor == false);
              this.loaded = true;
              if (this.breakdownType == "Tax Liability"){
                this.setTaxGraph(this.taxData);
                this.previousBreakdownType = "Tax Liability";
              }
              if (this.breakdownType == "Vendor Payments"){
                this.setVendorGraph(this.vendorData);
                this.previousBreakdownType = "Vendor Payments";
              }
              if (this.breakdownType == "Employee Payments"){
                this.setEmployeeGraph(this.employeeData);
                this.previousBreakdownType = "Employee Payments";
              }
              this.previousPayrollID = this.currentPayrollId;
            });
          });
        }
        //Breakdown Change
        if (this.previousBreakdownType != this.breakdownType){
          if (this.breakdownType == "Tax Liability"){
            this.setTaxGraph(this.taxData);
            this.previousBreakdownType = "Tax Liability";
          }
          if (this.breakdownType == "Vendor Payments"){
            this.setVendorGraph(this.vendorData);
            this.previousBreakdownType = "Vendor Payments";
          }
          if (this.breakdownType == "Employee Payments"){
            this.setEmployeeGraph(this.employeeData);
            this.previousBreakdownType = "Employee Payments";
          }
        }
      }
    }

    setEmployeeGraph(employees){
      this.emptyState = false;
      this.bankDepositData = employees;
      //Format the data
      let hourly = employees.filter(x => x.employeePayType == "Hourly");
      let hourlySum = this.sumEmployeePay(hourly);
      let salary = employees.filter(x => x.employeePayType == "Salary");
      let salarySum = this.sumEmployeePay(salary);
      let other = employees.filter(x => x.employeePayType != "Hourly" && x.employeePayType != "Salary");
      let otherSum = this.sumEmployeePay(other);
      //Set the chart data
      this.pieChartData = [hourlySum, salarySum, otherSum];
      let total = this.pieChartData.reduce(function (a, b){ return a + b });
      this.infoData = {
        icon: '',
        color: 'success',
        value: `${total.toLocaleString('en-US', {style:'currency', currency:'USD'})}`,
        title: `${this.breakdownType}`,
        tooltip: 'Employee payments are grouped together by whether they are hourly, salary, or other. Clicking on a piece of the chart will bring up a modal with the individual employee payments for that category.',
        showBottom: true
      };
      this.concatLabels(this.employeeLabels, this.pieChartData);
    }

    setVendorGraph(vendors){
      this.emptyState = false;
      this.bankDepositData = vendors;
      //Filter out Tax Vendors
      vendors = vendors.filter(x => x.vendorTypeId != 2);
      //Format the data
      let uniqueVendors = this.uniqueBy(vendors, "name");
      uniqueVendors = uniqueVendors.filter(x => x != "Tax Management Horizon Bank");
      let tempData: number[] = [];
      this.vendorLabels = [];
      uniqueVendors.forEach(vendorName => {
        let sum = 0;
        let loop = vendors.filter(x => x.name == vendorName);
        loop.forEach(element => {
          sum += element.netPay;
        });
        tempData.push(sum);
        this.vendorLabels.push(vendorName);
      });
      //Set the chart data
      let total = tempData.reduce(function (a, b){ return a + b });
      this.infoData = {
        icon: '',
        color: 'success',
        value: `${total.toLocaleString('en-US', {style:'currency', currency:'USD'})}`,
        title: `${this.breakdownType}`,
        tooltip: 'Vendor payments are grouped together by vendor. Clicking on a piece of the chart will bring up a modal with the individual vendor payments made to that vendor.',
        showBottom: true
      };
      this.pieChartData = tempData;
      this.concatLabels(this.vendorLabels, this.pieChartData);
    }

    setTaxGraph(taxinfo){
        this.emptyState = false;
        this.bankDepositData = taxinfo;
        //Format the data
        let federalSum = this.sumFederalTaxes(taxinfo.federalTaxes);
        let stateSum = this.sumStateTaxes(taxinfo.stateTaxes);
        let localSum = this.sumLocalTaxes(taxinfo.localTaxes);
        //Set the chart data
        this.pieChartData = [federalSum, stateSum, localSum];
        let total = federalSum + stateSum + localSum;
        this.infoData = {
          icon: '',
          color: 'success',
          value: `${total.toLocaleString('en-US', {style:'currency', currency:'USD'})}`,
          title: `${this.breakdownType}`,
          tooltip: 'Taxes are grouped together by whether they are federal, state, or local. Clicking on a piece of the chart will bring up a modal with the individual tax payments for that category.',
          showBottom: true
        };
        this.concatLabels(this.taxLabels, this.pieChartData);
    }

    sumEmployeePay(array){
      //Sum the Net Pay + Deduction Amount
      return array.reduce(function (total, currentValue){
        return total + currentValue.netPay + currentValue.employeeDeductionAmount;
      }, 0);
    }

    sumFederalTaxes(array){
      return array.reduce(function (total, currentValue){
        return total
          + currentValue.amount
          + currentValue.ssDefermentTax
          + currentValue.ffcraCredit;
      }, 0);
    }

    sumStateTaxes(array){
      return array.reduce(function (total, currentValue){
        return total
          + currentValue.amount;
      }, 0);
    }

    sumLocalTaxes(array){
      return array.reduce(function (total, currentValue){
        return total
          + currentValue.amount;
      }, 0);
    }

    uniqueBy(arr, prop){
      return arr.reduce((a, d) => {
          if (!a.includes(d[prop])) { a.push(d[prop]); }
          return a;
      }, []);
    }

    concatLabels(Labels: String[], data){
      if (this.pieChartLabels.length > 0){
        this.pieChartLabels.splice(Labels.length-1);
      }
      for (var x = 0; x < Labels.length;  x++){
        this.pieChartLabels[x] = `${data[x].toLocaleString('en-US', {style:'currency', currency:'USD'})} ` + Labels[x];
      }
    }

    openDialog(event) {
      if (event.active[0] != undefined){
        this.loadingSvc.showDialog();
        switch(this.breakdownType){
          case 'Employee Payments':
            this.analyticsApi.getPaycheckYTDQTDMTDTotals(this.currentPayrollId).subscribe((paychecks: any) => {
              this.loadingSvc.hideDialog();
              let employeeConfig = new MatDialogConfig<IBankDepositData>();
              this.employeeData = paychecks.filter(x => x.isVendor == false);

              employeeConfig.data = {
                  bankDepositData: this.employeeData.filter(x => x.employeePayType == this.employeeLabels[event.active[0]._index]),
                  featureName: this.breakdownType,
                  dateRange: this.dateRange,
                  checkDate: this.currentCheckDate
                };

              employeeConfig.width = "1000px";
              return this.dialog.open<BankDepositDialogComponent, IBankDepositData, null> (BankDepositDialogComponent, employeeConfig);
            }, (error: HttpErrorResponse) => {
              this.messageService.showWebApiException(error.error);
              this.loadingSvc.hideDialog();
            });
            return

          case 'Vendor Payments':
            this.analyticsApi.getPaycheckYTDQTDMTDTotals(this.currentPayrollId).subscribe((paychecks: any) => {
              this.loadingSvc.hideDialog();
              let vendorConfig = new MatDialogConfig<IBankDepositData>();
              let vd = paychecks.filter(x => x.isVendor == true);

              vendorConfig.data = {
                  bankDepositData: vd.filter(x => x.name == this.vendorLabels[event.active[0]._index]),
                  featureName: this.breakdownType,
                  dateRange: this.dateRange,
                  checkDate: this.currentCheckDate
                };

              vendorConfig.width = "1000px";
              return this.dialog.open<BankDepositDialogComponent, IBankDepositData, null> (BankDepositDialogComponent, vendorConfig);
            }, (error: HttpErrorResponse) => {
              this.messageService.showWebApiException(error.error);
              this.loadingSvc.hideDialog();
            });
            return

          case 'Tax Liability':
            this.analyticsApi.getYTDQTDMTDTaxTotals(this.currentPayrollId).subscribe((taxes: any) => {
              this.loadingSvc.hideDialog();
              let taxConfig = new MatDialogConfig<IBankDepositData>();

              taxConfig.data = {
                  bankDepositData: this.openTaxes(taxes, event.active[0]._index),
                  featureName: this.breakdownType,
                  dateRange: this.dateRange,
                  checkDate: this.currentCheckDate
                };

              taxConfig.width = "1000px";
              return this.dialog.open<TaxBankDepositDialogComponent, IBankDepositData, null> (TaxBankDepositDialogComponent, taxConfig);
            }, (error: HttpErrorResponse) => {
              this.messageService.showWebApiException(error.error);
              this.loadingSvc.hideDialog();
            });
            return
        }
      }
    }

    openTaxes(obj, index){
      if (index == 0){
        return obj.federalTaxes;
      }
      else if (index == 1){
        return obj.stateTaxes;
      }
      else if (index == 2){
        return obj.localTaxes;
      }
      else{
        return [];
      }
    }
}


