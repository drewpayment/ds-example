import { Component, OnInit, ViewChild, ElementRef, Input, SecurityContext, Output, EventEmitter } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { IScoreGroup } from '../shared/score-group.model';
import { IChartSettings } from '../shared/chart-settings.model';
import { forkJoin, Observable, merge } from 'rxjs';
import { map, switchMap, share, tap } from 'rxjs/operators';
import { ReviewsService } from '@ds/performance/reviews/shared/reviews.service';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';
import { EvaluationRoleType } from '../shared/evaluation-role-type.enum';
import { IWeightedScoreItem } from '../shared/weighted-score-item.model';
import { formatNumber } from '@angular/common';
import { DomSanitizer } from '@angular/platform-browser';
import { ScoreRangeLimit } from '@ds/performance/ratings/shared/score-range-limit.model';

@Component({
    selector: 'ds-scoring-graph',
    templateUrl: './scoring-graph.component.html',
    styleUrls: ['./scoring-graph.component.scss']
})
export class ScoringGraphComponent implements OnInit {
    isScoringEnabled: boolean = false;
    reviewTemplateId: number = 0;

    @ViewChild('legend', { static: false }) set legendReference(content: ElementRef<HTMLElement>) {
        this.legend = content;
    }
    @ViewChild('chartjs', { static: false }) set chartReference(content: BaseChartDirective) {
        this.chartRef = content;
    }

    constructor(
        private reviewsService: ReviewsService,
        private perfService: PerformanceReviewsService,
        private sanitizer: DomSanitizer
    ) { }

    @Input() evaluationId: number;
    @Input() reviewId: number;
    @Output() overallScore = new EventEmitter();
    @Input() isInModal = false;

