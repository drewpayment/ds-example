import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-layouts-docs',
  templateUrl: './layouts-docs.component.html',
  styleUrls: ['./layouts-docs.component.scss']
})
export class LayoutsDocsComponent implements OnInit {

  toggleStandardPage: false
  toggleSideBySide: false
  toggleAlignment: false
  toggleOverflow: false;
  toggleTextBreak: false;
  constructor() { }

  ngOnInit() {
  }

}
