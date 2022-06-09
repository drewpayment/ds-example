import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-empty-states-docs',
  templateUrl: './empty-states-docs.component.html',
  styleUrls: ['./empty-states-docs.component.scss']
})
export class EmptyStatesDocsComponent implements OnInit {

  toggleEmptyList: false;
  toggleEmptyCycle: false;
  toggleKanban: false;
  toggleAddTile: false;
  togglePayrollClosed: false;
  togglePermissions: false;
  toggleEmptyStatesVertical: false;
  PayrollClosedExample = 1;
  constructor() { }

  ngOnInit() {
  }

}
