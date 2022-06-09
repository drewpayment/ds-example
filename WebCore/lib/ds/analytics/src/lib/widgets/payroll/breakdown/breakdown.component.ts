import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { AnalyticsApiService } from '@ds/analytics/shared/services/analytics-api.service';
import { BaseChartDirective, Color } from 'ng2-charts';
import { ChartOptions } from "chart.js";
import { EarningsBreakdown } from '@ds/analytics/shared/models/EarningsBreakdown.model';
import { PayrollService } from '@ds/payroll/shared/payroll.service';
import { AccountService } from '@ds/core/account.service';
import * as moment from "moment";
import { Observable } from 'rxjs';
import { UserInfo } from '@ds/core/shared';
import { map, switchMap, tap } from 'rxjs/operators';

@Component({
  selector: 'ds-breakdown',
  templateUrl: './breakdown.component.html',
  styleUrls: ['./breakdown.component.css']
})
export class BreakdownComponent implements OnInit {
  @ViewChild(BaseChartDirective,{static: false}) chart: BaseChartDirective;
  private user: UserInfo;
  @Input() employeeIds: Number[];

  title: string = 'EARNINGS';
  cardType = "graph";
  loaded: boolean = false;
  emptyState: boolean = false;
  breakdownData: EarningsBreakdown[] = [];

  currentDate: Date = new Date();
  yearStartDate: Date;
  quarterStartDate: Date;
  allLabels: string[];
  allMonths: String[] = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
  currentFilter: string = "Last 12 Months";
  currentEarning: string = "Gross";

  availableChartTypes: String[] = ['line', 'bar'];
  headerDropdownOptions$: Observable<{ id: number, val: any }[]>;
  headerDropdownOptions: any[] = [{id: 'Gross', val: 'Gross'}];
  settingsItems: string[] = ['Last 12 Months', 'Year to Date', 'Quarter to Date'];

  public chartData: number[] = [];
  public chartLabels: String[] = [];
  public chartLegend = false;
  public chartType = 'line';
  dataType: String = "Gross";

  public chartOptions: (ChartOptions & { annotation?: any }) = {
    responsive: true,
    tooltips: {
      callbacks: {
        label: (tooltipItem, data) => {
          var label = new Date(this.allLabels[tooltipItem.index]).toString().split(' ')[0] + " " +
                      new Date(this.allLabels[tooltipItem.index]).toLocaleString('default', { month: 'short' }) + " " +
                      new Date(this.allLabels[tooltipItem.index]).getDate() + " " +
                      new Date(this.allLabels[tooltipItem.index]).getFullYear() + " " +
                      ': $' + tooltipItem.yLabel.toLocaleString('en-US');
          return label;
        },
        title: function () {
          return "";
        }
      }
    },
    scales: {
      xAxes: [{}],
      yAxes: [{
        id: 'y-axis-0',
        position: 'left',
        ticks: {
          beginAtZero: true,
          callback: function(value) { return `$${value.toLocaleString("en-US")}`;}
        }
      }]
    },
    elements: {
      line: { tension: 0 }
    }
  };
  public chartColors: Color[] = [
    {borderColor: '#7e52ae', backgroundColor: 'rgba(126, 82, 174, 0.5)'}];

  constructor(private accountService: AccountService, private analyticsApi: AnalyticsApiService, private payrollApi: PayrollService) { }

  ngOnInit() {
    let endDate = new Date();
    let startDate = new Date();
    startDate.setFullYear(startDate.getFullYear() - 1);

    this.headerDropdownOptions$ = this.accountService.getUserInfo()
      .pipe(
          tap(user => this.user = user),
          switchMap(user =>
            this.analyticsApi.GetEarningBreakdownByDateRange(user.clientId, startDate, endDate, this.employeeIds)),
          map(payrollBreakdown => {
            this.breakdownData = payrollBreakdown;

            this.breakdownData.forEach(x => {
              const p = {
                id: x.description,
                val: x.description
              }
              if (p.id != "Gross") this.headerDropdownOptions.push(p); //we hard code "Gross" in order to make it the first option. No need to add it again
              return p;
            });
            this.setQuarterStart();
            this.setYearStart();
            //Default: this.setData("Gross", "Last 12 Months")
            this.setData(this.currentEarning, this.currentFilter);
            this.loaded = true;
            return this.headerDropdownOptions;
          }),
      );
  }

