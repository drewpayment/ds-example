import { Component, OnInit, Input } from '@angular/core';
import { InfoData } from '@ds/analytics/shared/models/InfoData.model';
import { DateRange } from '@ds/analytics/shared/models/DateRange.model';
import { AnalyticsApiService } from '@ds/analytics/shared/services/analytics-api.service';
import { ChartType, ChartOptions } from 'chart.js';
import * as moment from 'moment';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { EmployeeTermination } from '@ds/analytics/shared/models/TerminationData.model';
import { TerminatedEmployeesDialogComponent } from './terminated-employees-dialog/terminated-employees-dialog.component';
import { ITerminationData } from '@ds/analytics/shared/models/ITerminationData.model';
import { AccountService } from '@ds/core/account.service';
import { MOMENT_FORMATS } from '@ds/core/shared';

@Component({
  selector: 'ds-terminated-employees',
  templateUrl: './terminated-employees.component.html',
  styleUrls: ['./terminated-employees.component.css']
})
export class TerminatedEmployeesComponent implements OnInit {

   @Input() dateRange: DateRange;
   @Input() employeeIds: Number[];

   //Dialog
   terminationData: EmployeeTermination[];

   //Widget
   cardType: string = "info"
   loaded: boolean;
   infoData: InfoData;
   emptyState = false;

   //Pie Chart
   pieChartOptions: ChartOptions = {
    responsive: true,
    tooltips: {
      callbacks: {
        label: (tooltipItem, data) => {
          return `${this.initialLabels[tooltipItem.index]}: ${((parseInt(this.pieChartLabels[tooltipItem.index]) / this.terminationData.length) * 100).toFixed(2)}%`;
        }
      }
    },

    legend: {
      position: 'right',
      tooltips: {
        callbacks: {
          label: (tooltipItem) => {
            return this.pieChartLabels[tooltipItem.index];
          },
        }
      },
      labels: {
        usePointStyle: true,
        padding: 15,
      }
    } as any,
    plugins: {
      datalabels: {
        formatter: (value, ctx) => {
          const label = ctx.chart.data.labels[ctx.dataIndex];
          return label;
        },
      },
    }
  };
   initialLabels: string[] = [];
   moreLabel: string[] = [];
   pieChartLabels: string[] = [];
   public pieChartData: number[] = [];
   pieChartType: ChartType = 'pie';
   pieChartLegend = true;
   pieChartColors = [
    {
      backgroundColor: ['#fc621a', '#da2121', '#ffb627', '#ff821c', '#ff4949', '#b04001', '#ffd73b', '#f53c05', ],
    },
  ];


  constructor(private analyticsApi: AnalyticsApiService, private accountService: AccountService, private dialog: MatDialog) { }

  ngOnInit() {
    this.accountService.getUserInfo().subscribe((user) =>
      this.analyticsApi.GetTerminationInformation(user.clientId, moment(this.dateRange.StartDate).format(MOMENT_FORMATS.DATE), moment(this.dateRange.EndDate).format(MOMENT_FORMATS.DATE), this.employeeIds, user.userId, user.userTypeId)
      .subscribe((data:any) => {
        //Check Empty State
        if (data.data.terminatedEmployees.length <= 0){
          this.emptyState = true;
        }

        //dialog
        this.terminationData = data.data.terminatedEmployees;

        //piechart
        let resultList= data.data.terminatedEmployees.map((x) => x.terminationReason);
        this.setData(resultList);

        this.infoData = {
          icon: 'people',
          color: 'danger',
          value: data.data.terminatedEmployees.length.toLocaleString("en"),
          title: 'Terminated Employees',
          showBottom: true
        };
        this.loaded = true;
      })
    )
  }

  setData(resultList){
    //Count each instance => {key:'reason', val: # of instances}
    var counts = {};
    resultList.forEach(function(x) { counts[x] = (counts[x] || 0)+1; });

    //Sort the keys(reason) by the value(amount)
    var keysSorted = [];
    keysSorted = Object.keys(counts).sort(function(a,b){return counts[b]-counts[a]});

    //Sort the values and check for nulls; Take the top 7, then add the rest to "more"
    var valsSorted = [];
    var more = 0;
    for (var x = 0; x < keysSorted.length; x++){
      if (x < 7){
        valsSorted.push(counts[keysSorted[x]]);
      }
      else {
        //Add the labels to moreLabel for dialog popup
        this.moreLabel.push(keysSorted[x]);
        //Add their values to a "more" total
        more += counts[keysSorted[x]];
      }
    }

    if (keysSorted.length > 7){
      //At the very end add the "more" total
      valsSorted.push(more);
      //Trim off excess labels and push the "more" label
      keysSorted.splice(7);
      keysSorted.push("More");
    }

    //Apply sorted vals to data
    this.pieChartData = valsSorted;

    //Apply to labels
    keysSorted.forEach(key => {
      this.initialLabels.push(key);
    });

    //Concat data and labels for Legend
    this.concatLabels(this.initialLabels, this.pieChartData);
  }

  concatLabels(Labels: String[], data){
    for (var x = 0; x < Labels.length;  x++){
      if (Labels[x] == "null"){
        Labels[x] = "Not Specified";
      }
      this.pieChartLabels[x] = data[x]+" "+Labels[x];
    }
  }

  openDialog(event){
    if (event.active[0] != undefined){
      var config = new MatDialogConfig<ITerminationData>();

      config.data = {
          TerminationData: this.terminationData.filter( x => this.reasonCheck(x.terminationReason, event.active[0]._index)),
          featureName: this.terminationData.filter( x => this.reasonCheck(x.terminationReason, event.active[0]._index))[0].terminationReason,
          dateRange: this.dateRange
        }
        config.width = '1000px';
      const dialogRef = this.dialog.open<TerminatedEmployeesDialogComponent, ITerminationData, null> (TerminatedEmployeesDialogComponent, config);
    };
  }

    reasonCheck(reason, index): Boolean{
      if (reason == null){
        reason = "Not Specified"
      }
      //If "more" is clicked, filter all of the options that are put in "more"
      if (this.initialLabels[index] == "More"){
        for (var x = 0; x < this.moreLabel.length; x++){
          if (this.moreLabel[x] == reason){
            return true;
          }
        }
        return false;
      }
      else {
        if (reason == this.initialLabels[index]){
          return true;
        }
        else{
          return false;
        }
      }
    }
}
