import { Component, OnInit, AfterViewInit, ElementRef, ViewChild, Input } from '@angular/core';
import { formatNumber } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { IFeedbackSetup, IFeedbackResponseData } from '@ds/performance/feedback/';
import { EvaluationRoleType } from '@ds/performance/evaluations/shared/evaluation-role-type.enum';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { INameVal } from '@ajs/labor/models';
import { EmployeeEvaluationsDialogComponent } from '@ds/performance/performance-manager/review-group-status-view/employee-evaluations/employee-evaluations-dialog.component';

@Component({
    selector: 'ds-multiselect-feedbacks-graph',
    templateUrl: './multiselect-feedbacks-graph.component.html',
    styleUrls: ['./multiselect-feedbacks-graph.component.scss']
})
export class MultiSelectFeedbacksGraphComponent implements OnInit, AfterViewInit {

    /** ChartJs specific vars SET 1*/
    piechartdata:GraphItem[];
    chartLabels:string[] = [];
    chartDataSet:any = [{ data: [] }];
    chartColors:string[] = null;
    chartType:string = 'pie';
    chartOptions:any = null;

    @ViewChild('multiselectBarChart', { static: false }) chartRef:BaseChartDirective;

    /** ChartJs specific vars SET 2*/
    piechartdata2:GraphItem[];
    chartDataSet2:any = [{ data: [] }];
    chartColors2:string[] = null;
    chartOptions2:any = null;

    @ViewChild('multiselectBarChart2', { static: false }) chartRef2:BaseChartDirective;

    @ViewChild('optionsLegend', { static: false }) legend:ElementRef<HTMLElement>;

    @Input()
    feedback: IFeedbackSetup;

    @Input()
    responseList: Array<IFeedbackResponseData>;

  showChart:boolean = false;
  customLegend:string;
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

  totalCount: number;
  showEmployeeGraph: boolean;
  showManagerGraph: boolean;
  showLegend: boolean;
  multipleCharts: boolean;

  constructor(private dialog: MatDialog ) { }

  ngOnInit() {
      if(this.responseList.length > 0  ) {
        this.buildChartData(this.responseList);
        this.buildLegend();

        if(this.showManagerGraph)   this.updateChart2(this.piechartdata2);
        if(this.showEmployeeGraph)  this.updateChart(this.piechartdata);

        if(this.showEmployeeGraph || this.showManagerGraph){
            this.showChart = true;
            this.showLegend = true;

            // display the legend
            setTimeout(() => {
                this.legend.nativeElement.innerHTML = this.customLegend;
            }, 100);
        }
        else{
            this.showChart = false;
            this.showLegend = false;
        }
      }

      this.GetClassForGraph();
  }

