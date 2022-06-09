import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { EmployeeExport } from '../../shared/models/employee-export.model';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'ds-employee-export-modal',
  templateUrl: './employee-export-modal.component.html',
  styleUrls: ['./employee-export-modal.component.scss']
})
export class EmployeeExportModalComponent implements OnInit {

  dataSource: MatTableDataSource<EmployeeExport>;
  filteredData: MatTableDataSource<EmployeeExport>;
  displayedColumns: string[] = ["select", "name", "number"];

  allowMultiSelect: boolean = true;
  selectedEmployees: EmployeeExport[] = [];
  selectionModal = new SelectionModel<EmployeeExport>();
  nothingSelected: boolean = false;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<EmployeeExport>) {

  }

  close(): void {
    this.dialogRef.close();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }


  ngOnInit() {
    this.dataSource = this.data.employeeData;
    this.filteredData = this.data.filterableData;
    this.dataSource = new MatTableDataSource<EmployeeExport>(this.data.employeeData);
    const tempSelectedEmployee = this.data.selectionData;
    this.selectionModal = tempSelectedEmployee;
    this.dataSource.filterPredicate = function(data, filter: string): boolean {
      return data.firstName.toLowerCase().includes(filter) || data.lastName.toLowerCase().includes(filter) || data.employeeNumber.toString().includes(filter);
  };
    this.dataSource.sortingDataAccessor = (item, sortColumn) => {
      switch (sortColumn) {
        case 'name':
          return item.lastName;
        case 'number':
          return Number(item.employeeNumber.toString());
        default:
          return item[sortColumn];
      }
    };
  }

  masterToggle() {
    this.isAllSelectedModal() ? this.selectionModal.clear() : this.dataSource.data.forEach(item => this.selectionModal.select(item));
  }

  isAllSelectedModal(): boolean {
    const numSelected = this.selectionModal.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected == numRows;
  }
  isSelected(item: EmployeeExport): boolean {
    return this.selectionModal.selected.find(i => i.employeeId == item.employeeId) != null;
  }

  rowsSelected() {
    // Check if any rows are selected. If there are none, set an invalid flag
    return this.nothingSelected = this.selectionModal.selected.length == 0;
  }

  selectedEEs() {
    this.dialogRef.close(this.selectionModal.selected.sort((a, b) => {
      const nameA = a.lastName.toLowerCase();
      const nameB = b.lastName.toLowerCase();

      if (nameA < nameB) return -1;
      if (nameA > nameB) return 1;
      return 0;
    }));
  }

  changeEmployeeStatus(value: number) {
    this.dataSource.data = this.data.employeeData;
    if (value == 1) {
      this.dataSource.data = this.data.employeeData.filter(x => x.isActive == true);
    }
    else if (value == 2) {
      this.dataSource.data = this.data.employeeData.filter(x => x.isActive == false);
    }
  }


  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }


}
