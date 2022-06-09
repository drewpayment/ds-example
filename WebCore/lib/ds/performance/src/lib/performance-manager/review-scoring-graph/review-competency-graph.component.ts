import { Component, OnInit, AfterViewInit,ChangeDetectionStrategy, ElementRef, ViewChild, Input } from '@angular/core';
import { formatNumber } from '@angular/common';
import { IReviewRating } from "@ds/performance/ratings";
import { BaseChartDirective } from 'ng2-charts';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { INameVal } from '@ajs/labor/models';
import { EmployeesRatingListDialogComponent } from './employees-rating-list-dialog/employees-rating-list-dialog.component';

@Component({
    selector: 'ds-review-competency-graph',
    templateUrl: './review-competency-graph.component.html',
    styleUrls: ['./review-competency-graph.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReviewCompetencyGraphComponent implements OnInit, AfterViewInit {

    /** ChartJs specific vars */
    piechartdata:GraphItem[];
    rawScoreData:any;
    chartLabels:string[] = [];
    chartDataSet:any = [{ data: [] }];
    chartColors:any = null;
    chartType:string = 'bar';
    chartOptions:any = null;
    chartInitialized:boolean = false;

    @ViewChild('ratingLegend', { static: false }) legend:ElementRef<HTMLElement>;
    @ViewChild('ratingPieChart', { static: false }) chartRef:BaseChartDirective;
    @ViewChild('ratingPieChartDiv', { static: false }) chartTemplate;

    @Input('clientId') clientId:number;
    @Input('competencyScores') competencyScores:Array<any>;
    @Input('allRatings') allRatings:Array<IReviewRating>;

  showChart = false;
  customLegend: any;
  private legendTemplate = '';

  private readonly lowAvgScoreColor  : string = '#2a8d33'; // Lowest Average Score
  private readonly highAvgScoreColor : string = '#6dba2c'; // Highest Average Score
  private readonly avgScoreColor     : string = '#CAEC73'; // Average Score

  private backgroundColors = [];

  allCompetencies: Array<INameVal>;

  constructor(private dialog: MatDialog ) { }

  ngOnInit() {
      if(!!this.competencyScores && !!this.allRatings &&
        this.competencyScores.length > 0 && this.allRatings.length > 0 ) {
        this.allCompetencies = [];

        this.competencyScores.forEach(x=>{
            if(!this.allCompetencies.find(y=>y.value == x.competencyId)){
                this.allCompetencies.push( {name:x.competencyName,value:x.competencyId} );
            }
        });

        this.piechartdata = this.buildChartData(this.competencyScores);
        this.generateRangeOfColors(this.allCompetencies.length);

        this.buildGraph(this.piechartdata);
      }
  }

  ngAfterViewInit() {}

  buildGraph(groups: GraphItem[]) {

    this.updateChart(groups);
    this.showChart = true;

    /**
     * This needs to be wrapped in a timeout, because it allows the chart to render before executing this code.
     * If we don't, a chart element will never be rendered on the DOM by the time this execution happens.
     */
    setTimeout(() => {
        if(!this.chartRef ) return;
        this.customLegend = this.chartRef.chart.generateLegend();
        this.legend.nativeElement.innerHTML = this.customLegend;
    }, 100);
}

buildChartData(data:any): GraphItem[] {

    let template: string[] = [];
    template.push(`<div class="ds-legend-group d-flex justify-content-center mb-0">`);
    var result = this.flattenCompetencies(data, template);
    template.push(`</div>`);

    this.legendTemplate = template.join('');
    return result;
}

flattenCompetencies(data:Array<any>, template: string[]): GraphItem[] {

    var locale = (<any>navigator).browserLanguage != null ? (<any>navigator).browserLanguage : navigator.language;
    if (data == null || data.length == 0) {
        return [];
    } else {
        var group = data;
        let resultingArray: GraphItem[] = [];
        var minValue = this.allRatings[0].rating;
        var minValueInx = -1;
        var maxValue = 0;
        var maxValueInx = -1;

        this.allCompetencies.forEach( (competency, index) => {
            // competencies on this rating
            var totScore = 0;
            let competencies = data.filter(x=> x.competencyId == competency.value);
            competencies.forEach(x=> totScore += x.score );
            var avgScore = Math.round( totScore * 10 / competencies.length ) / 10;

            let legend: GraphItem = { name: competency.name, value: avgScore, order: index };
            resultingArray.push(legend);

            if(avgScore > maxValue) { maxValue = avgScore; maxValueInx = index;}
            if(avgScore < minValue) { minValue = avgScore; minValueInx = index;}
        });

        template.push(`<div class="ds-legend">
        <div class="ds-legend-color" style="background-color:${this.highAvgScoreColor} !important;">
        </div>`);
        template.push("<span class='text-muted' >High Average Score</span></div>");

        template.push(`<div class="ds-legend">
        <div class="ds-legend-color" style="background-color:${this.lowAvgScoreColor} !important;">
        </div>`);
        template.push("<span class='text-muted' >Low Average Score</span></div>");

        return resultingArray;
    }

}


private updateChart(items: GraphItem[]) {
    this.chartLabels = items.map(g => g.name);
    this.chartColors = this.backgroundColors;
    var maxScore = this.allRatings[0].rating;

    this.chartDataSet = [{
        backgroundColor: this.backgroundColors,
        data: items.map(g => g.value),
    }];

    var self = this;

    this.chartOptions = {
        // responsive: true,
        legend: { display: false },
        title: { display: false },
        cutoutPercentage: 60,
        onClick: function(e, elemSrc){
            if(!!elemSrc && elemSrc.length > 0){
                var selectedCompetency = self.allCompetencies[elemSrc[0]._index];
                var employees = self.competencyScores.filter(x=>x.competencyId == selectedCompetency.value );
                self.openList(selectedCompetency, employees);
            }
            e.stopPropagation();
        },
        scales: {
            yAxes: [{
                position: 'left',
                display: true,
                scaleLabel: {
                    display: true,
                    labelString: 'Average Score'
                },
                ticks: {
                    min: 0,
                    max: maxScore,
                    stepSize: 1
                }
            }],
            xAxes: [{
              gridLines: {
                drawOnChartArea:false
              }
            }],
        },
        maintainAspectRatio: false,
        tooltips: { enabled: true },
        legendCallback: (chart) => {
            return this.legendTemplate;
        }
    }
}

openList(competency: INameVal, employees: Array<any>): MatDialogRef<EmployeesRatingListDialogComponent, any> {

    let modalInstance = this.dialog.open(EmployeesRatingListDialogComponent, {
        width: '40vw',
        data: {
            employeeList: employees,
            competency: competency,
            allRatings: this.allRatings,
            isCompetencyView: true
        }
    });

    modalInstance.afterClosed()
    .subscribe(result => {
        if(result == null) return;
    });

    return modalInstance;
}

generateRangeOfColors(count: number)
{
    if(count>1){
        var least = 10;
        var highest = 0;
        this.backgroundColors = [];

        // setup the background colors
        for( var i=0; i<this.piechartdata.length; i++ ) {
            this.backgroundColors.push(this.avgScoreColor);
        }

        // identify the least and highest scores
        for( var i=0; i<this.piechartdata.length; i++ ) {
            if(this.piechartdata[i].value < least)      least = this.piechartdata[i].value;
            if(this.piechartdata[i].value > highest)    highest = this.piechartdata[i].value;
        }

        // place the colors highest and least
        for(var i=(this.piechartdata.length-1);i>=0;i--){
            let k = this.piechartdata[i];
            if(highest == k.value)      this.backgroundColors[k.order] = this.highAvgScoreColor;
            else if(least == k.value)   this.backgroundColors[k.order] = this.lowAvgScoreColor;
        }
    }
    else if(count==1) {
        this.backgroundColors = [this.highAvgScoreColor];
    }
}

}

interface GraphItem {
  name: string;
  value: number;
  order: number;
}
