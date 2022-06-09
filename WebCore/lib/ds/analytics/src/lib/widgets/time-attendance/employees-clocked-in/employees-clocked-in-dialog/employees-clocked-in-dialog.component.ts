import { Component, OnInit, Inject, ViewChild, AfterViewInit } from '@angular/core';
import { ClockedInEmployees } from '@ds/analytics/shared/models/EmployeesClockedInData.model';
import { MatTableDataSource } from '@angular/material/table';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';


@Component({
  selector: 'ds-employees-clocked-in-dialog',
  templateUrl: './employees-clocked-in-dialog.component.html',
  styleUrls: ['./employees-clocked-in-dialog.component.css']
})
export class EmployeesClockedInDialogComponent implements OnInit {
  dataSource: MatTableDataSource<ClockedInEmployees>;
  displayedColumns: string[] = ['employeeName', 'supervisor', 'department', 'hoursScheduled', 'hoursWorked', 'overtime'];

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<EmployeesClockedInDialogComponent>) { }

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  close(): void {
    this.dialogRef.close();
}

ngAfterViewInit() {
}

  ngOnInit() {
    this.dataSource = new MatTableDataSource<ClockedInEmployees>(this.data.employeeData);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.dataSource.sortingDataAccessor = (item, sortColumn) => {
      switch (sortColumn) {
          case 'employeeName':
              return item.employeeName;
          case 'supervisor':
              return item.supervisorName;
          case 'department':
              return item.department;
          case 'hoursScheduled':
              return item.hoursScheduled;
          case 'hoursWorked':
              return item.hoursWorked;
          case 'overtime':
              return item.overtime;
           default:
              return item[this.sort.active];
      }
    };
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  print = () => {
    var employees = this.data.employeeData;
    if (employees != null){
      employees.sort((a, b) => { a.employeeName > b.employeeName ? -1 : 1 })
    }

    var printWindow = window.open(this.data.title, 'Print' + (new Date()).getTime(), 'width=1200,height=700');
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
    printWindow.document.write('<h1 class="print-header">'+ this.data.title + '</h1>');
    printWindow.document.write('<div class="print-page-section-group">');
    if (employees != null){
      if (employees.length == 1)
        printWindow.document.write('<div class="instruction-text">'+ employees.length + ' Employee</div>');
      else{
        printWindow.document.write('<div class="instruction-text">'+ employees.length + ' Employees</div>');
      }
    }
    else {
      printWindow.document.write('<div class="instruction-text">'+ "0" + ' Employee</div>');
    }
    printWindow.document.write('<div class="instruction-text">Date Range: ' + this.convertDate(new Date(this.data.dateRange.StartDate)) + ' - ' + this.convertDate(new Date(this.data.dateRange.EndDate)) + '</div>');
    printWindow.document.write('</div>');
    printWindow.document.write('</div>');
    printWindow.document.write('<table>');

    printWindow.document.write('<tr>');
    printWindow.document.write('<th>Name</th>');
    printWindow.document.write('<th>Supervisor</th>');
    printWindow.document.write('<th>Department</th>');
    printWindow.document.write('<th>Hours Scheduled</th>');
    printWindow.document.write('<th>Hours Worked</th>');
    printWindow.document.write('<th>Overtime Hours Worked</th>');
    printWindow.document.write('</tr>');

    if (employees != null){
    employees.forEach(employee => {
      printWindow.document.write('<tr>');
      printWindow.document.write('<td>' + employee.employeeName + '</td>');
      printWindow.document.write('<td>' + employee.supervisorName + '</td>');
      printWindow.document.write('<td>' + employee.department + '</td>');
      printWindow.document.write('<td>' + employee.hoursScheduled + '</td>');
      printWindow.document.write('<td>' + employee.hoursWorked + '</td>');
      if(employee.overtime === undefined) employee.overtime = '';
      printWindow.document.write('<td>' + employee.overtime + '</td>');
      printWindow.document.write('<tr>');
    })}

    printWindow.document.write('</table>');
    printWindow.document.write('</div>')
    printWindow.document.write('<script>window.print();</script>');
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    printWindow.focus();

    printWindow.addEventListener('afterprint', (event) => {
        printWindow.close();
    });
  }

  convertDate(date: Date){
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    return `${months[date.getMonth()]} ${date.getDate()}, ${date.getFullYear()}`;
  }
}
