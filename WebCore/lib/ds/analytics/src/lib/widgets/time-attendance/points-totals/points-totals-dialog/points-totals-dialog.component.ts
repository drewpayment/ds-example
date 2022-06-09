import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'ds-points-totals-dialog',
  templateUrl: './points-totals-dialog.component.html',
  styleUrls: ['./points-totals-dialog.component.css']
})
export class PointsTotalsDialogComponent implements OnInit {

  dataSource: MatTableDataSource<any>;
  displayedColumns: string[] = ["Employee Name", "Total Points", "Total Hours Lost"];
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<PointsTotalsDialogComponent>) {}

  close(): void {
    this.dialogRef.close();
  }

  ngAfterViewInit() {
  }

  ngOnInit() {
      this.dataSource = new MatTableDataSource<any>(this.data.dataObjects);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.dataSource.sortingDataAccessor = (item, sortColumn) => {
        switch (sortColumn) {
            case 'Employee Name':
                return item.name;
            case 'Total Points':
                return item.amount;
            case 'Total Hours Lost':
                return item.hours;
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
      printWindow.document.write('<h1 class="print-header">Point Totals</h1>');
      printWindow.document.write('<div class="print-page-section-group">');
      if (employees.length == 1)
        printWindow.document.write('<div class="instruction-text">'+ employees.length + ' Employee</div>');
      else{
        printWindow.document.write('<div class="instruction-text">'+ employees.length + ' Employees</div>');
      }
      printWindow.document.write('<div class="instruction-text">Date Range: ' + this.convertDate(new Date(this.data.dateRange.StartDate)) + ' - ' + this.convertDate(new Date(this.data.dateRange.EndDate)) + '</div>');
      printWindow.document.write('<div class="instruction-text">Feature Name: ' + this.data.featureName + '</div>');
      printWindow.document.write('</div>');
      printWindow.document.write('</div>');
      printWindow.document.write('<table>');

      printWindow.document.write('<tr>');
      printWindow.document.write('<th>Employee Name</th>');
      printWindow.document.write('<th>Total Points</th>');
      printWindow.document.write('<th>Total Hours Lost</th>');
      printWindow.document.write('</tr>');

      employees.forEach(employee => {
        printWindow.document.write('<tr>');
        printWindow.document.write('<td>' + employee.name + '</td>');
        printWindow.document.write('<td>' + employee.amount + '</td>');
        printWindow.document.write('<td>' + employee.hours.toFixed(2) + '</td>');
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
