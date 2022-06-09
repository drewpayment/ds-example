import { Component, OnInit, Input } from "@angular/core";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { AnalyticsApiService } from "@ds/analytics/shared/services/analytics-api.service";
import { ChartType, ChartOptions } from "chart.js";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { IDemographicData } from '@ds/analytics/shared/models/IDemographicData.model';
import { DemographicDialogComponent } from '../demographic-dialog/demographic-dialog.component';
import { DemographicInfo } from '@ds/analytics/shared/models/DemographicData.model';
import { AccountService } from '@ds/core/account.service';

@Component({
  selector: 'ds-length-of-service-graph',
  templateUrl: './length-of-service-graph.component.html'
})

export class LengthOfServiceGraphComponent implements OnInit {
    @Input() dateRange: DateRange;
    @Input() data: any;

    loaded: boolean;

    cardType: string = "graph";

    infoData: InfoData;

    demographicData: DemographicInfo[];
    statusLabels: string[] = [];
    initialLabels: string[] = [
      '0-3 months',
      '3-6 months',
      '6-9 months',
      '9-12 months',
      '1-2 years',
      '2-4 years',
      '5-10 years',
      '10+ years',
    ];

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

    public pieChartType: ChartType = "pie";
    public pieChartColors = [
        {
            backgroundColor: [
              "#fc621a",
              "#da2121",
              "#ffb627",
              "#ff821c",
              "#ff4949",
              "#b04001",
              "#ffd73b",
              "#f53c05",
            ],
        },
    ];
    public pieChartData: number[] = [0, 0, 0, 0, 0, 0, 0, 0];
    public pieChartLabels: string[] = [
      '0-3 months',
      '3-6 months',
      '6-9 months',
      '9-12 months',
      '1-2 years',
      '2-4 years',
      '5-10 years',
      '10+ years',
    ];
    public title: string = "Length Of Service";

    startDate = '1/1/0001 12:00:00 AM';
    endDate = '1/1/0001 12:00:00 AM';

    constructor(
        private analyticsApi: AnalyticsApiService,
        private accountService: AccountService,
        private dialog: MatDialog
    ) {}

    ngOnInit() {
      this.demographicData = this.data;
      let months = this.data.map((x) => x.lengthOfService);
      this.setData(months);
      this.loaded = true;
    }

    setData(resultList){
      var counts = {};
      resultList.forEach(function(x) { counts[x] = (counts[x] || 0)+1; });

      Object.keys(counts).forEach(key => {
        var service = parseInt(key);
        if (service <= 3){
          this.pieChartData[0] += counts[key];
        }
        else if (service > 3 && service <= 6){
          this.pieChartData[1] += counts[key];
        }
        else if (service > 6 && service <= 9){
          this.pieChartData[2] += counts[key];
        }
        else if (service > 9 && service <= 12){
          this.pieChartData[3] += counts[key];
        }
        else if (service > 12 && service <= 24){
          this.pieChartData[4] += counts[key];
        }
        else if (service > 24 && service <= 48){
          this.pieChartData[5] += counts[key];
        }
        else if (service > 48 && service <= 120){
          this.pieChartData[6] += counts[key];
        }
        else{
          this.pieChartData[7] += counts[key];
        }
      });
      this.concatLabels(this.pieChartData);
    }

    concatLabels(data){
      for (var x = 0; x < 8;  x++){
        this.pieChartLabels[x] = data[x]+" "+this.pieChartLabels[x];
      }
    }

    openDialog(event) {
      if (event.active[0] != undefined) {
          let config = new MatDialogConfig<IDemographicData>();

          config.data = {
              demographicData: this.demographicData.filter(x => this.reasonCheck(x.lengthOfService, event.active[0]._index)),
              featureName: `Length of Service: ${this.demographicData.filter(x => this.reasonCheck(x.lengthOfService, event.active[0]._index))[0].lengthOfService}`,
              dateRange: this.dateRange
            };

          config.width = "1000px";
          const dialogRef = this.dialog.open<DemographicDialogComponent, IDemographicData, null> (DemographicDialogComponent, config);
      }
    }

    reasonCheck(months, index): Boolean{
      //Correct Time
      if (months <= 3){
        months = this.initialLabels[0];
      }
      else if (months > 3 && months <= 6){
        months = this.initialLabels[1];
      }
      else if (months > 6 && months <= 9){
        months = this.initialLabels[2];
      }
      else if (months > 9 && months <= 12){
        months = this.initialLabels[3];
      }
      else if (months > 12 && months <= 24){
        months = this.initialLabels[4];
      }
      else if (months > 24 && months <= 48){
        months = this.initialLabels[5];
      }
      else if (months > 48 && months <= 120){
        months = this.initialLabels[6];
      }
      else{
        months = this.initialLabels[7];
      }

      //Filter
      if (months == this.initialLabels[index]){
        return true;
      }
      else{
        return false;
      }
    }
}
