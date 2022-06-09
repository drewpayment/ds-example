import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { BankDeposit } from '@ds/analytics/shared/models/BankDepositData.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'ds-bank-deposit-dialog',
  templateUrl: './bank-deposit-dialog.component.html',
  styleUrls: ['./bank-deposit-dialog.component.css']
})
export class BankDepositDialogComponent implements OnInit {

  dataSource: MatTableDataSource<BankDeposit>;
  displayedColumns: string[] = ['Name', 'Pay Type', 'Payment', 'MTD', 'QTD', 'YTD'];
  title: string;
  checkDate: string;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;


  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<BankDepositDialogComponent>
  ) {}

  close(): void {
    this.dialogRef.close();
  }

  ngOnInit() {
    this.title = this.data.featureName;
    this.checkDate = this.data.checkDate;
    this.dataSource = new MatTableDataSource<BankDeposit>(this.data.bankDepositData);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.dataSource.sortingDataAccessor = (item, sortColumn) => {
      switch (sortColumn) {
          case 'Name':
              return item.name;
          case 'Pay Type':
              return item.isVendor;
          case 'Payment':
              return item.netPay;
          case 'MTD':
              return item.mtdTotal;
          case 'QTD':
              return item.qtdTotal;
          case 'YTD':
              return item.ytdTotal;
           default:
              return item[this.sort.active];
      }
    };
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
}

print = () => {
    var employees = this.data.bankDepositData;

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
    printWindow.document.write('<h1 class="print-header">' + this.title + '</h1>');
    printWindow.document.write('<div class="print-page-section-group">');
    if (employees.length == 1)
      printWindow.document.write('<div class="instruction-text">'+ employees.length + ' Employee</div>');
    else{
      printWindow.document.write('<div class="instruction-text">'+ employees.length + ' Employees</div>');
    }
    printWindow.document.write('<div class="instruction-text">Date Range: ' + this.convertDate(new Date(this.data.dateRange.StartDate)) + ' - ' + this.convertDate(new Date(this.data.dateRange.EndDate)) + '</div>');
    printWindow.document.write('<div class="instruction-text">Exception Type: ' + (this.data.featureName ? this.data.featureName : 'N/A') + '</div>');
    printWindow.document.write('</div>');
    printWindow.document.write('</div>');
    printWindow.document.write('<table>');

    printWindow.document.write('<tr>');
    printWindow.document.write('<th>Name</th>');
    printWindow.document.write('<th>Pay Type</th>');
    printWindow.document.write('<th>Payment this payroll</th>');
    printWindow.document.write('<th>MTD</th>');
    printWindow.document.write('<th>QTD</th>');
    printWindow.document.write('<th>YTD</th>');
    printWindow.document.write('</tr>');

    employees.forEach(employee => {
      printWindow.document.write('<tr>');
      printWindow.document.write('<td>' + employee.name + '</td>');
      if (this.title == "Employee Payments"){
        printWindow.document.write('<td>' + employee.employeePayType + '</td>');
      }
      else{
        printWindow.document.write('<td>' + "Vender" + '</td>');
      }
      printWindow.document.write('<td>' + `$${employee.netPay.toLocaleString('en-US')}` + '</td>');
      printWindow.document.write('<td>' + `$${employee.mtdTotal.toLocaleString('en-US')}` + '</td>');
      printWindow.document.write('<td>' + `$${employee.qtdTotal.toLocaleString('en-US')}` + '</td>');
      printWindow.document.write('<td>' + `$${employee.ytdTotal.toLocaleString('en-US')}` + '</td>');
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

  isVendor(payType){
    return payType ? payType : "Vendor";
  }

}
