import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-form-elements-docs',
  templateUrl: './form-elements-docs.component.html',
  styleUrls: ['./form-elements-docs.component.scss']
})
export class FormElementsDocsComponent implements OnInit {

  toggleInput: boolean = false;
  toggleSelect: boolean = false;
  toggleTextarea:boolean = false;
  toggleCheckbox: boolean = false;
  toggleCircleCheckbox: boolean = false;
  toggleRadio: boolean = false;
  togglePillCheckbox: boolean = false;
  togglePillRadio: boolean = false;
  toggleValidationRadio: boolean = false;
  toggleFileInput: boolean = false;
  toggleFileInputQueue: boolean = false;
  toggleTimeSpanSelector: boolean = false;
  toggleInline: boolean = false;
  toggleBorderlessSelect: boolean = false;
  toggleSpacing: boolean = false;
  toggleNesting: boolean = false;
  toggleDaySelector: boolean = false;
  toggleInputGroups: boolean = false;
  toggleSearch: boolean = false;
  constructor() { }

  ngOnInit() {
  }

}
