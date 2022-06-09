import { Component, OnInit, AfterViewInit, ViewChild, Input } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { IFeedbackSetup, IFeedbackResponseData } from '@ds/performance/feedback/';
import { EvaluationRoleType } from '@ds/performance/evaluations/shared/evaluation-role-type.enum';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { isNullOrUndefined } from '../../../../../../../utilties';
import { EmployeeEvaluationsDialogComponent } from '@ds/performance/performance-manager/review-group-status-view/employee-evaluations/employee-evaluations-dialog.component';

@Component({
    selector: 'ds-yesno-feedbacks-graph',
    templateUrl: './yesno-feedbacks-graph.component.html',
    styleUrls: ['./yesno-feedbacks-graph.component.scss']
})
export class YesNoFeedbacksGraphComponent implements OnInit, AfterViewInit {

    /** ChartJs specific vars SET 1*/
    piechartdata:GraphItem[];
    chartLabels:string[] = [];
    chartDataSet:any = [{ data: [] }];
    chartColors:string[] = null;
    chartType:string = 'bar';
    chartOptions:any = null;

    @ViewChild('yesnoBarChart', { static: false }) chartRef:BaseChartDirective;

    /** ChartJs specific vars SET 2*/
    piechartdata2:GraphItem[];
    chartDataSet2:any = [{ data: [] }];
    chartColors2:string[] = null;
    chartOptions2:any = null;

    @ViewChild('yesnoBarChart2', { static: false }) chartRef2:BaseChartDirective;

    @Input()
    feedback: IFeedbackSetup;

    @Input()
    responseList: Array<{ feedback: IFeedbackResponseData, reviewId: number }>;

  showChart:boolean = false;
  customLegend:string;
  private readonly monoColors = [
    '#04b5b0',
    '#04b5b0',
    '#04b5b0'
  ];
  private readonly monoColors2 = [
    '#324b68',
    '#324b68',
    '#324b68'
  ];
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
    '#957acd'];

    totalCount: number;
    showEmployeeGraph: boolean;
    showManagerGraph: boolean;

  constructor(private dialog: MatDialog ) { }

  ngOnInit() {
      if(this.responseList.length > 0  ) {
        this.buildChartData(this.responseList.map(x => x.feedback));
        if(this.showEmployeeGraph)  this.updateChart(this.piechartdata);
        if(this.showManagerGraph)   this.updateChart2(this.piechartdata2);

        if(this.showEmployeeGraph || this.showManagerGraph)
            this.showChart = true;
      }
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

        var flattenChartData = (items:Array<IFeedbackResponseData>, isEmp: boolean) : GraphItem[] => {
            let resultingArray: GraphItem[] = [];
            // No Answer
            let responses = items.filter(x=> (x.responseItems.length == 0 || isNullOrUndefined(x.responseItems[0].booleanValue)));
            let legend: GraphItem = { isEmployee: isEmp, name: "No Answer", value: {count: responses.length, perc:0} };
            resultingArray.push(legend);

            // No
            responses = items.filter(x=> (x.responseItems.length > 0 && x.responseItems[0].booleanValue == false));
            legend = { isEmployee: isEmp, name: "No", value: {count: responses.length, perc:0} };
            resultingArray.push(legend);

            // Yes
            responses = items.filter(x=> (x.responseItems.length > 0 && x.responseItems[0].booleanValue == true));
            legend = { isEmployee: isEmp, name: "Yes", value: {count: responses.length, perc:0} };
            resultingArray.push(legend);

            var totCount = resultingArray[0].value.count + resultingArray[1].value.count + resultingArray[2].value.count ;
            resultingArray[0].value.perc = Math.round( resultingArray[0].value.count * 1000 /totCount ) / 10;
            resultingArray[1].value.perc = Math.round( resultingArray[1].value.count * 1000 /totCount ) / 10;
            resultingArray[2].value.perc = Math.round( resultingArray[2].value.count * 1000 /totCount ) / 10;

            return resultingArray.reverse();
        }

        this.piechartdata = flattenChartData(employeeData, true);
        this.piechartdata2 = flattenChartData(managerData, false);
    }

}


