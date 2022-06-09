import { Component, OnInit, Input, ViewChild } from "@angular/core";
import { ChartOptions, ChartType, ChartDataSets } from "chart.js";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { AnalyticsApiService } from "@ds/analytics/shared/services/analytics-api.service";
import { AccountService } from "@ds/core/account.service";
import { MatDialogConfig, MatDialog } from "@angular/material/dialog";
import { IScheduledWorkedData } from "@ds/analytics/shared/models/IScheduledWorkedData.model";
import { ScheduledVsWorkedHoursDialogComponent } from "./scheduled-vs-worked-hours-dialog/scheduled-vs-worked-hours-dialog.component";
import { HeaderDropDownOption } from "@ds/analytics/shared/models/HeaderDropDownOption.model";
import { BaseChartDirective } from 'ng2-charts';

@Component({
  selector: "ds-scheduled-vs-worked-hours",
  templateUrl: "./scheduled-vs-worked-hours.component.html",
  styleUrls: ["./scheduled-vs-worked-hours.component.css"],
})
export class ScheduledVsWorkedHoursComponent implements OnInit {
  @Input() employeeIds: Number[];
  @Input() dateRange: DateRange;
  @ViewChild(BaseChartDirective, { static: false }) chart: BaseChartDirective;

  //Widget
  cardType = "graph";
  title: string = "Scheduled vs Worked Hours";
  headerDropdownOptions: HeaderDropDownOption[] = [
    { id: "Department", val: "Department" },
    { id: "Cost Center", val: "Cost Center" },
    { id: "Supervisor", val: "Supervisor" },
  ];

  //Data
  loaded: boolean;
  data: any;
  emptyState: boolean = false;
  filterBy: string = "Department";
  yLabels: string[] = ["Scheduled", "Worked"];

  //Graph
  public barChartOptions: ChartOptions = {
    responsive: true,
    tooltips: {
      callbacks: {
        label: (tooltipItem) => {
          return `${this.yLabels[tooltipItem.datasetIndex]}: ${parseFloat(
            parseFloat(tooltipItem.value).toFixed(2)
          )}`;
        },
      },
    },
    legend: {
      labels: {
        usePointStyle: true,
        padding: 15,
      },
    },
    scales: {
      xAxes: [
        {
          ticks: {
            beginAtZero: true,
          },
        },
      ],
      yAxes: [
        {
          ticks: {
            beginAtZero: true,
            userCallback: function (value) {
              return value.toLocaleString();
            },
          } as any,
        },
      ],
    },
    plugins: {
      datalabels: {
        anchor: "end",
        align: "end",
      },
    },
  };
  public barChartLabels = [];
  public barChartType: ChartType = "bar";
  public barChartLegend = true;

  public barChartData: ChartDataSets[] = [];
  public barChartColors: Array<any> = [
    {
      // first color
      backgroundColor: "#6dbb2c",
      borderColor: "#fff",
      pointBackgroundColor: "rgba(225,10,24,0.2)",
      pointBorderColor: "#fff",
      pointHoverBackgroundColor: "#fff",
      pointHoverBorderColor: "rgba(225,10,24,0.2)",
    },
    {
      // second color
      backgroundColor: "#caec73",
      borderColor: "#fff",
      pointBackgroundColor: "rgba(225,10,24,0.2)",
      pointBorderColor: "#fff",
      pointHoverBackgroundColor: "#fff",
      pointHoverBorderColor: "rgba(225,10,24,0.2)",
    },
  ];

  constructor(
    private analyticsApi: AnalyticsApiService,
    private accountService: AccountService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.accountService.getUserInfo().subscribe((user) => {
      this.analyticsApi
        .GetGetClockEmployeeHoursComparison(
          user.clientId,
          this.dateRange.StartDate,
          this.dateRange.EndDate,
          this.employeeIds
        )
        .subscribe((data: any) => {
          if (data == null || data == [] || data.length <= 0) {
            this.loaded = true;
            this.emptyState = true;
          } else {
            this.data = data;
            this.sortCategory({ value: "Department" });
            this.loaded = true;
          }
        });
    });
  }