    /** ChartJs specific vars */
    piechartdata: GraphItem[];
    rawScoreData: IScoreGroup;
    chartSettings: IChartSettings;
    chartLabels: string[] = [];
    chartData: any = [{ data: [] }];
    chartColors: any = null;
    chartType = 'doughnut';
    chartOptions: any = null;
    chartInitialized = false;
    private chartRef: BaseChartDirective;
    private legend: ElementRef<HTMLElement>;
    showChart = false;
    customLegend: string;
    private legendTemplate = '';
    private readonly backgroundColors = [
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
    legendIndex: number;

    label$: Observable<string>;
    makeChart$: Observable<any>;
    overallScoreDesc = 'Overall score is calculated by adding the weighted values of subtotals.';
    converter: (val: number) => number = (val) => val;
    readonly isPercentPref = () => this.converter == this.convertToPercent;
    readonly possible = () => this.isPercentPref() ? 100 : 5;

    ngOnInit() {
        const review$ = this.reviewsService.getReviewsByReviewId(this.reviewId);
        const scoringSettings$ = this.perfService.getScoringSettings(this.reviewId);
        const score2Data$ = this.perfService.calculateEvalScore(this.evaluationId, false);
        const scoring$ = this.perfService.getScoreModelForCurrentClient();
        const combinedRequests$ = forkJoin(scoringSettings$, score2Data$, scoring$, review$).pipe(
            map((data) => ({ scoringSettings: data[0].data, score2: data[1], scoring: data[2], review: data[3] })),
            tap(result => {
                this.reviewTemplateId = result.review.reviewTemplateId;
                this.perfService.isScoringEnabledForReviewTemplate(result.review.reviewTemplateId).subscribe(x => {
                    this.isScoringEnabled = x.data;
                });

                //console.log(JSON.stringify(result));
                if (result.scoringSettings.isPreferencePercent) {
                    this.converter = this.convertToPercent;
                }

            }),
            share());

        this.label$ = combinedRequests$.pipe(map(result => {
            let resultString = '';
            const array = (result.scoring.data.scoreRangeLimits || []).sort((limitA, limitB) => limitB.maxScore - limitA.maxScore);
            const convertedScore = this.converter((result.score2 as any).score);
            for (let i = 0; i < array.length; i++) {
                const current = array[i];
                if (convertedScore < current.maxScore) {
                    resultString = current.label;
                }

            }
            return resultString;
        }));

        this.makeChart$ = combinedRequests$.pipe(tap(result => {
            this.piechartdata = this.buildChartData(result.score2);

            this.rawScoreData = result.score2;
            this.overallScore.next(this.converter(result.score2.score));
            this.buildGraph(this.piechartdata);
        }));
    }

    buildGraph(groups: GraphItem[]) {

        this.updateChart(groups);
        this.showChart = true;

        /**
         * This needs to be wrapped in a timeout, because it allows the chart to render before executing this code.
         * If we don't, a chart element will never be rendered on the DOM by the time this execution happens.
         */
        setTimeout(() => {
            if (!this.chartRef || !this.chartRef.chart) { return; }
            this.customLegend = this.chartRef.chart.generateLegend() as any;
            this.legend.nativeElement.innerHTML = this.customLegend;
        }, 100);
    }

    buildChartData(data: IScoreGroup | IWeightedScoreItem): GraphItem[] {
        const locale = (<any>navigator).browserLanguage != null ? (<any>navigator).browserLanguage : navigator.language;
        const template: (string | GraphItem)[] = [];
        this.legendIndex = 0;
        template.push(`<div class="ds-legend-group mb-4">`);
        const result = this.flattenEvaluationGroups(data, template);
        template.push(`</div>`);
        const percentStringToAppend = (this.isPercentPref() ? '%' : '');
        for (let i = 0; i < template.length; i++) {
            const item = template[i];
            if (typeof item === 'object') {
                let outOf = percentStringToAppend;
                outOf = ' / ' + this.possible() + percentStringToAppend;
                template[i] = '' + formatNumber(this.converter(item.value), locale, '1.0-2') + outOf;
            }
        }
        this.legendTemplate = template.join('');
        return result;
    }

    flattenEvaluationGroups(data: IScoreGroup | IWeightedScoreItem, template: (string | GraphItem)[]): GraphItem[] {
        if ((data as IScoreGroup).items == null || (data as IScoreGroup).items.length == 0) {
            return [{ name: data.name, value: data.score, possible: 5 * data.weightPercent, weightedValue: data.score * data.weightPercent }];
        } else {
            const group = data as IScoreGroup;
            let resultingArray: GraphItem[] = [];
            const legendGroup: GraphItem = {
                name: group.name, value: group.score, possible: this.possible(), weightedValue: data.score * data.weightPercent
            };
            const weightPercent = group.weightPercent;
            const hasAScoreGroup = group.items.some(x => (x as IScoreGroup).items != null);
            const isFirstLegendItem = this.legendIndex == 0;
            let i: number;
            const items = [];
            if (hasAScoreGroup) {
                if (!isFirstLegendItem) {
                    template.push(`<div class="h3">${this.sanitizer.sanitize(SecurityContext.HTML, group.name)}
                    <span class='text-muted'>(${formatNumber(weightPercent * 100, 'en', '1.0-0')}%)</span>: <span class='pl-1'>`);
                    template.push(legendGroup);
                    template.push(`</span> </div>`);
                } else {
                    this.legendIndex++;
                }

                for (i = 0; i < group.items.length; i++) {
                    const dto = group.items[i];
                    resultingArray = resultingArray.concat(this.flattenEvaluationGroups(dto, template));
                }

            } else if (!isFirstLegendItem && !hasAScoreGroup) {
                this.legendIndex++;
                resultingArray.push(legendGroup);

                template.push(`<div class="ds-legend">
            <div class="ds-legend-color" style="background-color:${this.backgroundColors[(this.legendIndex - 2 < 0 ? this.legendIndex : this.legendIndex - 2) % this.backgroundColors.length]} !important;">
        </div>
`);
                template.push(`<div>${this.sanitizer.sanitize(SecurityContext.HTML, legendGroup.name)} <span class='text-muted'>(${formatNumber(group.weightPercent * 100, 'en', '1.0-0') + ' / 100'}%)</span>: <span class='pl-1'>`);
                template.push(legendGroup);
                template.push('</span> </div></div>');
            }
            return resultingArray;
        }

    }

    private updateChart(groups: GraphItem[]) {

        this.chartLabels = groups.map(g => g.name);
        this.chartColors = this.backgroundColors;

        this.chartData = [{
            backgroundColor: this.backgroundColors,
            borderColor: 'rgba(255, 255, 255, 1)',
            borderWidth: '4',
            data: groups.filter(g => g.possible > 0).map(g => g.weightedValue)
        }];

        this.chartOptions = {
            legend: {
                display: false
            },
            cutoutPercentage: 70,
            // circumference: percentFilled * 2 * Math.PI,
            legendCallback: (chart) => {
                return this.legendTemplate;
            }
        };

        this.chartSettings = {
            chartData: this.chartData,
            chartColors: this.chartColors,
            chartInitialized: this.chartInitialized,
            chartLabels: this.chartLabels,
            chartOptions: this.chartOptions,
            chartType: this.chartType,
            customLegend: this.customLegend
        };
    }

    convertToPercent(average: number): number {
        return (average / 5) * 100;
    }

}

interface GraphItem {
    name: string;
    value: number;
    weightedValue: number;
    possible: number;
}
