import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { DateRange } from '@ds/analytics/shared/models/DateRange.model';

import { MatDialog, MatDialogConfig } from '@angular/material/dialog';

import { AnalyticsApiService } from '@ds/analytics/shared/services/analytics-api.service';
import { AccountService } from '@ds/core/account.service';
import { EmployeePointTotal } from '@ds/analytics/shared/models/EmployeePointTotal.model';

import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import { Color, BaseChartDirective } from 'ng2-charts';
import { PointsTotalsDialogComponent } from './points-totals-dialog/points-totals-dialog.component';
import { IScheduledWorkedData } from '@ds/analytics/shared/models/IScheduledWorkedData.model';
import { HeaderDropDownOption } from '@ds/analytics/shared/models/HeaderDropDownOption.model';

@Component({
  selector: 'ds-points-totals',
  templateUrl: './points-totals.component.html',
  styleUrls: ['./points-totals.component.css']
})
export class PointsTotalsComponent implements OnInit {

  @Input() employeeIds: Number[];
  @Input() dateRange: DateRange;

  @ViewChild(BaseChartDirective,{static: false}) chart: BaseChartDirective;

  title: string = 'Point Totals: ';
  cardType = "graph";
  headerDropdownOptions: HeaderDropDownOption[] =
  [
    {id: "Department", val: "Department"},
    {id:"Cost Center", val: "Cost Center"},
    {id:"Supervisor", val:"Supervisor"}
  ];
  loaded: boolean;

