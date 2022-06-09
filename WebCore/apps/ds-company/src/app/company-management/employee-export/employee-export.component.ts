import { AccountService } from '@ds/core/account.service';
import { Component, Inject, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserInfo } from "@ds/core/shared";
import { switchMap, tap } from 'rxjs/operators';
import { CompanyManagementService } from '../../services/company-management.service';
import { EmployeeExportModalComponent } from './employee-export-modal/employee-export-modal.component';
import { EmployeeExport } from '../shared/models/employee-export.model';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource } from '@angular/material/table';
import * as saveAs from 'file-saver';

@Component({
  selector: 'ds-employee-export',
  templateUrl: './employee-export.component.html',
  styleUrls: ['./employee-export.component.scss']
})
export class EmployeeExportComponent implements OnInit {
  loaded: Boolean = false;
  user: UserInfo;
  dataSource: MatTableDataSource<EmployeeExport>;
  selectedEEs: MatTableDataSource<EmployeeExport>;
  displayedColumns: string[] = ["select", "name", "number", "email"];
  allowMultiSelect: boolean = true;
  selectedEmployees: EmployeeExport[] = [];
  selection = new SelectionModel<EmployeeExport>(this.allowMultiSelect, this.selectedEmployees);
  nothingSelected: boolean = false;
  expensify: boolean = false;
  filteredData: EmployeeExport[] = [];
  selectedEEsLoaded: boolean = false;
  exportType: number = 0;

  @ViewChild(MatPaginator, { static: false })
  set paginator(value: MatPaginator) {
    if (this.selectedEEs) {
      this.selectedEEs.paginator = value;
    }
  }
  @ViewChild(MatSort, { static: false })
  set sort(value: MatSort) {
    if (this.selectedEEs) {
      this.selectedEEs.sort = value;
      this.selectedEEs.sortingDataAccessor = (item, sortColumn) => {
        switch (sortColumn) {
          case 'name':
            return item.lastName;
          case 'number':
            return Number(item.employeeNumber.toString());
          case 'email':
            return item.emailAddress;
          default:
            return item[sortColumn];
        }
      }
    }
  }

  constructor(
    private dialog: MatDialog,
    private companymanagementservice: CompanyManagementService,
    private accountservice: AccountService) { }

  ngAfterViewInit() {
    this.selectedEEs.paginator = this.paginator;
    this.selectedEEs.sort = this.sort;
  }

  ngOnInit() {
    this.selectedEEs = new MatTableDataSource();
    this.loaded = false;
    this.accountservice
      .getUserInfo()
      .pipe(
        switchMap((user) => {
          this.user = user;
          return this.companymanagementservice.GetEmployeeExportList(this.user.clientId)
        }),
        tap((data: any) => {
          this.dataSource = data;
          this.dataSource.data = data;

          this.selectedEEs.data = data;
          this.selectedEEs = new MatTableDataSource<EmployeeExport>();
        }),
        switchMap((client) => {
          var clientAccountFeature = this.companymanagementservice.GetClientAccountFeatures(this.user.clientId);
          return clientAccountFeature;
        }),
        tap((x: any) => {
          if (x !== null && x.accountFeature === 22) {
            this.expensify = true;
          }
          this.loaded = true;
        })
      ).subscribe();
  }

  addEmployeeModal() {
    this.filterData();
    const modalData = new SelectionModel<EmployeeExport>()
    const employeeData = this.dataSource
    this.dialog.open(EmployeeExportModalComponent,
      {
        width: '800px',
        data: {
          employeeData: employeeData,
          filterableData: this.filteredData,
          selectionData: Object.assign(modalData, this.selection)
        }
      })
      .afterClosed()
      .subscribe((data) => {
        if (data !== undefined) {
          this.selectedEEs.data = data;

          if (this.selectedEEs.data !== undefined && this.selectedEEs.data.length > 0) {
            this.selectedEEsLoaded = true;
            this.selection.clear();
            this.masterToggle();
          }
          if (this.selectedEEs.data.length === 0) {
            this.selectedEEsLoaded = false;
          }
        }
      });
  }

  filterData() {
    this.dataSource.data.forEach(x => {
      this.filteredData.push({
        employeeId: null,
        firstName: x.firstName,
        lastName: x.lastName,
        employeeNumber: x.employeeNumber,
        emailAddress: null,
        departmentId: null,
        divisionId: null,
        costCenterId: null,
        employmentStatus: null,
        isActive: null
      })
    })
  }

  masterToggle() {
    if (this.exportType == 0) {
      this.isAllSelected() ? this.selection.clear() : this.selectedEEs.data.forEach(item => this.selection.select(item));
    }
    else if (this.exportType == 1) {
      this.isAllSelectedExpenisfy() ? this.selection.clear() : this.selectedEEs.data.forEach(item => this.selection.select(item));
      var tempSelected: SelectionModel<EmployeeExport>;
      tempSelected = this.selection;

      tempSelected.selected.forEach(x => {
        if (x.emailAddress === null || x.emailAddress === '') {
          tempSelected.deselect(x)
        }
      });
    }
  }

  isAllSelected(): boolean {
    const numSelected = this.selection.selected.length;
    const numRows = this.selectedEEs.data.length;
    return numSelected == numRows;
  }