  sortCategory(event) {
    //Set Filter By for dialog
    this.filterBy = event.value;

    //Clear Data and Labels
    this.barChartLabels = [];
    this.barChartData = [];

    switch (event.value) {
      case "Department":
        var schedArray = [];
        var workArray = [];
        var schedCount = 0;
        var workCount = 0;
        var datArray = this.data.map((x) => x.departmentName);
        var unique = Array.from(new Set(datArray));
        unique.forEach((e) => {
          //Push the name as a label
          this.barChartLabels.push(e ? e : "Not Specified");
          //Filter the data by the name
          var filteredData = this.data.filter((x) => x.departmentName == e);
          //For each filtered data item, add the total scheduled and worked hours to their array's
          filteredData.forEach((element) => {
            //add totals to counter
            schedCount += element.hoursScheduled;
            workCount += element.actualHours;
          });
          schedArray.push(schedCount);
          workArray.push(workCount);
          //Reset the counters
          schedCount = 0;
          workCount = 0;
        });
        //Push the data as data: [totals array], label: [which array]
        this.barChartData.push({ data: schedArray, label: "Scheduled" });
        this.barChartData.push({ data: workArray, label: "Worked" });

        this.refreshChart();
        break;
      case "Cost Center":
        var schedArray = [];
        var workArray = [];
        var schedCount = 0;
        var workCount = 0;
        var costCenters = this.data.map((x) => x.costCenterCode);
        var uniquecostCenters = Array.from(new Set(costCenters));
        uniquecostCenters.forEach((e) => {
          //Push the name as a label
          this.barChartLabels.push(e ? e : "Not Specified");
          //Filter the data by the name
          var filteredData = this.data.filter((x) => x.costCenterCode == e);
          //For each filtered data item, add the total scheduled and worked hours to their array's
          filteredData.forEach((element) => {
            //add totals to counter
            schedCount += element.hoursScheduled;
            workCount += element.actualHours;
          });
          schedArray.push(schedCount);
          workArray.push(workCount);
          //Reset the counters
          schedCount = 0;
          workCount = 0;
        });
        //Push the data as data: [totals array], label: [which array]
        this.barChartData.push({ data: schedArray, label: "Scheduled" });
        this.barChartData.push({ data: workArray, label: "Worked" });

        this.refreshChart();
        break;
      case "Supervisor":
        var schedArray = [];
        var workArray = [];
        var schedCount = 0;
        var workCount = 0;
        var supervisors = this.data.map((x) => x.supervisorName);
        var uniqueSupervisors = Array.from(new Set(supervisors));
        uniqueSupervisors.forEach((e) => {
          //Push the name as a label
          this.barChartLabels.push(e ? e : "Not Specified");
          //Filter the data by the name
          var filteredData = this.data.filter((x) => x.supervisorName == e);
          //For each filtered data item, add the total scheduled and worked hours to their array's
          filteredData.forEach((element) => {
            //add totals to counter
            schedCount += element.hoursScheduled;
            workCount += element.actualHours;
          });
          schedArray.push(schedCount);
          workArray.push(workCount);
          //Reset the counters
          schedCount = 0;
          workCount = 0;
        });
        //Push the data as data: [totals array], label: [which array]
        this.barChartData.push({ data: schedArray, label: "Scheduled" });
        this.barChartData.push({ data: workArray, label: "Worked" });

        this.refreshChart();
        break;
    }
  }

  refreshChart() {
    if (this.chart !== undefined) {
      //Destroy the Chart
      this.chart.chart.destroy();
      (this.chart.chart as any) = 0;
      //Rebuild the Chart
      this.chart.datasets = this.barChartData;
      this.chart.labels = this.barChartLabels;
      this.chart.options = this.barChartOptions;
      this.chart.legend = this.barChartLegend;
      this.chart.colors = this.barChartColors;
      this.chart.ngOnInit();
    }
  }

  openDialog(event) {
    if (event.active[0] != undefined) {
      if (event.active[0]._model.label == "Not Specified") {
        event.active[0]._model.label = "";
      }
      switch (this.filterBy) {
        case "Department":
          let departmentconfig = new MatDialogConfig<IScheduledWorkedData>();
          departmentconfig.data = {
            dataObjects: this.data.filter(
              (x) => x.departmentName == event.active[0]._model.label
            ),
            featureName: this.data.filter(
              (x) => x.departmentName == event.active[0]._model.label
            )[0].departmentName
              ? this.data.filter(
                  (x) => x.departmentName == event.active[0]._model.label
                )[0].departmentName
              : "Not Specified",
            dateRange: this.dateRange,
          };
          departmentconfig.width = "1000px";
          return this.dialog.open<
            ScheduledVsWorkedHoursDialogComponent,
            IScheduledWorkedData,
            null
          >(ScheduledVsWorkedHoursDialogComponent, departmentconfig);

        case "Cost Center":
          let costcenterconfig = new MatDialogConfig<IScheduledWorkedData>();
          costcenterconfig.data = {
            dataObjects: this.data.filter(
              (x) => x.costCenterCode == event.active[0]._model.label
            ),
            featureName: this.data.filter(
              (x) => x.costCenterCode == event.active[0]._model.label
            )[0].costCenterCode
              ? this.data.filter(
                  (x) => x.costCenterCode == event.active[0]._model.label
                )[0].costCenterCode
              : "Not Specified",
            dateRange: this.dateRange,
          };
          costcenterconfig.width = "1000px";
          return this.dialog.open<
            ScheduledVsWorkedHoursDialogComponent,
            IScheduledWorkedData,
            null
          >(ScheduledVsWorkedHoursDialogComponent, costcenterconfig);

        case "Supervisor":
          let supervisorconfig = new MatDialogConfig<IScheduledWorkedData>();
          supervisorconfig.data = {
            dataObjects: this.data.filter(
              (x) => x.supervisorName == event.active[0]._model.label
            ),
            featureName: this.data.filter(
              (x) => x.supervisorName == event.active[0]._model.label
            )[0].supervisorName
              ? this.data.filter(
                  (x) => x.supervisorName == event.active[0]._model.label
                )[0].supervisorName
              : "Not Specified",
            dateRange: this.dateRange,
          };
          supervisorconfig.width = "1000px";
          return this.dialog.open<
            ScheduledVsWorkedHoursDialogComponent,
            IScheduledWorkedData,
            null
          >(ScheduledVsWorkedHoursDialogComponent, supervisorconfig);
      }
    }
  }
}
