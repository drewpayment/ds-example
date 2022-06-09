import { isNgTemplate } from '@angular/compiler';
import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { DemographicInfo } from '@ds/analytics/shared/models/DemographicData.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'ds-demographic-dialog',
  templateUrl: './demographic-dialog.component.html',
  styleUrls: ['./demographic-dialog.component.css']
})
export class DemographicDialogComponent implements OnInit {

  dataSource: MatTableDataSource<DemographicInfo>;
  displayedColumns: string[] = ['Name', 'Status', 'Pay Type', 'Length Of Service', 'Age', 'Gender', 'Ethnicity'];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  los: string;
  remainder: number;
  years: number;
  months: number;
  gender: string;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<DemographicDialogComponent>
  ) {}

  close(): void {
    this.dialogRef.close();
  }

  ngOnInit() {

    this.formatLengthOfService(this.data.demographicData);
    this.formatGender(this.data.demographicData);
    this.dataSource = new MatTableDataSource<DemographicInfo>(this.data.demographicData);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.dataSource.sortingDataAccessor = (item, sortColumn) => {

      switch (sortColumn) {
          case 'Name':
              return item.lastName;
          case 'Status':
              return item.status;
          case 'Pay Type':
              return item.payType;
          case 'Length Of Service':
              return item.lengthOfService;
          case 'Age':
              return item.age;
          case 'Gender':
              return item.gender;
          case 'Ethnicity':
              return item.ethnicity;
           default:
              return item[this.sort.active];
      }
    };
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  print = () => {
    var employees = this.data.demographicData;

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
    printWindow.document.write('<h1 class="print-header">Demographic Information</h1>');
    printWindow.document.write('<div class="print-page-section-group">');
    if (employees.length == 1)
      printWindow.document.write('<div class="instruction-text">'+ employees.length + ' Employee</div>');
    else{
      printWindow.document.write('<div class="instruction-text">'+ employees.length + ' Employees</div>');
    }
    printWindow.document.write('<div class="instruction-text">Date Range: ' + this.convertDate(new Date(this.data.dateRange.StartDate)) + ' - ' + this.convertDate(new Date(this.data.dateRange.EndDate)) + '</div>');
    printWindow.document.write('<div class="instruction-text">' + this.data.featureName + '</div>');
    printWindow.document.write('</div>');
    printWindow.document.write('</div>');
    printWindow.document.write('<table>');

    printWindow.document.write('<tr>');
    printWindow.document.write('<th>Name</th>');
    printWindow.document.write('<th>Status</th>');
    printWindow.document.write('<th>Pay Type</th>');
    printWindow.document.write('<th>Length of Service</th>');
    printWindow.document.write('<th>Age</th>');
    printWindow.document.write('<th>Gender</th>');
    printWindow.document.write('<th>Ethnicity</th>');
    printWindow.document.write('</tr>');

    employees.forEach(employee => {
      printWindow.document.write('<tr>');
      printWindow.document.write('<td>' + employee.lastName + ', ' + employee.firstName + '</td>');
      printWindow.document.write('<td>' + employee.status + '</td>');
      printWindow.document.write('<td>' + employee.payType + '</td>');
      printWindow.document.write('<td>' + employee.lengthOfService + ' Months</td>');
      printWindow.document.write('<td>' + employee.age + '</td>');
      printWindow.document.write('<td>' + employee.gender + '</td>');
      printWindow.document.write('<td>' + employee.ethnicity + '</td>');
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

 formatLengthOfService(array) {
      array.forEach(element => {
            this.los = '';
            this.years = 0;
            this.months = 0;

            // If length of service is more than 12 months
            if ( element.lengthOfService >= 12 ) {
                this.years = Math.floor(element.lengthOfService / 12);
                this.months = element.lengthOfService - (this.years * 12)
            }
            if ( element.lengthOfService < 12 ) {
                // They have worked less than 12 months
                this.months = element.lengthOfService;
            }

            if ( this.years > 0 ) {
                this.los += this.years + (this.years === 1 ? ' year' : ' years') + ( this.months === 0 ? "" : ", ");
            }
            if ( this.months > 0 ) {
                this.los += this.months + (this.months === 1 ? ' month' : ' months');
            }

            // Update formatted elem to preserve sorting
            element.lengthOfServiceFormatted = this.los;
            return element.lengthOfServiceFormatted;
      });



  }

  formatGender(array) {
      array.forEach(element => {
        this.gender = "";

        if ( element.gender == "M") {
            this.gender = "Male";
        }
        else if ( element.gender == "F") {
            this.gender = "Female";
        }
        else {
            this.gender = "Not Specified";
        }

        element.genderFormatted = this.gender;
        return element.genderFormatted;
      });
  }
}
