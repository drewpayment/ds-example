import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-multi-select-docs',
  templateUrl: './multi-select-docs.component.html',
  styleUrls: ['./multi-select-docs.component.scss']
})
export class MultiSelectDocsComponent implements OnInit {

  toggleMultiSelect: boolean = false;
  toggleBulletList: boolean = false;
  toggleMultiSelectBorderless: boolean = false;
  constructor() { }

  ngOnInit() {
  }

}
