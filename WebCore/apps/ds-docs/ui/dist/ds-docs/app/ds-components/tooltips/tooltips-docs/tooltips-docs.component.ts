import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-tooltips-docs',
  templateUrl: './tooltips-docs.component.html',
  styleUrls: ['./tooltips-docs.component.scss']
})
export class TooltipsDocsComponent implements OnInit {

  toggleHelp: boolean = false
  toggleTooltip: boolean = false
  constructor() { }

  ngOnInit() {
  }

}
