import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-list-docs',
  templateUrl: './list-docs.component.html',
  styleUrls: ['./list-docs.component.scss']
})
export class ListDocsComponent implements OnInit {

  toggleList = false
  toggleActionList = false
  toggleSelectionList = false
  toggleComplexList = false
  toggleLargeComplexList = false
  toggleEmptyList = false
  toggleButtonList = false
  toggleCollapseList = false
  constructor() { }

  ngOnInit() {
  }

}