  //Chart Variables
  public barChartData: ChartDataSets[] = [];
  public barChartLabels: String[];
  public pieChartLegend = false;
  public chartColors: Color[] = [
    {borderColor: '#ffb627', backgroundColor: '#ffb627'}
  ];
  public barChartOptions: ChartOptions = {
    responsive: true,
    tooltips: {
      callbacks: {
        label: (tooltipItem) => {
          return `Points: ${parseFloat(parseFloat(tooltipItem.value).toFixed(2))}`;
        }
      }
    },
    scales: { xAxes: [{}], yAxes: [{}] },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    }
  };
  public barChartType: ChartType = 'bar';

  currentView: string;
  employeePointTotals: EmployeePointTotal[] = [];
  employeePointTotalsObjects = [];
  employees = [];

  constructor(private analyticsApi: AnalyticsApiService, private accountService: AccountService, private dialog: MatDialog) { }

  ngOnInit() {
    this.accountService.getUserInfo().subscribe((user) => {
      this.analyticsApi.GetEmployeePointTotals(user.clientId, this.dateRange.StartDate, this.dateRange.EndDate, this.employeeIds).subscribe((data) => {
        this.employeePointTotals = data;
        if (this.employeePointTotals.length >= 1){
          this.combineEmployees();
        }
        if(this.employeePointTotals.length != 0){
          this.headerDropdownChanged({ event: 'Header', value: this.headerDropdownOptions[0].id })
        }
        this.loaded = true;
      })
    })
  }

  combineEmployees(){
    var employees = this.employeePointTotals.map(x => `${x.lastName}, ${x.firstName}`);
    var point = this.employeePointTotals.map(x => x.amount);
    var hour = this.employeePointTotals.map(x => x.hours);
    var department = this.employeePointTotals.map(x => x.department);
    var costCenter = this.employeePointTotals.map(x => x.employee.costCenter.description);
    var supervisor = this.employeePointTotals.map(x => x.supervisor);
    var clientDepartmentId = this.employeePointTotals.map(x => x.clientDepartmentId);
    var clientCostCenterId = this.employeePointTotals.map(x => x.employee.costCenter.clientCostCenterId);
    var directSupervisorId = this.employeePointTotals.map(x => x.employee.directSupervisorId)
    var tempEE = Array.from(new Set(employees));
    var obj = [];
    for (var i = 0; i < this.employeePointTotals.length; i++){
      for (var j = 0; j < tempEE.length; j++){
        if (`${this.employeePointTotals[i].lastName}, ${this.employeePointTotals[i].firstName}` === tempEE[j]){
          obj.push({
            name: tempEE[j],
            amount: point[i],
            hours: hour[i],
            department: department[i],
            costCenter: costCenter[i],
            supervisor: supervisor[i],
            clientDepartmentId: clientDepartmentId[i],
            clientCostCenterId: clientCostCenterId[i],
            directSupervisorId: directSupervisorId[i]
          });
        }
      }
    }

    var temp = [];
    var points: number = obj[0].amount;
    var hours: number = obj[0].hours;

    for (var i = 0; i < obj.length; i++){
      if (i == 0) continue;
      if (i != 0 && obj[i].name === obj[i-1].name){
        points = points + obj[i].amount;
        hours = hours + obj[i].hours;
      }
      if ((i != 0 && obj[i].name != obj[i-1].name) || (i == obj.length-1 && obj[i].name === obj[i-1].name)){
        temp.push({
          name: obj[i-1].name,
          amount: points,
          hours: hours,
          department: obj[i].department,
          costCenter: obj[i].costCenter,
          supervisor: obj[i].supervisor,
          clientDepartmentId: obj[i].clientDepartmentId,
          clientCostCenterId: obj[i].clientCostCenterId,
          directSupervisorId: obj[i].directSupervisorId
        })
        points = obj[i].amount;
        hours = obj[i].hours;
      }
    }

    this.employeePointTotals = temp;
  }

  headerDropdownChanged(event) {
    //Get event option. Create switch statement to order the array based on those options
    this.currentView = event.value;
    this.employeePointTotalsObjects = [];

    this.barChartData = [];
    this.barChartLabels = [];

    switch(event.value){
      case 'Department':
        var departments = [];

        this.employeePointTotals.forEach(e => {
          if(!departments.includes(e.clientDepartmentId)){
            departments.push(e.clientDepartmentId)
          }
        });

        for(var i = 0; i < this.employeePointTotals.length; i++){
          if (this.employeePointTotals[i].department === "") this.employeePointTotals[i].department = "Not Specified";
        }

        departments.forEach(d => {
          var departmentName = this.employeePointTotals.filter(x => x.clientDepartmentId == d)[0].department;
          this.employeePointTotalsObjects.push(
            {
              key: departmentName ? departmentName : 'Not Specified',
              amount: this.employeePointTotals.filter(x => x.clientDepartmentId == d).map(x => x.amount).reduce((prev, next) => { return prev + next }, 0)
            }
          )
        });
        break;

      case 'Cost Center':
        var costCenters = [];

        this.employeePointTotals.forEach(e => {
          if(!costCenters.includes(e.clientCostCenterId)){
            costCenters.push(e.clientCostCenterId)
          }
        });

        for(var i = 0; i < this.employeePointTotals.length; i++){
          if (this.employeePointTotals[i].costCenter === "") this.employeePointTotals[i].costCenter = "Not Specified";
        }

        costCenters.forEach(c => {
          var costCenterName = this.employeePointTotals.filter(x => x.clientCostCenterId == c)[0].costCenter;
          this.employeePointTotalsObjects.push(
            {
              key: costCenterName ? costCenterName : 'Not Specified',
              amount: this.employeePointTotals.filter(x => x.clientCostCenterId == c).map(x => x.amount).reduce((prev, next) => { return prev + next }, 0)
            }
          )
        });
        break;

      case 'Supervisor':
        var supervisors = [];

        this.employeePointTotals.forEach(e => {
          if(!supervisors.includes(e.directSupervisorId)){
            supervisors.push(e.directSupervisorId)
          }
        });

        for(var i = 0; i < this.employeePointTotals.length; i++){
          if (this.employeePointTotals[i].supervisor === "") this.employeePointTotals[i].supervisor = "Not Specified";
        }

        supervisors.forEach(s => {
          var supervisor = this.employeePointTotals.filter(x => x.directSupervisorId == s)[0].directSupervisorId;
          if (supervisor === 0) var supervisorName  = "Not Specified";
          if (supervisor !== 0) var supervisorName = this.employeePointTotals.filter(x => x.directSupervisorId == s)[0].supervisor;

          this.employeePointTotalsObjects.push({
            key: supervisorName,
            amount: this.employeePointTotals.filter(x => x.directSupervisorId == s).map(x => x.amount).reduce((prev, next) => { return prev + next }, 0)
          })
        });
        break;
    }

    this.barChartLabels = this.employeePointTotalsObjects.map(x => x.key);
    this.barChartData.push({ data: this.employeePointTotalsObjects.map(x => x.amount), label: 'Points' })
    this.refreshChart();
  }

  refreshChart(){
    if(this.chart !== undefined){
      this.chart.chart.destroy();
      (this.chart.chart as any) = 0;

      this.chart.datasets = this.barChartData;
      this.chart.labels = this.barChartLabels as any;
      this.chart.options = this.barChartOptions;
      this.chart.colors = this.chartColors;
      this.chart.ngOnInit();
    }
  }

  openDialog(event){
    if(event.active[0] == undefined) return;

    var labelClicked = event.active[0]._model.label;
    let config = new MatDialogConfig<any>();

    if(labelClicked == 'Not Specified'){
      labelClicked = 'Not Specified'
    }

    switch(this.currentView){
      case 'Department':
        config.data = {
          dataObjects: this.employeePointTotals.filter(x => x.department == labelClicked),
          featureName: labelClicked ? this.currentView + ': ' + labelClicked : this.currentView + ': ' + 'Not Specified',
          dateRange: this.dateRange
        }
        config.width = "1000px";
        return this.dialog.open<PointsTotalsDialogComponent>(PointsTotalsDialogComponent, config);
      case 'Cost Center':
        config.data = {
          dataObjects: this.employeePointTotals.filter(x => x.costCenter == labelClicked),
          featureName: labelClicked ? this.currentView + ': ' + labelClicked : this.currentView + ': ' + 'Not Specified',
          dateRange: this.dateRange
        }
        config.width = "1000px";
        return this.dialog.open<PointsTotalsDialogComponent>(PointsTotalsDialogComponent, config);
      case 'Supervisor':
        config.data = {
          dataObjects: this.employeePointTotals.filter(x => x.supervisor == labelClicked),
          featureName: labelClicked ? this.currentView + ': ' + labelClicked : this.currentView + ': ' + 'Not Specified',
          dateRange: this.dateRange
        }
        config.width = "1000px";
        return this.dialog.open<PointsTotalsDialogComponent>(PointsTotalsDialogComponent, config);
    }
   }
}
