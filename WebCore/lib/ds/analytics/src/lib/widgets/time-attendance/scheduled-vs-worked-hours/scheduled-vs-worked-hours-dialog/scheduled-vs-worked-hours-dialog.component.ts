import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ScheduleWorked } from '@ds/analytics/shared/models/ScheduledWorkedData.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'ds-scheduled-vs-worked-hours-dialog',
  templateUrl: './scheduled-vs-worked-hours-dialog.component.html',
  styleUrls: ['./scheduled-vs-worked-hours-dialog.component.css']
})
export class ScheduledVsWorkedHoursDialogComponent implements OnInit {

  dataSource: MatTableDataSource<ScheduleWorked>;
  displayedColumns: string[] = ['Name', 'Date', 'Schedule Start', 'Schedule End', 'Scheduled Hours', 'Actual Hours'];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<ScheduledVsWorkedHoursDialogComponent>) { }

  close(): void {
    this.dialogRef.close();
  }

  ngOnInit() {
    this.dataSource = new MatTableDataSource<ScheduleWorked>(this.data.dataObjects);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.dataSource.sortingDataAccessor = (item, sortColumn) => {
      switch (sortColumn) {
          case 'Name':
              return item.employeeName;
          case 'Date':
              return item.shortDate;
          case 'Schedule Start':
              return item.scheduleStart;
          case 'Schedule End':
              return item.stopSchedule;
          case 'Scheduled Hours':
              return item.hoursScheduled;
          case 'Actual Hours':
              return item.actualHours;
           default:
              return item[this.sort.active];
      }
    };
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  print = () => {
    var employees = this.data.dataObjects;

    employees.sort((a, b) => { a.employeeName > b.employeeName ? -1 : 1 })

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
    printWindow.document.write('<h1 class="print-header">Scheduled vs Worked Hours</h1>');
    printWindow.document.write('<div class="print-page-section-group">');
    printWindow.document.write('<div class="instruction-text">'+ employees.length + ' Employees</div>');
    printWindow.document.write('<div class="instruction-text">Date Range: ' + this.convertDate(new Date(this.data.dateRange.StartDate)) + ' - ' + this.convertDate(new Date(this.data.dateRange.EndDate)) + '</div>');
    printWindow.document.write('<div class="instruction-text">Feature Name: ' + (this.data.featureName ? this.data.featureName : 'Not Specified') + '</div>');
    printWindow.document.write('</div>');
    printWindow.document.write('</div>');
    printWindow.document.write('<table>');

    printWindow.document.write('<tr>');
    printWindow.document.write('<th>Name</th>');
    printWindow.document.write('<th>Schedule</th>');
    printWindow.document.write('<th>Schedule Start</th>');
    printWindow.document.write('<th>Schedule End</th>');
    printWindow.document.write('<th>Scheduled Hours</th>');
    printWindow.document.write('<th>Actual Hours</th>');
    printWindow.document.write('</tr>');

    employees.forEach(employee => {
      printWindow.document.write('<tr>');
      printWindow.document.write('<td>' + employee.employeeName + '</td>');
      printWindow.document.write('<td>' + employee.shortDate + '</td>');
      printWindow.document.write('<td>' + employee.startSchedule + '</td>');
      printWindow.document.write('<td>' + employee.stopSchedule + '</td>');
      printWindow.document.write('<td>' + employee.hoursScheduled + '</td>');
      printWindow.document.write('<td>' + employee.actualHours + '</td>');
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