private updateChart(items: GraphItem[] ) {
    this.chartLabels = items.map(g => g.name);
    this.chartColors = this.monoColors;

    this.chartDataSet = [{
        backgroundColor: this.monoColors,
        data: items.map(g => g.value.perc),
    }];

    this.chartOptions = this.getChartOptions(items,'Employees');
}

private updateChart2(items: GraphItem[] ) {
    this.chartLabels = items.map(g => g.name);
    this.chartColors2 = this.monoColors2;

    this.chartDataSet2 = [{
        backgroundColor: this.monoColors2,
        data: items.map(g => g.value.perc),
    }];

    this.chartOptions2 = this.getChartOptions(items,'Supervisors');
}

private getChartOptions(items: GraphItem[], titleTxt: string){
    var self = this;
    return {
        //responsive: false,
        legend: { display: false },
        // title: {
        //     display: true,
        //     text: titleTxt
        // },
        //cutoutPercentage: 70,
        onClick: function(e, elemSrc){
            if(!!elemSrc && elemSrc.length > 0){
                var selectedResponse = items[elemSrc[0]._index];
                let responses:Array<IFeedbackResponseData> = [];

                if(selectedResponse.isEmployee){
                    responses = self.responseList.map(x => x.feedback).filter(x => x.evaluationRoleType == EvaluationRoleType.Self );
                } else {
                    responses = self.responseList.map(x => x.feedback).filter(x => x.evaluationRoleType == EvaluationRoleType.Manager );
                }

                if(selectedResponse.name == "Yes"){
                    responses = responses.filter(x => (x.responseItems.length > 0 && x.responseItems[0].booleanValue == true));
                } else if(selectedResponse.name == "No"){
                    responses = responses.filter(x => (x.responseItems.length > 0 && x.responseItems[0].booleanValue == false));
                } else {
                    responses = responses.filter(x =>
                        (x.responseItems.length == 0 || isNullOrUndefined(x.responseItems[0].booleanValue)));
                }
                self.openList(responses, selectedResponse.name);
            }
            e.stopPropagation();
        },
        scales: {
            yAxes: [{
                // position: 'left',
                // display: true,
                ticks: {
                    min: 0,
                    max: 100,
                    stepSize: 20,
                    callback: function(value) {
                        return value + '% ';
                    }
                },
                gridLines: {
                  drawOnChartArea:false
                }
            }],
            xAxes: [{
              gridLines: {
                drawOnChartArea:false
              }

            }],
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

openList( employeeResponses: Array<IFeedbackResponseData>, responseTxt: string): MatDialogRef<EmployeeEvaluationsDialogComponent, any> {

    let modalInstance = this.dialog.open(EmployeeEvaluationsDialogComponent, {
        width: '500px',
        data: {
            responseList : employeeResponses,
            feedback: this.feedback,
            feedbackItemId: null,
            feedbackResponse: responseTxt,
        }
    });

    modalInstance.afterClosed()
    .subscribe(result => {
        if(result == null) return;
    });

    return modalInstance;
}

// dynamic css classes
GetClassForGraph(isEmployee: boolean): string{
    if( this.showEmployeeGraph && this.showManagerGraph ) return 'col-md-6';
    if( isEmployee && this.showEmployeeGraph) return 'col-md-12';
    if( !isEmployee && this.showManagerGraph) return 'col-md-12';
    return 'col-md-0';
}
GetStyleForGraph(isEmployee: boolean): string{
    if( this.showEmployeeGraph && this.showManagerGraph ) return '200px';
    return '400px';

}
}


interface GraphItem {
  isEmployee: boolean;
  name: string;
  value: any;
}
