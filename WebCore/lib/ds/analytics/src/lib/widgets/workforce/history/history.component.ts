import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { DateRange } from '@ds/analytics/shared/models/DateRange.model';
import { AnalyticsApiService } from '@ds/analytics/shared/services/analytics-api.service';
import { AccountService } from '@ds/core/account.service';
import * as moment from 'moment';
import { TerminationData } from '@ds/analytics/shared/models/TerminationData.model';
import { Color, BaseChartDirective } from 'ng2-charts';
import { ChartOptions, ChartDataSets } from "chart.js";
import { HistoryDialogComponent } from './history-dialog/history-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { HeaderDropDownOption } from "@ds/analytics/shared/models/HeaderDropDownOption.model";

@Component({
  selector: 'ds-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit {
  @Input() dateRange: DateRange;
  @ViewChild(BaseChartDirective,{static: false}) chart: BaseChartDirective;

  title: string = 'History';
  cardType = "graph";
  loaded: boolean = false;
  emptyState: boolean = false;
  rolling12Months: boolean = true;
  availableChartTypes: String[] = ['line', 'bar'];
  headerDropdownOptions: HeaderDropDownOption[];
  settingsItems: string[] = [];

  historyData: TerminationData[] = [];
  currentDate: Date = new Date();
  yearsDisplayed: number[] = [];
  yearsWithData: number[] = [];
  numYears: number = 0;
  dataType: String = "Turnover";
  public allMonths: String[] = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

  public chartData: any[] = [[]];
  public chartLabels: String[] = [];
  public chartOptions: (ChartOptions & { annotation?: any }) = {
    responsive: true,
    tooltips: {
      callbacks: {
        label: (tooltipItem, data) => {
          return tooltipItem.xLabel + ': ' + parseFloat((tooltipItem.yLabel as any)).toFixed(2) + '%';
        },
        title: function () {
          return "";
        }
      }
    },
    legend: {
      position: 'right',
      labels: { usePointStyle: true, fontSize: 15 }
    },
    scales: {
      xAxes: [{}],
      yAxes: [{
          id: 'y-axis-0',
          position: 'left',
          ticks: {
            callback: function(value) { return value + '%' }
          }
      }]
    },
    elements: {
      line: { fill: false, tension: 0 }
    }
  };
  public chartColors: Color[] = [
    {borderColor: '#04B5B0', backgroundColor: '#04B5B0'},
    {borderColor: '#324B68', backgroundColor: '#324B68'},
    {borderColor: '#CAEC73', backgroundColor: '#CAEC73'},
    {borderColor: '#957ACD', backgroundColor: '#957ACD'},
    {borderColor: '#4CA7CF', backgroundColor: '#4CA7CF'},
  ];
  public chartLegend = true;
  public chartType = 'line';

  constructor(private analyticsApi: AnalyticsApiService, private accountService: AccountService, private dialog: MatDialog) { }

  ngOnInit() {
    this.setHeaderDropDownOptions();
    this.setLabels();
    this.loadData();
  }

  loadData() {
    this.historyData.length = 0;
    this.accountService.getUserInfo().subscribe((user) => {
        this.analyticsApi.GetHistoryInformation(user.clientId).subscribe((turnoverData: any) => {
          if (turnoverData.data == null) {
            this.emptyState = true;
          } else {
            this.historyData = turnoverData.data;
            this.setChartData();
          }
          this.loaded = true;
        })
      })
  }

  setChartData() {
    this.chartData = [[]];
    var nums: number[][] = new Array(this.yearsDisplayed.length+1).fill(null).map(() => new Array(0));
    let obsDate = new Date();
    obsDate.setDate(1);
    var tmome = moment(obsDate);
    tmome.subtract(this.historyData.length-1, 'months');
    obsDate = tmome.toDate();
    var usedYears: number[] = [];
    this.yearsWithData = [];

    if(!this.historyData || this.historyData.length == 0 ) this.numYears = 1;
    else{
      let oldestDate = moment(this.historyData[this.historyData.length-1].monthStartDate).toDate();
      this.numYears = this.currentDate.getFullYear() - oldestDate.getFullYear() + 1;
    }

    for (var i = 0; i < this.numYears; i++)
      this.yearsWithData.push(this.currentDate.getFullYear()-i);

    if (this.yearsWithData.length < 2)
      this.settingsItems = ['Rolling 12 Months'];
    else
      this.settingsItems = ['Rolling 12 Months', 'Select Years to Compare'];

    if (this.rolling12Months) {
      this.emptyState = true;
      for (var i = 0; i <= 12; i++) {
        if (this.dataType == "Turnover") {
          nums[0][12-i] =  Number(this.historyData[i].turnoverRate);
          if (nums[0][12-i] != 0)
            this.emptyState = false;
        }
        if (this.dataType == "Retention") {
          nums[0][12-i] =  Number(this.historyData[i].retentionRate);
          if (nums[0][12-i] != 0 && nums[0][12-i] != 100)
            this.emptyState = false;
        }
        if (this.dataType == "Growth") {
          nums[0][12-i] =  Number(this.historyData[i].growthRate);
          if (nums[0][12-i] != 0)
            this.emptyState = false;
        }
      }
      this.chartData[0] = {data: nums[0]};
      this.chartOptions = {
        responsive: true,
        tooltips: {
          callbacks: {
            label: (tooltipItem, data) => {
              return tooltipItem.xLabel + ': ' + parseFloat((tooltipItem.yLabel as any)).toFixed(2) + '%';
            },
            title: function () {
              return "";
            }
          }
        },
        legend: { display: false },
        scales: {
          xAxes: [{}],
          yAxes: [{
              id: 'y-axis-0',
              position: 'left',
              ticks: {
                callback: function(value) { return value + '%' }
              }
          }]
        },
        elements: {
          line: { fill: false, tension: 0 }
        }
      };
      if (!this.emptyState)
      this.forceChartRefresh();
    }
    else {
      this.historyData.slice().reverse().forEach(e => {
        if (this.yearsDisplayed.includes(obsDate.getFullYear())) {
          if (!usedYears.includes(obsDate.getFullYear()))
            usedYears.push(obsDate.getFullYear());
          if (this.dataType == "Turnover") {
            nums[usedYears.length-1][obsDate.getMonth()] = Number(e.turnoverRate);
          }
          if (this.dataType == "Retention") {
            nums[usedYears.length-1][obsDate.getMonth()] = Number(e.retentionRate);
          }
          if (this.dataType == "Growth") {
            nums[usedYears.length-1][obsDate.getMonth()] = Number(e.growthRate);
          }
        }
        obsDate.setMonth(obsDate.getMonth() + 1);
      })
      for (var i = 0; i < this.yearsDisplayed.length; i++) {
        var tempData: ChartDataSets = {data: nums[i], label: this.yearsDisplayed[i] as any}
        this.chartData[i] = tempData;
      }
      this.chartOptions = {
        responsive: true,
        tooltips: {
          callbacks: {
            label: (tooltipItem, data) => {
                return tooltipItem.xLabel + ': ' + parseFloat(tooltipItem.yLabel as any).toFixed(2) + '%';
            },
            title: function () {
              return "";
            }
          }
        },
        legend: {
          position: 'right',
          labels: { usePointStyle: true, fontSize: 15 }
        },
        scales: {
          xAxes: [{}],
          yAxes: [{
              id: 'y-axis-0',
              position: 'left',
              ticks: {
                callback: function(value) { return value + '%' }
              }
          }]
        },
        elements: {
          line: { fill: false, tension: 0 }
        }
      };
      this.forceChartRefresh();
    }
  }

  setLabels() {
    if (!this.rolling12Months)
      this.chartLabels = this.allMonths;
    else {
      this.chartLabels = [];
      var y = this.currentDate.getUTCFullYear()-1;
      var m = this.currentDate.getMonth();
      for (var i = 0; i <= 12; i++) {
        if (m > 11) {
          y++;
          m = 0;
        }
        this.chartLabels.push(this.allMonths[m] + " " + y);
        m++;
      }
    }
  }

  setHeaderDropDownOptions(){
    this.headerDropdownOptions = [
      {id: "Turnover", val: "Turnover"},
      {id: "Retention", val: "Retention"},
      {id: "Growth", val: "Growth"},
    ]
  }

  headerDropdownChanged(event) {
    this.dataType = event.value;
    if (this.historyData.length > 0)
      this.setChartData();
  }

  chartChanged(event) {
    this.chartType = event.value;
  }

  settingSelected(event) {
    if (event.value == "Rolling 12 Months" && !this.rolling12Months) {
      this.rolling12Months = true;
      this.loaded = false;
      this.setLabels();
      this.loadData();
    }
    if (event.value == "Select Years to Compare") {
      this.rolling12Months = false;
      var config = {
        width: '500px',
        data: {
          yearsWithData: this.yearsWithData,
          yearsDisplayed: this.yearsDisplayed
        }
      };
      const dialogRef = this.dialog.open(HistoryDialogComponent, config);
      dialogRef.afterClosed().subscribe((res) => {
        if (res != null) {
          this.loaded = false;
          this.yearsDisplayed = res;
          this.setLabels();
          this.loadData();
        }
      })
    }
  }

  forceChartRefresh() {
    if (this.chart.chart != undefined){
      setTimeout(() => {
          this.chart.ngOnInit();
      }, 10);
    }
  }
}
