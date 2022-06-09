import { Component, OnInit, AfterViewInit, ElementRef, ViewChild, Input } from '@angular/core';
import { IReviewRating } from "@ds/performance/ratings";
import { BaseChartDirective } from 'ng2-charts';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { EmployeesRatingListDialogComponent } from './employees-rating-list-dialog/employees-rating-list-dialog.component';
import { ScoreModel } from '@ds/performance/ratings/shared/score-model.model';
import { IReviewGroupStatus } from '..';
import { Maybe } from '@ds/core/shared/Maybe';
import { IReviewStatusWithApprovalStatus } from '../shared/review-status-with-approval-status.model';
import { EvaluationRoleType } from '@ds/performance/evaluations';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';
import { ConvertToPercent } from '@ds/performance/evaluations/shared/shared-fn';

@Component({
    selector: 'ds-review-scoring-graph',
    templateUrl: './review-scoring-graph.component.html',
    styleUrls: ['./review-scoring-graph.component.scss'],
    //changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReviewScoringGraphComponent implements OnInit, AfterViewInit {
    isScoringEnabled: boolean = false;

    /** ChartJs specific vars */
    piechartdata:GraphItemWithRange[];
    rawScoreData:any;
    chartLabels:string[] = [];
    chartDataSet:any = [{ data: [] }];
    chartDataSet2:Array<number>;
    chartColors:any = null;
    chartType:string = 'doughnut';
    chartOptions:any = null;
    chartInitialized: boolean = false;

    legendData: Legend;

    private readonly defaultGraphItems: GraphItemWithRange[] = [
        {name: 'unacceptable', value: 0, scoreRange: {max: 1, min: 0}, reviews: []},
        {name: 'poor', value: 0, scoreRange: {max: 2, min: 1}, reviews: []},
        {name: 'meets expectations', value: 0, scoreRange: {max: 3, min: 2}, reviews: []},
        {name: 'exceeds exptectations', value: 0, scoreRange: {max: 4, min: 3}, reviews: []},
        {name: 'amazing', value: 0, scoreRange: {max: 5, min: 4}, reviews: []}
    ]

    @ViewChild('ratingLegend', { static: false }) legend:ElementRef<HTMLElement>;
    @ViewChild('ratingAverage', { static: false }) averageRating:ElementRef<HTMLElement>;
    @ViewChild('ratingPieChart', { static: false }) chartRef:BaseChartDirective;
    @ViewChild('ratingPieChartDiv', { static: false }) chartTemplate;

    @Input('clientId') clientId:number;
    @Input('competencyScores') competencyScores:Array<any>;
    @Input('allRatings') allRatings:Array<IReviewRating>;
    @Input('overallEmployees') totalEmployees: number;
    @Input('selectedEmployees') selectedEmployees: number;
    @Input() scoreModel: ScoreModel;
    private _group: IReviewGroupStatus;
    @Input()
    set group (group: IReviewGroupStatus) {
        this._group = group;
        //this.updateDatasource();
    }

  showChart = false;
  customLegend: any;
  private legendTemplate = '';
    readonly backgroundColors = [
        '#04b5b0',
        '#324b68',
        '#caec73',
        '#957acd',
        '#4ca7cf',
        '#0a7594',
        '#86D1cf',
        '#a4cd3c',
        '#393e44',
        '#306fa7',
        '#957acd'
    ];

  averageScore: number;
  averageScoreLabel: string;
  totalReviews: number;

    constructor(
        private dialog: MatDialog,
        private service: PerformanceReviewsService
    ) { }

  ngOnInit() {
    this.service.isScoringEnabledForReviewTemplate(this._group.reviewTemplateId).subscribe(x => {
        this.isScoringEnabled = x.data;
        if (this.isScoringEnabled) {
            if (!!this._group.reviews && !!this.allRatings &&
                this._group.reviews.length > 0 && this.allRatings.length > 0) {
                this.piechartdata = this.buildChartData(this.competencyScores);
                    const total = this.piechartdata.reduce((prev, next) => {
                        prev += next.value;
                        return prev;
                    }, 0);
                    const legendItems: GraphItemWithDescription[] = this.piechartdata.map(x => {
                        return {
                            description: new Maybe(this.scoreModel)
                            .map(x => x.scoreRangeLimits)
                            .map(limits => limits.find(limit => limit.label === x.name))
                            .map(limit => limit.description)
                            .valueOr(''),
                            item: x
                        }
                    })
                this.legendData = {
                    items: legendItems,
                    total: total
                }
                this.buildGraph(this.piechartdata);
            }
        }
    });
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
        if (this.legend) {
          this.legend.nativeElement.innerHTML = this.customLegend;
          var chartElement = this.legend.nativeElement.parentElement.previousElementSibling;
        }

        // Centering the Company Overall Rating
        // var avgRatingWidth = this.averageRating.nativeElement.clientWidth;
        // var avgRatingHeight = this.averageRating.nativeElement.clientHeight;
        // this.averageRating.nativeElement.style.left = (chartElement.clientWidth/2 - avgRatingWidth/2).toString() + 'px';
        // this.averageRating.nativeElement.style.top = (chartElement.clientWidth/2 - avgRatingHeight/2 -10 ).toString() + 'px';
    }, 100);
}

