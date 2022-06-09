import { Component, OnInit, AfterViewInit, ElementRef, ViewChild, Input } from '@angular/core';
import { PayrollService } from '../shared/payroll.service';
import { ActivatedRoute } from '@angular/router';
import { IBankDepositCountInfo } from '../shared';
import { BaseChartDirective } from 'ng2-charts';
import { IChartSettings } from '@ds/performance/evaluations';

@Component({
  selector: 'ds-payment-count-widget',
  templateUrl: './payment-count-widget.component.html',
  styleUrls: ['./payment-count-widget.component.scss']
})
export class PaymentCountWidgetComponent implements OnInit, AfterViewInit {

  chartType ='bar'
  colors: any = ['#42B659','#B2E6BC'];
  paymentTotal = 0;
  isLoading: Boolean = false;
  showChart: Boolean = false;
  bdCount: IBankDepositCountInfo;
  chartSettings: IChartSettings;
  chartLabels: string[] = [];
  chartData: any = [{ data: [] }];
  chartOptions: any = null;
  chartInitialized = false;
  customLegend: string;
  @ViewChild('paymentCountLegend', { static: true }) legend: ElementRef<HTMLElement>;
  @ViewChild('paymentInfoChart', { static: false }) chartRef: BaseChartDirective;
  @ViewChild('chartDiv', { static: false }) chartTemplate;
  @Input('payrollId') payrollId: number;
  constructor(private payrollApi: PayrollService, private route: ActivatedRoute) { }

  ngOnInit() {
    if (this.payrollId == null || this.payrollId == 0)
      this.payrollId = +this.route.snapshot.paramMap.get('payrollId');
    this.payrollApi.getPayrollHistoryBankDepositCountInfoByPayrollId(this.payrollId)
    .subscribe(data => {
        if (data != null) {
          this.bdCount      = data;
          this.paymentTotal = this.bdCount.employeeDirectDepositCount + this.bdCount.employeeCheckCount
          + this.bdCount.vendorDirectDepositCount + this.bdCount.vendorCheckCount;
          this.isLoading    = false;
          this.buildGraph();
          this.showChart = true;
          /**
           * This needs to be wrapped in a timeout, because it allows the chart to render before executing this code.
           * If we don't, a chart element will never be rendered on the DOM by the time this execution happens. 
           */
          setTimeout(() => {
              if (!this.chartRef || !this.chartRef.chart) return;
              this.customLegend = this.chartRef.chart.generateLegend() as any;
              this.legend.nativeElement.innerHTML = this.customLegend;
          }, 100);
        }
      });
  }

  ngAfterViewInit() {
    
  }

  buildGraph() {

    this.chartLabels = ['Direct Deposit', 'Checks'];
    this.chartData = [
      {
        label: 'Employees',
        backgroundColor: '#42B659',
        borderWidth: 0,
        data: [this.bdCount.employeeDirectDepositCount, this.bdCount.employeeCheckCount]
      }, {
        label: 'Vendors',
        backgroundColor: '#B2E6BC',
        borderWidth: 0,
        data: [this.bdCount.vendorDirectDepositCount, this.bdCount.vendorCheckCount]
      }
    ];
    this.chartOptions = {
      responsive: true,
      legend: {
        display: false,
      },
      scales: {
        yAxes: [{
          ticks: {
            beginAtZero: true,
            callback: function(value) {if (value % 1 === 0) {return value; }}
          }
        }]
      },
      legendCallback: function(chart) {
        let legendHtml = [];
        legendHtml.push('<div class="ds-legend-group center">');
        let item = chart.data.datasets;
        for (let i = 0; i < item.length; i++) {
            legendHtml.push('<div class="ds-legend"> ');
            legendHtml.push('<div class="ds-legend-color" style="background-color:' + item[i].backgroundColor + '!important;">');
            legendHtml.push('</div>');
            legendHtml.push('<div><span class="text-uppercase text-muted">' + item[i].label + '</span> </div>');
            legendHtml.push('</div>');
        }

        legendHtml.push('</div>');
        return legendHtml.join('');
      }
    };
  }

}
