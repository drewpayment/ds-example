import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { OvertimeData } from '@ds/analytics/shared/models/Overtime.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'ds-overtime-dialog',
  templateUrl: './overtime-dialog.component.html',
  styleUrls: ['./overtime-dialog.component.css']
})
export class OvertimeDialogComponent implements OnInit {
  dataSource: MatTableDataSource<OvertimeData>;
  displayedColumns: string[] = ['Employee Name', 'Supervisor Name', 'Department', 'Hours Worked', 'Overtime Hours'];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<OvertimeDialogComponent>) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource<OvertimeData>(this.data.overtimeWorkers);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.dataSource.sortingDataAccessor = (item, sortColumn) => {
      switch (sortColumn) {
          case 'employeeName':
              return item.employeeName;
          case 'supervisor':
              return item.supervisor;
          case 'department':
              return item.department;
          case 'hoursScheduled':
              return item.totalHoursScheduled;
          case 'hoursWorked':
              return item.totalHoursWorked;
          case 'overtime':
              return item.overtimeHours;
           default:
              return item[this.sort.active];
      }
    };
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  print = () => {
    var employees = this.data.overtimeWorkers;

    employees.sort((a, b) => { a.firstName > b.firstName ? -1 : 1 })

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
    printWindow.document.write('<h1 class="print-header">Employee Overtime</h1>');
    printWindow.document.write('<div class="print-page-section-group">');
    printWindow.document.write('<div class="instruction-text">'+ employees.length + ' Employees</div>');
    printWindow.document.write('</div>');
    printWindow.document.write('</div>');
    printWindow.document.write('<table>');

    printWindow.document.write('<tr>');
    printWindow.document.write('<th>Employee Name</th>');
    printWindow.document.write('<th>Supervisor Name</th>');
    printWindow.document.write('<th>Department</th>');
    printWindow.document.write('<th>Hours Scheduled</th>');
    printWindow.document.write('<th>Hours Worked</th>');
    printWindow.document.write('<th>OT Hours</th>');
    printWindow.document.write('</tr>');

    employees.forEach(employee => {
      printWindow.document.write('<tr>');
      printWindow.document.write('<td>' + employee.employeeName + '</td>');
      printWindow.document.write('<td>' + employee.supervisor + '</td>');
      printWindow.document.write('<td>' + employee.department + '</td>');
      printWindow.document.write('<td>' + employee.totalHoursScheduled[employee.overtimeFrequency-1] + '</td>');
      printWindow.document.write('<td>' + employee.totalHoursWorked[employee.overtimeFrequency-1] + '</td>');
      printWindow.document.write('<td>' + employee.overtimeHours + '</td>');
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

  convertDate(date: Date) {
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    return `${months[date.getMonth()]} ${date.getDate()}, ${date.getFullYear()}`;
  }

  close(): void {
    this.dialogRef.close();
  }
}