  ngAfterViewInit() {}

buildChartData(data:Array<IFeedbackResponseData>): void {

    var locale = (<any>navigator).browserLanguage != null ? (<any>navigator).browserLanguage : navigator.language;
    if (data == null || data.length == 0) {
        this.showEmployeeGraph = false;
        this.showManagerGraph = false;
        return;
    }
    else
    {
        var group = data;

        let employeeData = data.filter(x => x.evaluationRoleType == EvaluationRoleType.Self);
        let managerData = data.filter(x => x.evaluationRoleType == EvaluationRoleType.Manager);

        // decide which graphs to show
        this.showEmployeeGraph = employeeData.length > 0;
        this.showManagerGraph = managerData.length > 0;

        var flattenChartData = (items:Array<IFeedbackResponseData>, isEmp: boolean) : GraphItem[] =>
        {
            let resultingArray: GraphItem[] = [];
            var totCount = 0;

            // Multi Select options list
            this.feedback.feedbackItems.forEach(fe => {

                let responses = items.filter(x=>
                    (   x.responseItems.length > 0 &&
                        x.responseItems[0].textValue &&
                        x.responseItems[0].textValue.split(',').indexOf(fe.feedbackItemId.toString()) > -1 ));

                let legend: GraphItem = { id:fe.feedbackItemId, name: fe.itemText, value: {count: responses.length, perc:0}, isEmployee: isEmp };
                resultingArray.push(legend);
                totCount += legend.value.count;
            });

            resultingArray.forEach(item => {
                item.value.perc = Math.round( item.value.count * 1000 /totCount ) / 10;
            });

            return resultingArray;
        }

        this.piechartdata = flattenChartData(employeeData, true);
        this.piechartdata2 = flattenChartData(managerData, false);
    }

}

private buildLegend(): void{
    let template: string[] = [];
    template.push(`<div class="ds-legend-group mb-0">`);

    for(var inx=0; inx< this.feedback.feedbackItems.length; inx++){
        template.push(`<div class="ds-legend pl-0 mb-1">
            <div class="ds-legend-color" style="background-color:${this.backgroundColors[inx % this.backgroundColors.length]} !important;"></div>`);
        template.push(`<div class='text-truncate'>${this.feedback.feedbackItems[inx].itemText}</div></div>`);
    }
    template.push(`</div>`);

    this.customLegend = template.join('');
}

private updateChart(items: GraphItem[] ) {
    this.chartLabels = items.map(g => g.name);
    this.chartColors = this.backgroundColors;

    this.chartDataSet = [{
        backgroundColor: this.backgroundColors,
        data: items.map(g => g.value.perc),
    }];

    this.chartOptions = this.getChartOptions(items,'Employees');
}

private updateChart2(items: GraphItem[] ) {
    this.chartLabels = items.map(g => g.name);
    this.chartColors2 = this.backgroundColors;

    this.chartDataSet2 = [{
        backgroundColor: this.backgroundColors,
        data: items.map(g => g.value.perc),
    }];

    this.chartOptions2 = this.getChartOptions(items,'Supervisors');
}

private getChartOptions(items: GraphItem[], titleTxt: string){
    var self = this;
    return {
        responsive: true,
        legend: { display: false },
        // title: {
        //     display: true,
        //     text: titleTxt
        // },
        onClick: function(e, elemSrc){
            if(!!elemSrc && elemSrc.length > 0){
                var selectedResponse = items[elemSrc[0]._index];
                let responses:Array<IFeedbackResponseData> = [];

                if(selectedResponse.isEmployee){
                    responses = self.responseList.filter(x => x.evaluationRoleType == EvaluationRoleType.Self );
                } else {
                    responses = self.responseList.filter(x => x.evaluationRoleType == EvaluationRoleType.Manager );
                }


                responses = responses.filter(x=>
                    (   x.responseItems.length > 0 &&
                        x.responseItems[0].textValue &&
                        x.responseItems[0].textValue.split(',').indexOf(selectedResponse.id.toString()) > -1 ));

                self.openList(responses, selectedResponse.id);
            }
            e.stopPropagation();
        },
        tooltips: {
            callbacks: {
                label: function(tooltipItem, data) {
                    var item = items[tooltipItem.index].value;
                    return item.count + ' (' + item.perc + '%)'  ;
                }
            }
        }
    }
}

openList( employeeResponses: Array<IFeedbackResponseData>, feedbackItemId: number): MatDialogRef<EmployeeEvaluationsDialogComponent, any> {

    let modalInstance = this.dialog.open(EmployeeEvaluationsDialogComponent, {
        width: '500px',
        data: {
            responseList : employeeResponses,
            feedback: this.feedback,
            feedbackItemId: feedbackItemId,
            feedbackResponse: null,
        }
    });

    modalInstance.afterClosed()
    .subscribe(result => {
        if(result == null) return;
    });

    return modalInstance;
}

// dynamic css classes
// GetClassForGraph(isEmployee: boolean): string{
//     if( this.showEmployeeGraph && this.showManagerGraph ) return 'col-12 col-xl-4';
//     if( isEmployee && this.showEmployeeGraph) return 'col-12 col-xl-auto';
//     if( !isEmployee && this.showManagerGraph) return 'col-12 col-xl-auto';
//     return 'col-md-0';
// }
GetClassForGraph(){
  this.multipleCharts = false;

  if( this.showEmployeeGraph && this.showManagerGraph ) {
    this.multipleCharts = true;
  }
}

// Set by CSS
// GetStyleForGraph(isEmployee: boolean): string{
//     if( this.showEmployeeGraph && this.showManagerGraph ) return '125px';
//     return '200px';
// }
// GetClassForLegend(): string{
//     if( this.showEmployeeGraph && this.showManagerGraph )  return 'col-12 col-xl-4';
//     if( this.showEmployeeGraph) return 'col-12 col-xl-auto';
//     if( this.showManagerGraph) return 'col-12 col-xl-auto';
//     return 'col-md-0';
// }
}


interface GraphItem {
  id: number;
  name: string;
  value: any;
  isEmployee: boolean;
}
