import { Component, OnInit, Input } from "@angular/core";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { AnalyticsApiService } from "@ds/analytics/shared/services/analytics-api.service";
import { ChartType, ChartOptions } from "chart.js";
import * as moment from "moment";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { ActiveEmployee } from "@ds/analytics/shared/models/ActiveEmployeeData.model";
import { ActiveEmployeeDialogComponent } from "./active-employee-dialog/active-employee-dialog.component";
import { IActiveEmployee } from "@ds/analytics/shared/models/IActiveEmployee.model";
import { AccountService } from '@ds/core/account.service';
import { MOMENT_FORMATS } from '@ds/core/shared';


@Component({
    selector: "ds-active-employees",
    templateUrl: "./active-employees.component.html",
    styleUrls: ["./active-employees.component.css"],
})
export class ActiveEmployeesComponent implements OnInit {
    @Input() employeeIds: Number[];
    @Input() dateRange: DateRange;

    loaded: boolean;
    emptyState = false;

    cardType: string = "info";

    infoData: InfoData;

    activeEmployeeData: ActiveEmployee[];
    statusLabels: string[] = [];
    notSpecifiedLabel: string[] = [];

    public pieChartOptions: ChartOptions = {
        responsive: true,
        tooltips: {
          callbacks: {
            label: (tooltipItem, data) => {
              return `${this.statusLabels[tooltipItem.index]}: ${((parseInt(this.pieChartLabels[tooltipItem.index]) / this.activeEmployeeData.length) * 100).toFixed(2)}%`;
            }
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
            ],
        },
    ];
    public pieChartLabels: string[] = [];

    constructor(
        private analyticsApi: AnalyticsApiService,
        private accountService: AccountService,
        private dialog: MatDialog
    ) {}

    ngOnInit() {
        this.accountService.getUserInfo().subscribe((user) => {
          this.analyticsApi.GetActiveEmployees(user.clientId,moment(this.dateRange.StartDate).format(MOMENT_FORMATS.DATE),moment(this.dateRange.EndDate).format(MOMENT_FORMATS.DATE), this.employeeIds, user.userId, user.userTypeId)
            .subscribe((activeEmployeeData: any) => {
              if (activeEmployeeData.length <= 0){
                this.emptyState = true;
                this.loaded = true;
              }

                this.activeEmployeeData = activeEmployeeData;
                let info = activeEmployeeData.map((x) => x.employeeStatus);
                this.countNumbers(info)
                var endDate = new Date(this.dateRange.EndDate);
                var month = endDate.getMonth() + 1;
                var day = endDate.getDate();
                var year = endDate.getFullYear();
                this.infoData = {
                    icon: "people",
                    color: "info",
                    value: activeEmployeeData.length.toLocaleString("en"),
                    title:
                        `Active Employees On ${month}/${day}/${year}`,
                    showBottom: true,
                };
                this.loaded = true;
            });
        });
      }

    countNumbers(a){
        //Count each instance
        var counts = {};
        a.forEach(function(x) { counts[x] = (counts[x] || 0)+1; });
        //Sort the keys(reason) by the value(amount)
        var keysSorted = [];
        keysSorted = Object.keys(counts).sort(function(a,b){return counts[b]-counts[a]})
        //Sort the values and check for nulls; Take the top 7, then add the rest to "not specified"
        var valsSorted = [];
        var notspecified = 0;
        for (var x = 0; x < keysSorted.length; x++){
          if (keysSorted[x] != "null" && x < 8){
            valsSorted.push(counts[keysSorted[x]]);
          }
          else {
            this.notSpecifiedLabel.push(keysSorted[x]);
            notspecified += counts[keysSorted[x]];
          }
        }
        //At the very end add the "not specified" total
        valsSorted.push(notspecified);
        //Filter out the null from labels and add "not specified"
        keysSorted = keysSorted.filter(key => key != "null");
        keysSorted.splice(7);
        keysSorted.push("Not specified");
        if(notspecified == 0){
          keysSorted.pop()
        }
        //Apply sorted vals to data
        this.pieChartData = valsSorted;
        //Apply to labels
        keysSorted.forEach(key => {
          this.statusLabels.push(key);
        });
        //Concat data and labels for Legend
        this.concatLabels(this.statusLabels, this.pieChartData);
      }
      concatLabels(Labels: String[], data){
        for (var x = 0; x < Labels.length;  x++){
          this.pieChartLabels[x] = `${data[x]} ` + Labels[x];
        }
      }

    openDialog(event) {
        if (event.active[0] != undefined) {
            let config = new MatDialogConfig<IActiveEmployee>();

            config.data = {
                activeEmployee: this.activeEmployeeData.filter(x => this.reasonCheck(x.employeeStatus, event.active[0]._index)),
                featureName: this.activeEmployeeData.filter(x => this.reasonCheck(x.employeeStatus, event.active[0]._index))[0].employeeStatus,
                dateRange: this.dateRange,
                title: this.infoData.title
            };

            config.width = "1000px";

            return this.dialog.open<ActiveEmployeeDialogComponent,IActiveEmployee,null>(ActiveEmployeeDialogComponent, config);
        }
    }

    reasonCheck(reason, index): Boolean{
        if (this.statusLabels[index] == "Not specified"){
          for (var x = 0; x < this.notSpecifiedLabel.length; x++){
            if (this.notSpecifiedLabel[x] == reason || reason == null){
              return true;
            }
          }
          return false;
        }
        else {
          if (reason == this.statusLabels [index]){
            return true;
          }
          else{
            return false;
          }
        }
      }
}