buildChartData(data:any): GraphItemWithRange[] {

    let template: string[] = [];
    template.push(`<div class="ds-legend-group">`);
    var result = this.flattenCompetencies(data, template);
    template.push(`</div>`);

    this.legendTemplate = template.join('');
    return result;
}

flattenCompetencies(data:Array<any>, template: string[]): GraphItemWithRange[] {
        let aggregate = 0;
        // var totalStars = this.allRatings.length - 1;// exclude not-rated
        // this.totalReviews = data.length;

        var result: GraphItemWithRange[] = this.defaultGraphItems;
        this.scoreModel.scoreRangeLimits = this.scoreModel.scoreRangeLimits.sort((a, b) => b.maxScore - a.maxScore);

        if(this.scoreModel && this.scoreModel.isActive && this.scoreModel.hasScoreRange && this.scoreModel.scoreRangeLimits != null && this.scoreModel.scoreRangeLimits.length){
            result = this.scoreModel.scoreRangeLimits.map(x => {
                var currentIndex = this.scoreModel.scoreRangeLimits.indexOf(x);
                const min = currentIndex < this.scoreModel.scoreRangeLimits.length - 1 ?  this.scoreModel.scoreRangeLimits[currentIndex + 1].maxScore : 0;
                return {
                    name: x.label,
                    value: 0,
                    scoreRange: {max: x.maxScore, min: min},
                    reviews: []
                } as GraphItemWithRange
            })
        };

        var completedReviews = this._group.reviews.filter(review => review.review.evaluations.some(evaluation => evaluation.role === EvaluationRoleType.Manager && evaluation.completedDate != null) && new Maybe(review).map(r => r.score).map(r => r.score).value() != null);
        this.totalReviews = completedReviews.length;

        if(new Maybe(this.scoreModel).map(x => x.scoreMethodType).map(x => x.scoreMethodTypeId).map(x => x == 2).valueOr(false)) {
            completedReviews = completedReviews.map(x => {
                if(x.score && x.score.score){
                    x.score.score = ConvertToPercent(x.score.score);
                }
                return x;
            })
        }

        completedReviews.forEach(review => {
            const safeReview = new Maybe(review);
            var item = result.find(item => safeReview.map(r => r.score).map(r => r.score).map(score => (item.scoreRange.min < score || (item.scoreRange.min == 0 && score == 0)) && score <= item.scoreRange.max).valueOr(false));
            if(item){
                item.value++;
                item.reviews.push(review);
            }
        });

        result = result.sort((a, b) => b.scoreRange.max - a.scoreRange.max);

        completedReviews.forEach(data => { aggregate += new Maybe(data).map(x => x.score).map(x => x.score).value() ? data.score.score : 0; });
        this.averageScore = this.totalReviews > 0 ? Math.round( aggregate * 10 / this.totalReviews) / 10 : 0;
        this.averageScoreLabel = result.find(x=>
            x.scoreRange.max > this.averageScore && x.scoreRange.min <= this.averageScore).name;

        return result;

}


private updateChart(items: GraphItem[]) {
    this.chartLabels = items.map(g => g.name);
    this.chartColors = this.backgroundColors;

    this.chartDataSet = [{
        backgroundColor: this.backgroundColors,
        borderColor: 'rgba(255, 255, 255, 1)',
        borderWidth: '4',
        data: items.map(g => g.value)
    }];

    this.chartDataSet2 = items.map(g => g.value);
    var self = this;

    this.chartOptions = {
        responsive: true,
        legend: { display: false },
        title: { display: false },
        cutoutPercentage: 60,
        onClick: function(e, elemSrc){
            if(!!elemSrc && elemSrc.length > 0){
                var selectedRating = self.piechartdata[elemSrc[0]._index];
                self.openList(selectedRating, null);
            }
            e.stopPropagation();
        },
        tooltips: {
            enabled: true,
            callbacks: {
                label: function(tooltipItem, context) {
                    var count = context.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
                    var tot = 0;
                    for(var i=0; i<context.datasets[tooltipItem.datasetIndex].data.length; i++){
                        tot += context.datasets[tooltipItem.datasetIndex].data[i];
                    }

                    var perc = ( count * 100) / tot ;
                    return count + ' (' + Math.round(perc) + '%)'  ;
                }
            }
        },
        legendCallback: (chart) => {
            return this.legendTemplate;
        },
        borderWidth: 1,
        hoverBorderWidth: 1,
    }
}

openList(rating: GraphItemWithRange, employees: Array<any>): MatDialogRef<EmployeesRatingListDialogComponent, any> {

    let modalInstance = this.dialog.open(EmployeesRatingListDialogComponent, {
        width: '40vw',
        data: {
            employeeList: employees,
            rating: rating,
            allRatings: this.allRatings,
        }
    });

    modalInstance.afterClosed()
    .subscribe(result => {
        if(result == null) return;
    });

    return modalInstance;
}

}

interface GraphItem {
  name: string;
  value: number;
}

interface GraphItemWithDescription {
item: GraphItem;
description: string;
}

interface Legend {
total: number;
items: GraphItemWithDescription[];
}

export interface GraphItemWithRange extends GraphItem {
    scoreRange: {max: number, min: number},
    reviews: IReviewStatusWithApprovalStatus[]
}