  setData(earning, filter){
    this.chartData = [];
    this.chartLabels = [];
    let sum = 0;
    let checks = this.breakdownData.filter(x => x.description == earning);

    if (checks[0] != null){
      //Create a temporary object to prevent over-writing data when filtered
      let tempObj = { breakdownByPaydate: [],
                              clientEarningId: 0,
                              description: "",
                              payDateList: [],
                              totalAmount: 0,
                            };
      //Assign our data to the temp object and call it "payDateEarnings"
      const payDateEarnings = Object.assign(tempObj, checks[0]);

      //Check for filter options
      if (filter == "Year to Date"){
        let yStart = new Date(this.yearStartDate);
        payDateEarnings.breakdownByPaydate = payDateEarnings.breakdownByPaydate.filter( x => new Date(x.payDate) > yStart);
        payDateEarnings.payDateList = payDateEarnings.payDateList.filter( x => new Date(x) > yStart);
      }
      if (filter == "Quarter to Date"){
        let qStart = new Date(this.quarterStartDate);
        payDateEarnings.breakdownByPaydate = payDateEarnings.breakdownByPaydate.filter( x => new Date(x.payDate) > qStart);
        payDateEarnings.payDateList = payDateEarnings.payDateList.filter( x => new Date(x) > qStart);
      }

      //Add full dates to allLabels for custom hover tooltip
      this.allLabels = payDateEarnings.payDateList;

      //Loop through the paydates and add to chart data
      payDateEarnings.breakdownByPaydate.forEach(element => {
        //Sum is for empty state check
        sum += element.totalAmount;
        //Set Chart Data
        this.chartData.push(element.totalAmount);
      });
      //Set Chart Labels
      this.setLabels(payDateEarnings.payDateList);
    }

    //Check for Empty State
    if (sum == 0){
      this.emptyState = true;
      return;
    }
    else{
      this.emptyState = false;
    }
    //Force chart refresh so the labels can update
    this.forceChartRefresh();
  }

  setLabels(dates){
    let temp: string[] = [];
    for (var i=0; i < dates.length; i++){
      let date = "";
      let month = new Date(dates[i]);
      if (!temp.includes(month.toLocaleString('default', { month: 'short' }) + " " + month.getFullYear())){
        date = month.toLocaleString('default', { month: 'short' }) + " " + month.getFullYear();
        temp.push(date);
      }
      this.chartLabels[i] = date;
    }
  }

  setYearStart(){
    let today = new Date();
    this.yearStartDate = new Date(today.getFullYear(), 0, 1);
  }

  setQuarterStart(){
    this.analyticsApi.GetQuarterStartDates().subscribe((quarters: Date[]) => {
      let today = new Date();
      for (var i=quarters.length-1; i >= 0; i--){
        let quarter = new Date(quarters[i]);
        if (today.getTime() > quarter.getTime()){
          this.quarterStartDate = quarters[i];
          return;
        }
      }
    });
  }

  setDropdown() {
    //this.headerDropdownOptions = [{id: 'Gross', val: 'Gross'}];
    this.breakdownData.forEach(element => {
      if (!this.headerDropdownOptions.includes(element.description))
        this.headerDropdownOptions.push({ id: element.description, val: element.description});
    });
  }

  updateColors() {
    if (this.chartType == 'line')
      this.chartColors = [{borderColor: '#7e52ae', backgroundColor: 'rgba(126, 82, 174, 0.5)'}];
    else
      this.chartColors = [{borderColor: '#7e52ae', backgroundColor: '#7e52ae'}];
  }

  headerDropdownChanged(event) {
      this.currentEarning = event.value;
      this.setData(event.value, this.currentFilter);
  }

  chartChanged(event) {
    this.chartType = event.value;
    this.updateColors();
  }

  settingSelected(event) {
    if (this.currentFilter != event.value) {
      this.currentFilter = event.value;
      this.setData(this.currentEarning, this.currentFilter);
    }
  }

  forceChartRefresh() {
    setTimeout(() => {
        this.chart.ngOnInit();
    }, 10);
  }
}
