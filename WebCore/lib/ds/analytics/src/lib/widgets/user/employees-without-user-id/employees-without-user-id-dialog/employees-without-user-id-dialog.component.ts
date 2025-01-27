import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { UserPerformanceDashboardDialogData, UserPerformanceDashboardEmployee } from '@ds/analytics/models';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';


@Component({
  selector: 'ds-employees-without-user-id-dialog',
  templateUrl: './employees-without-user-id-dialog.component.html',
  styleUrls: ['./employees-without-user-id-dialog.component.css']
})
export class EmployeesWithoutUserIdDialogComponent implements OnInit {
  dataSource: MatTableDataSource<UserPerformanceDashboardEmployee>;
  displayedColumns: string[] = ["Name", "Supervisor", "Department", "HireDate"];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
      @Inject(MAT_DIALOG_DATA) public data: UserPerformanceDashboardDialogData,
      public dialogRef: MatDialogRef<EmployeesWithoutUserIdDialogComponent>,
  ) {}

  close(): void {
      this.dialogRef.close();
  }

  ngAfterViewInit() {
  }

  ngOnInit() {
      this.dataSource = new MatTableDataSource<UserPerformanceDashboardEmployee>(this.data.employees);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.dataSource.sortingDataAccessor = (item, sortColumn) => {
        switch (sortColumn) {
            case 'Name':
                return item.lastName;
            case 'Supervisor':
                return item.supervisor;
            case 'Department':
                return item.department;
            case 'HireDate':
              return item.hireDate;
            default:
                return item[this.sort.active];
        }
    };
  }

  applyFilter(filterValue: string) {
      this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  print = () => {
      var employees = this.data.employees;

    //   employees.sort((a, b) => a.firstName > b.firstName ? -1 : 1 )

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
      if (employees.length == 1)
        printWindow.document.write('<div class="instruction-text">'+ employees.length.toLocaleString('en-US') + ' Employee</div>');
      else{
        printWindow.document.write('<div class="instruction-text">'+ employees.length.toLocaleString('en-US') + ' Employees</div>');
      }
      printWindow.document.write('</div>');
      printWindow.document.write('</div>');
      printWindow.document.write('<table>');

      printWindow.document.write('<tr>');
      printWindow.document.write('<th>Name</th>');
      printWindow.document.write('<th>Supervisor</th>');
      printWindow.document.write('<th>Department</th>');
      printWindow.document.write('<th>Hire Date</th>');
      printWindow.document.write('</tr>');

      employees.forEach(employee => {
        printWindow.document.write('<tr>');
        printWindow.document.write('<td>' + `${employee.lastName}, ${employee.firstName}` + '</td>');
        printWindow.document.write('<td>' + employee.supervisor + '</td>');
        if (employee.department === null || employee.department === "undefined" || employee.department === undefined){
          printWindow.document.write('<td>' + " " + '</td>');
        }
        else{
          printWindow.document.write('<td>' + employee.department + '</td>');
        }
        if (!employee.hireDate){
          printWindow.document.write('<td>' + " " + '</td>');
        }
        else{
          printWindow.document.write('<td>' + new Date(employee.hireDate).toLocaleDateString('en-US') + '</td>');
        }
        printWindow.document.write('<tr>');
      })

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
