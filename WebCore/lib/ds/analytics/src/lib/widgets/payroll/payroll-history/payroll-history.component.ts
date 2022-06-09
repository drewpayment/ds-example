import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { AnalyticsApiService } from '@ds/analytics/shared/services/analytics-api.service';
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import * as moment from "moment";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { PayrollHistory } from '@ds/analytics/shared/models/PayrollHistory.model';
import { MOMENT_FORMATS } from '@ds/core/shared';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'ds-payroll-history',
  templateUrl: './payroll-history.component.html',
  styleUrls: ['./payroll-history.component.css']
})
export class PayrollHistoryComponent implements OnInit {
  @Input() employeeIds: Number[];
  @Input() dateRange: DateRange;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  data: MatTableDataSource<PayrollHistory>;
  displayedColumns: string[] = ['payrollId', 'checkDate', 'periodStart', 'periodEnded', 'totalGross', 'totalNet', 'payrollRunDescription', 'acceptedDate'];

  title: string = 'PAYROLL HISTORY';
  cardType = "graph";
  infoData: InfoData;
  emptyState = false;
  loaded: boolean = false;

  constructor(
    private analyticsApi: AnalyticsApiService,
  ) { }

  ngOnInit() {
    this.analyticsApi.GetPayrollHistory(moment(this.dateRange.StartDate).format(MOMENT_FORMATS.DATE),moment(this.dateRange.EndDate).format(MOMENT_FORMATS.DATE), this.employeeIds)
        .subscribe((history: any) => {
          if(history.length <= 0){
            this.loaded = true;
            this.emptyState = true;
          }
            this.data = new MatTableDataSource<PayrollHistory>(history);
            this.data.paginator = this.paginator;
            this.data.sort = this.sort;
            this.data.sortingDataAccessor = (item, sortColumn) => {
              switch (sortColumn) {
                  case 'payrollId':
                      return item.payrollId;
                  case 'checkDate':
                      return item.checkDate;
                  case 'periodStart':
                      return item.periodStart;
                  case 'periodEnded':
                      return item.periodEnded;
                  case 'totalGross':
                      return item.totalGross;
                  case 'totalNet':
                      return item.totalNet;
                  case 'payrollRunDescription':
                      return item.payrollRunDescription;
                  case 'acceptedDate':
                      return item.runDate;
                   default:
                      return item[this.sort.active];
              }
            };
            this.loaded = true;
          })

  }
}
