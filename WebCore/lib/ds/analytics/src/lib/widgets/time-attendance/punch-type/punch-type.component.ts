import { Component, OnInit, Input } from "@angular/core";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";

import { AnalyticsApiService } from "@ds/analytics/shared/services/analytics-api.service";
import * as moment from "moment";
import { ChartType, ChartOptions } from "chart.js";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { PunchTypeDialogComponent } from './punch-type-dialog/punch-type-dialog.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PunchName } from '@ds/analytics/shared/models/PunchType.model';
import { IPunchNames } from '@ds/analytics/shared/models/IPunchNames.model';
import { AccountService } from '@ds/core/account.service';
import { MOMENT_FORMATS } from '@ds/core/shared';

@Component({
    selector: "ds-punch-type",
    templateUrl: "./punch-type.component.html",
    styleUrls: ["./punch-type.component.css"],
})
export class PunchTypeComponent implements OnInit {
    @Input() employeeIds: Number[];
    @Input() dateRange: DateRange;

    loaded: boolean;
    emptyState = false;
    infoData: InfoData;
    punchNames: PunchName[];
    statusLabels: string[] = [];
    data = [];

    public pieChartOptions: ChartOptions = {
        responsive: true,
        tooltips: {
            callbacks: {
              label: (tooltipItem) => {
                return `${this.statusLabels[tooltipItem.index]}: ${((parseInt(this.pieChartLabels[tooltipItem.index]) / this.data.length) * 100).toFixed(2)}%`;
              },
            }
          },
        legend: {
            position: "bottom",
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

    cardType: string = "graph";
    title: string = "Punch Source";
    constructor(
        private analyticsApi: AnalyticsApiService,
        private accountService: AccountService,
        private dialog: MatDialog
    ) {}

    ngOnInit() {
        this.accountService.getUserInfo().subscribe((user) => {
            this.analyticsApi.GetPunchTypes(user.clientId, moment(this.dateRange.StartDate).format(MOMENT_FORMATS.DATE),moment(this.dateRange.EndDate).format(MOMENT_FORMATS.DATE), this.employeeIds)
                .subscribe((punchName: any) => {
                    if (punchName.length <= 0){
                        this.emptyState = true;
                        this.loaded = true;
                      }
                    else{
                        this.punchNames = punchName;
                        this.renameClockNames();
                        let employees = this.punchNames.map(x => `${x.lastName}, ${x.firstName}`);
                        let clockName = this.punchNames.map((x) => x.clockName);
                        this.combineEmployees(employees, clockName);
                        this.sortLabels(this.data.map(x => x.clockName));
                        this.loaded = true;
                        this.renameClockNames();
                    }
                });
        });
    }

    combineEmployees(ee, c){
        var tempEE = Array.from(new Set(ee));
        var obj = [];
        for (var i = 0; i < this.punchNames.length; i++){
          for (var j = 0; j < tempEE.length; j++){
            if (`${this.punchNames[i].lastName}, ${this.punchNames[i].firstName}` === tempEE[j]){
              obj.push({
                name: tempEE[j],
                clockName: c[i],
              });
            }
          }
        };

        obj.sort(function (a, b) {
            if (a.name > b.name) return 1;
            if (a.name < b.name) return -1;
            if (a.clockName > b.clockName) return 1;
            if (a.clockName < b.clockName) return -1;
        })

        var temp = [];
        var count: number = 1

        for (var i = 0; i < obj.length; i++){
            if (i == 0) continue;
            if (i != 0 && obj[i].name === obj[i-1].name && obj[i].clockName === obj[i-1].clockName){
                count = count + 1;
            }
            if ((i != 0 && (obj[i].name != obj[i-1].name || obj[i].clockName !== obj[i-1].clockName)) || (i == obj.length-1 && obj[i].name === obj[i-1].name)){
                temp.push({
                    name: obj[i-1].name,
                    clockName: obj[i-1].clockName,
                    count: count
                })
                count = 1;
            }
        }
        this.data = temp;
    }

    sortLabels(data) {
        var desktop = [];
        var mobileSite = [];
        var mobileApp = [];
        var timeClock = [];
        var allLabels = [];
        var counts = {};
        data.forEach(function (x) {
            counts[x] = (counts[x] || 0) + 1;
        });
        //Sort the keys(reason) by the value(amount)
        var keysSorted = [];
        keysSorted = Object.keys(counts).sort(function (a, b) {
            return counts[b] - counts[a];
        });
        //Sort the values and check for nulls; Take the top 7, then add the rest to "not specified"
        for (var x = 0; x < keysSorted.length; x++) {
            if(keysSorted[x]){
                if (keysSorted[x] == "Desktop") {
                    desktop.push(counts[keysSorted[x]]);
                } else if (keysSorted[x] == "Mobile Site") {
                    mobileSite.push(counts[keysSorted[x]]);
                } else if (keysSorted[x] == "Mobile App" || keysSorted[x] == "MOBILE") {
                    mobileApp.push(counts[keysSorted[x]]);
                } else {
                    timeClock.push(counts[keysSorted[x]]);
                }
            }
        }

        desktop = desktop.reduce((a, b) => a + b, 0);
        mobileSite = mobileSite.reduce((a, b) => a + b, 0);
        mobileApp = mobileApp.reduce((a, b) => a + b, 0);
        timeClock = timeClock.reduce((a, b) => a + b, 0);

        allLabels.push([desktop, "Desktop"], [mobileSite, "Mobile Site"], [mobileApp, "Mobile App"], [timeClock, "Time Clock"]);

        allLabels.sort().reverse()

        for (var i = 0; i < 4; i++){
            this.pieChartData.push(allLabels[i][0]);
            this.statusLabels.push(allLabels[i][1]);
        }

        this.concatLabels(this.statusLabels, this.pieChartData);
    }

    concatLabels(Labels: String[], data) {
        for (var x = 0; x < Labels.length; x++) {
            this.pieChartLabels[x] = `${data[x].toLocaleString()} ` + Labels[x];
        }
    }

    renameClockNames(){
        for (var i = 0; i < this.punchNames.length; i++){
            if (this.punchNames[i].clockName == null) this.punchNames[i].clockName = "Desktop"
            else if (this.punchNames[i].clockName == "Mobile Site") this.punchNames[i].clockName = "Mobile Site"
            else if (this.punchNames[i].clockName == "Mobile App" || this.punchNames[i].clockName == "MOBILE") this.punchNames[i].clockName = "Mobile App"
            else {
                this.punchNames[i].clockName = "Time Clock"
            }
        }
    }

    openDialog(event) {
        if (event.active[0] != undefined) {
            let config = new MatDialogConfig<IPunchNames>();

            config.data = {
                punchNames: this.data.filter(x => x.clockName == this.statusLabels[event.active[0]._index]),
                featureName: this.data.filter(x => x.clockName == this.statusLabels[event.active[0]._index])[0].clockName,
                dateRange: this.dateRange
            };

            config.width = "1000px";

            return this.dialog.open<PunchTypeDialogComponent,IPunchNames,null>(PunchTypeDialogComponent, config);
        }
    }
}
