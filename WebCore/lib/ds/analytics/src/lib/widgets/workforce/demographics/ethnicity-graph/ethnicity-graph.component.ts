import { Component, OnInit, Input } from "@angular/core";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { AnalyticsApiService } from "@ds/analytics/shared/services/analytics-api.service";
import { ChartType, ChartOptions } from "chart.js";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { DemographicInfo } from '@ds/analytics/shared/models/DemographicData.model';
import { DemographicDialogComponent } from '../demographic-dialog/demographic-dialog.component';
import { IDemographicData } from '@ds/analytics/shared/models/IDemographicData.model';
import { AccountService } from '@ds/core/account.service';

@Component({
  selector: 'ds-ethnicity-graph',
  templateUrl: './ethnicity-graph.component.html',
})

export class EthnicityGraphComponent implements OnInit {
    @Input() dateRange: DateRange;
    @Input() data: any;

    loaded: boolean;

    cardType: string = "graph";

    infoData: InfoData;

    demographicData: DemographicInfo[];
    statusLabels: string[] = [];
    initialLabels: string[] = [];

    public pieChartOptions: ChartOptions = {
        responsive: true,
        tooltips: {
          callbacks: {
            label: (tooltipItem) => {
              return `${this.initialLabels[tooltipItem.index]}: ${((parseInt(this.pieChartLabels[tooltipItem.index]) / this.demographicData.length) * 100).toFixed(2)}%`;
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
              "#7e52ae",
              "#b36fd0",
              "#402998",
              "#8a1f8d",
              "#7769c2",
              "#cca4ec",
              "#ac17a0",
              "#570e6e",
            ],
        },
    ];
    public pieChartLabels: string[] = [];
    public title: string = "Ethnicity";

    startDate = '1/1/0001 12:00:00 AM';
    endDate = '1/1/0001 12:00:00 AM';

    constructor(
        private analyticsApi: AnalyticsApiService,
        private accountService: AccountService,
        private dialog: MatDialog
    ) {}

    ngOnInit() {
      let info = this.data.map((x) => x.ethnicity);
      this.demographicData = this.data;
      this.setData(info);
      this.loaded = true;
    }

    setData(resultList){
      //Count each instance
      var counts = {};
      resultList.forEach(function(x) { counts[x] = (counts[x] || 0)+1; });
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
        notspecified += counts[keysSorted[x]];
        }
      }
      //At the very end add the "not specified" total
      valsSorted.push(notspecified);
      //Filter out the null from labels and add "not specified"
      keysSorted = keysSorted.filter(key => key != "null");
      keysSorted.splice(7);
      keysSorted.push("Not Specified");

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
        this.pieChartLabels[x] = data[x]+" "+Labels[x];
      }
    }

    openDialog(event) {
      if (event.active[0] != undefined) {
          let config = new MatDialogConfig<IDemographicData>();

          config.data = {
              demographicData: this.demographicData.filter(x => this.reasonCheck(x.ethnicity, event.active[0]._index)),
              featureName: `Ethnicity: ${this.demographicData.filter(x => this.reasonCheck(x.ethnicity, event.active[0]._index))[0].ethnicity}`,
              dateRange: this.dateRange
            };

          config.width = "1000px";
          const dialogRef = this.dialog.open<DemographicDialogComponent, IDemographicData, null> (DemographicDialogComponent, config);
      }
    }

    reasonCheck(ethnicity, index): Boolean{
      if (ethnicity == null){
        ethnicity = "Not Specified";
      }
      if (ethnicity == this.initialLabels[index]){
        return true;
      }
      else{
        return false;
      }
    }
}
