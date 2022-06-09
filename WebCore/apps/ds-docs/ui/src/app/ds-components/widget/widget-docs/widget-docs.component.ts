import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-widget-docs',
  templateUrl: './widget-docs.component.html',
  styleUrls: ['./widget-docs.component.scss']
})
export class WidgetDocsComponent implements OnInit {

    toggleWidget = false;
    toggleTopWidget = false;
  constructor() { }

  ngOnInit() {
  }

}
