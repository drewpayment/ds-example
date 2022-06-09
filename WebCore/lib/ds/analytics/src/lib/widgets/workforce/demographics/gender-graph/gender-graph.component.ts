import { Component, OnInit, Input } from "@angular/core";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { AnalyticsApiService } from "@ds/analytics/shared/services/analytics-api.service";
import { ChartType, ChartOptions } from "chart.js";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { DemographicInfo } from '@ds/analytics/shared/models/DemographicData.model';
import { IDemographicData } from '@ds/analytics/shared/models/IDemographicData.model';
import { DemographicDialogComponent } from '../demographic-dialog/demographic-dialog.component';
import { AccountService } from '@ds/core/account.service';

@Component({
  selector: 'ds-gender-graph',
  templateUrl: './gender-graph.component.html'
})

export class GenderGraphComponent implements OnInit {
    @Input() dateRange: DateRange;
    @Input() data: any;

    loaded: boolean;

    cardType: string = "graph";

    infoData: InfoData;

    demographicData: DemographicInfo[];
    initialLabels: string[] = [];
    statusLabels: string[] = [];
    notSpecifiedLabel: string[] = [];

    public pieChartOptions: ChartOptions = {
        responsive: true,
        tooltips: {
          callbacks: {
            label: (tooltipItem) => {
              return `${this.statusLabels[tooltipItem.index]}: ${((parseInt(this.pieChartLabels[tooltipItem.index]) / this.demographicData.length) * 100).toFixed(2)}%`;
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
            ],
        },
    ];

    public pieChartLabels: string[] = [];
    public title: string = "Gender";

    constructor(
        private analyticsApi: AnalyticsApiService,
        private accountService: AccountService,
        private dialog: MatDialog
    ) {}

    ngOnInit() {
      this.demographicData = this.data;
      let info = this.data.map((x) => x.gender);
      this.setData(info);
      this.loaded = true;
    }

    setData(resultList){
        //Count each instance
        var counts = {};
        resultList.forEach(function(x) { counts[x] = (counts[x] || 0)+1; });

        //Sort the keys(reason) by the value(amount)
        var keysSorted = [];
        keysSorted = Object.keys(counts).sort(function(a,b){return counts[b]-counts[a]});

        //Sort the values and check for nulls; then add the rest to "not specified"
        var valsSorted = [];
        var notspecified = 0;
        for (var x = 0; x < keysSorted.length; x++){
        //   if (keysSorted[x] != " " && keysSorted[x] != "N" && keysSorted[x] != "1" && keysSorted[x] != "null"){
        //     valsSorted.push(counts[keysSorted[x]]);
        //   }
          if (keysSorted[x] != " " && keysSorted[x] != "N" && keysSorted[x] != "1" && keysSorted[x] != "null"){
            valsSorted.push(counts[keysSorted[x]]);
          }
          else {
            notspecified += counts[keysSorted[x]];
          }
        }

        //At the very end add the "not specified" total
        valsSorted.push(notspecified);

        //Filter out the null and 'N' from labels and add "not specified"
        keysSorted = keysSorted.filter(key => key != " " && key != "N" && key != "1" && key != "null");
        keysSorted.push("Not Specified");

        //Apply sorted vals to data
        this.pieChartData = valsSorted;

        //Apply to labels
        keysSorted.forEach(key => {
          this.statusLabels.push(key);
          this.initialLabels.push(key);
        });

        //Concat data and labels for Legend
        this.concatLabels(this.statusLabels, this.pieChartData);
      }
      concatLabels(Labels: String[], data){
        for (var x = 0; x < Labels.length;  x++){
          if (Labels[x] == "M"){
            Labels[x] = "Male"
          }
          else if (Labels[x] == "F"){
            Labels[x] = "Female"
          }
          this.pieChartLabels[x] = `${data[x]} ` + Labels[x];
        }
      }

      openDialog(event) {
        if (event.active[0] != undefined) {
            let config = new MatDialogConfig<IDemographicData>();

            config.data = {
                demographicData: this.demographicData.filter(x => this.reasonCheck(x.gender, event.active[0]._index)),
                featureName: `Gender: ${this.demographicData.filter(x => this.reasonCheck(x.gender, event.active[0]._index))[0].gender}`,
                dateRange: this.dateRange
              };

            config.width = "1000px";
            const dialogRef = this.dialog.open<DemographicDialogComponent, IDemographicData, null> (DemographicDialogComponent, config);
        }
      }

      reasonCheck(reason, index): Boolean{
        if (reason == " " || reason == "N" || reason == "null" || reason == "1"){
          reason = "Not Specified";
        }
        if (reason == this.initialLabels[index]){
          return true;
        }
        else{
          return false;
        }
      }
}
