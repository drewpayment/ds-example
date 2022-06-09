import { ClientStatisticsApiService } from './../shared/client-statistics-api.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ClientPayroll } from '../shared/models/clientPayroll';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'ds-payroll-table',
  templateUrl: './payroll-table.component.html',
  styleUrls: ['./payroll-table.component.scss']
})
export class PayrollTableComponent implements OnInit {

    @ViewChild(MatPaginator, {}) paginator: MatPaginator;
    @ViewChild(MatSort, {}) sort: MatSort;

    tableData: any;
    clients: ClientPayroll[];

    searchLength: number;
    loading: boolean = true;

    displayedColumns: string[] = ['clientName', 'clientCode', 'dataEntry', 'frequency', 'processDay', 'status', 'lastCheckDateRun', 'lastPayrollAccepted', 'primaryContact', 'email', 'phoneNumber'];
    pageOptions = [];

    startDate: Date;
    endDate: Date;

    constructor(private apiSerivice: ClientStatisticsApiService) {
        this.loading = true;

        this.apiSerivice.getClientsRunningPayroll().subscribe((data) => {
            this.clients = data;
            this.searchLength = data.length;
            this.tableData = new MatTableDataSource<ClientPayroll>(data);
            this.pageOptions = [10, 25, 50 ];
            this.tableData.paginator = this.paginator;
            this.tableData.sort = this.sort;
            this.loading = false;
        });
    }

    ngOnInit() {
        this.getDates();
    }

    applyFilter(filterValue: string) {
        this.tableData.filter = filterValue.trim().toLowerCase();
        this.searchLength = this.tableData.filteredData.length;
    }

    getDates(){
        var start = new Date();
        var curDayStart = start.getDay();

        start.setDate(start.getDate() - curDayStart)
        this.startDate = start;

        var end = new Date();
        var curDayEnd = end.getDay();

        end.setDate(end.getDate() + (6 - curDayEnd));
        this.endDate = end;
    }

}
