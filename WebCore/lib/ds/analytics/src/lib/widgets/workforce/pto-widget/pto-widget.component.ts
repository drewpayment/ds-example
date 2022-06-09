import { Component, OnInit, Input, ViewChild } from "@angular/core";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { ChartOptions, ChartType, ChartDataSets } from "chart.js";
import { BaseChartDirective } from "ng2-charts";
import { AnalyticsApiService } from "@ds/analytics/shared/services/analytics-api.service";
import { AccountService } from "@ds/core/account.service";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { PTOInfo } from "@ds/analytics/shared/models/Pto.model";
import { PtoWidgetDiaglogComponent } from "./pto-widget-diaglog/pto-widget-diaglog.component";
import { IPto } from "@ds/analytics/shared/models/IPto.model";
import * as moment from "moment";
import { MOMENT_FORMATS } from "@ds/core/shared";

@Component({
  selector: "ds-pto-widget",
  templateUrl: "./pto-widget.component.html",
  styleUrls: ["./pto-widget.component.css"],
})
export class PtoWidgetComponent implements OnInit {
  @Input() employeeIds: Number[];
  @Input() dateRange: DateRange;
  @ViewChild(BaseChartDirective, { static: false }) chart: BaseChartDirective;

  //card info
  cardType = "graph";
  title: string = "TIME OFF";

  //empty state info
  loaded: boolean;
  emptyState: boolean = false;

  //data info
  ptoData: PTOInfo[] = [];
  notSpecifiedLabel: string[] = [];
  yLabels: string[] = ["Available", "Accrued", "Used"];

  //graph info
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
      position: "right",
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
    {
      // third color
      backgroundColor: "#2a8d33",
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
        .GetPTOEvents(
          user.clientId,
          moment(this.dateRange.StartDate).format(MOMENT_FORMATS.DATE),
          moment(this.dateRange.EndDate).format(MOMENT_FORMATS.DATE),
          this.employeeIds,
          user.userId,
          user.userTypeId
        )
        .subscribe((data: any) => {
          this.ptoData = data;
          if (this.ptoData.length > 0) {
            let ee = this.ptoData.map((x) => x.employeeName);
            ee = Array.from(new Set(ee));
            this.combineData(ee);
            this.fillData(this.ptoData);
          } else {
            this.emptyState = true;
          }
          this.loaded = true;
        });
    });
  }

  combineData(ee) {
    var obj = [];
    for (var i = 0; i < this.ptoData.length; i++) {
      for (var j = 0; j < ee.length; j++) {
        if (this.ptoData[i].employeeName === ee[j]) {
          obj.push({
            policyName: this.ptoData[i].policyName,
            employeeName: ee[j],
            date: this.ptoData[i].originalRequestDate,
            available: this.ptoData[i].unitsAvailable,
            accrued: this.ptoData[i].startingUnits,
            used: this.ptoData[i].pendingUnits,
          });
        }
      }
    }
    obj.sort(function (a, b) {
      if (a.employeeName > b.employeeName) return 1;
      if (a.employeeName < b.employeeName) return -1;
      if (a.date > b.date) return 1;
      if (a.date < b.date) return -1;
    });

    var list = [];
    for (var i = 0; i < obj.length; i++) {
      if (i === 0) continue;
      if (obj[i].employeeName !== obj[i - 1].employeeName)
        list.push({
          employeeName: obj[i - 1].employeeName,
          date: obj[i - 1].date,
          availableCount: obj[i - 1].available,
          policyName: obj[i - 1].policyName,
          accrued: obj[i - 1].accrued,
          used: obj[i - 1].used,
        });
      if (i + 1 === obj.length) {
        list.push({
          employeeName: obj[i].employeeName,
          date: obj[i].date,
          availableCount: obj[i].available,
          policyName: obj[i].policyName,
          accrued: obj[i].accrued,
          used: obj[i].used,
        });
      }
    }
    list = Array.from(new Set(list));
    this.ptoData = list;
  }

  fillData(data) {
    var totalAvailable: number[] = [];
    var availableCount: number = 0;
    var totalAccrued: number[] = [];
    var accruedCount: number = 0;
    var totalPending: number[] = [];
    var pendingCount: number = 0;

    const result = data.reduce((acc, d) => {
      const found = acc.find((a) => a.policyName === d.policyName);
      const value = {
        value: d.policyName,
        available: d.availableCount,
        accrued: d.accrued,
        used: d.used,
        date: d.originalRequestDate,
      };
      if (!found) {
        acc.push({ policyName: d.policyName, pto: [value] });
      } else {
        found.pto.push(value);
      }
      return acc;
    }, []);

    for (var i = 0; i < result.length; i++) {
      availableCount = 0;
      accruedCount = 0;
      pendingCount = 0;
      this.barChartLabels.push(result[i].policyName);
      for (var j = 0; j < result[i].pto.length; j++) {
        availableCount += result[i].pto[j].available;
        accruedCount += result[i].pto[j].accrued;
        pendingCount += result[i].pto[j].used;
      }
      totalAvailable[i] = availableCount;
      totalAccrued[i] = accruedCount;
      totalPending[i] = pendingCount;
    }

    this.barChartData.push({ data: totalAvailable, label: "Available" });
    this.barChartData.push({ data: totalAccrued, label: "Accrued" });
    this.barChartData.push({ data: totalPending, label: "Used" });
  }

  openDialog(event) {
    if (event.active[0] != undefined) {
      let config = new MatDialogConfig<IPto>();

      config.data = {
        ptoInfo: this.ptoData.filter(
          (x) => x.policyName == this.barChartLabels[event.active[0]._index]
        ),
        featureName: this.ptoData.filter(
          (x) => x.policyName == this.barChartLabels[event.active[0]._index]
        )[0].policyName,
        dateRange: this.dateRange,
      };

      config.width = "1000px";

      return this.dialog.open<PtoWidgetDiaglogComponent, IPto, null>(
        PtoWidgetDiaglogComponent,
        config
      );
    }
  }
}
