import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-table-docs',
  templateUrl: './table-docs.component.html',
  styleUrls: ['./table-docs.component.scss']
})
export class TableDocsComponent implements OnInit {

  toggleTable = false;
  toggleViewTable = false;
  toggleEditTable = false;
  toggleBasicTable = false;
  toggleEditResponsiveTable = false;
  toggleResponsiveTable = false;
  toggleStickyTable = false;
  toggleEditDialogTable = false;
  toggleAddPageTable = false;
  toggleValidationTable = false;
  toggleViewStickyTable = false;
  tableExample = 1;
  tableEditExample = 1;
  tableEditDialogExample = 1;
  tableStickyExample = 1;
  toggleAddPageExample = 1;
  tableViewExample = 1;
  tableValidationExample = 1;
  tableViewStickyExample = 1;
  
  
  constructor() { }

  ngOnInit() {
  }

}