  isAllSelectedExpenisfy(): boolean {
    const numSelected = this.selection.selected.filter(x => x.emailAddress !== null).length;
    const numRows = this.selectedEEs.data.filter(x => x.emailAddress !== null).length;
    return numSelected == numRows;
  }

  isSelected(item: EmployeeExport): boolean {
    return this.selection.selected.find(i => i.employeeId == item.employeeId) != null;
  }

  rowsSelected() {
    // Check if any rows are selected. If there are none, set an invalid flag
    return this.nothingSelected = this.selection.selected.length == 0;
  }

  cancel() {
    this.selectedEEs.data = [];
    this.selection.clear();
    this.selectedEEsLoaded = false;
    this.exportType = 0;
  }

  changeExportType(value: number) {
    var tempSelected: SelectionModel<EmployeeExport>;
    if (value == 0) {
      this.exportType = value;
      this.masterToggle();
    }
    if (value == 1) {
      this.exportType = value;
      tempSelected = this.selection;

      tempSelected.selected.forEach(x => {
        if (x.emailAddress === null || x.emailAddress === '') {
          tempSelected.deselect(x)
        }
      });
    }
  }

  print = () => {
    var employees = this.selection.selected;

    var printWindow = window.open("Employee Export", 'Print' + (new Date()).getTime(), 'width=1200,height=700');
    printWindow.document.write('<!DOCTYPE html><html><head>');

    for (var i = 0; i < document.styleSheets.length; i++) {
      if (document.styleSheets[i].href) {
        printWindow.document.write('<link href= "' + document.styleSheets[i].href + '" rel="stylesheet" type="text/css" >');
      }
    }

    printWindow.document.write('</head><body style="background-color:#fff;">');
    printWindow.document.write('<style> html{font-size: 12px} @page{size:landscape;}</style>');
    printWindow.document.write('<div class="print-page">');
    printWindow.document.write('<div class="print-page-header">');
    printWindow.document.write('<h1 class="print-header">Employee Export</h1>');
    printWindow.document.write('<div class="print-page-section-group">');
    printWindow.document.write('<table>');

    printWindow.document.write('<tr>');
    printWindow.document.write('<th>Employee Name</th>');
    printWindow.document.write('<th>Employee Number</th>');
    printWindow.document.write('<th>Email</th>');
    printWindow.document.write('</tr>');

    employees.forEach(employee => {
      printWindow.document.write('<tr>');
      printWindow.document.write(`<td> ${employee.lastName}, ${employee.firstName} ${employee.isActive ? "" : "- T"} </td>`);
      printWindow.document.write('<td>' + employee.employeeNumber + '</td>');
      printWindow.document.write(`<td> ${employee.emailAddress ? employee.emailAddress : ""} </td>`);
      printWindow.document.write('<tr>');
    })

    printWindow.document.write('</table>');
    printWindow.document.write('</div>');
    printWindow.document.write('</div>');

    printWindow.document.write('</div>')
    printWindow.document.write('<script>window.print();</script>');
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    printWindow.focus();

    printWindow.addEventListener('afterprint', (event) => {
      printWindow.close();
    });
  }

  downloadCSV() {
    if (this.selection.selected.length >= 1 && this.exportType == 0) {
      let csvHeader: string = "EmployeeId, FirstName, LastName, EmployeeNumber, EmailAddress, DepartmentId, DivisionId, CostCenterId, EmployementStatus, IsActive";
      let csvBody: string = "";

      this.selection.selected.sort((a, b) => {
        const nameA = a.lastName.toLowerCase();
        const nameB = b.lastName.toLowerCase();

        if (nameA < nameB) return -1;
        if (nameA > nameB) return 1;
        return 0;
      })

      this.selection.selected.forEach(x => {
        csvBody += `${x.employeeId}, ${x.firstName}, ${x.lastName}, ${x.employeeNumber}, ${x.emailAddress ? x.emailAddress : ""}, ${x.departmentId ? x.departmentId : ""}, ${x.divisionId ? x.divisionId : ""}, ${x.costCenterId ? x.costCenterId : ""}, ${x.employmentStatus ? x.employmentStatus : ""}, ${x.isActive ? "TRUE" : "FALSE"}\n`;
      });

      let byteString = `${csvHeader}\n${csvBody}`;

      let ab = new ArrayBuffer(byteString.length);
      let ia = new Uint8Array(ab);

      for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
      }
      var file = new Blob([ab], { type: 'application/octet-stream' });
      saveAs(file, "Employees.csv");
    }
    else if (this.selection.selected.length >= 1 && this.exportType == 1) {
      var csvHeader: string = "EmailAddress, PayrollId";
      var csvBody: string = "";

      this.selection.selected.forEach(x => {
        csvBody += `${x.emailAddress}, ${x.employeeNumber}\n`;
      });
      let byteString = `${csvHeader}\n${csvBody}`;

      let ab = new ArrayBuffer(byteString.length);
      let ia = new Uint8Array(ab);

      for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
      }
      var file = new Blob([ab], { type: 'application/octet-stream' });
      saveAs(file, "ExpensifyPeople.csv");
    }
  }
}
