import { Component, OnInit, AfterViewInit, ElementRef, ViewChild, Input } from '@angular/core';
import { PayrollService } from '../shared/payroll.service';
import { IBankDepositInfo } from '../shared/bank-deposit-info.model';
import { ActivatedRoute } from '@angular/router';
import { CurrencyPipe } from '@angular/common';
import { DsPopupService } from '@ajs/ui/popup/ds-popup.service';
import { BaseChartDirective } from 'ng2-charts';

@Component({
  selector: 'ds-bank-deposit-widget',
  templateUrl: './bank-deposit-widget.component.html',
  styleUrls: ['./bank-deposit-widget.component.scss']
})
export class BankDepositWidgetComponent implements OnInit, AfterViewInit {

    bankDepositInfo: IBankDepositInfo;
    bankDepositTotal = 0;
    materialIcon = 'arrow_upward';
    percentDifference = 0;
    showPrevious: Boolean = false;
    showChart: Boolean = false;
    isLoading: Boolean = true;
    chartType = 'doughnut';
    chartLabels: string[] = [];
    chartData: any = [{ data: [] }];
    colors: string[] = ['rgb(21,54,92)', 'rgb(78,157,203)', 'rgb(203,224,241)']; // #15365C dark navy blue, #4E9DCB blue, #CBE0F1 light blue
    chartOptions: any = null;
    chartInitialized = false;
    customLegend: string;
    @ViewChild('depositLegend', { static: false }) legend: ElementRef<HTMLElement>;
    @ViewChild('bankDepositInfoChart', { static: false }) chartRef: BaseChartDirective;
    @ViewChild('chartDiv', { static: false }) chartTemplate;
    @Input('payrollId') payrollId: number;

    /********************************************
     * @constructor
     * 
     * @param { PaycheckListService } api
     ********************************************/
  constructor(private payrollApi: PayrollService, private route: ActivatedRoute, private DsPopup: DsPopupService) { }

  ngOnInit() {
    if (this.payrollId == null || this.payrollId == 0)
        this.payrollId = +this.route.snapshot.paramMap.get('payrollId');
    this.payrollApi.getPayrollHistoryBankDepositInfoByPayrollId(this.payrollId).subscribe(data => {
        if (data != null) {
            this.bankDepositInfo      = data;
            this.bankDepositTotal     = this.bankDepositInfo.bankDepositTotal;
            this.materialIcon         = this.bankDepositInfo.materialIcon;
            this.percentDifference    = this.bankDepositInfo.percentDifference;
            this.showPrevious         = this.bankDepositInfo.showPrevious;
            this.isLoading            = false;
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

  grossToNetReport() {
    const w = window,
    d = document,
    e = d.documentElement,
    g = d.getElementsByTagName('body')[0],
    x = w.innerWidth || e.clientWidth || g.clientWidth,
    y = w.innerHeight || e.clientHeight || g.clientHeight;
    
    const urlBuilder = `api/payroll/gross-to-net/payroll/${this.payrollId}`;
    this.DsPopup.open(urlBuilder, '_blank', { height: y / 1.25, width: x / 1.25, top: 0, left: 0 });

  }

  buildGraph() {

    // var ctx  = document.getElementById("bankDepositInfoChart");
    // var body = document.getElementById("chartBody");

    // body.classList.remove("d-flex");
    // body.classList.remove("align-items-center");

    this.chartLabels = ['Tax','EE','Vendor'];
    this.chartData   =  [{
        backgroundColor : this.colors,
        borderColor     : 'rgb(255,255,255)',
        data            : [this.bankDepositInfo.taxPaymentTotal, this.bankDepositInfo.eePaymentTotal, this.bankDepositInfo.vendorPaymentTotal],
    }];

    this.chartOptions = {
        responsive: true,
        maintainAspectRatio: false,
        legend: {display: false},
        tooltips: {enabled: false},
        cutoutPercentage: 70,
        legendCallback: function(chart) {
            let legendHtml = [];
            legendHtml.push('<div class="ds-legend-group">');
            legendHtml.push('<div class="h3">Payments</div>');
            let item = chart.data.datasets[0];
            for (let i = 0; i < item.data.length; i++) {
                legendHtml.push('<div class="ds-legend mb-4" style="padding-left:0;"> ');
                legendHtml.push('<div class="ds-legend-color" style="background-color:' + item.backgroundColor[i] + '!important;">');
                legendHtml.push('</div>');
                legendHtml.push('<div><span class="text-uppercase text-muted">' + chart.data.labels[i] + '</span> &nbsp; <span>' + (new CurrencyPipe('en-USD')).transform(item.data[i], 'USD') + '</span> </div>');
                legendHtml.push('</div>');
            }

            legendHtml.push('</div>');
            return legendHtml.join('');
        }
    }; // end of options

  } // end of buildGraph

}
