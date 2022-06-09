import { Component, OnInit } from '@angular/core';
import { InfoData } from '@ds/analytics/shared/models/InfoData.model';

@Component({
  selector: 'ds-chart-widget',
  templateUrl: './chart-widget.component.html',
  styleUrls: ['./chart-widget.component.scss']
})
export class ChartWidgetComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  headerDropdownOptions: string[] = ['Testing 1', 'Testing 2', 'Testing 3']; //Header Dropdown options
  settingsItems: string[] = ['Cancel', 'Remove', 'Cookie']; //Settings options
  chartOptions: string[] = ['pie', 'line', 'bar', 'donut']; //Select which charts you want to display

  cardType: string = 'info'; //"graph" or "info"
  size: Number = 4; //Columns in bootstrap grid
  title: string = 'Card Head: '; //Title of the card

  infoData: InfoData = {
    icon: 'today', //Icon used in the header
    color: 'info', //Icon color, choices from bootstrap text-colors
    value: '100%', //Text to the right of the icon
    title: 'Overall Information', //Title of the Info card
    showBottom: true //Show information below the title
  };

  headerDropdownChanged(event){
    //Header Dropdown has changed. Do something
    console.log(event)
  }

  chartChanged(event){
    //Chart view has changed. Do something
    console.log(event)
  }

  settingSelected(event){
    //Settings item selected. Do something
    console.log(event)
  }

}
