import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-grid-docs',
  templateUrl: './grid-docs.component.html',
  styleUrls: ['./grid-docs.component.scss']
})
export class GridDocsComponent implements OnInit {

  toggleGrid: false;

  TabExample = 1;
  constructor() { }

  ngOnInit() {
  }

}
