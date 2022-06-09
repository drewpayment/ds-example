import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-chart-docs',
  templateUrl: './chart-docs.component.html',
  styleUrls: ['./chart-docs.component.scss']
})
export class ChartDocsComponent implements OnInit {

  toggleLegend: false
  toggleLegendLevels: false
  toggleColors: false;
  toggleWidget: false;
  color = 1;
  widgetHTMLActive = true;
  toggleLegendTop: false;
  
  constructor() { }

  ngOnInit() {
  }

}
