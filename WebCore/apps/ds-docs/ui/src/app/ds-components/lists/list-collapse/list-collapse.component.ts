import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-list-collapse',
  templateUrl: './list-collapse.component.html',
  styleUrls: ['./list-collapse.component.scss']
})
export class ListCollapseComponent implements OnInit {

  showDetail = false
  constructor() { }

  ngOnInit() {
  }

}
