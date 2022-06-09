import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-auto-complete-docs',
  templateUrl: './auto-complete-docs.component.html',
  styleUrls: ['./auto-complete-docs.component.scss']
})
export class AutoCompleteDocsComponent implements OnInit {

  toggleAutoComplete = false;
  toggleAutoCompleteContact = false;
  toggleAutoCompleteMultiple = false;
  AutoCompleteExample = 1;
  AutoCompleteMultipleExample = 1;
  AutoCompleteContactExample = 1;
  constructor() { }

  ngOnInit() {
  }

}
