import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { IFeaturesChartDialogData } from '../shared/models/IFeaturesChartDialogData';
import { Client } from '../shared/models/client';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'ds-features-chart-modal',
  templateUrl: './features-chart-modal.component.html',
  styleUrls: ['./features-chart-modal.component.scss']
})
export class FeaturesChartModalComponent implements OnInit {

    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;

    tableData: MatTableDataSource<any>;
    featureData: IFeaturesChartDialogData;
    searchLength: number;

    displayedColumns: string[] = ['clientName', 'clientCode', 'fullName', 'emailAddress', 'phoneNumber'];
    pageOptions = [];

    constructor(private dialogRef: MatDialogRef<FeaturesChartModalComponent, IFeaturesChartDialogData>, @Inject(MAT_DIALOG_DATA) private data: IFeaturesChartDialogData) {
        this.featureData = data;
        this.searchLength = data.clientList.length;
    }

    ngAfterViewInit(){
        this.tableData.sort = this.sort;
        this.tableData.sortingDataAccessor = (data, header) => data[header.toString()].toLocaleLowerCase();
    }

    ngOnInit() {
        this.tableData = new MatTableDataSource<Client>(this.data.clientList);
        this.tableData.paginator = this.paginator;
    }

    applyFilter(filterValue: string) {
        this.tableData.filter = filterValue.trim().toLowerCase();
        this.searchLength = this.tableData.filteredData.length;
    }

    cancel(){
        this.dialogRef.close(null);
    }

    print = () => {
        let clients: Array<any> = this.featureData.clientList;

        clients.sort((a,b) => (a.clientName.toLower > b.clientName) ? 1 : ((b.clientName > a.clientName) ? -1 : 0));

        var printWindow = window.open(this.featureData.featureName, 'Print' + (new Date()).getTime(), 'left=20,top=20,width=0,height=0');
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
        printWindow.document.write('<h1 class="print-header">'+ this.featureData.featureName + '</h1>');
        printWindow.document.write('<div class="print-page-section-group">');
        printWindow.document.write('<div class="instruction-text">'+ clients.length + ' Active Clients</div>');
        printWindow.document.write('</div>');
        printWindow.document.write('</div>');
        printWindow.document.write('<table>');

        printWindow.document.write('<tr>');
        printWindow.document.write('<th>Client</th>');
        printWindow.document.write('<th>Code</th>');
        printWindow.document.write('<th>Primary Contact</th>');
        printWindow.document.write('<th>Email</th>');
        printWindow.document.write('<th>Phone</th>');
        printWindow.document.write('</tr>');

        clients.forEach(client => {
            var contact = {
                firstName: '',
                lastName: '',
                emailAddress: '',
                phoneNumber: '',
                phoneExtension: ''
            };

            if(client.contact != undefined){
                contact = {
                    firstName: client.contact.firstName,
                    lastName: client.contact.lastName,
                    emailAddress: client.contact.emailAddress,
                    phoneNumber: client.contact.phoneNumber,
                    phoneExtension: client.contact.phoneExtension
                };
            }

            printWindow.document.write('<tr>');
            printWindow.document.write('<td>' + client.clientName + '</td>');
            printWindow.document.write('<td>' + client.clientCode + '</td>');
            printWindow.document.write('<td>' + contact.firstName + ' ' + contact.lastName + '</td>');
            printWindow.document.write('<td>' + contact.emailAddress + '</td>');
            printWindow.document.write('<td>' + contact.phoneNumber + (contact.phoneExtension != '' ? ' ext. ' + contact.phoneExtension : ' ') + '</td>');
            printWindow.document.write('<tr>');
        });

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
}
