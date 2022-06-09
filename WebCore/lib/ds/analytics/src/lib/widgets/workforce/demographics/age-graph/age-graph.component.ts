import { Component, OnInit, Input } from "@angular/core";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { ChartType, ChartOptions } from "chart.js";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { IDemographicData } from '@ds/analytics/shared/models/IDemographicData.model';
import { DemographicDialogComponent } from '../demographic-dialog/demographic-dialog.component';
import { DemographicInfo } from '@ds/analytics/shared/models/DemographicData.model';

@Component({
  selector: 'ds-age-graph',
  templateUrl: './age-graph.component.html'
})

export class AgeGraphComponent implements OnInit {
    @Input() dateRange: DateRange;
    @Input() data: any;

    loaded: boolean;

    cardType: string = "graph";

    infoData: InfoData;

    demographicData: DemographicInfo[];
    initialLabels: string[] = ['Under 18', '18-24', '25-34', '35-44', '45-54', '55+'];

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

    public pieChartType: ChartType = "pie";
    public pieChartColors = [
        {
            backgroundColor: [
              "#6dbb2c",
              "#caec73",
              "#2a8d33",
              "#44ce5a",
              "#3da66b",
              "#abcb30",
              "#036d38",
              "#91e386",
            ],
        },
    ];
    public pieChartData: number[] = [0, 0, 0, 0, 0, 0];
    public pieChartLabels: string[] = ['Under 18', '18-24', '25-34', '35-44', '45-54', '55+'];
    public statusLabels: string[] = ['Under 18', '18-24', '25-34', '35-44', '45-54', '55+'];
    public title: string = "Age";

    startDate = '1/1/0001 12:00:00 AM';
    endDate = '1/1/0001 12:00:00 AM';

    constructor(private dialog: MatDialog) {}

    ngOnInit() {
      this.demographicData = this.data;
      let info = this.data.map((x) => x.age);
      this.setData(info);
      this.loaded = true;
    }

    setData(resultList){
      var counts = {};
      resultList.forEach(function(x) { counts[x] = (counts[x] || 0)+1; });

      Object.keys(counts).forEach(key => {
        var age = parseInt(key);
        if (age <= 18){
          this.pieChartData[0] += counts[key];
        }
        else if (age > 18 && age <= 24){
          this.pieChartData[1] += counts[key];
        }
        else if (age > 24 && age <= 34){
          this.pieChartData[2] += counts[key];
        }
        else if (age > 34 && age <= 44){
          this.pieChartData[3] += counts[key];
        }
        else if (age > 44 && age <= 54){
          this.pieChartData[4] += counts[key];
        }
        else {
          this.pieChartData[5] += counts[key];
        }
      });
      this.concatLabels(this.pieChartData);
    }

    concatLabels(data){
      for (var x = 0; x < 6;  x++){
        this.pieChartLabels[x] = data[x]+" "+this.pieChartLabels[x];
      }
    }

    openDialog(event) {
      if (event.active[0] != undefined) {
          let config = new MatDialogConfig<IDemographicData>();

          config.data = {
              demographicData: this.demographicData.filter(x => this.reasonCheck(x.age, event.active[0]._index)),
              featureName: `Age: ${this.demographicData.filter(x => this.reasonCheck(x.age, event.active[0]._index))[0].age}`,
              dateRange: this.dateRange
            };

          config.width = "1000px";
          const dialogRef = this.dialog.open<DemographicDialogComponent, IDemographicData, null> (DemographicDialogComponent, config);
      }
    }

    reasonCheck(age, index): Boolean{
      //Correct Age to Labels
      if (age <= 18){
        age = this.initialLabels[0];
      }
      else if (age > 18 && age <= 24){
        age = this.initialLabels[1];
      }
      else if (age > 24 && age <= 34){
        age = this.initialLabels[2];
      }
      else if (age > 34 && age <= 44){
        age = this.initialLabels[3];
      }
      else if (age > 44 && age <= 54){
        age = this.initialLabels[4];
      }
      else {
        age = this.initialLabels[5];
      }

      //Filter
      if (age == this.initialLabels[index]){
        return true;
      }
      else{
        return false;
      }
    }
}

